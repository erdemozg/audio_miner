import { useEffect, useState } from "react";
import Router from "next/router";
import Alert from "../components/alert";
import { authService } from "../lib/services/auth.service";
import { profileService } from "../lib/services/profile.service";
import { useAppSelector } from "../lib/client_state/hooks";
import { selectUser } from "../lib/client_state/user";
import {
  YoutubeIcon,
  DropboxIcon,
  LoadingIcon,
} from "../components/icons";

import { 
  UserIcon, 
  DotsVerticalIcon, 
  PencilAltIcon,
  TrashIcon,
  ExclamationCircleIcon,
  XCircleIcon,
  LinkIcon
} from "@heroicons/react/solid";


/**
 * modal component to add a new playlist
 */
const PlaylistModal = ({item, onModalClose, onSaveSuccess}) => {

  console.log(item);

  const [isLoading, setLoading] = useState(false);
  const [id, setId] = useState(item.id || null);
  const [youtubePlaylistID, setYoutubePlaylistID] = useState(item.yt_playlist_id || "");
  const [playlistName, setPlaylistName] = useState(item.yt_playlist_name || "");
  const [dropboxToken, setDropboxToken] = useState(item.dropbox_token || "");
  const [formErrorMessage, setFormErrorMessage] = useState("");

  const checkForm = () => {
    if (youtubePlaylistID.length == 0) {
      return {
        ok: false,
        message: "Please fill-in the required fields!",
      };
    }
    if (playlistName.length == 0) {
      return {
        ok: false,
        message: "Please fill-in the required fields!",
      };
    }
    return {
      ok: true,
      message: "",
    };
  };

  const savePlaylist = () => {
    setFormErrorMessage("");
    setLoading(true);

    const { ok, message } = checkForm();

    if (ok) {
      profileService
        .saveUserPlaylist(id, youtubePlaylistID, playlistName, dropboxToken)
        .then((res) => {
          setLoading(false);
          if(res.ok){
            onModalClose();
            if(onSaveSuccess){
              onSaveSuccess();
            }
          }
          else {
            console.log(res);
            setFormErrorMessage("Something went wrong!"); 
          }
        })
        .catch((error) => {
          setLoading(false);
          console.log(error);
          if(error == "playlist_id_exists"){
            setFormErrorMessage("This playlist id was already added!");
          }
          else {
            setFormErrorMessage("Something went wrong!");
          }
        });
    } else {
      setFormErrorMessage(message);
      setLoading(false);
    }
  };

  return(
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full">
      <div className="px-4 mt-8 flex items-center justify-center">
        <div className="p-8 bg-white rounded-2xl w-full max-w-xl shadow-lg space-y-2">
          <div className="text-center mb-10">
            <span className="text-2xl font-light tracking-wide">
              {!!id && <span>Edit </span>}
              {!id && <span>New </span>}
              Youtube Playlist
            </span>
          </div>
          <div>
            <label
              className="text-sm text-gray-500 tracking-wider"
              htmlFor="yt_playlist_id"
            >
              Youtube Playlist ID *
            </label>
            <div className="mt-1 relative">
              <input
                className="py-2 px-4 border-2 border-indigo-500 outline-none rounded-md w-full shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-700"
                type="text"
                name="yt_playlist_id"
                id="yt_playlist_id"
                autoComplete="yt_playlist_id"
                placeholder="Youtube Playlist ID"
                value={youtubePlaylistID}
                onChange={(e) => setYoutubePlaylistID(e.target.value)}
              />
            </div>
          </div>
          <div>
            <label
              className="text-sm text-gray-500 tracking-wider"
              htmlFor="yt_playlist_name"
            >
              Playlist Name *
            </label>
            <div className="mt-1 relative">
              <input
                className="py-2 px-4 border-2 border-indigo-500 outline-none rounded-md w-full shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-700"
                type="text"
                name="yt_playlist_name"
                autoComplete="yt_playlist_name"
                id="yt_playlist_name"
                placeholder="Playlist Name"
                value={playlistName}
                onChange={(e) => setPlaylistName(e.target.value)}
              />
            </div>
          </div>
          <div>
            <label
              className="text-sm text-gray-500 tracking-wider"
              htmlFor="dropbox_token"
            >
              Dropbox App Token
            </label>
            <div className="mt-1 relative">
              <input
                className="py-2 px-4 border-2 border-indigo-500 outline-none rounded-md w-full shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-700"
                type="text"
                name="dropbox_token"
                autoComplete="dropbox_token"
                id="dropbox_token"
                placeholder="Dropbox App Token"
                value={dropboxToken}
                onChange={(e) => setDropboxToken(e.target.value)}
              />
            </div>
          </div>
          <div className="w-full">
            <button
              onClick={savePlaylist}
              className="mt-4 py-3 bg-indigo-500 text-white text-lg font-semibold rounded-md w-full outline-none shadow-md focus:ring-2 focus:ring-offset-1 focus:ring-indigo-300"
            >
              {isLoading ? (
                <LoadingIcon className="animate-spin h-7 w-7 m-auto text-white" />
              ) : (
                "Save"
              )}
            </button>
          </div>
          {formErrorMessage && <Alert message={formErrorMessage} type="error" />}
          <div>
            <div className="my-5 w-full h-px bg-gray-300"></div>
          </div>
          <div className="w-full">
            <button 
              onClick={() => onModalClose() }
              className="py-3 bg-gray-300 text-gray-600 text-lg font-semibold rounded-md w-full outline-none shadow-md focus:ring-2 focus:ring-offset-1 focus:ring-gray-300">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};


/**
 * playlist item card component
 */
const PlaylistCard = ({item, onEditClicked, onDeleteSuccess}) => {

  const [menuShown, setMenuShown] = useState(false);

  const openModal = () => {
    setMenuShown(false);
    onEditClicked(item)
  };

  const deleteItem = () => {
    setMenuShown(false);
    const confirmed = confirm("Item will be deleted?");
    if (confirmed) {
      profileService.deleteUserPlaylist(item.id)
        .then(response => {
          onDeleteSuccess();
        })
        .catch(error => {
          console.log(error);
          alert("Something went wrong!");
        });
    }
  };

  return(
    <div className="py-2 px-4 w-full shadow-lg bg-gray-100 rounded-lg flex flex-row items-center">
      <YoutubeIcon className="h-14 w-14 text-indigo-500 mr-4" />
      <div className="flex flex-col flex-grow w-1/2 overflow-hidden">
        <span className="text-gray-500 font-semibold">
          {item.yt_playlist_name}
        </span>
        <span className="text-gray-500 text-sm">
          {item.yt_playlist_id}
        </span>
      </div>
      {!!item.sync_error && 
        <div title={"there was an error querying this playlist:\n" + item.sync_error}>
          <ExclamationCircleIcon className="h-6 w-6 text-red-400 mr-2" />
        </div>
      }
      {!!item.dropbox_token && 
        <div title="contains dropbox integration token">
          <DropboxIcon className="h-6 w-6 text-gray-300 mr-1" />
        </div>
      }
      <div className="cursor-pointer" onClick={() => setMenuShown(!menuShown)}>
        <DotsVerticalIcon className="h-8 w-8 text-gray-500" />
      </div>     
      { menuShown &&
        <div className="relative">
          <ul className="absolute top-4 -left-20 w-24 bg-yellow-300 rounded-md space-y-1">
          <li>
              <a 
                href={`https://www.youtube.com/playlist?list=${item.yt_playlist_id}`}
                target="_blank"
                rel="noreferrer"
                className="pt-1 px-2 flex flex-row hover:bg-yellow-400"
              >
                <LinkIcon className="h-5 w-5 mr-2" />
                <span className="text-sm">Open</span>
              </a>
            </li>
            <li>
              <div 
                className="pt-1 px-2 flex flex-row cursor-pointer hover:bg-yellow-400" 
                onClick={openModal}
              >
                <PencilAltIcon className="h-5 w-5 mr-2" />
                <span className="text-sm">Edit</span>
              </div>
            </li>
            <li>
              <div 
                className="pt-1 px-2 flex flex-row cursor-pointer text-red-700 hover:bg-yellow-400"
                onClick={deleteItem}
              >
                <TrashIcon className="h-5 w-5 mr-2" />
                <span className="text-sm">Delete</span>
              </div>
            </li>
            <li>
              <div 
                className="py-1 px-2 flex flex-row cursor-pointer hover:bg-yellow-400"
                onClick={() => setMenuShown(false)}
              >
                <XCircleIcon className="h-5 w-5 mr-2" />
                <span className="text-sm">Close</span>
              </div>
            </li>
          </ul>
        </div>
      }
    </div>
  );
}


/**
 * profile page component.
 */
export default function Profile() {

  const [isLoading, setLoading] = useState(false);
  const [playlistModalShown, setPlaylistModalShown] = useState(false);
  const [playlistModalItemToEdit, setPlaylistModalItemToEdit] = useState({});
  const [playlists, setPlaylists] = useState([]);
  const user = useAppSelector(selectUser);

  useEffect(() => {
    refreshPlaylists();
  }, []);

  const handleLogout = () => {
    authService.logout().then(response => {
      Router.push("/");
    })
    .catch(err => {
      console.log(err);
    });
  };

  const refreshPlaylists = () => {
    setLoading(true);
    profileService
      .getUserPlaylists()
      .then((playlists) => {
        setLoading(false);
        setPlaylists(playlists);
      })
      .catch((error) => {
        setLoading(false);
        console.log(error);
      });
  };

  const showPlaylistItemModal = (itemToEdit) => {
    setPlaylistModalItemToEdit(itemToEdit);
    setPlaylistModalShown(true);
  };

  return (
    <div className="px-6 mt-8 flex items-center justify-center">
      <div className="p-8 bg-white rounded-2xl w-full max-w-xl shadow-lg space-y-6">
        <div className="w-full flex flex-row items-center justify-center ">
          <UserIcon className="h-24 w-24 text-indigo-500 text-center" />
        </div>
        <div className="text-center text-lg text-gray-500 font-semibold tracking-wide">
          <span>{user.username}’s youtube playlists to audiomine</span>
        </div>
        {isLoading && <LoadingIcon className="animate-spin h-7 w-7 m-auto text-indigo-500" />}
        {
          playlists.map((item, index) => 
            <PlaylistCard 
              key={item.id} 
              item={item}
              onDeleteSuccess={refreshPlaylists} 
              onEditClicked={showPlaylistItemModal} />
          )
        }
        <div className="pb-1 w-full border-2 border-indigo-500 rounded-lg">
            <div className="cursor-pointer" onClick={() => showPlaylistItemModal({})}>
              <div className=" flex flex-col items-center justify-center">
                <span className="font-light text-4xl text-indigo-500">+</span>
                <span className="font-light text-lg text-indigo-500">
                  Add New Playlist
                </span>
              </div>
            </div>
        </div>
        <div>
          <div className="w-full h-px bg-gray-300"></div>
        </div>
        <div className="w-full">
          <div className="cursor-pointer" onClick={handleLogout}>
            <div className="py-3 bg-gray-300 text-gray-600 text-lg text-center font-semibold rounded-md w-full outline-none shadow-md focus:ring-2 focus:ring-offset-1 focus:ring-gray-300">
              Log Out
            </div>
          </div>
        </div>
      </div>
      {playlistModalShown && 
        <PlaylistModal 
          item={playlistModalItemToEdit}
          onModalClose={() => setPlaylistModalShown(false) }
          onSaveSuccess={refreshPlaylists}
        />
      }
    </div>
  );
}

Profile.auth = true;
