import mustache from 'mustache';

/**
 * Wraps event listeners for tracking/removal.
 * @typedef TrackedListener
 * @type {object}
 * @property {string} type Event type.
 * @property {EventListener} listener Callback function for event.
 */

/**
 * Extend the HTMLElement class to create custom dialogs.
 */
class BaseDialog extends HTMLElement {
    /** @type {HTMLDialogElement} */
    dialog;

    /**
     * Tracks added event listeners so they can be cleaned up.
     * @type { TrackedListener[] }
     */
    documentEventListeners = [];

    /**
     * Add event tracked listener to the document.
     * @param {string} type Event type
     * @param {EventListener} listener Callback function
     */
    addDocumentEventListener(type, listener) {
        document.addEventListener(type, listener);
        this.documentEventListeners.push({ type, listener });
    }

    /**
     * Remove all tracked document event listeners.
     */
    removeDocumentEventListeners() {
        let eventListenerReference;
        // eslint-disable-next-line no-cond-assign
        while ((eventListenerReference = this.documentEventListeners.shift())) {
            document.removeEventListener(eventListenerReference.type, eventListenerReference.listener);
            eventListenerReference = undefined;
        }
    }

    /**
     * Traps focus within the dialog.
     */
    registerTabTrap() {
        /** @type {HTMLElement[]} */
        // @ts-ignore target type is HTMLElement but VS can't infer that
        const tabbableElements = Array.from(this.dialog.querySelectorAll('a, button, input'))
            .filter((tabbableElement) => {
                // @ts-ignore disabled doesn't exist on Element but does exist on button/input
                if (['button', 'input'].some((x) => x === tabbableElement.tagName) && tabbableElement.disabled === true) {
                    return false;
                }

                const computedStyle = window.getComputedStyle(tabbableElement);
                if (computedStyle.display === 'none' || computedStyle.visibility === 'hidden') {
                    return false;
                }

                return true;
            });

        if (tabbableElements.length === 0) {
            return;
        }

        const firstTabbableElement = tabbableElements[0];
        const lastTabbableElement = tabbableElements[tabbableElements.length - 1];

        this.addDocumentEventListener('focusin', (event) => {
            // @ts-ignore target type is Node but VS can't infer that
            if (!this.dialog.contains(event.target)) {
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
    }

    /**
     * Walk up the DOM tree to find the correct node with the data attributes needed.
     * @param {Event} event Click event that triggers the dialog.
     * @returns {HTMLElement} HTML element with the required data attributes.
     */
    findTarget(event) {
        let { target } = event;
        // @ts-ignore target will be an HTMLElement so dataset will exist
        while (!target.dataset.dialogContent && target !== this) {
            // @ts-ignore parentNode will be an HTMLElement
            target = target.parentNode;
        }
        // @ts-ignore target will be an HTMLElement
        return target;
    }

    /**
     * Create a dialog element from an HTML string.
     * @param {string} content Text to display in the body of the dialog.
     * @param {string} ok Label for the ok button.
     * @param {string|undefined} cancel Label for the cancel button.
     * @param {Function|undefined} onClose Function to run after dialog closes.
     * @returns {HTMLDialogElement} Dialog element.
     */
    showDialog(content, ok, cancel = undefined, onClose = undefined) {
        if (this.dialog) {
            return;
        }

        const div = document.createElement('div');
        div.innerHTML = this.makeDialogHtml(content, ok, cancel);
        // @ts-ignore HTMLDialogElement is the correct type but VS can't infer that
        this.dialog = div.firstChild;
        document.body.appendChild(this.dialog);

        this.registerTabTrap();
        this.dialog.addEventListener('close', () => {
            const { returnValue } = this.dialog;
            this.removeDocumentEventListeners();
            this.dialog.remove();
            this.dialog = undefined;

            if (onClose) {
                onClose(returnValue);
            }
        });
        this.dialog.showModal();
    }

    /**
     * Create the HTML for the dialog element.
     * @param {string} content Text to display in the body of the dialog.
     * @param {string} ok Label for the ok button.
     * @param {string|undefined} cancel Label for the cancel button
     * @returns {string} HTML for the dialog.
     */
    makeDialogHtml(content, ok, cancel) {
        return `<dialog hx-disable>
            <p class="p-2">${mustache.escape(content)}</p>
            <form method="dialog" class="ml-2">${this.makeDialogButtons(ok, cancel)}</form>
        </dialog>`;
    }

    /**
     * Create the HTML for the dialog button elements.
     * @param {string} ok Label for the ok button.
     * @param {string|undefined} cancel Label for the cancel button
     * @returns {string} HTML for the buttons.
     */
    // eslint-disable-next-line class-methods-use-this
    makeDialogButtons(ok, cancel) {
        if (cancel) {
            return `<button class="button primary" value="cancel" autofocus>${mustache.escape(cancel)}</button>
                <button class="button dark" value="ok">${mustache.escape(ok)}</button>`;
        }
        return `<button class="button success" value="ok" autofocus>${mustache.escape(ok)}</button>`;
    }
}

export default BaseDialog;
