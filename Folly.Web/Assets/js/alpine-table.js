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
 * Component for rendering tables dynamically from HTML markup.
 * @param {string} key
 * @param {string} src
 * @returns {AlpineComponent}
 */
const alpineTable = (key, src) => ({
    // from html placeholder for the table
    key,
    src,

    // internal state
    /** @type {Array<IndexedRow>} */
    rows: [],

    /** @type {Array<IndexedRow>} */
    filteredRows: [],

    /** @type {Array<SortColumn>} */
    sortColumns: [],

    filteredRowTotal: 0,
    currentPage: 0,
    perPage: 10,
    maxPage: 0,
    searchQuery: '',
    debounceTimer: 0,

    /**
     * Load a value from session storage.
     * @param {string} name Key to store the value as.
     * @returns {string} Value from session storage if found, else null.
     */
    fetchSetting(name) {
        return sessionStorage.getItem(`${this.key}_${name}`);
    },

    /**
     * Saves a value to session storage.
     * @param {string} name Key to fetch the value from.
     * @param {string|number} value Value to save to storage.
     */
    saveSetting(name, value) {
        sessionStorage.setItem(`${this.key}_${name}`, value.toString());
    },

    /**
     * Sorts rows by their original index.
     * @param {IndexedRow} a
     * @param {IndexedRow} b
     * @returns {boolean}
     */
    defaultCompare(a, b) {
        if (a._index > b._index) {
            return 1;
        }
        return a._index < b._index ? -1 : 0;
    },

    /**
     * Sorts array based on SortColumns passed to it.
     * @this {Array<SortColumn>}
     * @param {object} a First element for comparison.
     * @param {object} b Second element for comparison.
     * @returns Negative if a is less than b, positive if a is greater than b, and zero if they are equal
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
    },

    /**
     * Searches for a string in the properties of an object.
     * @this {string} String to search for.
     * @param {IndexedRow} obj Object to search in.
     * @returns True if object contains search string, else false.
     */
    filterArray(obj) {
        const tokens = (this || '').split(' ');
        return Array.from(Object.values(obj)).some((x) => {
            if (x.indexOf('_') < 0 && Object.prototype.hasOwnProperty.call(obj, x)) {
                const objVal = (`${obj[x]}`).toLowerCase();
                if (tokens.every((y) => objVal.indexOf(y) > -1)) {
                    return true;
                }
            }
            return false;
        });
    },

    /**
     * Create a new array of results, filter by the search query, and sort.
     */
    filterData() {
        const filteredData = this.searchQuery ? (this.rows?.filter(this.filterArray.bind(this.searchQuery.toLowerCase())) ?? []) : [...(this.rows ?? [])];

        // sort the new array
        filteredData.sort(this.sortColumns?.length ? this.compare.bind(this.sortColumns) : this.defaultCompare);

        // cache the total number of filtered records and max number of pages for paging
        this.filteredRowTotal = filteredData.length;
        this.maxPage = Math.max(Math.ceil(this.filteredRowTotal / this.perPage) - 1, 0);

        // determine the correct slice of data for the current page, and reassign our array to trigger the update
        this.filteredRows = filteredData.slice(this.perPage * this.currentPage, (this.perPage * this.currentPage) + this.perPage);

        // after rendering, notify that the table has been updated
        this.$nextTick(() => {
            this.$root.dispatchEvent(new CustomEvent('alpine-table-updated', { bubbles: true, composed: true }));
        });
    },

    /**
     * Handle input in search box and filter data.
     * @param {InputEvent} event Input event to get search value from.
     */
    onSearchQueryInput(event) {
        const val = (event?.target)?.value;
        if (this.debounceTimer) {
            clearTimeout(this.debounceTimer);
        }
        this.debounceTimer = window.setTimeout(() => {
            if (this.searchQuery !== val) {
                this.currentPage = 0;
                this.saveSetting(TableSetting.SearchQuery, val);
            }
            this.searchQuery = val;
            this.filterData();
        }, 250);
    },

    /**
     * Handle input for per page select box and update data to display.
     * @param {InputEvent} event Input event to get per page value from.
     */
    onPerPageInput(event) {
        const newVal = parseInt((event?.target)?.value ?? '10', 10) ?? 10;
        if (this.perPage !== newVal) {
            this.currentPage = 0;
            this.saveSetting(TableSetting.PerPage, newVal);
        }
        this.perPage = newVal;

        this.filterData();
    },

    /**
     * Move to first page of table.
     */
    onFirstPageClick() {
        this.setPage(0);
    },

    /**
     * Move to last page of table.
     */
    onLastPageClick() {
        this.setPage(this.maxPage);
    },

    /**
     * Move to previous page of table.
     */
    onPreviousPageClick() {
        this.setPage(Math.max(this.currentPage - 1, 0));
    },

    /**
     * Move to next page of table.
     */
    onNextPageClick() {
        this.setPage(Math.min(this.currentPage + 1, this.maxPage));
    },

    /**
     * Move to specific page of table.
     * @param {number} page Page to move to, zero based.
     */
    setPage(page) {
        this.currentPage = page;
        this.saveSetting(TableSetting.CurrentPage, this.currentPage);
        this.filterData();
    },

    /**
     * Handle click in table header to sort by a column.
     * @param {string} property Name of property to sort on.
     */
    onSortClick(property) {
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
    },

    /**
     * Determines which class to use to display sort icon.
     * @param {string} property Property to find sort icon for.
     * @returns {string} Class name to use for this property.
     */
    sortClass(property) {
        const index = property ? this.sortColumns.findIndex((x) => x.property === property) : -1;
        if (index === -1) {
            return '';
        }
        return this.sortColumns[index].sortOrder === SortOrder.Asc ? 'alpine-sort-asc' : 'alpine-sort-desc';
    },

    /**
     * Find the number of the first row displayed for the current page.
     * @returns {number} Row number.
     */
    get startRowNumber() {
        return this.filteredRowTotal ? (this.currentPage * this.perPage) + 1 : 0;
    },

    /**
     * Find the number of the last row displayed for the current page.
     * @returns {number} Row number.
     */
    get endRowNumber() {
        return this.filteredRowTotal ? Math.min((this.currentPage + 1) * this.perPage, this.filteredRowTotal) : 0;
    },

    /**
     * Check if the first page is currently displayed.
     * @returns {boolean} True if current page is the first, else false.
     */
    get isFirstPage() {
        return this.currentPage === 0;
    },

    /**
     * Check if the last page is currently displayed.
     * @returns {boolean} True if current page is the last, else false.
     */
    get isLastPage() {
        return this.currentPage === this.maxPage;
    },

    /**
     * Check if 10 records per page are currently displayed.
     * @returns {boolean} True if page page is set to 10, else false.
     */
    get is10PerPage() {
        return this.perPage === 10;
    },

    /**
     * Check if 20 records per page are currently displayed.
     * @returns {boolean} True if page page is set to 20, else false.
     */
    get is20PerPage() {
        return this.perPage === 20;
    },

    /**
     * Check if 50 records per page are currently displayed.
     * @returns {boolean} True if page page is set to 50, else false.
     */
    get is50PerPage() {
        return this.perPage === 50;
    },

    /**
     * Check if 100 records per page are currently displayed.
     * @returns {boolean} True if page page is set to 100, else false.
     */
    get is100PerPage() {
        return this.perPage === 100;
    },

    /**
     * Fetch data from the server at the URL specified in the `src` property.
     */
    async fetchData() {
        if (!this.src.length) {
            return;
        }

        try {
            this.rows = (await fetch(this.src, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then((res) => res.json()))
                .map((x, index) => ({ ...x, _index: index })) ?? [];
        } catch {
            this.rows = [];
        }

        this.filterData();
    },

    /**
     * Initialize table by loading settings from sessionStorage and fetching data from the server.
     */
    async init() {
        // check sessionStorage for saved settings
        this.perPage = parseInt(this.fetchSetting(TableSetting.PerPage) ?? '10', 10);
        this.currentPage = parseInt(this.fetchSetting(TableSetting.CurrentPage) ?? '0', 10);
        this.searchQuery = this.fetchSetting(TableSetting.SearchQuery) ?? '';
        this.sortColumns = JSON.parse(this.fetchSetting(TableSetting.Sort) ?? '[]');

        await this.fetchData();
    },
});

export default alpineTable;
