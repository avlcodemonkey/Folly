// @ts-check

import './htmxConfig';
import Alpine from 'alpinejs';
import AlpineTable from './alpineTable';

Alpine.data('table', AlpineTable);

Alpine.start();
