// @ts-ignore VS doesn't like this import but it builds fine
import ky, { Options } from 'ky';
import mustache from 'mustache';
import BaseComponent from './BaseComponent';
import FetchError from './FetchError';
import HttpHeaders from '../constants/HttpHeaders';

/**
 * Enum for table setting keys.
 * @readonly
 * @enum {string}
 */
const TableSetting = Object.freeze({
    CurrentPage: 'currentPage',
    PerPage: 'perPage',
    Search: 'search',
    Sort: 'sort',
    FormData: 'formData',
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
 * Enum for identifiers to query DOM elements.
 * @readonly
 * @enum {string}
 */
const Elements = Object.freeze({
    Search: 'search',
    FirstPage: 'first-page',
    PreviousPage: 'previous-page',
    NextPage: 'next-page',
    LastPage: 'last-page',
    PerPage: 'per-page',
    Retry: 'retry',
    StartRow: 'start-row',
    EndRow: 'end-row',
    FilteredRows: 'filtered-rows',
    Loading: 'loading',
    Error: 'error',
    Empty: 'empty',
    SortAscTemplate: 'sort-asc-template',
    SortDescTemplate: 'sort-desc-template',
    SortAsc: 'sort-asc',
    SortDesc: 'sort-desc',
    Status: 'status',
    MaxResults: 'max-results',
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
class Table extends BaseComponent {
    /**
     * Unique identifier for this table.
     * @type {string}
     */
    key = '';

    /**
     * Url to request data from the server.
     * @type {string}
     */
    srcUrl = '';

    /**
     * ID for form to include with request.
     * @type {string}
     */
    srcForm = '';

    /**
     * Max number of results to display.
     * @type {number|undefined}
     */
    maxResults = undefined;

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
    search = '';

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

    /**
     * Initialize table by loading settings from sessionStorage, processing HTML to add event handlers, and fetching data from the server.
     */
    constructor() {
        super();

        // set the prefix to use when querying/caching elements
        super.elementPrefix = 'table';

        this.key = this.dataset.key;
        this.srcUrl = this.dataset.srcUrl;
        this.srcForm = this.dataset.srcForm;

        this.maxResults = Number(this.dataset.maxResults);
        if (Number.isNaN(this.maxResults)) {
            this.maxResults = undefined;
        }

        // check sessionStorage for saved settings
        this.perPage = Number(this.loadSetting(TableSetting.PerPage) ?? '10');
        this.currentPage = Number(this.loadSetting(TableSetting.CurrentPage) ?? '0');
        this.search = this.loadSetting(TableSetting.Search) ?? '';
        this.sortColumns = JSON.parse(this.loadSetting(TableSetting.Sort) ?? '[]');

        this.setupHeader();
        this.setupFooter();
        this.setupStatus();
        this.setupSorting();
        this.setupForm();

        this.fetchData();
    }

    /**
     * Clean up when removing component.
     */
    disconnectedCallback() {
        super.disconnectedCallback();
    }

    /**
     * Sets the default value and adds event handler for the search input.
     */
    setupHeader() {
        const searchInput = /** @type {HTMLInputElement} */ (this.getElement(Elements.Search));
        if (searchInput) {
            searchInput.value = this.search;
            searchInput.addEventListener('input', (/** @type {InputEvent} */ e) => this.onSearchInput(e));
            searchInput.focus();
        }
    }

    /**
     * Add event handlers for the buttons for moving between pages.
     */
    setupFooter() {
        const firstPageButton = this.getElement(Elements.FirstPage);
        if (firstPageButton) {
            firstPageButton.addEventListener('click', () => this.onFirstPageClick());
        }

        const previousPageButton = this.getElement(Elements.PreviousPage);
        if (previousPageButton) {
            previousPageButton.addEventListener('click', () => this.onPreviousPageClick());
        }

        const nextPageButton = this.getElement(Elements.NextPage);
        if (nextPageButton) {
            nextPageButton.addEventListener('click', () => this.onNextPageClick());
        }

        const lastPageButton = this.getElement(Elements.LastPage);
        if (lastPageButton) {
            lastPageButton.addEventListener('click', () => this.onLastPageClick());
        }

        const tablePerPageSelect = /** @type {HTMLSelectElement} */ (this.getElement(Elements.PerPage));
        if (tablePerPageSelect) {
            tablePerPageSelect.value = `${this.perPage}`;
            tablePerPageSelect.addEventListener('change', (/** @type {InputEvent} */ e) => this.onPerPageChange(e));
        }
    }

    /**
     * Add event handlers for table status functionality.
     */
    setupStatus() {
        const tableRetryButton = this.getElement(Elements.Retry);
        if (tableRetryButton) {
            tableRetryButton.addEventListener('click', () => this.onRetryClick());
        }
    }

    /**
     * Add event handlers for table sorting functionality.
     */
    setupSorting() {
        this.querySelectorAll('th[data-property]').forEach((/** @type {HTMLElement} */ x) => {
            x.addEventListener('click', () => this.onSortClick(x));
        });
    }

    /**
     * Add event handlers for form submission.
     */
    setupForm() {
        if (this.srcForm) {
            const formElement = /** @type {HTMLFormElement} */ (document.getElementById(this.srcForm));
            if (formElement) {
                this.loadFormData();

                formElement.addEventListener('submit', (/** @type {SubmitEvent} */ e) => {
                    // make sure the form doesn't actually submit
                    e.preventDefault();
                    e.stopImmediatePropagation();

                    if (this.loading) {
                        return;
                    }

                    // @todo may want to debounce later
                    this.fetchData();
                });
            }
        }
    }

    /**
     * Fetch data from the server at the URL specified in the `src` property.
     */
    async fetchData() {
        if (!(this.srcUrl || this.srcForm)) {
            return;
        }

        this.loading = true;
        this.error = false;

        // first clear out the existing data and update the table
        this.rows = [];
        this.filterData();
        this.update();

        // now request new data
        try {
            let url = this.srcUrl;
            const headers = {};
            headers[HttpHeaders.RequestedWith] = 'XMLHttpRequest';

            const options = /** @type {Options} */ ({
                headers,
            });

            if (this.srcForm) {
                const formElement = /** @type {HTMLFormElement} */ (document.getElementById(this.srcForm));
                if (formElement) {
                    url = formElement.action;
                    options.body = new FormData(formElement);
                    options.method = formElement.method;

                    this.saveFormData();
                }
            }

            const json = await ky(url, options).json();
            if (!(json && Array.isArray(json))) {
                throw new FetchError(`Request to '${this.srcUrl}' returned invalid response.`);
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
        const searchInput = /** @type {HTMLInputElement} */ (this.getElement(Elements.Search));
        if (searchInput) {
            searchInput.disabled = this.loading || this.error;
        }
    }
    /**
     * Updates the start, end, and total numbers.
     */
    updateRowNumbers() {
        const startRowSpan = this.getElement(Elements.StartRow);
        if (startRowSpan) {
            startRowSpan.textContent = `${this.loading || this.error ? 0 : this.startRowNumber}`;
        }

        const endRowSpan = this.getElement(Elements.EndRow);
        if (endRowSpan) {
            endRowSpan.textContent = `${this.loading || this.error ? 0 : this.endRowNumber}`;
        }

        const filteredRowsSpan = this.getElement(Elements.FilteredRows);
        if (filteredRowsSpan) {
            filteredRowsSpan.textContent = `${this.loading || this.error ? 0 : this.filteredRowTotal}`;
        }

        const maxResultsSpan = this.getElement(Elements.MaxResults);
        if (maxResultsSpan) {
            maxResultsSpan.classList.toggle('is-hidden', !this.maxResults || this.rows.length <= this.maxResults);
        }
    }

    /**
     * Enable/disable paging buttons.
     */
    updatePageButtons() {
        const shouldDisable = this.loading || this.error;

        const firstPageButton = /** @type {HTMLButtonElement} */ (this.getElement(Elements.FirstPage));
        if (firstPageButton) {
            firstPageButton.disabled = shouldDisable || this.isFirstPage;
        }

        const previousPageButton = /** @type {HTMLButtonElement} */ (this.getElement(Elements.PreviousPage));
        if (previousPageButton) {
            previousPageButton.disabled = shouldDisable || this.isFirstPage;
        }

        const nextPageButton = /** @type {HTMLButtonElement} */ (this.getElement(Elements.NextPage));
        if (nextPageButton) {
            nextPageButton.disabled = shouldDisable || this.isLastPage;
        }

        const lastPageButton = /** @type {HTMLButtonElement} */ (this.getElement(Elements.LastPage));
        if (lastPageButton) {
            lastPageButton.disabled = shouldDisable || this.isLastPage;
        }

        const tablePerPageSelect = /** @type {HTMLSelectElement} */ (this.getElement(Elements.PerPage));
        if (tablePerPageSelect) {
            tablePerPageSelect.disabled = shouldDisable;
        }
    }

    /**
     * Shows/hides the table loading and error indicators.
     */
    updateStatus() {
        const isLoading = this.getElement(Elements.Loading);
        if (isLoading) {
            isLoading.classList.toggle('is-hidden', !this.loading);
        }

        const hasError = this.getElement(Elements.Error);
        if (hasError) {
            hasError.classList.toggle('is-hidden', !this.error);
        }

        const hasNoData = this.getElement(Elements.Empty);
        if (hasNoData) {
            hasNoData.classList.toggle('is-hidden', this.loading || this.error || this.filteredRowTotal !== 0);
        }
    }

    /**
     * Updates table headers to show correct sorting icons.
     */
    updateSortHeaders() {
        const sortAscTemplate = this.getElement(Elements.SortAscTemplate);
        const sortDescTemplate = this.getElement(Elements.SortDescTemplate);

        this.querySelectorAll('th[data-property]').forEach((/** @type {HTMLElement} */ th) => {
            const { property } = th.dataset;
            const sortOrder = this.sortOrder(property);
            const sortAsc = th.querySelector(`[data-table-${Elements.SortAsc}]`);
            const sortDesc = th.querySelector(`[data-table-${Elements.SortDesc}]`);

            // make sure the TH has the correct sort icon
            if (sortOrder === SortOrder.Asc) {
                if (sortDesc) {
                    sortDesc.remove();
                }
                if (!sortAsc) {
                    th.insertAdjacentHTML('beforeend', sortAscTemplate.innerHTML);
                }
                th.setAttribute('aria-sort', 'ascending');
            } else if (sortOrder === SortOrder.Desc) {
                if (sortAsc) {
                    sortAsc.remove();
                }
                if (!sortDesc) {
                    th.insertAdjacentHTML('beforeend', sortDescTemplate.innerHTML);
                }
                th.setAttribute('aria-sort', 'descending');
            } else {
                if (sortAsc) {
                    sortAsc.remove();
                }
                if (sortDesc) {
                    sortDesc.remove();
                }
                th.removeAttribute('aria-sort');
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
        const existingRows = this.querySelectorAll(`tbody tr:not([data-table-${Elements.Status}])`);
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
    loadSetting(name) {
        return sessionStorage.getItem(`${this.key}_${name}`);
    }

    /**
     * Saves a value to session storage.
     * @param {string} name Key to save the value to.
     * @param {string|number} value Value to save to storage.
     */
    saveSetting(name, value) {
        sessionStorage.setItem(`${this.key}_${name}`, value.toString());
    }

    /**
     * Loads form data from storage and populates the form.
     */
    loadFormData() {
        const formElement = /** @type {HTMLFormElement} */ (document.getElementById(this.srcForm));
        if (!formElement) {
            return;
        }

        const json = this.loadSetting(TableSetting.FormData);
        if (!json) {
            return;
        }

        try {
            const data = JSON.parse(json);
            if (!data) {
                return;
            }
            Object.entries(data).forEach(([key, value]) => {
                const input = formElement.elements.namedItem(key);
                if (input) {
                    if (Array.isArray(value)) {
                        // @todo are there other types that need special logic?

                        // @ts-ignore input will be a HTMLSelectElement
                        input.options.forEach((opt) => {
                            // eslint-disable-next-line no-param-reassign
                            opt.selected = value.includes(opt.value);
                        });
                    } else {
                        // @ts-ignore input will be a HTMLInputElement
                        input.value = value;
                    }
                }
            });
        } catch { /* empty */ }
    }

    /**
     * Saves form data to session storage.
     */
    saveFormData() {
        const formElement = /** @type {HTMLFormElement} */ (document.getElementById(this.srcForm));
        if (!formElement) {
            return;
        }

        const data = {};
        new FormData(formElement).forEach((value, key) => {
            if (!Object.prototype.hasOwnProperty.call(data, key)) {
                data[key] = value;
                return;
            }
            if (!Array.isArray(data[key])) {
                data[key] = [data[key]];
            }
            data[key].push(value);
        });
        this.saveSetting(TableSetting.FormData, JSON.stringify(data));
    }

    /**
     * Sorts rows by their original index.
     * @param {IndexedRow} a First row.
     * @param {IndexedRow} b Row to compare first row to.
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
     * Searches for a string(`this`) in the properties of an object.
     * @this {string}
     * @param {IndexedRow} obj Object to search in.
     * @returns {boolean} True if object contains search string, else false.
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
        let filteredData = this.search ? (this.rows?.filter(this.filterArray.bind(this.search.toLowerCase())) ?? [])
            : [...(this.rows ?? [])];

        if (this.maxResults) {
            filteredData = filteredData.slice(0, this.maxResults);
        }

        // sort the new array
        if (filteredData.length) {
            filteredData.sort(this.sortColumns?.length ? this.compare.bind(this.sortColumns) : Table.defaultCompare);
        }

        // cache the total number of filtered records and max number of pages for paging
        this.filteredRowTotal = filteredData.length;
        this.maxPage = Math.max(Math.ceil(this.filteredRowTotal / this.perPage) - 1, 0);
        if (this.currentPage > this.maxPage) {
            this.currentPage = 0;
            this.saveSetting(TableSetting.CurrentPage, 0);
        }

        // determine the correct slice of data for the current page, and reassign our array to trigger the update
        this.filteredRows = filteredData.slice(this.perPage * this.currentPage, (this.perPage * this.currentPage) + this.perPage);

        this.update();
    }

    /**
     * Handle input in search box and filter data.
     * @param {InputEvent} event Input event to get search value from.
     */
    onSearchInput(event) {
        if (this.loading || this.error) {
            return;
        }

        const target = /** @type {HTMLInputElement} */ (event?.target);
        const newValue = target?.value;
        if (this.debounceTimer) {
            clearTimeout(this.debounceTimer);
        }
        this.debounceTimer = window.setTimeout(() => {
            if (this.search !== newValue) {
                this.currentPage = 0;
                this.saveSetting(TableSetting.CurrentPage, 0);
            }

            this.search = newValue;
            this.saveSetting(TableSetting.Search, newValue);

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

        const target = /** @type {HTMLSelectElement} */ (event?.target);
        const newVal = Number(target?.value ?? '10');
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
     * @param {HTMLElement} elem TH that was clicked on.
     */
    onSortClick(elem) {
        if (this.loading || this.error) {
            return;
        }

        const { property } = elem.dataset;
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
