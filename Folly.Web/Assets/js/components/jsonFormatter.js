import { format } from 'fecha';

// https://stackoverflow.com/a/7220510
function syntaxHighlight(json) {
    let formattedJson = json;
    if (typeof formattedJson !== 'string') {
        formattedJson = JSON.stringify(json, undefined, 2);
    }

    formattedJson = formattedJson.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

    return formattedJson.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, (match) => {
        let cls = 'number';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) {
                cls = 'key';
            } else {
                cls = 'string';
            }
        } else if (/true|false/.test(match)) {
            cls = 'boolean';
        } else if (/null/.test(match)) {
            cls = 'null';
        }
        return `<span class="${cls}">${match}</span>`;
    });
}

/**
 * Web component for displaying json in a pretty way.
 */
class JsonFormatter extends HTMLElement {
    /** @type {string} */
    format;

    constructor() {
        super();

        try {
            this.innerHTML = `<pre>${syntaxHighlight(this.textContent)}</pre>`;
        } catch { /* empty */ }
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-json', JsonFormatter);
}

export default JsonFormatter;
