import alpine from '../external/alpine.js';
import '../external/htmx/src/htmx.js';

import alpineTable from './alpine-table.js';
import onHtmxConfirm from './eventHandlers/onHtmxConfirm.js';
import onTableUpdated from './eventHandlers/onTableUpdated.js';

// disable some htmx features for security
htmx.config.allowScriptTags = false;
htmx.config.selfRequestsOnly = true;
htmx.config.allowEval = false;
htmx.config.historyCacheSize = 0;

document.body.addEventListener('alpine-table-updated', onTableUpdated);

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

alpine.data('table', alpineTable);

alpine.start();
