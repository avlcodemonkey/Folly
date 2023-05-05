import * as htmx from 'htmx.org';

/**
 * Listen for updates to alpine-table and process the table content with htmx.
 * @param {any} e
 */
const onTableUpdated = (e: Event) => {
    if (e?.target) {
        htmx.process(e.target as Element);
    }
};
export default onTableUpdated;
