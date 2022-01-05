import os
import requests
import logging

DATA_API_BASE_ADDRESS = os.environ.get("DATA_API_BASE_ADDRESS", "")

logging.basicConfig(level=logging.DEBUG)

def make_request(method, url_path, json=None):
    
    response = None
    return_value = None
    
    url = f"{DATA_API_BASE_ADDRESS}{url_path}"

    if method == "get":
        response = requests.get(url)
    elif method == "post":
        response = requests.post(url, json=json)
    elif method == "put":
        response = requests.put(url, json=json)
    elif method == "delete":
        response = requests.delete(url)
    
    response_ok = response is not None and response.ok

    if response_ok:
        if 'application/json' in response.headers.get('Content-Type'):
            try:
                response_json = response.json()
                if response_json["success"] == True:
                    if "data" in response_json:
                        return_value = response_json["data"]
                else:
                    message = f"response was not successful: for {url} - {method}"
                    if "message" in response_json:
                        message += f"\nmessage: {response_json['message']}"
                    process_request_log("error", message)
            except ValueError:
                process_request_log("error", f"value error parsing response for {url} - {method}")
        else:
            process_request_log("error, "f"content-type of the response was not application/json for {url} - {method}")
    else:
        message = f"response was not ok for {url} - {method}"
        message += f"\nstatus_code: {response.status_code}, text:{response.text}"
        process_request_log("error", message)

    return response_ok, return_value


def process_request_log(type, message):
    if type == "error":
        logging.error(message)
    elif type == "warning":
        logging.warn(message)
    else:
        logging.info(message)

### user

def get_user(id):
    _, return_value = make_request("get", f"/api/users/{id}")
    return return_value

def get_user_by_username(username):
    _, return_value =  make_request("get", f"/api/users/getbyusername/{username}")
    return return_value
    
def create_user(user):
    response_ok, _ = make_request("post", f"/api/users", json=user)
    if response_ok:
        return True, ""
    else:
        return False, "error"

### playlist

def get_playlists_by_user_id(user_id):
    _, return_value = make_request("get", f"/api/playlists/getbyuserid/{user_id}")
    return return_value

def get_playlists_by_yt_playlist_id(yt_playlist_id):
    _, return_value = make_request("get", f"/api/playlists/getbyytplaylistid/{yt_playlist_id}")
    return return_value

def add_playlist(playlist):
    response_ok, _ = make_request("post", f"/api/playlists", json=playlist)
    return response_ok
    
def update_playlist(playlist):
    response_ok, _ = make_request("put", f"/api/playlists", json=playlist)
    return response_ok
    
def delete_playlist(id):
    response_ok, _ = make_request("delete", f"/api/playlists/{id}")
    return response_ok

### media items

def get_media_items(playlist_id, skip, take, title, user, exclude_deleted_items):
    
    url = f"/api/mediaitems"
    url = f"{url}?playlist_id={playlist_id}"
    url = f"{url}&skip={skip}"
    url = f"{url}&take={take}"
    url = f"{url}&title={title}"
    url = f"{url}&user={user}"
    url = f"{url}&exclude_deleted_items={exclude_deleted_items}"
    
    _, return_value = make_request("get", url)
    
    return return_value if return_value is not None else []

def get_media_item_by_guid(guid):
    _, return_value = make_request("get", f"/api/mediaitems/{guid}")
    return return_value
    
def delete_media_item(id):
    response_ok, _ = make_request("delete", f"/api/mediaitems/{id}")
    return response_ok
