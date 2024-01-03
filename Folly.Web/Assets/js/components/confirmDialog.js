import BaseDialog from './baseDialog';

/**
 * @typedef {import("./confirmClickEvent").default} ConfirmClickEvent
 */

/**
 * Extend the custom dialog class to create the confirm web component.
 */
class ConfirmDialog extends BaseDialog {
    static observedAttributes = ['data-content', 'data-ok', 'data-cancel'];

    /** @type {string} */
    cancel;

    constructor() {
        super();

        this.cancel = this.getAttribute('data-cancel');
        this.addEventListener('click', this);
    }

    makeDialogHtml() {
        return `<dialog hx-disable>
            <p class="p-2">${this.content}</p>
            <form method="dialog" class="ml-2">
                <button class="button primary" value="cancel" autofocus>${this.cancel}</button>
                <button class="button dark" value="ok">${this.ok}</button>
            </form>
        </dialog>`;
    }

    /**
     * Update the okay, content, and cancel properties when their attributes on the element change.
     * @param {string} name Name of attribute that changed.
     * @param {string} oldValue Old value of the attribute.
     * @param {string} newValue New value of the attribute.
     */
    attributeChangedCallback(name, oldValue, newValue) {
        this.baseAttributeChangedCallback(name, oldValue, newValue);

        if (name === 'data-cancel' && oldValue !== newValue) {
            this.cancel = newValue;
        }
    }

    /**
     * Open alert dialog when user clicks on the element.
     * @param {ConfirmClickEvent} event Click event that triggers the dialog.
     */
    handleEvent(event) {
        if (event.isConfirmed) {
            return;
        }

        if (!(this.content && this.ok && this.cancel)) {
            return;
        }

        event.preventDefault();
        event.stopImmediatePropagation();

        if (!this.dialog) {
            this.dialog = this.createDialog();
            document.body.appendChild(this.dialog);
        }

        const closeHandler = () => {
            const { returnValue } = this.dialog;
            this.removeDocumentEventListeners();
            this.dialog.removeEventListener('close', closeHandler);
            this.dialog.remove();
            this.dialog = undefined;

            if (returnValue === 'ok') {
                // eslint-disable-next-line no-param-reassign
                event.isConfirmed = true;
                this.dispatchEvent(event);
            }
        };

        this.registerTabTrap();
        this.dialog.addEventListener('close', closeHandler);
        this.dialog.showModal();
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-confirm', ConfirmDialog);
}

export default ConfirmDialog;
