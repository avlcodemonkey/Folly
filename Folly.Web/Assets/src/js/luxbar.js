// make sure that clicking on a luxbar menu item closes the menu when on mobile
document.querySelectorAll('.luxbar-menu a:not([href="#"])').forEach((x) => {
    x.addEventListener('click', () => {
        const checkbox = /** @type {HTMLInputElement} */ (document.getElementById('luxbar-checkbox'));

        if (checkbox) {
            checkbox.checked = false;
        }
    });
});
