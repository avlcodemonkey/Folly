import autocomplete from 'autocompleter';
// @ts-ignore VS doesn't like this import but it builds fine
import ky from 'ky';
import FetchError from './fetchError';

/**
 * @typedef AutocompleteItem
 * @type {object}
 * @property {string} label Label to display for the item.
 * @property {string} value Value to use when selecting the item.
 */

/**
 * @typedef {import("autocompleter").AutocompleteResult} AutocompleteResult
 */

/**
 * @typedef {import("autocompleter").PreventSubmit} PreventSubmit
 */

/**
 * Web component for an input autocomplete.
 */
class Autocomplete extends HTMLElement {
    /** @type {AutocompleteResult} */
    autocompleter;

    constructor() {
        super();

        /**
         * Input shown to the user to interact with.
         * @type {HTMLInputElement}
         */
        const displayInput = this.querySelector('[data-autocomplete-display]');

        /**
         * Hidden input that stores the selected value from the automcomplete
         * @type {HTMLInputElement}
         */
        const valueInput = this.querySelector('[data-autocomplete-value]');

        const { srcUrl, emptyMessage } = this.dataset;
        if (!(srcUrl && displayInput && valueInput)) {
            return;
        }

        this.autocompleter = autocomplete({
            minLength: 1,
            preventSubmit: 2, // PreventSubmit.OnSelect
            emptyMsg: emptyMessage,
            input: displayInput,
            debounceWaitMs: 250,
            async fetch(query, update) {
                let suggestions = [];
                try {
                    const options = {
                        headers: { 'X-Requested-With': 'XMLHttpRequest' },
                        searchParams: new URLSearchParams([['query', query]]),
                    };

                    const json = await ky.get(srcUrl, options).json();
                    if (!(json && Array.isArray(json))) {
                        throw new FetchError(`Request to '${srcUrl}' returned invalid response.`);
                    }

                    suggestions = json ?? [];
                } catch {
                    suggestions = [];
                }
                update(suggestions);
            },
            onSelect(/** @type {AutocompleteItem} */ item) {
                valueInput.value = item.value;
                displayInput.value = item.label;
                displayInput.dataset.label = item.label;
            },
        });

        // clear out the value in the hidden input when changing the search value. it'll get re-set in onSelect
        displayInput.addEventListener('change', () => {
            if (displayInput.value !== displayInput.dataset.label) {
                displayInput.value = '';
                valueInput.value = '';
            }
        });
    }

    disconnectedCallback() {
        // this could cause trouble if we later start detaching/reattaching autocompletes from the DOM
        this.autocompleter?.destroy();
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-autocomplete', Autocomplete);
}

export default Autocomplete;
