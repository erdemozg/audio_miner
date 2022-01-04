import os

MAX_VIDEO_LENGTH_IN_SECONDS = int(os.environ.get("MAX_VIDEO_LENGTH_IN_SECONDS", default=str(30*60)))
RABBITMQ_BROKER_URI = os.environ.get("RABBITMQ_BROKER_URI", default="")
DATA_API_BASE_ADDRESS = os.environ.get("DATA_API_BASE_ADDRESS", default="")
