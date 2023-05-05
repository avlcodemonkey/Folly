/**
 * Add custom behavior to htmx:confirm event.
 * @param {CustomEvent} e
 */
const onHtmxConfirm = async (e: CustomEvent) => {
    const { elt } = e.detail;

    console.log(elt);

    if (elt.hasAttribute('hx-confirm-content')) {
        e.preventDefault();
        /*
        const result = await dialog.confirm(elt.getAttribute('hx-confirm-content'), {
            okText: elt.getAttribute('hx-confirm-ok') ?? 'Okay',
            cancelText: elt.getAttribute('hx-confirm-cancel') ?? 'Cancel',
            focus: 'cancel',
        });
        if (result) {
            e.detail.issueRequest();
        }
        */
    } else if (elt.hasAttribute('hx-alert-content')) {
        e.preventDefault();
        /*
        await dialog.alert(elt.getAttribute('hx-alert-content'));
        */
        elt.focus();
    }
};

export default onHtmxConfirm;
