
from celery import Celery
from worker.business.youtube_video_processor import process
from worker.business.dropbox_client import upload_to_dropbox as upload_to_dropbox_method
from worker import RABBITMQ_BROKER_URI

celery_instance = Celery(__name__, broker=RABBITMQ_BROKER_URI)

@celery_instance.task
def process_youtube_video(user_id, db_laylist_id, yt_playlist_id, yt_video_id, yt_playlist_item_id):
    print(f"@process_youtube_video, youtube id: {yt_video_id}")
    process(user_id, db_laylist_id, yt_playlist_id, yt_video_id, yt_playlist_item_id)

@celery_instance.task
def upload_to_dropbox(item_guid, mp3_path):
    print(f"@upload_to_dropbox, mp3_path: {mp3_path}")
    upload_to_dropbox_method(item_guid, mp3_path)
