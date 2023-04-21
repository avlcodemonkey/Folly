import Alpine from './lib/alpinejs/index';

import alpineTable from './alpine-table';
import onHtmxConfirm from './eventHandlers/onHtmxConfirm';
import onTableUpdated from './eventHandlers/onTableUpdated';
import onSidebarToggled from './eventHandlers/onSidebarToggled';

document.getElementById('sidebar-button').addEventListener('click', onSidebarToggled);

document.body.addEventListener('alpine-table-updated', onTableUpdated);

document.body.addEventListener('htmx:confirm', onHtmxConfirm);

Alpine.data('table', alpineTable);

Alpine.start();
