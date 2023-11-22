import * as htmx from 'htmx.org';

/**
 * Listen for updates to alpine-table and process the table content with htmx.
 * @param {Event} event
 */
const onTableUpdated = (event) => {
    if (event?.target) {
        htmx.default.process(event.target);
    }
};

export default onTableUpdated;
