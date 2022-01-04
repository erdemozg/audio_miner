import os
import requests
import logging

DATA_API_BASE_ADDRESS = os.environ.get("DATA_API_BASE_ADDRESS", default="")

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



def get_all_playlists():
    _, return_value = make_request("get", f"/api/playlists")
    return return_value


def get_media_items_for_playlist(playlist_id):
    _, return_value = make_request("get", f"/api/mediaitems?playlist_id={playlist_id}")
    return return_value


def set_playlist_sync_error(playlist_id, error_message):
    _, playlist = make_request("get", f"/api/playlists/{playlist_id}")
    if playlist:
        playlist["sync_error"] = error_message
        response_ok, _ = make_request("put", f"/api/playlists", json=playlist)
        return response_ok
    return False
