// @ts-check

import Alpine from 'alpinejs';
import AlpineTable from './alpineTable';
import * as htmx from 'htmx.org';

import onHtmxConfirm from './eventHandlers/onHtmxConfirm';

// disable some htmx features for security
htmx.config.allowScriptTags = false;
htmx.config.selfRequestsOnly = true;
htmx.config.allowEval = false;
htmx.config.historyCacheSize = 0;

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

Alpine.data('table', AlpineTable);

Alpine.start();
