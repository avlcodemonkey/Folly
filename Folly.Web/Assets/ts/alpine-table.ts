import { AlpineComponent } from 'alpinejs';

const TableSetting = Object.freeze({
    CurrentPage: 'currentPage',
    PerPage: 'perPage',
    SearchQuery: 'searchQuery',
    Sort: 'sort',
});

enum SortOrder {
    Asc = 'asc',
    Desc = 'desc',
}

interface IndexedRow {
    _index: number;
}

interface SortColumn {
    property: string;
    sortOrder: SortOrder;
}

const alpineTable: AlpineComponent = (key: string, src: string) => ({
    // from html placeholder for the table
    key,
    src,

    // internal state
    rows: [] as Array<IndexedRow>,
    filteredRows: [] as Array<IndexedRow>,
    filteredRowTotal: 0,
    sortColumns: [] as Array<SortColumn>,
    currentPage: 0,
    perPage: 10,
    maxPage: 0,
    searchQuery: '',
    debounceTimer: 0,

    fetchSetting(name: string) {
        return sessionStorage.getItem(`${this.key}_${name}`);
    },

    saveSetting(name: string, value: string | number) {
        sessionStorage.setItem(`${this.key}_${name}`, value.toString());
    },

    defaultCompare(a: IndexedRow, b: IndexedRow) {
        if (a._index > b._index) {
            return 1;
        }
        return a._index < b._index ? -1 : 0;
    },

    compare(this: Array<SortColumn>, a: unknown, b: unknown) {
        let i = 0;
        const len = this.length;
        for (; i < len; i += 1) {
            const sort = this[i];
            const aa = a[sort.property as keyof typeof a];
            const bb = b[sort.property as keyof typeof b];

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

    filterArray(this: string, obj: IndexedRow) {
        const tokens = (this || '').split(' ');
        return Array.from(Object.values(obj)).some((x) => {
            if (x.indexOf('_') < 0 && Object.prototype.hasOwnProperty.call(obj, x)) {
                const objVal = (`${obj[x as keyof typeof obj]}`).toLowerCase();
                if (tokens.every((y) => objVal.indexOf(y) > -1)) {
                    return true;
                }
            }
            return false;
        });
    },

    filterData() {
        // create a new array and filter by the search query
        
        const filteredData = this.searchQuery ? (this.rows?.filter(this.filterArray.bind(this.searchQuery.toLowerCase())) ?? []) : [...(this.rows ?? [])];

        // sort the new array
        filteredData.sort(this.sortColumns?.length ? this.compare.bind(this.sortColumns) : this.defaultCompare);

        // cache the total number of filtered records and max number of pages for paging
        this.filteredRowTotal = filteredData.length;
        this.maxPage = Math.max(Math.ceil(this.filteredRowTotal / this.perPage) - 1, 0);

        // determine the correct slice of data for the current page, and reassign our array to trigger the update
        this.filteredRows = filteredData.slice(this.perPage * this.currentPage, (this.perPage * this.currentPage) + this.perPage);

        // after rendering, notify that the table has been updated
        // @ts-ignore @todo figure out how to get typescript to work with these magics from alpine
        this.$nextTick(() => {
            // @ts-ignore
            this.$root.dispatchEvent(new CustomEvent('alpine-table-updated', { bubbles: true, composed: true }));
        });
    },

    onSearchQueryInput(event: InputEvent) {
        const val = (event?.target as HTMLInputElement)?.value;
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

    onPerPageInput(event: InputEvent) {
        const newVal = parseInt((event?.target as HTMLInputElement)?.value ?? '10', 10) ?? 10;
        if (this.perPage !== newVal) {
            this.currentPage = 0;
            this.saveSetting(TableSetting.PerPage, newVal);
        }
        this.perPage = newVal;

        this.filterData();
    },

    onFirstPageClick() {
        this.setPage(0);
    },

    onLastPageClick() {
        this.setPage(this.maxPage);
    },

    onPreviousPageClick() {
        this.setPage(Math.max(this.currentPage - 1, 0));
    },

    onNextPageClick() {
        this.setPage(Math.min(this.currentPage + 1, this.maxPage));
    },

    setPage(page: number) {
        this.currentPage = page;
        this.saveSetting(TableSetting.CurrentPage, this.currentPage);
        this.filterData();
    },

    onSortClick(property: string) {
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

    sortClass(property: string) {
        const index = property ? this.sortColumns.findIndex((x) => x.property === property) : -1;
        if (index === -1) {
            return '';
        }
        return this.sortColumns[index].sortOrder === SortOrder.Asc ? 'alpine-sort-asc' : 'alpine-sort-desc';
    },

    get startRowNumber() {
        return this.filteredRowTotal ? (this.currentPage * this.perPage) + 1 : 0;
    },

    get endRowNumber() {
        return this.filteredRowTotal ? Math.min((this.currentPage + 1) * this.perPage, this.filteredRowTotal) : 0;
    },

    get isFirstPage() {
        return this.currentPage === 0;
    },

    get isLastPage() {
        return this.currentPage === this.maxPage;
    },

    get is10PerPage() {
        return this.perPage === 10;
    },

    get is20PerPage() {
        return this.perPage === 20;
    },

    get is50PerPage() {
        return this.perPage === 50;
    },

    get is100PerPage() {
        return this.perPage === 100;
    },

    async fetchData() {
        if (!this.src.length) {
            return;
        }

        try {
            this.rows = (await fetch(this.src, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then((res) => res.json()))
                .map((x: object, index: number) => ({ ...x, _index: index })) ?? [];
        } catch {
            this.rows = [];
        }

        this.filterData();
    },

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
