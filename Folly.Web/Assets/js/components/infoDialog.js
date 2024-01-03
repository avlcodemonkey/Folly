import BaseDialog from './baseDialog';

/**
 * Extend the custom dialog class to create the alert web component.
 */
class InfoDialog extends BaseDialog {
    static observedAttributes = ['data-content', 'data-ok'];

    constructor() {
        super();

        this.addEventListener('click', this);
    }

    makeDialogHtml() {
        return `<dialog hx-disable>
            <p class="p-2">${this.content}</p>
            <form method="dialog" class="ml-2">
                <button class="button success" value="ok" autofocus>${this.ok}</button>
            </form>
        </dialog>`;
    }

    /**
     * Update the okay and content properties when their attributes on the element change.
     * @param {string} name Name of attribute that changed.
     * @param {string} oldValue Old value of the attribute.
     * @param {string} newValue New value of the attribute.
     */
    attributeChangedCallback(name, oldValue, newValue) {
        this.baseAttributeChangedCallback(name, oldValue, newValue);
    }

    /**
     * Open alert dialog when user clicks on the element.
     * @param {Event} event Click event that triggers the dialog.
     */
    handleEvent(event) {
        if (!(this.content && this.ok)) {
            return;
        }

        event.preventDefault();

        if (!this.dialog) {
            this.dialog = this.createDialog();
            document.body.appendChild(this.dialog);
        }

        const closeHandler = () => {
            this.removeDocumentEventListeners();
            this.dialog.removeEventListener('close', closeHandler);
            this.dialog.remove();
            this.dialog = undefined;
            this.focus();
        };

        this.registerTabTrap();
        this.dialog.addEventListener('close', closeHandler);
        this.dialog.showModal();
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-info', InfoDialog);
}

export default InfoDialog;
