import Alpine from 'alpinejs';
import alpineTable from './alpine-table';
import * as htmx from 'htmx.org';
import onHtmxConfirm from './eventHandlers/onHtmxConfirm';
import onTableUpdated from './eventHandlers/onTableUpdated';

// disable some htmx features for security
htmx.config.allowScriptTags = false;
htmx.config.selfRequestsOnly = true;
htmx.config.allowEval = false;
htmx.config.historyCacheSize = 0;

document.body.addEventListener('alpine-table-updated', onTableUpdated);

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

//@ts-ignore ignore until I can figure out how to define the component type correctly
Alpine.data('table', alpineTable);

Alpine.start();
