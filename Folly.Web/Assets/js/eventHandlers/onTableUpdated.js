import * as htmx from '../../vendor/htmx/src/htmx';

/**
 * Listen for updates to alpine-table and process the table content with htmx.
 * @param {any} e
 */
const onTableUpdated = (e) => {
    if (e?.target) {
        htmx.process(e.target);
    }
};
export default onTableUpdated;
