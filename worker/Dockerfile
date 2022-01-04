FROM python:3.8-slim-buster
WORKDIR /

RUN pip install --upgrade pip

RUN apt-get update \
    && apt-get install build-essential gcc libpq-dev ffmpeg -y \
    && apt-get clean

COPY requirements.txt /worker/requirements.txt
RUN pip install -r /worker/requirements.txt

CMD ["celery", "-A", "worker.celery_tasks.celery_instance", "worker", "--pool", "threads", "--loglevel=INFO"]
