## About

Save a video to your (public) youtube playlist and its audio will end up in your dropbox.

<p float="left">
  <img src="https://raw.githubusercontent.com/everlost/audio_miner/main/screenshots/1-home.PNG" alt="screenshot" width="200"/>
  <img src="https://raw.githubusercontent.com/everlost/audio_miner/main/screenshots/2-profile.PNG" alt="screenshot" width="200"/> 
</p>


## How To Run

I prefer to run this application on my raspberry pi using docker-compose.

`docker-compose up -d`

Don't forget to go through the docker-compose.yml file and;

- provide actual values for youtube api key and jwt secret
- configure host ports for services based on availability
- configure BACKEND_API_URL_ARG for frontend service depending on your network configuration


## Main Features Of System Components

backend:

- python/flask web api
- consumed by frontend single-page application
- provides access/refresh token endpoints for authentication

frontend:

- nextjs/react app to manage playlists and view processed items
- uses tailwindcss for component styling
- uses redux-toolkit for state management
- implements access/refresh token pattern for authentication

internal_data_api:

- .net core web api
- provides endpoints to interact with database
- is meant to be for internal-use only (between containers)
- uses entity framework on sqlite database, with code-first approach
- consumed by backend, playlist_watcher and worker services

playlist_watcher:

- python app that watches users' playlists for new items
- passes new items to worker's task queue for processing

worker:

- python app that uses celery distributed task queue to process youtube videos
- downloads audio from videos and converts them
- uploads to dropbox


## Todo:

There's a lot of room for improvement in this project but it's been working fine for me.

Nevertheless I'd like to add these features:

- Ability to add items manually with a link
- Dark theme
- Ability to send push notifications via tools like https://ntfy.sh/


## Knows Issues

I had trouble running rabbitmq docker container on my raspberry pi (clock_gettime error).

I fixed it with these commands:

`wget http://ftp.us.debian.org/debian/pool/main/libs/libseccomp/libseccomp2_2.4.4-1~bpo10+1_armhf.deb`

`sudo dpkg -i libseccomp2_2.4.4-1~bpo10+1_armhf.deb`
