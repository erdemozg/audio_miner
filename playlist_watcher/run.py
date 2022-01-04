import os
import time
from youtube_playlist_watcher import process_playlists

interval_in_seconds = os.environ.get("YT_QUERY_INTERVAL_SECONDS", default=60)

if __name__ == '__main__':
    while True:
        try:
            process_playlists()
        except:
            pass
        time.sleep(int(interval_in_seconds))
