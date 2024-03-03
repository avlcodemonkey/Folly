// @ts-ignore VS doesn't like this import but it builds fine
import ky, { HTTPError } from 'ky';

/**
 * Enum for http request methods.
 * @readonly
 * @enum {string}
 */
const HttpVerbs = Object.freeze({
    GET: 'get',
    POST: 'post',
    PUT: 'put',
    DELETE: 'delete',
});

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

/**
 * Web component for pushState + ajax support.
 */
class PJax extends HTMLElement {
    /**
     * Selector for the target container that responses should be replaced into.
     * @type {string}
     */
    target = '';

    /**
     * Selector for the loading indicator.
     * @type {string}
     */
    loadingIndicator = '';

    /**
     * Version number to include with requests.
     * @type {string}
     */
    version = '';

    /**
     * List of file types to ignore when intercepting links.
     * @type {string[]}
     */
    ignoreFileTypes = ['pdf', 'doc', 'docx', 'zip', 'rar', '7z', 'gif', 'jpeg', 'jpg', 'png'];

    /**
     * Name of attribute that indicates an element should be ignored.
     * @type {string}
     */
    excludeAttribute = 'data-pjax-no-follow';

    /**
     * Bound popState event listener.
     * @type {(event: any) => Promise<void>}
     */
    popStateListener;

    /**
     * Bound click event listener.
     * @type {(event: any) => Promise<void>}
     */
    clickListener;

    /**
     * Bound submit event listener.
     * @type {(event: any) => Promise<void>}
     */
    submitListener;

    /**
     * Path of the current page to use when creating history.
     * @type {string}
     */
    currentUrlForHistory;

    /**
     * Request method of the current page to use when creating history.
     * @type {string}
     */
    currentMethodForHistory;

    /**
     * Url origin
     * @type {string}
     */
    origin;

    /**
     * Initialize pjax by setting up event listeners.
     */
    constructor() {
        super();

        // @todo maybe convert to use destructing?
        if (this.dataset.target) {
            this.target = this.dataset.target;
        }
        if (this.dataset.loadingIndicator) {
            this.loadingIndicator = this.dataset.loadingIndicator;
        }
        if (this.dataset.version) {
            this.version = this.dataset.version;
        }
        if (this.dataset.ignoreFileTypes) {
            this.ignoreFileTypes = this.dataset.ignoreFileTypes.split(',') ?? [];
        }
        if (this.dataset.excludeAttribute) {
            this.excludeAttribute = this.dataset.excludeAttribute;
        }

        this.popStateListener = (event) => this.onPopState(event);
        this.clickListener = (event) => this.onClickListener(event);
        this.submitListener = (event) => this.onSubmitListener(event);

        window.addEventListener('popstate', this.popStateListener);
        this.addEventListener('click', this.clickListener);
        this.addEventListener('submit', this.submitListener);

        // create a state object for the current page with out special properties
        window.history.replaceState({ url: document.location.href, title: document.title, method: HttpVerbs.GET }, '');

        const currentUrl = new URL(document.location.href);
        this.currentUrlForHistory = currentUrl.pathname;
        this.currentMethodForHistory = HttpVerbs.GET;
        this.origin = currentUrl.origin;
    }

    /**
     * Removes event listeners.
     */
    disconnectedCallback() {
        window.removeEventListener('popstate', this.popStateListener);
        this.removeEventListener('click', this.clickListener);
        this.removeEventListener('submit', this.submitListener);
    }

    /**
     * Listens for back/forward navigation events and updates page accordingly.
     * @param {PopStateEvent} event PopState event
     */
    async onPopState(event) {
        if (event.state !== null) {
            // stop if url or method is missing from state
            if (!(event.state.url && event.state.method && URL.canParse(event.state.url, this.origin))) {
                return;
            }

            // If there is a state object, handle it as a page load.
            await this.requestPage(new URL(event.state.url, this.origin), event.state.method, false);
        }
    }

    /**
     * Link click listener.
     * @param {Event} event Click event
     */
    async onClickListener(event) {
        /** @type {HTMLLinkElement} */
        // @ts-ignore HTMLLinkElement is correct type but js can't cast
        let target = event.target; // || event.srcElement;
        if (target.nodeName !== 'A') {
            // @ts-ignore HTMLLinkElement is correct type but js can't cast
            target = target.closest('a');
        }

        // Ignore clicks unless its a link
        if (!(target && target.nodeName === 'A')) {
            return;
        }

        // Ignore clicks if link has the exclude class
        if (this.excludeAttribute && target.hasAttribute(this.excludeAttribute)) {
            return;
        }

        if (!URL.canParse(target.href)) {
            return;
        }

        // Ignore external links
        const url = new URL(target.href);
        if (url.origin !== this.origin) {
            return;
        }

        // Ignore anchors on the same page
        if (url.pathname === document.location.pathname && url.hash.length > 0) {
            return;
        }

        // Skip link if file type is within ignored types array
        if (this.ignoreFileTypes.indexOf(url.pathname.split('.').pop().toLowerCase()) !== -1) {
            return;
        }

        // Allow middle click (pages in new windows)
        // @todo how should i fix this?
        if (event.which > 1 || event.metaKey || event.ctrlKey) {
            return;
        }

        // Don't fire normal event
        event.preventDefault();

        // Take no action if we are already on said page and reload isn't allowed
        if (document.location.href === target.href && !target.hasAttribute('data-pjax-reload')) {
            return;
        }

        let method = target.dataset.pjaxMethod;
        if (!(method && method.toUpperCase() in HttpVerbs)) {
            method = HttpVerbs.GET;
        }

        // @todo shouldn't have to worry about confirmation, but may want to handle form changes still
        // Check for confirmation or form changes before loading content.
        // if (target.getAttribute('data-confirm')) {
        //    Alertify.dismissAll();
        //    Alertify.confirm(target.getAttribute('data-confirm'), pjax.handle.bind(null, options), function(e) { e.target.focus(); });
        // } else if (target.getAttribute('data-prompt')) {
        //    Alertify.dismissAll();
        //    Alertify.prompt(target.getAttribute('data-prompt'), function(promptValue) {
        //        if (!promptValue) {
        //            Alertify.error(pjax.errorMessagePrompt);
        //            return false;
        //        }
        //        options.url += ((!/[?&]/.test(options.url)) ? '?prompt' : '&prompt') + '=' + encodeURIComponent(promptValue);
        //        pjax.invoke(options);
        //    });
        // } else {
        //    var form = $.get('form.has-changes');
        //    if (form)
        //        Alertify.confirm(form.getAttribute('data-confirm'), pjax.handle.bind(null, options), function(e) { e.target.focus(); });
        //    else
        //        pjax.handle(options);
        // }

        await this.requestPage(url, method, true);
    }

    /**
     * Form change listener.
     * @todo might want to implement this later still
     */
    // $.on(document, 'change', function(event) {
    //    var target = event.target || event.srcElement;
    //    // Ignore change unless its form input
    //    if (['INPUT', 'SELECT', 'TEXTAREA'].indexOf(target.nodeName) === -1)
    //        return;
    //    $.addClass($.closest('FORM', target), 'has-changes');
    // });

    /**
     * Form submit listener.
     * @param {Event} event Submit event.
     */
    async onSubmitListener(event) {
        /** @type {HTMLFormElement} */
        // @ts-ignore HTMLFormElement is correct type but js can't cast
        // @todo do i need to replace srcElement somehow?
        let target = event.target; // || event.srcElement;
        if (target.nodeName !== 'FORM') {
            target = target.closest('form');
        }

        // Ignore submit unless its a form
        if (!(target && target.nodeName === 'FORM')) {
            return;
        }

        // Ignore submit if the form has the exclude attribute
        if (this.excludeAttribute && target.hasAttribute(this.excludeAttribute)) {
            return;
        }

        if (!URL.canParse(target.action)) {
            return;
        }

        // Ignore submit if the protocol or host don't match
        const url = new URL(target.action);
        if (url.protocol !== document.location.protocol || url.host !== document.location.host) {
            return;
        }

        // Don't fire normal event
        event.preventDefault();

        // handle the submission
        await this.submitForm(target);
    }

    /**
     * Performs request to load content from server and handles the result.
     * @param {URL} url Url to request.
     * @param {string} method Http method to use for request.
     * @param {boolean} updateHistory Update browser history if true.
     */
    async requestPage(url, method, updateHistory) {
        this.showLoadingIndicator();

        // keep browser from caching requests by tacking milliseconds to end of url
        url.searchParams.set('_t', `${Date.now()}`);

        try {
            const response = await ky(url, {
                method: method || 'GET',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'X-PJax': 'true',
                    'X-PJax-Version': this.version,
                },
            });

            await this.processResponse(url, method, updateHistory, response);
        } catch (error) {
            await this.handleResponseError(error);
        } finally {
            this.hideLoadingIndicator();
        }
    }

    /**
     * Performs form submission to server and handles the result.
     * @param {HTMLFormElement} form
     */
    async submitForm(form) {
        this.showLoadingIndicator();

        const url = new URL(form.action);

        let method = form.dataset.pjaxMethod;
        if (!(method && method.toUpperCase() in HttpVerbs)) {
            method = form.method;
        }
        if (!(method && method.toUpperCase() in HttpVerbs)) {
            method = HttpVerbs.POST;
        }

        try {
            const response = await ky(url, {
                method,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'X-PJax': 'true',
                    'X-PJax-Version': this.version,
                },
                body: new FormData(form),
            });

            await this.processResponse(url, method, true, response);
        } catch (error) {
            await this.handleResponseError(error);
        } finally {
            this.hideLoadingIndicator();
        }
    }

    /**
     * Process a response from the server.
     * @param {URL} requestUrl Requested url.
     * @param {string} requestMethod Request method.
     * @param {boolean} updateHistory Update browser history if true.
     * @param {Response} response Fetch response.
     */
    async processResponse(requestUrl, requestMethod, updateHistory, response) {
        const body = await getResponseBody(response);

        // reload the page if the refresh header was included
        if (response.headers.has('x-pjax-refresh')) {
            document.location.href = requestUrl.href;
            return;
        }

        // make sure request is successful and response is HTML
        if (response.ok && !isJson(response)) {
            if (updateHistory) {
                const newUrl = response.headers.has('x-pjax-push-url') ? response.headers.get('x-pjax-push-url') : requestUrl.pathname;
                const newMethod = (response.headers.has('x-pjax-push-method') ? response.headers.get('x-pjax-push-method') : requestMethod).toLowerCase();

                // @todo this isn't building history correctly for deleted
                if (this.currentUrlForHistory !== newUrl || this.currentMethodForHistory !== newMethod) {
                    // don't add page to history unless the url or method changed
                    // this makes sure requests like deletes don't create duplicate history
                    this.currentUrlForHistory = newUrl;
                    this.currentMethodForHistory = newMethod;

                    // push the page into the history
                    window.history.pushState({
                        url: this.currentUrlForHistory, title: document.title, method: this.currentMethodForHistory,
                    }, '', this.currentUrlForHistory);
                }
            }

            // Update the DOM with the new content
            const targetElement = this.target ? document.querySelector(this.target) : this;
            if (targetElement) {
                targetElement.innerHTML = body;
            }

            // set the document title if title header exists
            if (response.headers.has('x-pjax-title')) {
                document.title = response.headers.get('x-pjax-title');
            }

            // @todo should this scroll to 0,0 or use this.getBoundingClientRect().top/left
            window.scrollTo(0, 0);
        } else {
            // @todo implement error handler
            // error response should be json
            // dialog.alert(body?.error ?? body.message);
        }
    }

    /**
     * Shows the loading indicator.
     */
    showLoadingIndicator() {
        const element = this.loadingIndicator ? document.querySelector(this.loadingIndicator) : undefined;
        if (element) {
            element.classList.add('pjax-request');
        }
    }

    /**
     * Hides the loading indicator.
     */
    hideLoadingIndicator() {
        const element = this.loadingIndicator ? document.querySelector(this.loadingIndicator) : undefined;
        if (element) {
            element.classList.remove('pjax-request');
        }
    }

    /**
     * Handle response errors.
     * @param {HTTPError} error
     */
    async handleResponseError(error) {
        // @todo error handling needs more testing
        if (error.response?.status && [400, 401, 402, 403].indexOf(error.response.status) > -1) {
            // try to find a safe place to redirect this user to since we don't know what went wrong
            const locationHeader = error.response.headers.get('location');
            if (locationHeader) {
                window.location.href = locationHeader;
            } else {
                window.location.reload();
            }
        } else {
            // @todo how do i display errors to the user?  maybe use a nilla-info?
            // dialog.alert((await error.response?.text()) ?? 'An unhandled error occurred.');
        }
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-pjax', PJax);
}

export default PJax;
