import { fetchWrapper } from "../network/fetch-wrapper";

/**
 * contains methods to query common info.
 */
export const commonService = {
    getMediaItems,
    getMediaItem,
    deleteItem
};

function getMediaItems(skip, take, title, user) {
    return fetchWrapper.get(`/mediaitems?skip=${skip}&take=${take}&title=${title}&user=${user}`)
        .then((data) => {
            return data;
        });
}

function getMediaItem(itemGuid) {
    return fetchWrapper.get(`/mediaitems/${itemGuid}`)
        .then((data) => {
            return data;
        });
}

function deleteItem(itemGuid) {
    return fetchWrapper.delete(`/mediaitems/${itemGuid}`)
        .then((data) => {
            return data;
        });
}
