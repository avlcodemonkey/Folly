// @ts-check

import './htmxConfig';
import './alert';

import Alpine from 'alpinejs';
import AlpineTable from './alpineTable';

Alpine.data('table', AlpineTable);

Alpine.start();
