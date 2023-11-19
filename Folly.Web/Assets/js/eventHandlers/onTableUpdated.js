import * as htmx from '../../external/htmx/src/htmx.js';

/**
 * Listen for updates to alpine-table and process the table content with htmx.
 * @param {Event} e
 */
const onTableUpdated = (e) => {
    if (e?.target) {
        htmx.process(e.target/* as Element*/);
    }
};
export default onTableUpdated;
