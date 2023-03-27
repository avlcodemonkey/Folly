import * as htmx from './htmx.org/htmx';
import Alpine from './alpinejs/index';
import alpineTable from './alpine-table';
import { dialog } from './dialog/index';

document.getElementById('sidebar-button').addEventListener('click', () => {
    document.body.classList.toggle('sidebar-toggled');
});

// listen for updates to alpine-table and process the table content
document.body.addEventListener('alpine-table-updated', (e) => {
    if (!e?.target) {
        return;
    }

    htmx.process(e.target);
});

document.body.addEventListener('htmx:confirm', async (e) => {
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
});

Alpine.data('table', alpineTable);

Alpine.start();
