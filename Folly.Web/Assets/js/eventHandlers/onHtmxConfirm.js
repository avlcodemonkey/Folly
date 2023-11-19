/**
 * Add custom behavior to htmx:confirm event to enable alert/confirm dialogs.
 * @param {CustomEvent} e
 */
const onHtmxConfirm = async (e) => {
    const { elt } = e.detail;

    if (elt.hasAttribute('hx-confirm-content')) {
        e.preventDefault();

        const dialog = document.getElementById('confirm-dialog')/* as HTMLDialogElement*/;
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
            dialog.removeEventListener('close', closeHandler);
            if (dialog.returnValue === 'ok') {
                e.detail.issueRequest();
            }
        };
        dialog.addEventListener('close', closeHandler);

        dialog.showModal();
    } else if (elt.hasAttribute('hx-alert-content')) {
        e.preventDefault();

        const dialog = document.getElementById('alert-dialog')/* as HTMLDialogElement*/;
        const content = document.getElementById('alert-dialog-content');
        const okBtn = document.getElementById('alert-dialog-ok');
        if (!(dialog && content && okBtn)) {
            return;
        }

        content.innerHTML = elt.getAttribute('hx-alert-content');
        okBtn.innerHTML = elt.getAttribute('hx-alert-ok');

        const closeHandler = () => {
            dialog.removeEventListener('close', closeHandler);
            elt.focus();
        };
        dialog.addEventListener('close', closeHandler);

        dialog.showModal();
    }
};

export default onHtmxConfirm;
