/**
 * Get content type from reponse.
 * @param {Response} response
 * @returns {string} content type
 */
function getContentType(response) {
    return response && response.headers.has('content-type') ? response.headers.get('content-type') : '';
}

/**
 * Check if response is json.
 * @param {Response} response
 * @returns {boolean} True if response is json.
 */
function isJson(response) {
    return getContentType(response).indexOf('application/json') > -1;
}

/**
* Get the body from the response.
* @param {Response} response
* @returns {Promise<string>} Response content.
*/
async function getResponseBody(response) {
    const body = await (getContentType(response).indexOf('application/json') > -1 ? response.json() : response.text());
    return body;
}

export { getContentType, isJson, getResponseBody };
