FROM python:3.8-slim-buster

WORKDIR /

RUN pip install --upgrade pip

COPY requirements.txt /playlist_watcher/requirements.txt

RUN pip install -r /playlist_watcher/requirements.txt

CMD [ "python", "-u", "/playlist_watcher/run.py" ]
