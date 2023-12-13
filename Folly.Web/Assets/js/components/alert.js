// @ts-check

/**
 * Dismissable alert web component.
 */
class Alert extends HTMLElement {
    constructor() {
        super();

        this.querySelectorAll('.button-dismiss').forEach((btn) => {
            btn.addEventListener('click', this);
        });
    }

    /**
     * Hide the alert when the dismiss button is clicked.
     */
    handleEvent() {
        this.style.display = 'none';
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('fw-alert', Alert);
}

export default Alert;