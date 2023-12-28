/**
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

    /** @type {string} */
    content;

    /** @type {string} */
    ok;

    constructor() {
        super();

        this.content = this.getAttribute('data-content');
        this.ok = this.getAttribute('data-ok');
    }

    /**
     * Update the okay and content properties when their attributes on the element change.
     * @param {string} name Name of attribute that changed.
     * @param {string} oldValue Old value of the attribute.
     * @param {string} newValue New value of the attribute.
     */
    baseAttributeChangedCallback(name, oldValue, newValue) {
        if (name === 'data-content' && oldValue !== newValue) {
            this.content = newValue;
        }
        if (name === 'data-ok' && oldValue !== newValue) {
            this.ok = newValue;
        }
    }

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

    // eslint-disable-next-line class-methods-use-this
    makeDialogHtml() {
        return '';
    }

    /**
     * Create a dialog element from an HTML string.
     * @returns {HTMLDialogElement}
     */
    createDialog() {
        const div = document.createElement('div');
        div.innerHTML = this.makeDialogHtml();
        // @ts-ignore HTMLDialogElement is the correct type but VS can't infer that
        return div.firstChild;
    }
}

export default BaseDialog;
