/**
 * Extend the HTMLElement class to provide common functionality for custom components.
 */
class BaseComponent extends HTMLElement {
    /**
     * Stores commonly queried DOM elements to improve performance.
     * @type {Node[]}
     */
    elementCache = [];

    /**
     * Prefix for finding elements via data attributes, and caching elements in memory.
     * @type {string}
     */
    elementPrefix = undefined;

    /**
     * Initialize the component.
     */
    constructor() {
        super();

        this.elementCache = [];
    }

    /**
     * Clean up when the component is removed from the document.
     * Don't want to keep references to any elements that have been removed.
     */
    disconnectedCallback() {
        this.elementCache = [];
    }

    /**
     * Gets the specified DOM element from cache, or finds using querySelector and add to cache.
     * Will match element with attribute `data-{elementPrefix}-{elementKey}` or `data-{elementKey}` if prefix is empty.
     * @param {string} elementKey Key for the element to find.
     * @returns {HTMLElement} Element to find.
     */
    getElement(elementKey) {
        const key = this.elementPrefix ? `${this.elementPrefix}-${elementKey}` : elementKey;
        if (!this.elementCache[key]) {
            this.elementCache[key] = this.querySelector(`[data-${key}]`);
        }
        return this.elementCache[key];
    }
}

export default BaseComponent;
