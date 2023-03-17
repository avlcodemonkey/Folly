import htmx from './htmx.org/htmx.js';
import Alpine from './alpinejs/index.js';
import table from './alpine-table.js';
import { dialog } from './dialog/index.js';

window.htmx = htmx;
window.Alpine = Alpine;

document.getElementById('sidebar-button').addEventListener('click', () => {
    document.body.classList.toggle('sidebar-toggled');
});

// listen for updates to alpine-table and process the table content
document.body.addEventListener('alpine-table-updated', (e) => {
    if (!e?.target) {
        return;
    }

    window.htmx.process(e.target);
});

document.body.addEventListener('htmx:confirm', async (e) => {
    const elt = e.detail.elt;
    if (elt.hasAttribute('hx-confirm-content')) {
        e.preventDefault();
        const result = await dialog.confirm(elt.getAttribute('hx-confirm-content'), {
            okText: elt.getAttribute('hx-confirm-ok') ?? 'Okay',
            cancelText: elt.getAttribute('hx-confirm-cancel') ?? 'Cancel',
            focus: 'cancel'
        });
        if (result) {
            e.detail.issueRequest();
        }
    }
});

Alpine.data('table', table);

Alpine.start();
