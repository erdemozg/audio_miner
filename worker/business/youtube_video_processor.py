from __future__ import unicode_literals
import os
import requests
import shutil
import uuid
import youtube_dl
from celery import Celery
from worker.business import data_api_client
from worker.business.logger import MyLogger
from worker import RABBITMQ_BROKER_URI

celery_instance = Celery(__name__, broker=RABBITMQ_BROKER_URI)


def process(user_id, db_playlist_id, yt_playlist_id, yt_video_id, yt_playlist_item_id):
    print(f'processing {yt_video_id}')
    video_title = ''
    video_duration = 0
    thumbnail_address = ''
    with youtube_dl.YoutubeDL({}) as ydl_metadata:
        info_dict = ydl_metadata.extract_info(
            f"https://www.youtube.com/watch?v={yt_video_id}", 
            download=False
        )
        # print("info_dict", info_dict)
        video_title = info_dict.get('title', None)
        video_duration = info_dict.get("duration", None)
        thumbnail_address = info_dict.get("thumbnail", None)
    item_guid = str(uuid.uuid4())
    thumbnail_path = download_thumbnail(thumbnail_address, item_guid)
    media_item = {
        "item_guid": item_guid,
        "user_id": user_id,
        "playlist_id": db_playlist_id,
        "yt_playlist_id": yt_playlist_id,
        "yt_playlist_item_id": yt_playlist_item_id,
        "yt_video_id": yt_video_id,
        "yt_video_title": video_title,
        "status": "pending",
        "progress": 0,
        "thumbnail_path": thumbnail_path,
        "mp3_path": "",
        "duration": video_duration
    }
    is_added = data_api_client.add_media_item(media_item)
    if is_added:
        try:
            ydl_opts = {
                'format': 'bestaudio/best',
                'logger': MyLogger(),
                'progress_hooks': [progress_hook],
                'outtmpl': f'/files/{item_guid}/{item_guid}.%(ext)s'
            }
            with youtube_dl.YoutubeDL(ydl_opts) as ydl_downloader:
                ydl_downloader.download([f'https://www.youtube.com/watch?v={yt_video_id}'])
        except Exception as e:
            error_message = str(e)
            MyLogger().error(f'Exception @process_video_celery: {error_message}')
            
            # youtube_dl sometimes throws exception about an "m4a" file not found,
            # although mp3 file successfully generated. so, ignore that exception.
            if not error_message.startswith("[Errno 2] No such file or directory"):
                update_media_item_progress(item_guid, 'error', 0, error_message=error_message)
            


def progress_hook(d):
    try:
        print("progress_hook(d): ", d)
        item_guid = os.path.basename(d["filename"]).split('.')[0]
        file_extension = os.path.basename(d["filename"]).split('.')[1]
        status = d["status"]
        if status == 'finished':
            status = 'converting'
        update_media_item_progress(item_guid, status, int(100 * d['downloaded_bytes'] / d['total_bytes']))
        if d["status"] == "finished":
            source_audio_path = f"/files/{item_guid}/{item_guid}.{file_extension}"
            mp3_path = f"/files/{item_guid}/{item_guid}.mp3"
            try:
                res = os.system(f"ffmpeg -i {source_audio_path} -vn -ar 44100 -ac 2 -b:a 192k {mp3_path}")
                if res == 0:
                    update_media_item_progress(item_guid, 'ready', 100, mp3_path=mp3_path)
                    upload_to_dropbox(item_guid, mp3_path)
                else:
                    MyLogger().error(f'error @progress_hook -> ffmpeg: returned value was {str(res)}')
                    update_media_item_progress(item_guid, 'error', 0)
            except Exception as ffmpegException:
                error_message = str(ffmpegException)
                MyLogger().error(f'exception @progress_hook -> ffmpeg: {error_message}')
                update_media_item_progress(item_guid, 'error', 0, error_message=error_message)
            if os.path.exists(source_audio_path):
                os.remove(source_audio_path)
    except Exception as e:
        error_message = str(e)
        update_media_item_progress(item_guid, 'error', 0, error_message=error_message)
        MyLogger().error(f'exception @progress_hook: {error_message}')


def download_thumbnail(address, item_guid):
    ret = ''
    if len(address) > 0:
        r = requests.get(address, stream=True)
        if r.status_code == 200:
            _, file_extension = os.path.splitext(address)
            file_extension = file_extension.split("?")[0]
            write_path = f'/files/{item_guid}/{item_guid}{file_extension}'
            write_folder = f'/files/{item_guid}'
            if not os.path.isdir(write_folder):
                os.makedirs(write_folder)
            with open(write_path, 'wb') as f:
                r.raw.decode_content = True
                shutil.copyfileobj(r.raw, f)
                ret = write_path
    return ret


def update_media_item_progress(item_guid, status, progress, mp3_path=None, thumbnail_path=None, error_message=None):
    media_item = data_api_client.get_media_item(item_guid)
    if media_item:
        media_item["status"] = status
        media_item["progress"] = progress
        if mp3_path is not None:
            media_item["mp3_path"] = mp3_path
        if thumbnail_path is not None:
            media_item["thumbnail_path"] = thumbnail_path
        if error_message is not None:
            media_item["error_message"] = error_message
        data_api_client.update_media_item(media_item)


def upload_to_dropbox(item_guid, mp3_path):
    celery_instance.send_task(
        'worker.celery_tasks.upload_to_dropbox', 
        args=[
            item_guid, 
            mp3_path
        ], 
        kwargs={}
    )
