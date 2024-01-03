import * as htmx from 'htmx.org';
// @ts-ignore VS doesn't like this import but it builds fine
import ky from 'ky';
import mustache from 'mustache';
import FetchError from './fetchError';

/**
 * Enum for table setting keys.
 * @readonly
 * @enum {string}
 */
const TableSetting = Object.freeze({
    CurrentPage: 'currentPage',
    PerPage: 'perPage',
    SearchQuery: 'searchQuery',
    Sort: 'sort',
});

/**
 * Enum for sorting order.
 * @readonly
 * @enum {string}
 */
const SortOrder = Object.freeze({
    Asc: 'asc',
    Desc: 'desc',
});

/**
 * @typedef IndexedRow
 * @type {object}
 * @property {number} _index Unique identifier for the row.
 */

/**
 * @typedef SortColumn
 * @type {object}
 * @property {string} property Name of property to sort on.
 * @property {SortOrder} sortOrder Order to sort this property.
 */

/**
 * Web component for rendering table data.
 */
class Table extends HTMLElement {
    /**
     * Unique identifier for this table.
     * @type {string}
     */
    key = '';

    /**
     * Url to request data from the server.
     * @type {string}
     */
    src = '';

    /**
     * Data fetched from the server.
     * @type {Array<IndexedRow>}
     */
    rows = [];

    /**
     * Data from the server filtered based on current display settings.
     * @type {Array<IndexedRow>}
     */
    filteredRows = [];

    /**
     * Sort settings for the table.
     * @type {Array<SortColumn>}
     */
    sortColumns = [];

    /**
     * Total number of rows based on current table filters.
     * @type {number}
     */
    filteredRowTotal = 0;

    /**
     * Current page number.
     * @type {number}
     */
    currentPage = 0;

    /**
     * Number of rows to display per page.
     * @type {number}
     */
    perPage = 10;

    /**
     * Total number of pages.
     * @type {number}
     */
    maxPage = 0;

    /**
     * String to filter data for.
     * @type {string}
     */
    searchQuery = '';

    /**
     * Tracks the timeout for re-filtering data.
     * @type {number}
     */
    debounceTimer = 0;

    /**
     * Indicates data is currently being fetched from the server if true.
     * @type {boolean}
     */
    loading = false;

    /**
     * Indicates an error occurred fetching data from the server if true.
     * @type {boolean}
     */
    error = false;

    constructor() {
        super();

        this.initializeTable();
    }

    /**
     * Initialize table by loading settings from sessionStorage, processing HTML to add event handlers, and fetching data from the server.
     */
    initializeTable() {
        this.key = this.getAttribute('data-key');
        this.src = this.getAttribute('data-src');

        // check sessionStorage for saved settings
        this.perPage = parseInt(this.fetchSetting(TableSetting.PerPage) ?? '10', 10);
        this.currentPage = parseInt(this.fetchSetting(TableSetting.CurrentPage) ?? '0', 10);
        this.searchQuery = this.fetchSetting(TableSetting.SearchQuery) ?? '';
        this.sortColumns = JSON.parse(this.fetchSetting(TableSetting.Sort) ?? '[]');

        this.setupHeader();
        this.setupFooter();
        this.setupStatus();
        this.setupSorting();

        this.fetchData();
    }

    /**
     * Sets the default value and adds event handler for the search input.
     */
    setupHeader() {
        /** @type {HTMLInputElement} */
        const searchInput = this.querySelector('.table-search-query');
        if (searchInput) {
            searchInput.value = this.searchQuery;
            searchInput.addEventListener('input', (/** @type {InputEvent} */ e) => this.onSearchQueryInput(e));
            searchInput.focus();
        }
    }

    /**
     * Add event handlers for the buttons for moving between pages.
     */
    setupFooter() {
        const firstPageButton = this.querySelector('.table-first-page-button');
        if (firstPageButton) {
            firstPageButton.addEventListener('click', () => this.onFirstPageClick());
        }
        const previousPageButton = this.querySelector('.table-previous-page-button');
        if (previousPageButton) {
            previousPageButton.addEventListener('click', () => this.onPreviousPageClick());
        }
        const nextPageButton = this.querySelector('.table-next-page-button');
        if (nextPageButton) {
            nextPageButton.addEventListener('click', () => this.onNextPageClick());
        }
        const lastPageButton = this.querySelector('.table-last-page-button');
        if (lastPageButton) {
            lastPageButton.addEventListener('click', () => this.onLastPageClick());
        }

        /** @type {HTMLSelectElement} */
        const tablePerPageInput = this.querySelector('.table-per-page');
        if (tablePerPageInput) {
            tablePerPageInput.value = `${this.perPage}`;
            tablePerPageInput.addEventListener('change', (/** @type {InputEvent} */ e) => this.onPerPageChange(e));
        }
    }

    /**
     * Add event handlers for table status functionality.
     */
    setupStatus() {
        const tableRetryButton = this.querySelector('.table-retry-button');
        if (tableRetryButton) {
            tableRetryButton.addEventListener('click', () => this.onRetryClick());
        }
    }

    /**
     * Add event handlers for table sorting functionality.
     */
    setupSorting() {
        this.querySelectorAll('th[data-property]').forEach((x) => {
            x.addEventListener('click', () => this.onSortClick(x));
        });
    }

    /**
     * Fetch data from the server at the URL specified in the `src` property.
     */
    async fetchData() {
        if (!this.src.length) {
            return;
        }

        this.loading = true;
        this.error = false;
        this.update();

        try {
            const json = await ky.get(this.src, { headers: { 'X-Requested-With': 'XMLHttpRequest' } }).json();
            if (!(json && Array.isArray(json))) {
                throw new FetchError(`Request to '${this.src}' returned invalid response.`);
            }
            this.rows = json.map((x, index) => ({ ...x, _index: index })) ?? [];
        } catch {
            this.rows = [];
            this.error = true;
        } finally {
            this.loading = false;
        }

        this.filterData();
    }

    /**
     * Updates the DOM based on the table properties.
     */
    update() {
        this.updateSearch();
        this.updateRowNumbers();
        this.updatePageButtons();
        this.updateStatus();
        this.updateSortHeaders();
        this.updateRows();
    }

    /**
     * Enable/disable search input.
     */
    updateSearch() {
        /** @type {HTMLInputElement} */
        const searchInput = this.querySelector('.table-search-query');
        if (searchInput) {
            searchInput.disabled = this.loading || this.error;
        }
    }
    /**
     * Updates the start, end, and total numbers.
     */
    updateRowNumbers() {
        const rowNumbers = this.querySelector('.table-row-numbers');
        if (!rowNumbers) {
            return;
        }
        rowNumbers.classList.toggle('is-hidden', this.loading || this.error);

        const startRowNumber = this.querySelector('.table-start-row-number');
        if (startRowNumber) {
            startRowNumber.textContent = `${this.startRowNumber}`;
        }
        const endRowNumber = this.querySelector('.table-end-row-number');
        if (endRowNumber) {
            endRowNumber.textContent = `${this.endRowNumber}`;
        }
        const filteredRowTotal = this.querySelector('.table-filtered-row-total');
        if (filteredRowTotal) {
            filteredRowTotal.textContent = `${this.filteredRowTotal}`;
        }
    }

    /**
     * Enable/disable paging buttons.
     */
    updatePageButtons() {
        const shouldDisable = this.loading || this.error;

        /** @type {HTMLButtonElement} */
        const firstPageButton = this.querySelector('.table-first-page-button');
        if (firstPageButton) {
            firstPageButton.disabled = shouldDisable || this.isFirstPage;
        }

        /** @type {HTMLButtonElement} */
        const previousPageButton = this.querySelector('.table-previous-page-button');
        if (previousPageButton) {
            previousPageButton.disabled = shouldDisable || this.isFirstPage;
        }

        /** @type {HTMLButtonElement} */
        const nextPageButton = this.querySelector('.table-next-page-button');
        if (nextPageButton) {
            nextPageButton.disabled = shouldDisable || this.isLastPage;
        }

        /** @type {HTMLButtonElement} */
        const lastPageButton = this.querySelector('.table-last-page-button');
        if (lastPageButton) {
            lastPageButton.disabled = shouldDisable || this.isLastPage;
        }

        /** @type {HTMLSelectElement} */
        const tablePerPageInput = this.querySelector('.table-per-page');
        if (tablePerPageInput) {
            tablePerPageInput.disabled = shouldDisable;
        }
    }

    /**
     * Shows/hides the table loading and error indicators.
     */
    updateStatus() {
        const isLoading = this.querySelector('.table-is-loading');
        if (isLoading) {
            isLoading.classList.toggle('is-hidden', !this.loading);
        }

        const hasError = this.querySelector('.table-has-error');
        if (hasError) {
            hasError.classList.toggle('is-hidden', !this.error);
        }

        const hasNoData = this.querySelector('.table-has-no-data');
        if (hasNoData) {
            hasNoData.classList.toggle('is-hidden', this.loading || this.error || this.filteredRowTotal !== 0);
        }
    }

    /**
     * Updates table headers to show correct sorting icons.
     */
    updateSortHeaders() {
        const sortAscTemplate = this.querySelector('.sort-asc-template');
        const sortDescTemplate = this.querySelector('.sort-desc-template');

        this.querySelectorAll('th[data-property]').forEach((th) => {
            const property = th.getAttribute('data-property');
            const sortOrder = this.sortOrder(property);
            const sortAsc = th.querySelector('.sort-asc');
            const sortDesc = th.querySelector('.sort-desc');

            // make sure the TH has the correct sort icon
            if (sortOrder === SortOrder.Asc) {
                if (sortDesc) {
                    sortDesc.remove();
                }
                if (!sortAsc) {
                    th.insertAdjacentHTML('beforeend', sortAscTemplate.innerHTML);
                }
            } else if (sortOrder === SortOrder.Desc) {
                if (sortAsc) {
                    sortAsc.remove();
                }
                if (!sortDesc) {
                    th.insertAdjacentHTML('beforeend', sortDescTemplate.innerHTML);
                }
            } else {
                if (sortAsc) {
                    sortAsc.remove();
                }
                if (sortDesc) {
                    sortDesc.remove();
                }
            }
        });
    }

    /**
     * Determines which type of sorting to use to display sort icon.
     * @param {string} property Property to find sort icon for.
     * @returns {SortOrder|undefined} Order of sorting for this property.
     */
    sortOrder(property) {
        const index = property ? this.sortColumns.findIndex((x) => x.property === property) : -1;
        if (index === -1) {
            return undefined;
        }
        return this.sortColumns[index].sortOrder;
    }

    /**
     * Removes existing table rows and renders new rows using the filtered data.
     */
    updateRows() {
        const existingRows = this.querySelectorAll('tbody tr:not(.table-status)');
        existingRows.forEach((x) => x.remove());

        const tbody = this.querySelector('tbody');
        if (!tbody) {
            throw new DOMException('Table body missing.');
        }
        const template = tbody.querySelector('template');
        if (!template) {
            throw new DOMException('Table row template missing.');
        }

        const html = this.filteredRows.map((row) => mustache.render(template.innerHTML, row)).join('\n');
        tbody.insertAdjacentHTML('beforeend', html);
    }

    /**
     * Load a value from session storage.
     * @param {string} name Key to store the value as.
     * @returns {string} Value from session storage if found, else null.
     */
    fetchSetting(name) {
        return sessionStorage.getItem(`${this.key}_${name}`);
    }

    /**
     * Saves a value to session storage.
     * @param {string} name Key to fetch the value from.
     * @param {string|number} value Value to save to storage.
     */
    saveSetting(name, value) {
        sessionStorage.setItem(`${this.key}_${name}`, value.toString());
    }

    /**
     * Sorts rows by their original index.
     * @param {IndexedRow} a
     * @param {IndexedRow} b
     * @returns {number} Negative if a is less than b, positive if a is greater than b, and zero if they are equal
     */
    static defaultCompare(a, b) {
        if (a._index > b._index) {
            return 1;
        }
        return a._index < b._index ? -1 : 0;
    }

    /**
     * Sorts rows based on SortColumns property.
     * @this {Array<SortColumn>}
     * @param {object} a First element for comparison.
     * @param {object} b Second element for comparison.
     * @returns {number} Negative if a is less than b, positive if a is greater than b, and zero if they are equal
     */
    compare(a, b) {
        let i = 0;
        const len = this.length;
        for (; i < len; i += 1) {
            const sort = this[i];
            const aa = a[sort.property];
            const bb = b[sort.property];

            if (aa === null) {
                return 1;
            }
            if (bb === null) {
                return -1;
            }
            if (aa < bb) {
                return sort.sortOrder === SortOrder.Asc ? -1 : 1;
            }
            if (aa > bb) {
                return sort.sortOrder === SortOrder.Asc ? 1 : -1;
            }
        }
        return 0;
    }

    /**
     * Searches for a string in the properties of an object.
     * @this {string} String to search for.
     * @param {IndexedRow} obj Object to search in.
     * @returns True if object contains search string, else false.
     */
    filterArray(obj) {
        const tokens = (this || '').split(' ');
        const { _index, ...originalObj } = obj;
        return Object.values(originalObj).some((x) => {
            const objVal = `${x}`.toLowerCase();
            return tokens.every((y) => objVal.indexOf(y) > -1);
        });
    }

    /**
     * Create a new array of results, filter by the search query, and sort.
     */
    filterData() {
        const filteredData = this.searchQuery ? (this.rows?.filter(this.filterArray.bind(this.searchQuery.toLowerCase())) ?? [])
            : [...(this.rows ?? [])];

        // sort the new array
        filteredData.sort(this.sortColumns?.length ? this.compare.bind(this.sortColumns) : Table.defaultCompare);

        // cache the total number of filtered records and max number of pages for paging
        this.filteredRowTotal = filteredData.length;
        this.maxPage = Math.max(Math.ceil(this.filteredRowTotal / this.perPage) - 1, 0);

        // determine the correct slice of data for the current page, and reassign our array to trigger the update
        this.filteredRows = filteredData.slice(this.perPage * this.currentPage, (this.perPage * this.currentPage) + this.perPage);

        this.update();

        // after rendering let htmx reprocess the table to add event listeners
        setTimeout(() => {
            htmx.process(this);
        }, 0);
    }

    /**
     * Handle input in search box and filter data.
     * @param {InputEvent} event Input event to get search value from.
     */
    onSearchQueryInput(event) {
        if (this.loading || this.error) {
            return;
        }

        // @ts-ignore target will be a HTMLInputElement with a value
        const val = (event?.target)?.value;
        if (this.debounceTimer) {
            clearTimeout(this.debounceTimer);
        }
        this.debounceTimer = window.setTimeout(() => {
            if (this.searchQuery !== val) {
                this.currentPage = 0;
                this.saveSetting(TableSetting.CurrentPage, 0);
            }

            this.searchQuery = val;
            this.saveSetting(TableSetting.SearchQuery, val);

            this.filterData();
        }, 250);
    }

    /**
     * Handle input for per page select box and update data to display.
     * @param {InputEvent} event Input event to get per page value from.
     */
    onPerPageChange(event) {
        if (this.loading || this.error) {
            return;
        }

        // @ts-ignore target will be a HTMLSelectElement with a value
        const newVal = parseInt((event?.target)?.value ?? '10', 10) ?? 10;
        if (this.perPage !== newVal) {
            this.currentPage = 0;
            this.saveSetting(TableSetting.CurrentPage, 0);
        }

        this.perPage = newVal;
        this.saveSetting(TableSetting.PerPage, newVal);

        this.filterData();
    }

    /**
     * Move to first page of table.
     */
    onFirstPageClick() {
        this.setPage(0);
    }

    /**
     * Move to last page of table.
     */
    onLastPageClick() {
        this.setPage(this.maxPage);
    }

    /**
     * Move to previous page of table.
     */
    onPreviousPageClick() {
        this.setPage(Math.max(this.currentPage - 1, 0));
    }

    /**
     * Move to next page of table.
     */
    onNextPageClick() {
        this.setPage(Math.min(this.currentPage + 1, this.maxPage));
    }

    /**
     * Move to specific page of table.
     * @param {number} page Page to move to, zero based.
     */
    setPage(page) {
        if (this.loading || this.error) {
            return;
        }

        this.currentPage = page;
        this.saveSetting(TableSetting.CurrentPage, this.currentPage);
        this.filterData();
    }

    /**
     * Handle click in table header to sort by a column.
     * @param {Element} elem TH that was clicked on.
     */
    onSortClick(elem) {
        if (this.loading || this.error) {
            return;
        }

        const property = elem.getAttribute('data-property');
        if (!property) {
            return;
        }

        const index = this.sortColumns.findIndex((x) => x.property === property);
        if (index === -1) {
            this.sortColumns.push({ property, sortOrder: SortOrder.Asc });
        } else if (this.sortColumns[index].sortOrder === SortOrder.Asc) {
            this.sortColumns[index].sortOrder = SortOrder.Desc;
        } else {
            this.sortColumns = this.sortColumns.filter((x) => x.property !== property);
        }

        this.saveSetting(TableSetting.Sort, JSON.stringify(this.sortColumns));

        this.filterData();
    }

    /**
     * Lets user try to reload data in case of a failure.
     */
    async onRetryClick() {
        if (!this.loading) {
            await this.fetchData();
        }
    }

    /**
     * Find the number of the first row displayed for the current page.
     * @returns {number} Row number.
     */
    get startRowNumber() {
        return this.filteredRowTotal ? (this.currentPage * this.perPage) + 1 : 0;
    }

    /**
     * Find the number of the last row displayed for the current page.
     * @returns {number} Row number.
     */
    get endRowNumber() {
        return this.filteredRowTotal ? Math.min((this.currentPage + 1) * this.perPage, this.filteredRowTotal) : 0;
    }

    /**
     * Check if the first page is currently displayed.
     * @returns {boolean} True if current page is the first, else false.
     */
    get isFirstPage() {
        return this.currentPage === 0;
    }

    /**
     * Check if the last page is currently displayed.
     * @returns {boolean} True if current page is the last, else false.
     */
    get isLastPage() {
        return this.currentPage === this.maxPage;
    }
}

// Define the new web component
if ('customElements' in window) {
    customElements.define('nilla-table', Table);
}

export default Table;
