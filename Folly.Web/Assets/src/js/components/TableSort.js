import TableSortDirection from '../constants/TableSortDirection';

class TableSort {
    /**
     * Name of property to sort on.
     * @type {string}
     */
    property = '';

    /**
     * Direction to sort this property.
     * @type {TableSortDirection}
     */
    direction = undefined;
}

export default TableSort;
