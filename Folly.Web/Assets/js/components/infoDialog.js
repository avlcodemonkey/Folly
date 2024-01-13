import BaseDialog from './baseDialog';

/**
 * Extend the custom dialog class to create the alert web component.
 */
class InfoDialog extends BaseDialog {
    constructor() {
        super();

        // add event listener for all links and buttons inside the component
        this.querySelectorAll('a, button').forEach((x) => x.addEventListener('click', this));
    }

    /**
     * Open alert dialog when user clicks on the element.
     * @param {Event} event Click event that triggers the dialog.
     */
    handleEvent(event) {
        const target = this.findTarget(event);
        const { dialogContent, dialogOk } = target.dataset;
        if (!(dialogContent && dialogOk)) {
            return;
        }

        event.preventDefault();
        event.stopImmediatePropagation();

        this.showDialog(dialogContent, dialogOk);
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-info', InfoDialog);
}

export default InfoDialog;
