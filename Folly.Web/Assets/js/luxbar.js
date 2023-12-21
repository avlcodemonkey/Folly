// @ts-check

// make sure that clicking on a luxbar menu item closes the menu when on mobile
document.querySelectorAll('.luxbar-menu a:not([href="#"])').forEach((x) => {
    x.addEventListener('click', () => {
        document.getElementById('luxbar-checkbox').checked = false;
    });
});
