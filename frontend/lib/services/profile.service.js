import { fetchWrapper } from "../network/fetch-wrapper";

/**
 * contains a method to query user profile info.
 * demonstrates the access to a protected api endpoint.
 */
export const profileService = {
  getUserPlaylists,
  saveUserPlaylist,
  deleteUserPlaylist
};

function getUserPlaylists() {
  return fetchWrapper.get(`/user_playlists`)
    .then((response) => {
        return response;
    });
}

function saveUserPlaylist(playlistID, youtubePlaylistID, playlistName, dropboxToken) {

  const postObject = {
    "id": playlistID, 
    "yt_playlist_id": youtubePlaylistID, 
    "yt_playlist_name": playlistName, 
    "dropbox_token": dropboxToken
  }

  if(playlistID != null){
    return fetchWrapper.put(`/user_playlists`, postObject)
      .then((response) => {
        return response;
      });
  }
  else {
    return fetchWrapper.post(`/user_playlists`, postObject)
      .then((response) => {
        return response;
      });
  }
}

function deleteUserPlaylist(playlistID) {
  return fetchWrapper.delete(`/user_playlists/${playlistID}`)
    .then((response) => {
      return response;
    });
}
