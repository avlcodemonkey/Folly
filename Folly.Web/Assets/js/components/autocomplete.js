import autocomplete, { PreventSubmit } from 'autocompleter';
import ky from 'ky';
import FetchError from './fetchError';

/**
 * Web component for an input autocomplete.
 */
class Autocomplete extends HTMLElement {
    constructor() {
        super();

        const { srcUrl, emptyMessage } = this.dataset;
        const displayInput = this.querySelector('[data-autocomplete-display]');
        const valueInput = this.querySelector('[data-autocomplete-value]');
        if (!(srcUrl && displayInput && valueInput)) {
            return;
        }

        autocomplete({
            minLength: 1,
            preventSubmit: 1, // PreventSubmit.Always
            emptyMsg: emptyMessage,
            input: displayInput,
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
            onSelect(item) {
                valueInput.value = item.value;
                displayInput.value = item.label;
            },
        });

        // clear out the value in the hidden input when changing the search value. it'll get re-set in onSelect
        // @todo not working as expected - it clears out the value when you tab to the next field
        displayInput.addEventListener('keyup', (e) => {
            console.log(e);
            valueInput.value = '';
        });
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-autocomplete', Autocomplete);
}

export default Autocomplete;
