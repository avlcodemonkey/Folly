import {
    beforeEach, describe, expect, it,
} from 'vitest';
import { tick, isRendered } from '../utils';
import '../../src/js/components/alert';

const textContent = 'Alert test';

const dismissableAlertHtml = `
    <nilla-alert>
        <div>
            <span>${textContent}</span>
            <span><button data-dismiss>close</button></span>
            <span><button data-do-nothing>do nothing</button></span>
        </div>
    </nilla-alert>
`;

const notDismissableAlertHtml = `
    <nilla-alert>
        <div>
            <span>${textContent}</span>
            <span><button data-do-nothing>do nothing</button></span>
        </div>
    </nilla-alert>
`;

/**
 * Gets the alert element.
 * @returns {HTMLElement | null | undefined} Alert element
 */
function getAlert() {
    return document.body.querySelector('nilla-alert');
}

/**
 * Gets the button to dismiss the alert.
 * @returns {HTMLElement | null | undefined} Dismiss button
 */
function getDismissButton() {
    return getAlert()?.querySelector('[data-dismiss]');
}
/**
 * Gets the button that should do nothing.
 * @returns {HTMLElement | null | undefined} Dismiss button
 */
function getDoNothingButton() {
    return getAlert()?.querySelector('[data-do-nothing]');
}

describe('dismissable alert', async () => {
    beforeEach(async () => {
        document.body.innerHTML = dismissableAlertHtml;
        await isRendered(getAlert);
    });

    it('should have test text', async () => {
        const node = getAlert();
        expect(node.innerHTML).toContain(textContent);
    });

    it('should hide on dismiss button click', async () => {
        getDismissButton()?.click();
        await tick();
        expect(getAlert()?.classList ?? []).toContain('is-hidden');
    });

    it('should not hide on do nothing button click', async () => {
        getDoNothingButton()?.click();
        await tick();
        expect(getAlert()?.classList ?? []).not.toContain('is-hidden');
    });
});

describe('not dismissable alert', async () => {
    beforeEach(async () => {
        document.body.innerHTML = notDismissableAlertHtml;
        await isRendered(getAlert);
    });

    it('should have test text', async () => {
        const node = getAlert();
        expect(node.innerHTML).toContain(textContent);
    });

    it('should have no dismiss button', async () => {
        expect(getDismissButton()).toBeFalsy();
    });

    it('should not hide on do nothing button click', async () => {
        getDoNothingButton()?.click();
        await tick();
        expect(getAlert()?.classList ?? []).not.toContain('is-hidden');
    });
});
