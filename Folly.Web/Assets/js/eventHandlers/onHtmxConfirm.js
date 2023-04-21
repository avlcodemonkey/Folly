/**
 * Run `npm run build` in the vendor/dialog folder first to create this file.
 * @todo find a non-TS dialog library or someother way to avoid this issue
 */
import { dialog } from '../../vendor/dialog/dist/index';

/**
 * Add custom behavior to htmx:confirm event.
 * @param {CustomEvent} e
 */
const onHtmxConfirm = async (e) => {
    const { elt } = e.detail;

    if (elt.hasAttribute('hx-confirm-content')) {
        e.preventDefault();
        const result = await dialog.confirm(elt.getAttribute('hx-confirm-content'), {
            okText: elt.getAttribute('hx-confirm-ok') ?? 'Okay',
            cancelText: elt.getAttribute('hx-confirm-cancel') ?? 'Cancel',
            focus: 'cancel',
        });
        if (result) {
            e.detail.issueRequest();
        }
    } else if (elt.hasAttribute('hx-alert-content')) {
        e.preventDefault();
        await dialog.alert(elt.getAttribute('hx-alert-content'));
        elt.focus();
    }
};

export default onHtmxConfirm;