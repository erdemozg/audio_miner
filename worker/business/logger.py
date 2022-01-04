import requests
from worker.business import data_api_client

class MyLogger(object):
    def debug(self, msg):
        print("logging error:", msg)
    def warning(self, msg):
        print("logging error:", msg)
    def error(self, msg):
        print("logging error:", msg)
        data_api_client.add_log({
            "type": 'ERROR',
            "submitter": 'youtube_video_processor',
            "message": msg
        })
