// @ts-check

import Alpine from 'alpinejs';
import * as htmx from 'htmx.org';

import alpineTable from './alpine-table';
import onHtmxConfirm from './eventHandlers/onHtmxConfirm';
import onTableUpdated from './eventHandlers/onTableUpdated';

// disable some htmx features for security
htmx.config.allowScriptTags = false;
htmx.config.selfRequestsOnly = true;
htmx.config.allowEval = false;
htmx.config.historyCacheSize = 0;

document.body.addEventListener('alpine-table-updated', onTableUpdated);

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

Alpine.data('table', alpineTable);

Alpine.start();
