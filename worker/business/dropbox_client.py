import os
import dropbox
import string
import uuid
from worker.business import data_api_client
from worker.business.logger import MyLogger


def upload_to_dropbox(item_guid, mp3_path):
    media_item = data_api_client.get_media_item(item_guid)
    if media_item:
        playlist_id = media_item['playlist_id']
        if playlist_id is not None:
            playlist = data_api_client.get_playlist(playlist_id)
            if playlist is not None and 'dropbox_token' in playlist:
                dropbox_token = playlist['dropbox_token']
                if dropbox_token is not None and len(dropbox_token) > 0:
                    try:
                        dbx = dropbox.Dropbox(dropbox_token)
                        mode = dropbox.files.WriteMode.overwrite
                        ext = os.path.basename(mp3_path).split('.')[1]
                        sane_filename = sanitize_file_name(media_item["yt_video_title"])
                        path = f'/{sane_filename}.{ext}'
                        with open(mp3_path, 'rb') as f:
                            data = f.read()
                        print("uploading to dropbox")
                        res = dbx.files_upload(data, path, mode, mute=False)
                        print("dropbox file_upload result: ", res)
                        set_media_item_synced(item_guid)
                    except Exception as e:
                        process_error_message(f'exception @upload_to_dropbox: {str(e)}')


def set_media_item_synced(item_guid):
    media_item = data_api_client.get_media_item(item_guid)
    media_item["is_synced_to_dropbox"] = True
    data_api_client.update_media_item(media_item)


def process_error_message(msg):
    MyLogger().error(msg)


def sanitize_file_name(potential_file_path_name):
    valid_filename = ""
    chars_and_digists = "{0}{1}{2}".format(string.ascii_letters, string.digits, "ğüşıöçĞÜŞİÖÇ")
    valid_chars = "-_.() {0}".format(chars_and_digists)
    valid_filename = "".join(ch for ch in potential_file_path_name if ch in valid_chars)
    valid_chars = "{0}".format(chars_and_digists)
    test_filename = "".join(ch for ch in potential_file_path_name if ch in valid_chars)
    if len(test_filename) == 0 or len(valid_filename) == 0:
        valid_filename = guid = str(uuid.uuid4())
    return valid_filename
