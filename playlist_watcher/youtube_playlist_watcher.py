import os
import requests
from celery import Celery
import data_api_client

RABBITMQ_BROKER_URI = os.environ.get("RABBITMQ_BROKER_URI", default="")
YOUTUBE_API_KEY = os.environ.get("YOUTUBE_API_KEY", default="")
DATA_API_BASE_ADDRESS = os.environ.get("DATA_API_BASE_ADDRESS", default="")

celery_instance = Celery(__name__, broker=RABBITMQ_BROKER_URI)


def get_playlist_items_from_youtube(playlist_id, yt_playlist_id):
    youtube_api_key = YOUTUBE_API_KEY
    items = []
    if youtube_api_key is not None and len(youtube_api_key) > 0:
        response = requests.get(f"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&playlistId={yt_playlist_id}&key={youtube_api_key}&maxResults=50")
        if response.ok:
            playlist_result = response.json()
            items.extend(playlist_result["items"])
            while "nextPageToken" in playlist_result:
                response = requests.get(f"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&playlistId={yt_playlist_id}&key={youtube_api_key}&maxResults=50&pageToken={playlist_result['nextPageToken']}")
                if response.ok:
                    playlist_result = response.json()
                    items.extend(playlist_result["items"])
        else:
            data_api_client.set_playlist_sync_error(playlist_id, response.text)
            
    else:
        print("api key env var not found")
    return items


def process_playlists():
    playlists = data_api_client.get_all_playlists()
    print("playlists", playlists)
    if playlists and len(playlists) > 0:
        for playlist in playlists:
            if playlist["sync_error"] is not None and len(playlist["sync_error"]) > 0:
                print(f"skipping playlist as sync_error is not empty: {playlist['id']}")
                continue
            playlist_items_on_youtube = get_playlist_items_from_youtube(playlist["id"], playlist["yt_playlist_id"])
            print("playlist_items_on_youtube", playlist_items_on_youtube)
            playlist_items_on_db = data_api_client.get_media_items_for_playlist(playlist["id"])
            print("playlist_items_on_db", playlist_items_on_db)
            new_items = []
            for playlist_item_on_youtube in playlist_items_on_youtube:
                existing_item = [x for x in playlist_items_on_db if x["yt_video_id"] == playlist_item_on_youtube["contentDetails"]["videoId"]]
                print('existing_item', existing_item)
                if len(existing_item) == 0:
                    new_items.append(playlist_item_on_youtube)
            if len(new_items) > 0:
                for new_item in new_items:
                    celery_instance.send_task(
                        'worker.celery_tasks.process_youtube_video', 
                        args=[
                            playlist["user_id"],
                            playlist["id"],
                            playlist["yt_playlist_id"],
                            new_item['contentDetails']['videoId'],
                            new_item["id"]
                        ], 
                        kwargs={}
                    )
                print(f"added {len(new_items)} new items")
            else:
                print('no new items')
