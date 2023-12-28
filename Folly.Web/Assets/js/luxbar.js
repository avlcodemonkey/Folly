// @ts-check

// make sure that clicking on a luxbar menu item closes the menu when on mobile
document.querySelectorAll('.luxbar-menu a:not([href="#"])').forEach((x) => {
    x.addEventListener('click', () => {
        /** @type {HTMLInputElement} */
        // @ts-ignore HTMLInputElement is the correct type but VS can't infer that
        const checkbox = document.getElementById('luxbar-checkbox');

        if (checkbox) {
            checkbox.checked = false;
        }
    });
});
