import "htmx.org";
import "./tiny-components.js";

window.htmx = require('htmx.org');

document.getElementById('sidebar-button').addEventListener('click', () => {
    document.body.classList.toggle('sidebar-toggled');
});

// listen for updates to lit-table and process the table content
document.addEventListener('lit-table-updated', (e) => {
    if (!(e?.target?.shadowRoot && e?.detail)) {
        return;
    }

    const target = e.target.shadowRoot.getElementById(e.detail);
    if (target) {
        window.htmx.process(target);
    }
});
