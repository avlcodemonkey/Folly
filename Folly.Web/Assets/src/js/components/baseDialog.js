import mustache from 'mustache';
import BaseComponent from './baseComponent';

/**
 * Extend the base component to provide common functionality for custom dialogs.
 */
class BaseDialog extends BaseComponent {
    /** @type {HTMLDialogElement} */
    dialog;

    /**
     * Bound focusin event listener.
     * @type {(event: any) => void|undefined}
     */
    focusInListener;

    /**
     * Clean up when the component is removed from the document.
     */
    disconnectedCallback() {
        // listener should have been removed when the dialog closed, but better safe
        if (this.focusInListener) {
            document.removeEventListener('focusin', this.focusInListener);
            this.focusInListener = undefined;
        }

        super.disconnectedCallback();
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

        this.focusInListener = (event) => {
            if (!this.dialog.contains(event.target)) {
                event.preventDefault();
                firstTabbableElement.focus();
            }
        };
        document.addEventListener('focusin', this.focusInListener);

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

        // manually close the dialog when clicking a button instead of using `form method="dialog"` so this is testable
        this.dialog.querySelector('[data-dialog-footer]').querySelectorAll('button').forEach((x) => x.addEventListener('click', () => {
            this.dialog.close(x.value);
        }));

        this.dialog.addEventListener('close', () => {
            const { returnValue } = this.dialog;

            if (this.focusInListener) {
                document.removeEventListener('focusin', this.focusInListener);
                this.focusInListener = undefined;
            }
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
     * @param {string} content HTML to display in the body of the dialog.
     * @param {string} ok Text label for the ok button.
     * @param {string|undefined} cancel Text label for the cancel button
     * @returns {string} HTML for the dialog.
     */
    makeDialogHtml(content, ok, cancel) {
        return `<dialog>
            <p class="p-2">${content}</p>
            <div class="ml-2" data-dialog-footer>${this.makeDialogButtons(ok, cancel)}</div>
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
            return `<button type="button" class="button primary" value="cancel" autofocus>${mustache.escape(cancel)}</button>
                <button type="button" class="button dark" value="ok">${mustache.escape(ok)}</button>`;
        }
        return `<button type="button" class="button success" value="ok" autofocus>${mustache.escape(ok)}</button>`;
    }
}

export default BaseDialog;
