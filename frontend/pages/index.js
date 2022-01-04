import { useState, useEffect } from "react";
import Link from "next/link";
import getConfig from "next/config";
import { secondsToHms } from "../lib/helpers/format_helpers";
import { commonService } from "../lib/services/common.service";
import { openLink } from "../lib/helpers/utils";
import { useAppSelector } from "../lib/client_state/hooks";
import { selectUser } from "../lib/client_state/user";
import {
  DropboxIcon,
  YoutubeIcon,
  LoadingIcon
} from "../components/icons";

import {
  RefreshIcon,
  AdjustmentsIcon,
  DotsVerticalIcon,
  UserCircleIcon,
  CalendarIcon,
  InformationCircleIcon,
  XCircleIcon,
  CloudDownloadIcon,
  TrashIcon,
  ExclamationCircleIcon
} from "@heroicons/react/solid";

var human = require("human-time");

const { publicRuntimeConfig } = getConfig();

const api_base_address = publicRuntimeConfig.apiUrl;

const ItemCard = ({item, loggedInUserId, onDeleteSuccess}) => {

  const [itemState, setItemState] = useState(item);
  const [menuShown, setMenuShown] = useState(false);
  const [errorShown, setErrorShown] = useState(false);

  const thumbnail_path = `${api_base_address}${itemState.thumbnail_path}`;
  const file_path = `${api_base_address}${itemState.mp3_path}`;
  const yt_url = `https://www.youtube.com/watch?v=${itemState.yt_video_id}`;

  const updateItemInfo = () => {
    commonService
      .getMediaItem(itemState.item_guid)
      .then(data => setItemState(data))
      .catch((error) => {
        console.log(error);
      });
  };

  const toggleMenu = () => {
    if (!menuShown) {
      updateItemInfo();
    }
    setMenuShown(!menuShown);
  };

  const menuItemClicked = (command) => {

    setMenuShown(false);

    if (command == "delete") {
      const confirmed = confirm("Item will be deleted?");
      if (confirmed) {
        commonService.deleteItem(itemState.item_guid)
          .then(response => {
            onDeleteSuccess();
          })
          .catch(error => {
            console.log(error);
            alert("Something went wrong!");
          });
      }
    }
    else if (command == "download"){
      openLink(file_path, true);
    }

  };

  const capitalizeFirstLetter = (input) => {
    if(!!input && input.length > 0)
      return input.charAt(0).toUpperCase() + input.substring(1);
    return "";
  }

  setTimeout(() => {
    if (menuShown) {
      updateItemInfo();
    }
  }, 3000)

  return (
    <div className="mt-8 bg-white rounded-lg w-full shadow-lg">
      <div className="flex flex-col sm:flex-row">
        <div className="relative flex-shrink-0 rounded-tl-lg rounded-tr-lg sm:rounded-none sm:rounded-bl-lg sm:rounded-tl-lg overflow-hidden">
          <YoutubeIcon className="h-6 w-6 text-red-500 absolute top-2 left-2"/>
          {itemState.duration && (
            <span className="absolute top-2 right-2 px-1 text-white bg-gray-900 text-xs bg-opacity-50 rounded">
              {secondsToHms(itemState.duration)}
            </span>
          )}
          <a href={yt_url} target="_blank" rel="noreferrer">
            <img
              className="h-48 w-full sm:h-full sm:w-72 object-cover"
              src={thumbnail_path}
              alt="cover photo" />
          </a>
        </div>

        <div className="w-full p-4 flex flex-col items-start">
          <div className="w-full flex flex-row items-center">
            <span className="text-xl font-light flex-1">
              {itemState.yt_video_title}
            </span>
            <div onClick={toggleMenu} className="cursor-pointer">
              <div className="text-gray-500">
                <DotsVerticalIcon className="h-6 w-6" />
              </div>
            </div>
            { menuShown &&
              <div className="relative">
                <ul className="absolute top-4 -left-32 w-32 flex flex-col justify-start bg-yellow-300 rounded-md space-y-1">
                  { ["pending", "downloading", "converting"].includes(itemState.status) &&
                    <li>
                      <div
                        onClick={() => menuItemClicked("")}  
                        className="flex flex-row pt-1 px-2 cursor-pointer hover:bg-yellow-400"
                      >
                        <LoadingIcon className="animate-spin h-5 w-5 m-auto mr-2" />
                        <span className="flex-1 text-sm" style={{whiteSpace: "nowrap"}}>
                          {itemState.status == "downloading" ? (
                            `${itemState.progress || "0"}%`
                          ) : (
                            capitalizeFirstLetter(itemState.status)
                          )}
                        </span>
                      </div>
                    </li>
                  }
                  { itemState.status == "ready" &&
                    <li>
                      <div
                        onClick={() => menuItemClicked("download")} 
                        className="flex flex-row pt-1 px-2 cursor-pointer hover:bg-yellow-400"
                      >
                        <CloudDownloadIcon className="h-5 w-5 mr-2" />
                        <span className="flex-1  text-sm">Download</span>
                      </div>
                    </li>
                  }
                  { itemState.user_id == loggedInUserId &&
                    <li>
                      <div
                        className="flex flex-row pt-1 px-2 flex flex-row cursor-pointer text-red-700 hover:bg-yellow-400"
                        onClick={() => menuItemClicked("delete")}
                      >
                        <TrashIcon className="h-5 w-5 mr-2" />
                        <span className="flex-1 text-sm">Delete</span>
                      </div>
                    </li>
                  }
                  <li>
                    <div
                      onClick={() => menuItemClicked("")} 
                      className="flex flex-row py-1 px-2 cursor-pointer center-items hover:bg-yellow-400"
                    >
                      <XCircleIcon className="h-5 w-5 mr-2" />
                      <span className="flex-1 text-sm">Close</span>
                    </div>
                  </li>
                </ul>
              </div>
            }
          </div>
          <div className="mt-4 flex items-center text-gray-400">
            <div className="mr-2">
              <UserCircleIcon className="h-6 w-6" />
            </div>
            <span className="text-sm">{itemState.username}</span>
          </div>
          <div className="mt-2 flex items-center text-gray-400">
            <div className="mr-2">
              <CalendarIcon className="h-6 w-6" />
            </div>
            <span className="text-sm">
              {human(new Date(`${itemState.created_at}Z`))}
            </span>
          </div>
          {itemState.is_synced_to_dropbox && (
            <div className="mt-2 flex items-center text-gray-400">
              <div className="mr-2">
                <DropboxIcon className="h-6 w-6" />
              </div>
              <span className="text-sm">synced to dropbox</span>
            </div>
          )}
          {itemState.error_message && (
            <div className="mt-2 flex items-center text-gray-400">
              <div className="mr-2">
                <ExclamationCircleIcon className="h-6 w-6 text-red-400" />
              </div>
              {errorShown ? (
                <span className="text-sm text-red-400" onClick={() => setErrorShown(false)}>{itemState.error_message}</span>
              ) : (
                <div  className="cursor-pointer text-sm text-red-400" onClick={() => setErrorShown(true)}>
                    There was an error processing this item...
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default function Home() {

  const itemsCountToTakeOnEachQuery = 20;

  const user = useAppSelector(selectUser);
  const [isLoading, setLoading] = useState(false);
  const [endOfList, setEndOfList] = useState(false);
  const [isFilterPanelShown, setFilterPanelShown] = useState(false);
  const [data, setData] = useState([]);
  const [titleFilter, setTitleFilter] = useState("");
  const [userFilter, setUserFilter] = useState("");
  const loggedInUserId = !!user ? user.id : null;

  const loadMore = (skip) => {
    setLoading(true);
    commonService
      .getMediaItems(skip, itemsCountToTakeOnEachQuery, titleFilter, userFilter)
      .then(items => {
        setLoading(false);
        if (skip == 0) {
          setData(items);
        }
        else {
          setData([...data, ...items]);
        }
        setEndOfList(items.length == 0 || items.length < itemsCountToTakeOnEachQuery);
      })
      .catch((error) => {
        setLoading(false);
        console.log(error);
      });
  };

  const filtersApplied = titleFilter.length > 0 || userFilter.length > 0;

  const applyFiltersWithTimeOut = (interval) => {
    const delayDebounceFn = setTimeout(() => {
      loadMore(0);
    }, interval)

    return () => clearTimeout(delayDebounceFn);
  };

  const removeItemFromList = (item) => {
    const _data = data.filter(p => p.item_guid != item.item_guid);
    setData(_data);
  };

  useEffect(() => {
    return applyFiltersWithTimeOut(titleFilter.length > 0 ? 1000 : 0);
  }, [titleFilter])

  useEffect(() => {
    return applyFiltersWithTimeOut(userFilter.length > 0 ? 1000 : 0);
  }, [userFilter])

  useEffect(() => {
    loadMore(0);
  }, []);

  return (
    <>
      { data.length > 0 || filtersApplied ? (
        <div className="mx-auto my-8 px-6 max-w-3xl flex flex-col">
          <div className="w-full flex justify-between">
            <div>
              <button onClick={() => { loadMore(0); }} className="px-4 py-2 bg-indigo-500 text-white text-lg font-semibold rounded-lg w-full outline-none shadow-lg focus:ring-2 focus:ring-offset-1 focus:ring-indigo-300">
                {
                  isLoading ? (
                    <LoadingIcon className="animate-spin h-5 w-5 m-auto text-white" />
                  ) : (
                    <RefreshIcon className="h-5 w-5" />
                  )
                }
              </button>
            </div>
            <div>
              <button onClick={() => { setFilterPanelShown(!isFilterPanelShown); }} className="relative pl-12 pr-4 py-2 bg-gray-300 text-gray-600 font-semibold text-sm rounded-md w-full outline-none shadow-lg focus:ring-2 focus:ring-offset-1 focus:ring-gray-300">
                <div className="absolute top-2 left-4">
                  <AdjustmentsIcon className="h-5 w-5" />
                </div>
                Filters
              </button>
            </div>
          </div>

          { isFilterPanelShown &&
            <div className="mt-4 px-4 py-4 w-full flex flex-row space-x-5 rounded-lg bg-gray-100 border-gray-300 border-2">
              <div className="flex-1">
                <div className="mt-1 relative">
                  { titleFilter.length > 0 &&
                    <div onClick={() => { setTitleFilter(""); }} className="absolute top-2 right-2 text-gray-300 cursor-pointer">
                      <XCircleIcon className="h-6 w-6" />
                    </div>
                  }
                  <input
                    className="py-2 border-2 border-gray-300 outline-none text-sm rounded-md w-full shadow-sm focus:border-gray-300 focus:outline-none focus:ring-1 focus:ring-indigo-700"
                    type="text"
                    placeholder="Title"
                    value={titleFilter}
                    onChange={(e) => setTitleFilter(e.target.value)} />
                </div>
              </div>
              <div className="flex-1">
                <div className="mt-1 relative">
                  { userFilter.length > 0 &&
                    <div onClick={() => { setUserFilter(""); }} className="absolute top-2 right-2 text-gray-300 cursor-pointer">
                      <XCircleIcon className="h-6 w-6" />
                    </div>
                  }
                  <input
                    className="py-2 border-2 border-gray-300 outline-none text-sm rounded-md w-full shadow-sm focus:border-gray-300 focus:outline-none focus:ring-1 focus:ring-indigo-700"
                    type="text"
                    placeholder="User"
                    value={userFilter}
                    onChange={(e) => setUserFilter(e.target.value)}/>
                </div>
              </div>
            </div>
          }

          {data.map((item, index) => {
            return(
              <ItemCard 
                key={item.id} 
                item={item} 
                onDeleteSuccess={ () => removeItemFromList(item) } 
                loggedInUserId={loggedInUserId} />
              );
          })}

          { !endOfList &&
            <div className="mt-6 flex flex-row justify-center items-center">
              <button onClick={() => { loadMore(data.length); }} className="px-4 py-2 bg-indigo-500 text-white text-sm rounded-lg outline-none shadow-lg focus:ring-2 focus:ring-offset-1 focus:ring-indigo-300">
                Load More
              </button>
            </div>
          }
        </div>
      ) : (
        <div className="p-8 flex flex-col items-center justify-center">
          <div className="mx-auto w-full max-w-lg bg-white p-8 rounded flex flex-col items-center justify-center">
            <InformationCircleIcon className="h-63 w-36 text-gray-300" />
            <div className="mt-4 text-gray-500">
              <p className="text-center">No items were added.</p>
              <p className="mt-2 text-center">
                Go to your&nbsp;
                <Link href="/profile" passHref={true}>
                  <a className="text-indigo-500">profile</a>
                </Link>
                &nbsp;to manage playlists.
              </p>
              <p className="mt-6 text-center">
                <div 
                  className="cursor-pointer text-indigo-500"
                  onClick={() => { loadMore(0); }}>Click to refresh</div>
              </p>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
