import requests
from worker.business.logger import MyLogger
from worker import DATA_API_BASE_ADDRESS

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
        MyLogger().error(message)
    elif type == "warning":
        MyLogger().warning(message)
    else:
        MyLogger().debug(message)


def add_media_item(media_item):
    response_ok, _ = make_request("post", f"/api/mediaitems", json=media_item)
    return response_ok


def get_media_item(item_guid):
    _, return_value = make_request("get", f"/api/mediaitems/{item_guid}")
    return return_value


def update_media_item(media_item):
    response_ok, _ = make_request("put", f"/api/mediaitems", json=media_item)
    return response_ok


def get_playlist(playlist_id):
    _, return_value = make_request("get", f"/api/playlists/{playlist_id}")
    return return_value


def add_log(log_object):
    response_ok, _ = make_request("post", f"/api/logs", json=log_object)
    return response_ok
