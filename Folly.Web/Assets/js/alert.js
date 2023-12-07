// @ts-check

/**
 * @typedef TrackedListener
 * @type {object}
 * @property {string} type Event type.
 * @property {EventListener} listener Callback function for event.
 */

/**
 * Tracks added event listeners so they can be cleaned up.
 * @type { TrackedListener[] }
 */
const documentEventListeners = [];

/**
 * Add event tracked listener to the document.
 * @param {string} type Event type
 * @param {EventListener} listener Callback function
 */
const addDocumentEventListener = (type, listener) => {
    document.addEventListener(type, listener);
    documentEventListeners.push({ type, listener });
};

/**
 * Remove all tracked document event listeners.
 */
const removeDocumentEventListeners = () => {
    let eventListenerReference;
    // eslint-disable-next-line no-cond-assign
    while ((eventListenerReference = documentEventListeners.shift())) {
        document.removeEventListener(eventListenerReference.type, eventListenerReference.listener);
        eventListenerReference = undefined;
    }
};

/**
 * Traps focus within the dialog element.
 * @param {HTMLElement} dialog Dialog to trab focus for.
 */
const registerTabTrap = (dialog) => {
    /** @type {HTMLElement[]} */
    const tabbableElements = Array.from(dialog.querySelectorAll('a, button, input'))
        .filter((tabbableElement) => {
            const computedStyle = window.getComputedStyle(tabbableElement);
            if (computedStyle.display === 'none' || computedStyle.visibility === 'hidden') {
                return false;
            }
            if (['button', 'input'].includes(tabbableElement.tagName) && tabbableElement.disabled === true) {
                return false;
            }
            return true;
        });

    if (tabbableElements.length === 0) {
        return;
    }

    const firstTabbableElement = tabbableElements[0];
    const lastTabbableElement = tabbableElements[tabbableElements.length - 1];

    addDocumentEventListener('focusin', (event) => {
        if (!dialog.contains(event.target)) {
            event.preventDefault();
            firstTabbableElement.focus();
        }
    });

    firstTabbableElement.addEventListener('keydown', (event) => {
        if (event.key === 'Tab' && event.shiftKey) {
            event.preventDefault();
            lastTabbableElement.focus();
        }
    });

    lastTabbableElement.addEventListener('keydown', (event) => {
        if (event.key === 'Tab' && !event.shiftKey) {
            event.preventDefault();
            firstTabbableElement.focus();
        }
    });
};

const createElement = (htmlString) => {
    const div = document.createElement('div');
    div.innerHTML = htmlString.trim();
    return div.firstChild;
};

// Extend the HTMLElement class to create the web component
class Alert extends HTMLElement {
    /**
     * The class constructor object
     */
    constructor() {
        // Always call super first in constructor
        super();

        const dialogHtml = `<dialog id="alert-dialog" hx-disable>
            <p id="alert-dialog-content" class="p-2"></p>
            <form method="dialog" class="ml-2">
                <button class="button success" value="ok" id="alert-dialog-ok" autofocus="">Okay</button>
            </form>
        </dialog>`;

        /** @type {HTMLDialogElement} */
        const dialog = document.getElementById('alert-dialog');
        if (!dialog) {
            document.body.appendChild(createElement(dialogHtml));
        }

        console.log('Constructed', this);
    }

    /**
     * Runs each time the element is appended to or moved in the DOM
     */
    connectedCallback() {
        this.querySelectorAll('[hx-alert-content]').forEach((x) => {
            x.addEventListener('click', this.onClick);
        });

        console.log('connected!', this);
    }

    onClick(event) {
        event.preventDefault();

        /** @type {HTMLDialogElement} */
        const dialog = document.getElementById('alert-dialog');

        const content = document.getElementById('alert-dialog-content');
        const okBtn = document.getElementById('alert-dialog-ok');

        if (!(dialog && content && okBtn)) {
            return;
        }

        content.innerHTML = this.getAttribute('hx-alert-content');
        okBtn.innerHTML = this.getAttribute('hx-alert-ok');

        const closeHandler = () => {
            removeDocumentEventListeners();
            dialog.removeEventListener('close', closeHandler);
            this.focus();
        };

        registerTabTrap(dialog);
        dialog.addEventListener('close', closeHandler);

        dialog.showModal();
    }

    /**
     * Runs when the element is removed from the DOM
     */
    disconnectedCallback() {
        this.querySelectorAll('[hx-alert-content]').forEach((x) => {
            x.removeEventListener('click', this.onClick);
        });

        console.log('disconnected', this);
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('fw-alert', Alert);
}

export default Alert;
