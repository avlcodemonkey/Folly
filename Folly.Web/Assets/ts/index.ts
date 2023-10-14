import Alpine from 'alpinejs';
import alpineTable from './alpine-table';
import onHtmxConfirm from './eventHandlers/onHtmxConfirm';
import onTableUpdated from './eventHandlers/onTableUpdated';

import '../css/main.scss';

document.body.addEventListener('alpine-table-updated', onTableUpdated);

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

//@ts-ignore ignore until I can figure out how to define the component type correctly
Alpine.data('table', alpineTable);

Alpine.start();
