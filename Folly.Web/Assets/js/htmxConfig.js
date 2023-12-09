// @ts-check

import * as htmx from 'htmx.org';

// disable some htmx features for security
htmx.config.allowScriptTags = false;
htmx.config.selfRequestsOnly = true;
htmx.config.allowEval = false;
htmx.config.historyCacheSize = 0;

// enable logging for htmx
if (document.body.getAttribute('data-environment') === 'development') {
    // uncomment this line to enable detailed htmx logging
    // htmx.logAll();
}
