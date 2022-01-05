import os
import hashlib

import data_api_client


"""
query a user with user id.
return user dict if found, none otherwise.
"""
def get_user_by_id(user_id):
    return data_api_client.get_user(user_id)


"""
get_media_item
"""
def get_media_items(playlist_id, skip=0, take=2_000_000_000, title="", user="", exclude_deleted_items=False):
    ret = data_api_client.get_media_items(
        playlist_id=str(playlist_id) if playlist_id is not None else "",
        skip=str(skip),
        take=str(take),
        title=title,
        user=user,
        exclude_deleted_items=exclude_deleted_items)
    return ret


"""
delete_media_item
"""
def delete_media_item(user_id, item_guid):
    media_item = data_api_client.get_media_item_by_guid(item_guid)
    if media_item is not None and media_item["user_id"] == user_id:
        ret = data_api_client.delete_media_item(media_item["id"])
        if ret:
            if media_item["thumbnail_path"] is not None and len(media_item["thumbnail_path"]) > 0:
                try_delete_file(media_item["thumbnail_path"])
            if media_item["mp3_path"] is not None and len(media_item["mp3_path"]) > 0:
                try_delete_file(media_item["mp3_path"])
            return True
    return False


"""
get_media_item
"""
def get_media_item(item_guid):
    return data_api_client.get_media_item_by_guid(item_guid)


def try_delete_file(path):
    try:
        if os.path.isfile(path):
            os.remove(path)
    except:
        pass


"""
get_user_by_username
"""
def get_user_by_username(username):
    return data_api_client.get_user_by_username(username)


"""
create_user
"""
def create_user(username, password):
    existing_user = data_api_client.get_user_by_username(username)
    if existing_user is not None:
        return False, "user_already_exists"
    return data_api_client.create_user({"username": username, "password": hashlib.sha512(password.encode('utf-8')).hexdigest()})


"""
check user credentials and return true and user id if they are ok, false and none otherwise.
"""
def check_user_credentials(username, password):
    existing_user = data_api_client.get_user_by_username(username)
    if existing_user is not None and existing_user["password"] == hashlib.sha512(password.encode('utf-8')).hexdigest():
        return True, existing_user["id"]
    else:
        return False, None


"""
get playlist definitions for user
"""
def get_user_playlists(user_id):
    return data_api_client.get_playlists_by_user_id(user_id)


"""
add_user_playlist
"""
def add_user_playlist(user_id, yt_playlist_id, yt_playlist_name, dropbox_token):

    existing_data = data_api_client.get_playlists_by_yt_playlist_id(yt_playlist_id)

    if existing_data:
        return False, "playlist_id_exists"

    ret = data_api_client.add_playlist({
        "user_id": user_id,
        "yt_playlist_id": yt_playlist_id,
        "yt_playlist_name": yt_playlist_name,
        "dropbox_token": dropbox_token
    })

    return ret, ""


"""
update_user_playlist
"""
def update_user_playlist(id, yt_playlist_id, yt_playlist_name, dropbox_token):
    ret = data_api_client.update_playlist({
        "id": id,
        "yt_playlist_id": yt_playlist_id,
        "yt_playlist_name": yt_playlist_name,
        "dropbox_token": dropbox_token
    })
    return ret


"""
delete user playlist
"""
def delete_user_playlist(playlist_id):
    return data_api_client.delete_playlist(playlist_id)
