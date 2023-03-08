import "htmx.org/dist/htmx.js";
import "./tiny-components.js";

document.getElementById('sidebar-button').addEventListener('click', () => {
    document.body.classList.toggle('sidebar-toggled');
});
