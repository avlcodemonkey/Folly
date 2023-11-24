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

/**
 * Add custom behavior to htmx:confirm event to enable alert/confirm dialogs.
 * @param {CustomEvent} event
 */
const onHtmxConfirm = async (event) => {
    const { elt } = event.detail;

    if (elt.hasAttribute('hx-confirm-content')) {
        event.preventDefault();

        /** @type {HTMLDialogElement} */
        const dialog = document.getElementById('confirm-dialog');

        const content = document.getElementById('confirm-dialog-content');
        const okBtn = document.getElementById('confirm-dialog-ok');
        const cancelBtn = document.getElementById('confirm-dialog-cancel');

        if (!(dialog && content && okBtn && cancelBtn)) {
            return;
        }

        content.innerHTML = elt.getAttribute('hx-confirm-content');
        okBtn.innerHTML = elt.getAttribute('hx-confirm-ok');
        cancelBtn.innerHTML = elt.getAttribute('hx-confirm-cancel');

        const closeHandler = () => {
            removeDocumentEventListeners();
            dialog.removeEventListener('close', closeHandler);
            if (dialog.returnValue === 'ok') {
                event.detail.issueRequest();
            }
        };

        registerTabTrap(dialog);
        dialog.addEventListener('close', closeHandler);

        dialog.showModal();
    } else if (elt.hasAttribute('hx-alert-content')) {
        event.preventDefault();

        /** @type {HTMLDialogElement} */
        const dialog = document.getElementById('alert-dialog');

        const content = document.getElementById('alert-dialog-content');
        const okBtn = document.getElementById('alert-dialog-ok');

        if (!(dialog && content && okBtn)) {
            return;
        }

        content.innerHTML = elt.getAttribute('hx-alert-content');
        okBtn.innerHTML = elt.getAttribute('hx-alert-ok');

        const closeHandler = () => {
            removeDocumentEventListeners();
            dialog.removeEventListener('close', closeHandler);
            elt.focus();
        };

        registerTabTrap(dialog);
        dialog.addEventListener('close', closeHandler);

        dialog.showModal();
    }
};

export default onHtmxConfirm;
