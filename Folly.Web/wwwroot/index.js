"use strict";
(() => {
  var __create = Object.create;
  var __defProp = Object.defineProperty;
  var __getOwnPropDesc = Object.getOwnPropertyDescriptor;
  var __getOwnPropNames = Object.getOwnPropertyNames;
  var __getProtoOf = Object.getPrototypeOf;
  var __hasOwnProp = Object.prototype.hasOwnProperty;
  var __commonJS = (cb, mod) => function __require() {
    return mod || (0, cb[__getOwnPropNames(cb)[0]])((mod = { exports: {} }).exports, mod), mod.exports;
  };
  var __copyProps = (to, from, except, desc) => {
    if (from && typeof from === "object" || typeof from === "function") {
      for (let key of __getOwnPropNames(from))
        if (!__hasOwnProp.call(to, key) && key !== except)
          __defProp(to, key, { get: () => from[key], enumerable: !(desc = __getOwnPropDesc(from, key)) || desc.enumerable });
    }
    return to;
  };
  var __toESM = (mod, isNodeMode, target) => (target = mod != null ? __create(__getProtoOf(mod)) : {}, __copyProps(
    // If the importer is in node compatibility mode or this is not an ESM
    // file that has been converted to a CommonJS file using a Babel-
    // compatible transform (i.e. "__esModule" has not been set), then set
    // "default" to the CommonJS "module.exports" for node compatibility.
    isNodeMode || !mod || !mod.__esModule ? __defProp(target, "default", { value: mod, enumerable: true }) : target,
    mod
  ));

  // node_modules/htmx.org/dist/htmx.min.js
  var require_htmx_min = __commonJS({
    "node_modules/htmx.org/dist/htmx.min.js"(exports, module) {
      (function(e2, t2) {
        if (typeof define === "function" && define.amd) {
          define([], t2);
        } else if (typeof module === "object" && module.exports) {
          module.exports = t2();
        } else {
          e2.htmx = e2.htmx || t2();
        }
      })(typeof self !== "undefined" ? self : exports, function() {
        return function() {
          "use strict";
          var z = { onLoad: t, process: Tt, on: le, off: ue, trigger: ie, ajax: dr, find: b, findAll: f, closest: d, values: function(e2, t2) {
            var r2 = Jt(e2, t2 || "post");
            return r2.values;
          }, remove: B, addClass: j, removeClass: n, toggleClass: U, takeClass: V, defineExtension: yr, removeExtension: br, logAll: F, logger: null, config: { historyEnabled: true, historyCacheSize: 10, refreshOnHistoryMiss: false, defaultSwapStyle: "innerHTML", defaultSwapDelay: 0, defaultSettleDelay: 20, includeIndicatorStyles: true, indicatorClass: "htmx-indicator", requestClass: "htmx-request", addedClass: "htmx-added", settlingClass: "htmx-settling", swappingClass: "htmx-swapping", allowEval: true, inlineScriptNonce: "", attributesToSettle: ["class", "style", "width", "height"], withCredentials: false, timeout: 0, wsReconnectDelay: "full-jitter", wsBinaryType: "blob", disableSelector: "[hx-disable], [data-hx-disable]", useTemplateFragments: false, scrollBehavior: "smooth", defaultFocusScroll: false, getCacheBusterParam: false, globalViewTransitions: false }, parseInterval: v, _: e, createEventSource: function(e2) {
            return new EventSource(e2, { withCredentials: true });
          }, createWebSocket: function(e2) {
            var t2 = new WebSocket(e2, []);
            t2.binaryType = z.config.wsBinaryType;
            return t2;
          }, version: "1.9.2" };
          var C = { addTriggerHandler: xt, bodyContains: ee, canAccessLocalStorage: D, filterValues: er, hasAttribute: q, getAttributeValue: G, getClosestMatch: c, getExpressionVars: fr, getHeaders: Qt, getInputValues: Jt, getInternalData: Y, getSwapSpecification: rr, getTriggerSpecs: ze, getTarget: de, makeFragment: l, mergeObjects: te, makeSettleInfo: S, oobSwap: me, selectAndSwap: Me, settleImmediately: Bt, shouldCancel: Ke, triggerEvent: ie, triggerErrorEvent: ne, withExtensions: w };
          var R = ["get", "post", "put", "delete", "patch"];
          var O = R.map(function(e2) {
            return "[hx-" + e2 + "], [data-hx-" + e2 + "]";
          }).join(", ");
          function v(e2) {
            if (e2 == void 0) {
              return void 0;
            }
            if (e2.slice(-2) == "ms") {
              return parseFloat(e2.slice(0, -2)) || void 0;
            }
            if (e2.slice(-1) == "s") {
              return parseFloat(e2.slice(0, -1)) * 1e3 || void 0;
            }
            if (e2.slice(-1) == "m") {
              return parseFloat(e2.slice(0, -1)) * 1e3 * 60 || void 0;
            }
            return parseFloat(e2) || void 0;
          }
          function $(e2, t2) {
            return e2.getAttribute && e2.getAttribute(t2);
          }
          function q(e2, t2) {
            return e2.hasAttribute && (e2.hasAttribute(t2) || e2.hasAttribute("data-" + t2));
          }
          function G(e2, t2) {
            return $(e2, t2) || $(e2, "data-" + t2);
          }
          function u(e2) {
            return e2.parentElement;
          }
          function J() {
            return document;
          }
          function c(e2, t2) {
            while (e2 && !t2(e2)) {
              e2 = u(e2);
            }
            return e2 ? e2 : null;
          }
          function T(e2, t2, r2) {
            var n2 = G(t2, r2);
            var i2 = G(t2, "hx-disinherit");
            if (e2 !== t2 && i2 && (i2 === "*" || i2.split(" ").indexOf(r2) >= 0)) {
              return "unset";
            } else {
              return n2;
            }
          }
          function Z(t2, r2) {
            var n2 = null;
            c(t2, function(e2) {
              return n2 = T(t2, e2, r2);
            });
            if (n2 !== "unset") {
              return n2;
            }
          }
          function h(e2, t2) {
            var r2 = e2.matches || e2.matchesSelector || e2.msMatchesSelector || e2.mozMatchesSelector || e2.webkitMatchesSelector || e2.oMatchesSelector;
            return r2 && r2.call(e2, t2);
          }
          function H(e2) {
            var t2 = /<([a-z][^\/\0>\x20\t\r\n\f]*)/i;
            var r2 = t2.exec(e2);
            if (r2) {
              return r2[1].toLowerCase();
            } else {
              return "";
            }
          }
          function i(e2, t2) {
            var r2 = new DOMParser();
            var n2 = r2.parseFromString(e2, "text/html");
            var i2 = n2.body;
            while (t2 > 0) {
              t2--;
              i2 = i2.firstChild;
            }
            if (i2 == null) {
              i2 = J().createDocumentFragment();
            }
            return i2;
          }
          function L(e2) {
            return e2.match(/<body/);
          }
          function l(e2) {
            var t2 = !L(e2);
            if (z.config.useTemplateFragments && t2) {
              var r2 = i("<body><template>" + e2 + "</template></body>", 0);
              return r2.querySelector("template").content;
            } else {
              var n2 = H(e2);
              switch (n2) {
                case "thead":
                case "tbody":
                case "tfoot":
                case "colgroup":
                case "caption":
                  return i("<table>" + e2 + "</table>", 1);
                case "col":
                  return i("<table><colgroup>" + e2 + "</colgroup></table>", 2);
                case "tr":
                  return i("<table><tbody>" + e2 + "</tbody></table>", 2);
                case "td":
                case "th":
                  return i("<table><tbody><tr>" + e2 + "</tr></tbody></table>", 3);
                case "script":
                  return i("<div>" + e2 + "</div>", 1);
                default:
                  return i(e2, 0);
              }
            }
          }
          function K(e2) {
            if (e2) {
              e2();
            }
          }
          function A(e2, t2) {
            return Object.prototype.toString.call(e2) === "[object " + t2 + "]";
          }
          function N(e2) {
            return A(e2, "Function");
          }
          function I(e2) {
            return A(e2, "Object");
          }
          function Y(e2) {
            var t2 = "htmx-internal-data";
            var r2 = e2[t2];
            if (!r2) {
              r2 = e2[t2] = {};
            }
            return r2;
          }
          function k(e2) {
            var t2 = [];
            if (e2) {
              for (var r2 = 0; r2 < e2.length; r2++) {
                t2.push(e2[r2]);
              }
            }
            return t2;
          }
          function Q(e2, t2) {
            if (e2) {
              for (var r2 = 0; r2 < e2.length; r2++) {
                t2(e2[r2]);
              }
            }
          }
          function P(e2) {
            var t2 = e2.getBoundingClientRect();
            var r2 = t2.top;
            var n2 = t2.bottom;
            return r2 < window.innerHeight && n2 >= 0;
          }
          function ee(e2) {
            if (e2.getRootNode && e2.getRootNode() instanceof ShadowRoot) {
              return J().body.contains(e2.getRootNode().host);
            } else {
              return J().body.contains(e2);
            }
          }
          function M(e2) {
            return e2.trim().split(/\s+/);
          }
          function te(e2, t2) {
            for (var r2 in t2) {
              if (t2.hasOwnProperty(r2)) {
                e2[r2] = t2[r2];
              }
            }
            return e2;
          }
          function y(e2) {
            try {
              return JSON.parse(e2);
            } catch (e3) {
              x(e3);
              return null;
            }
          }
          function D() {
            var e2 = "htmx:localStorageTest";
            try {
              localStorage.setItem(e2, e2);
              localStorage.removeItem(e2);
              return true;
            } catch (e3) {
              return false;
            }
          }
          function X(t2) {
            try {
              var e2 = new URL(t2);
              if (e2) {
                t2 = e2.pathname + e2.search;
              }
              if (!t2.match("^/$")) {
                t2 = t2.replace(/\/+$/, "");
              }
              return t2;
            } catch (e3) {
              return t2;
            }
          }
          function e(e) {
            return sr(J().body, function() {
              return eval(e);
            });
          }
          function t(t2) {
            var e2 = z.on("htmx:load", function(e3) {
              t2(e3.detail.elt);
            });
            return e2;
          }
          function F() {
            z.logger = function(e2, t2, r2) {
              if (console) {
                console.log(t2, e2, r2);
              }
            };
          }
          function b(e2, t2) {
            if (t2) {
              return e2.querySelector(t2);
            } else {
              return b(J(), e2);
            }
          }
          function f(e2, t2) {
            if (t2) {
              return e2.querySelectorAll(t2);
            } else {
              return f(J(), e2);
            }
          }
          function B(e2, t2) {
            e2 = s(e2);
            if (t2) {
              setTimeout(function() {
                B(e2);
                e2 = null;
              }, t2);
            } else {
              e2.parentElement.removeChild(e2);
            }
          }
          function j(e2, t2, r2) {
            e2 = s(e2);
            if (r2) {
              setTimeout(function() {
                j(e2, t2);
                e2 = null;
              }, r2);
            } else {
              e2.classList && e2.classList.add(t2);
            }
          }
          function n(e2, t2, r2) {
            e2 = s(e2);
            if (r2) {
              setTimeout(function() {
                n(e2, t2);
                e2 = null;
              }, r2);
            } else {
              if (e2.classList) {
                e2.classList.remove(t2);
                if (e2.classList.length === 0) {
                  e2.removeAttribute("class");
                }
              }
            }
          }
          function U(e2, t2) {
            e2 = s(e2);
            e2.classList.toggle(t2);
          }
          function V(e2, t2) {
            e2 = s(e2);
            Q(e2.parentElement.children, function(e3) {
              n(e3, t2);
            });
            j(e2, t2);
          }
          function d(e2, t2) {
            e2 = s(e2);
            if (e2.closest) {
              return e2.closest(t2);
            } else {
              do {
                if (e2 == null || h(e2, t2)) {
                  return e2;
                }
              } while (e2 = e2 && u(e2));
              return null;
            }
          }
          function r(e2) {
            var t2 = e2.trim();
            if (t2.startsWith("<") && t2.endsWith("/>")) {
              return t2.substring(1, t2.length - 2);
            } else {
              return t2;
            }
          }
          function _(e2, t2) {
            if (t2.indexOf("closest ") === 0) {
              return [d(e2, r(t2.substr(8)))];
            } else if (t2.indexOf("find ") === 0) {
              return [b(e2, r(t2.substr(5)))];
            } else if (t2.indexOf("next ") === 0) {
              return [W(e2, r(t2.substr(5)))];
            } else if (t2.indexOf("previous ") === 0) {
              return [oe(e2, r(t2.substr(9)))];
            } else if (t2 === "document") {
              return [document];
            } else if (t2 === "window") {
              return [window];
            } else {
              return J().querySelectorAll(r(t2));
            }
          }
          var W = function(e2, t2) {
            var r2 = J().querySelectorAll(t2);
            for (var n2 = 0; n2 < r2.length; n2++) {
              var i2 = r2[n2];
              if (i2.compareDocumentPosition(e2) === Node.DOCUMENT_POSITION_PRECEDING) {
                return i2;
              }
            }
          };
          var oe = function(e2, t2) {
            var r2 = J().querySelectorAll(t2);
            for (var n2 = r2.length - 1; n2 >= 0; n2--) {
              var i2 = r2[n2];
              if (i2.compareDocumentPosition(e2) === Node.DOCUMENT_POSITION_FOLLOWING) {
                return i2;
              }
            }
          };
          function re(e2, t2) {
            if (t2) {
              return _(e2, t2)[0];
            } else {
              return _(J().body, e2)[0];
            }
          }
          function s(e2) {
            if (A(e2, "String")) {
              return b(e2);
            } else {
              return e2;
            }
          }
          function se(e2, t2, r2) {
            if (N(t2)) {
              return { target: J().body, event: e2, listener: t2 };
            } else {
              return { target: s(e2), event: t2, listener: r2 };
            }
          }
          function le(t2, r2, n2) {
            Sr(function() {
              var e3 = se(t2, r2, n2);
              e3.target.addEventListener(e3.event, e3.listener);
            });
            var e2 = N(r2);
            return e2 ? r2 : n2;
          }
          function ue(t2, r2, n2) {
            Sr(function() {
              var e2 = se(t2, r2, n2);
              e2.target.removeEventListener(e2.event, e2.listener);
            });
            return N(r2) ? r2 : n2;
          }
          var fe = J().createElement("output");
          function ce(e2, t2) {
            var r2 = Z(e2, t2);
            if (r2) {
              if (r2 === "this") {
                return [he(e2, t2)];
              } else {
                var n2 = _(e2, r2);
                if (n2.length === 0) {
                  x('The selector "' + r2 + '" on ' + t2 + " returned no matches!");
                  return [fe];
                } else {
                  return n2;
                }
              }
            }
          }
          function he(e2, t2) {
            return c(e2, function(e3) {
              return G(e3, t2) != null;
            });
          }
          function de(e2) {
            var t2 = Z(e2, "hx-target");
            if (t2) {
              if (t2 === "this") {
                return he(e2, "hx-target");
              } else {
                return re(e2, t2);
              }
            } else {
              var r2 = Y(e2);
              if (r2.boosted) {
                return J().body;
              } else {
                return e2;
              }
            }
          }
          function ve(e2) {
            var t2 = z.config.attributesToSettle;
            for (var r2 = 0; r2 < t2.length; r2++) {
              if (e2 === t2[r2]) {
                return true;
              }
            }
            return false;
          }
          function ge(t2, r2) {
            Q(t2.attributes, function(e2) {
              if (!r2.hasAttribute(e2.name) && ve(e2.name)) {
                t2.removeAttribute(e2.name);
              }
            });
            Q(r2.attributes, function(e2) {
              if (ve(e2.name)) {
                t2.setAttribute(e2.name, e2.value);
              }
            });
          }
          function pe(e2, t2) {
            var r2 = wr(t2);
            for (var n2 = 0; n2 < r2.length; n2++) {
              var i2 = r2[n2];
              try {
                if (i2.isInlineSwap(e2)) {
                  return true;
                }
              } catch (e3) {
                x(e3);
              }
            }
            return e2 === "outerHTML";
          }
          function me(e2, i2, a2) {
            var t2 = "#" + i2.id;
            var o2 = "outerHTML";
            if (e2 === "true") {
            } else if (e2.indexOf(":") > 0) {
              o2 = e2.substr(0, e2.indexOf(":"));
              t2 = e2.substr(e2.indexOf(":") + 1, e2.length);
            } else {
              o2 = e2;
            }
            var r2 = J().querySelectorAll(t2);
            if (r2) {
              Q(r2, function(e3) {
                var t3;
                var r3 = i2.cloneNode(true);
                t3 = J().createDocumentFragment();
                t3.appendChild(r3);
                if (!pe(o2, e3)) {
                  t3 = r3;
                }
                var n2 = { shouldSwap: true, target: e3, fragment: t3 };
                if (!ie(e3, "htmx:oobBeforeSwap", n2))
                  return;
                e3 = n2.target;
                if (n2["shouldSwap"]) {
                  ke(o2, e3, e3, t3, a2);
                }
                Q(a2.elts, function(e4) {
                  ie(e4, "htmx:oobAfterSwap", n2);
                });
              });
              i2.parentNode.removeChild(i2);
            } else {
              i2.parentNode.removeChild(i2);
              ne(J().body, "htmx:oobErrorNoTarget", { content: i2 });
            }
            return e2;
          }
          function xe(e2, t2, r2) {
            var n2 = Z(e2, "hx-select-oob");
            if (n2) {
              var i2 = n2.split(",");
              for (let e3 = 0; e3 < i2.length; e3++) {
                var a2 = i2[e3].split(":", 2);
                var o2 = a2[0].trim();
                if (o2.indexOf("#") === 0) {
                  o2 = o2.substring(1);
                }
                var s2 = a2[1] || "true";
                var l2 = t2.querySelector("#" + o2);
                if (l2) {
                  me(s2, l2, r2);
                }
              }
            }
            Q(f(t2, "[hx-swap-oob], [data-hx-swap-oob]"), function(e3) {
              var t3 = G(e3, "hx-swap-oob");
              if (t3 != null) {
                me(t3, e3, r2);
              }
            });
          }
          function ye(e2) {
            Q(f(e2, "[hx-preserve], [data-hx-preserve]"), function(e3) {
              var t2 = G(e3, "id");
              var r2 = J().getElementById(t2);
              if (r2 != null) {
                e3.parentNode.replaceChild(r2, e3);
              }
            });
          }
          function be(a2, e2, o2) {
            Q(e2.querySelectorAll("[id]"), function(e3) {
              if (e3.id && e3.id.length > 0) {
                var t2 = e3.id.replace("'", "\\'");
                var r2 = e3.tagName.replace(":", "\\:");
                var n2 = a2.querySelector(r2 + "[id='" + t2 + "']");
                if (n2 && n2 !== a2) {
                  var i2 = e3.cloneNode();
                  ge(e3, n2);
                  o2.tasks.push(function() {
                    ge(e3, i2);
                  });
                }
              }
            });
          }
          function we(e2) {
            return function() {
              n(e2, z.config.addedClass);
              Tt(e2);
              bt(e2);
              Se(e2);
              ie(e2, "htmx:load");
            };
          }
          function Se(e2) {
            var t2 = "[autofocus]";
            var r2 = h(e2, t2) ? e2 : e2.querySelector(t2);
            if (r2 != null) {
              r2.focus();
            }
          }
          function a(e2, t2, r2, n2) {
            be(e2, r2, n2);
            while (r2.childNodes.length > 0) {
              var i2 = r2.firstChild;
              j(i2, z.config.addedClass);
              e2.insertBefore(i2, t2);
              if (i2.nodeType !== Node.TEXT_NODE && i2.nodeType !== Node.COMMENT_NODE) {
                n2.tasks.push(we(i2));
              }
            }
          }
          function Ee(e2, t2) {
            var r2 = 0;
            while (r2 < e2.length) {
              t2 = (t2 << 5) - t2 + e2.charCodeAt(r2++) | 0;
            }
            return t2;
          }
          function Ce(e2) {
            var t2 = 0;
            if (e2.attributes) {
              for (var r2 = 0; r2 < e2.attributes.length; r2++) {
                var n2 = e2.attributes[r2];
                if (n2.value) {
                  t2 = Ee(n2.name, t2);
                  t2 = Ee(n2.value, t2);
                }
              }
            }
            return t2;
          }
          function Re(t2) {
            var r2 = Y(t2);
            if (r2.timeout) {
              clearTimeout(r2.timeout);
            }
            if (r2.webSocket) {
              r2.webSocket.close();
            }
            if (r2.sseEventSource) {
              r2.sseEventSource.close();
            }
            if (r2.listenerInfos) {
              Q(r2.listenerInfos, function(e2) {
                if (e2.on) {
                  e2.on.removeEventListener(e2.trigger, e2.listener);
                }
              });
            }
            if (r2.onHandlers) {
              for (let e2 = 0; e2 < r2.onHandlers.length; e2++) {
                const n2 = r2.onHandlers[e2];
                t2.removeEventListener(n2.name, n2.handler);
              }
            }
          }
          function o(e2) {
            ie(e2, "htmx:beforeCleanupElement");
            Re(e2);
            if (e2.children) {
              Q(e2.children, function(e3) {
                o(e3);
              });
            }
          }
          function Oe(e2, t2, r2) {
            if (e2.tagName === "BODY") {
              return Ne(e2, t2, r2);
            } else {
              var n2;
              var i2 = e2.previousSibling;
              a(u(e2), e2, t2, r2);
              if (i2 == null) {
                n2 = u(e2).firstChild;
              } else {
                n2 = i2.nextSibling;
              }
              Y(e2).replacedWith = n2;
              r2.elts = [];
              while (n2 && n2 !== e2) {
                if (n2.nodeType === Node.ELEMENT_NODE) {
                  r2.elts.push(n2);
                }
                n2 = n2.nextElementSibling;
              }
              o(e2);
              u(e2).removeChild(e2);
            }
          }
          function qe(e2, t2, r2) {
            return a(e2, e2.firstChild, t2, r2);
          }
          function Te(e2, t2, r2) {
            return a(u(e2), e2, t2, r2);
          }
          function He(e2, t2, r2) {
            return a(e2, null, t2, r2);
          }
          function Le(e2, t2, r2) {
            return a(u(e2), e2.nextSibling, t2, r2);
          }
          function Ae(e2, t2, r2) {
            o(e2);
            return u(e2).removeChild(e2);
          }
          function Ne(e2, t2, r2) {
            var n2 = e2.firstChild;
            a(e2, n2, t2, r2);
            if (n2) {
              while (n2.nextSibling) {
                o(n2.nextSibling);
                e2.removeChild(n2.nextSibling);
              }
              o(n2);
              e2.removeChild(n2);
            }
          }
          function Ie(e2, t2) {
            var r2 = Z(e2, "hx-select");
            if (r2) {
              var n2 = J().createDocumentFragment();
              Q(t2.querySelectorAll(r2), function(e3) {
                n2.appendChild(e3);
              });
              t2 = n2;
            }
            return t2;
          }
          function ke(e2, t2, r2, n2, i2) {
            switch (e2) {
              case "none":
                return;
              case "outerHTML":
                Oe(r2, n2, i2);
                return;
              case "afterbegin":
                qe(r2, n2, i2);
                return;
              case "beforebegin":
                Te(r2, n2, i2);
                return;
              case "beforeend":
                He(r2, n2, i2);
                return;
              case "afterend":
                Le(r2, n2, i2);
                return;
              case "delete":
                Ae(r2, n2, i2);
                return;
              default:
                var a2 = wr(t2);
                for (var o2 = 0; o2 < a2.length; o2++) {
                  var s2 = a2[o2];
                  try {
                    var l2 = s2.handleSwap(e2, r2, n2, i2);
                    if (l2) {
                      if (typeof l2.length !== "undefined") {
                        for (var u2 = 0; u2 < l2.length; u2++) {
                          var f2 = l2[u2];
                          if (f2.nodeType !== Node.TEXT_NODE && f2.nodeType !== Node.COMMENT_NODE) {
                            i2.tasks.push(we(f2));
                          }
                        }
                      }
                      return;
                    }
                  } catch (e3) {
                    x(e3);
                  }
                }
                if (e2 === "innerHTML") {
                  Ne(r2, n2, i2);
                } else {
                  ke(z.config.defaultSwapStyle, t2, r2, n2, i2);
                }
            }
          }
          function Pe(e2) {
            if (e2.indexOf("<title") > -1) {
              var t2 = e2.replace(/<svg(\s[^>]*>|>)([\s\S]*?)<\/svg>/gim, "");
              var r2 = t2.match(/<title(\s[^>]*>|>)([\s\S]*?)<\/title>/im);
              if (r2) {
                return r2[2];
              }
            }
          }
          function Me(e2, t2, r2, n2, i2) {
            i2.title = Pe(n2);
            var a2 = l(n2);
            if (a2) {
              xe(r2, a2, i2);
              a2 = Ie(r2, a2);
              ye(a2);
              return ke(e2, r2, t2, a2, i2);
            }
          }
          function De(e2, t2, r2) {
            var n2 = e2.getResponseHeader(t2);
            if (n2.indexOf("{") === 0) {
              var i2 = y(n2);
              for (var a2 in i2) {
                if (i2.hasOwnProperty(a2)) {
                  var o2 = i2[a2];
                  if (!I(o2)) {
                    o2 = { value: o2 };
                  }
                  ie(r2, a2, o2);
                }
              }
            } else {
              ie(r2, n2, []);
            }
          }
          var Xe = /\s/;
          var g = /[\s,]/;
          var Fe = /[_$a-zA-Z]/;
          var Be = /[_$a-zA-Z0-9]/;
          var je = ['"', "'", "/"];
          var p = /[^\s]/;
          function Ue(e2) {
            var t2 = [];
            var r2 = 0;
            while (r2 < e2.length) {
              if (Fe.exec(e2.charAt(r2))) {
                var n2 = r2;
                while (Be.exec(e2.charAt(r2 + 1))) {
                  r2++;
                }
                t2.push(e2.substr(n2, r2 - n2 + 1));
              } else if (je.indexOf(e2.charAt(r2)) !== -1) {
                var i2 = e2.charAt(r2);
                var n2 = r2;
                r2++;
                while (r2 < e2.length && e2.charAt(r2) !== i2) {
                  if (e2.charAt(r2) === "\\") {
                    r2++;
                  }
                  r2++;
                }
                t2.push(e2.substr(n2, r2 - n2 + 1));
              } else {
                var a2 = e2.charAt(r2);
                t2.push(a2);
              }
              r2++;
            }
            return t2;
          }
          function Ve(e2, t2, r2) {
            return Fe.exec(e2.charAt(0)) && e2 !== "true" && e2 !== "false" && e2 !== "this" && e2 !== r2 && t2 !== ".";
          }
          function _e(e2, t2, r2) {
            if (t2[0] === "[") {
              t2.shift();
              var n2 = 1;
              var i2 = " return (function(" + r2 + "){ return (";
              var a2 = null;
              while (t2.length > 0) {
                var o2 = t2[0];
                if (o2 === "]") {
                  n2--;
                  if (n2 === 0) {
                    if (a2 === null) {
                      i2 = i2 + "true";
                    }
                    t2.shift();
                    i2 += ")})";
                    try {
                      var s2 = sr(e2, function() {
                        return Function(i2)();
                      }, function() {
                        return true;
                      });
                      s2.source = i2;
                      return s2;
                    } catch (e3) {
                      ne(J().body, "htmx:syntax:error", { error: e3, source: i2 });
                      return null;
                    }
                  }
                } else if (o2 === "[") {
                  n2++;
                }
                if (Ve(o2, a2, r2)) {
                  i2 += "((" + r2 + "." + o2 + ") ? (" + r2 + "." + o2 + ") : (window." + o2 + "))";
                } else {
                  i2 = i2 + o2;
                }
                a2 = t2.shift();
              }
            }
          }
          function m(e2, t2) {
            var r2 = "";
            while (e2.length > 0 && !e2[0].match(t2)) {
              r2 += e2.shift();
            }
            return r2;
          }
          var We = "input, textarea, select";
          function ze(e2) {
            var t2 = G(e2, "hx-trigger");
            var r2 = [];
            if (t2) {
              var n2 = Ue(t2);
              do {
                m(n2, p);
                var i2 = n2.length;
                var a2 = m(n2, /[,\[\s]/);
                if (a2 !== "") {
                  if (a2 === "every") {
                    var o2 = { trigger: "every" };
                    m(n2, p);
                    o2.pollInterval = v(m(n2, /[,\[\s]/));
                    m(n2, p);
                    var s2 = _e(e2, n2, "event");
                    if (s2) {
                      o2.eventFilter = s2;
                    }
                    r2.push(o2);
                  } else if (a2.indexOf("sse:") === 0) {
                    r2.push({ trigger: "sse", sseEvent: a2.substr(4) });
                  } else {
                    var l2 = { trigger: a2 };
                    var s2 = _e(e2, n2, "event");
                    if (s2) {
                      l2.eventFilter = s2;
                    }
                    while (n2.length > 0 && n2[0] !== ",") {
                      m(n2, p);
                      var u2 = n2.shift();
                      if (u2 === "changed") {
                        l2.changed = true;
                      } else if (u2 === "once") {
                        l2.once = true;
                      } else if (u2 === "consume") {
                        l2.consume = true;
                      } else if (u2 === "delay" && n2[0] === ":") {
                        n2.shift();
                        l2.delay = v(m(n2, g));
                      } else if (u2 === "from" && n2[0] === ":") {
                        n2.shift();
                        var f2 = m(n2, g);
                        if (f2 === "closest" || f2 === "find" || f2 === "next" || f2 === "previous") {
                          n2.shift();
                          f2 += " " + m(n2, g);
                        }
                        l2.from = f2;
                      } else if (u2 === "target" && n2[0] === ":") {
                        n2.shift();
                        l2.target = m(n2, g);
                      } else if (u2 === "throttle" && n2[0] === ":") {
                        n2.shift();
                        l2.throttle = v(m(n2, g));
                      } else if (u2 === "queue" && n2[0] === ":") {
                        n2.shift();
                        l2.queue = m(n2, g);
                      } else if ((u2 === "root" || u2 === "threshold") && n2[0] === ":") {
                        n2.shift();
                        l2[u2] = m(n2, g);
                      } else {
                        ne(e2, "htmx:syntax:error", { token: n2.shift() });
                      }
                    }
                    r2.push(l2);
                  }
                }
                if (n2.length === i2) {
                  ne(e2, "htmx:syntax:error", { token: n2.shift() });
                }
                m(n2, p);
              } while (n2[0] === "," && n2.shift());
            }
            if (r2.length > 0) {
              return r2;
            } else if (h(e2, "form")) {
              return [{ trigger: "submit" }];
            } else if (h(e2, 'input[type="button"]')) {
              return [{ trigger: "click" }];
            } else if (h(e2, We)) {
              return [{ trigger: "change" }];
            } else {
              return [{ trigger: "click" }];
            }
          }
          function $e(e2) {
            Y(e2).cancelled = true;
          }
          function Ge(e2, t2, r2) {
            var n2 = Y(e2);
            n2.timeout = setTimeout(function() {
              if (ee(e2) && n2.cancelled !== true) {
                if (!Qe(r2, Lt("hx:poll:trigger", { triggerSpec: r2, target: e2 }))) {
                  t2(e2);
                }
                Ge(e2, t2, r2);
              }
            }, r2.pollInterval);
          }
          function Je(e2) {
            return location.hostname === e2.hostname && $(e2, "href") && $(e2, "href").indexOf("#") !== 0;
          }
          function Ze(t2, r2, e2) {
            if (t2.tagName === "A" && Je(t2) && (t2.target === "" || t2.target === "_self") || t2.tagName === "FORM") {
              r2.boosted = true;
              var n2, i2;
              if (t2.tagName === "A") {
                n2 = "get";
                i2 = t2.href;
              } else {
                var a2 = $(t2, "method");
                n2 = a2 ? a2.toLowerCase() : "get";
                if (n2 === "get") {
                }
                i2 = $(t2, "action");
              }
              e2.forEach(function(e3) {
                et(t2, function(e4, t3) {
                  ae(n2, i2, e4, t3);
                }, r2, e3, true);
              });
            }
          }
          function Ke(e2, t2) {
            if (e2.type === "submit" || e2.type === "click") {
              if (t2.tagName === "FORM") {
                return true;
              }
              if (h(t2, 'input[type="submit"], button') && d(t2, "form") !== null) {
                return true;
              }
              if (t2.tagName === "A" && t2.href && (t2.getAttribute("href") === "#" || t2.getAttribute("href").indexOf("#") !== 0)) {
                return true;
              }
            }
            return false;
          }
          function Ye(e2, t2) {
            return Y(e2).boosted && e2.tagName === "A" && t2.type === "click" && (t2.ctrlKey || t2.metaKey);
          }
          function Qe(e2, t2) {
            var r2 = e2.eventFilter;
            if (r2) {
              try {
                return r2(t2) !== true;
              } catch (e3) {
                ne(J().body, "htmx:eventFilter:error", { error: e3, source: r2.source });
                return true;
              }
            }
            return false;
          }
          function et(i2, a2, e2, o2, s2) {
            var l2 = Y(i2);
            var t2;
            if (o2.from) {
              t2 = _(i2, o2.from);
            } else {
              t2 = [i2];
            }
            if (o2.changed) {
              l2.lastValue = i2.value;
            }
            Q(t2, function(r2) {
              var n2 = function(e3) {
                if (!ee(i2)) {
                  r2.removeEventListener(o2.trigger, n2);
                  return;
                }
                if (Ye(i2, e3)) {
                  return;
                }
                if (s2 || Ke(e3, i2)) {
                  e3.preventDefault();
                }
                if (Qe(o2, e3)) {
                  return;
                }
                var t3 = Y(e3);
                t3.triggerSpec = o2;
                if (t3.handledFor == null) {
                  t3.handledFor = [];
                }
                if (t3.handledFor.indexOf(i2) < 0) {
                  t3.handledFor.push(i2);
                  if (o2.consume) {
                    e3.stopPropagation();
                  }
                  if (o2.target && e3.target) {
                    if (!h(e3.target, o2.target)) {
                      return;
                    }
                  }
                  if (o2.once) {
                    if (l2.triggeredOnce) {
                      return;
                    } else {
                      l2.triggeredOnce = true;
                    }
                  }
                  if (o2.changed) {
                    if (l2.lastValue === i2.value) {
                      return;
                    } else {
                      l2.lastValue = i2.value;
                    }
                  }
                  if (l2.delayed) {
                    clearTimeout(l2.delayed);
                  }
                  if (l2.throttle) {
                    return;
                  }
                  if (o2.throttle) {
                    if (!l2.throttle) {
                      a2(i2, e3);
                      l2.throttle = setTimeout(function() {
                        l2.throttle = null;
                      }, o2.throttle);
                    }
                  } else if (o2.delay) {
                    l2.delayed = setTimeout(function() {
                      a2(i2, e3);
                    }, o2.delay);
                  } else {
                    ie(i2, "htmx:trigger");
                    a2(i2, e3);
                  }
                }
              };
              if (e2.listenerInfos == null) {
                e2.listenerInfos = [];
              }
              e2.listenerInfos.push({ trigger: o2.trigger, listener: n2, on: r2 });
              r2.addEventListener(o2.trigger, n2);
            });
          }
          var tt = false;
          var rt = null;
          function nt() {
            if (!rt) {
              rt = function() {
                tt = true;
              };
              window.addEventListener("scroll", rt);
              setInterval(function() {
                if (tt) {
                  tt = false;
                  Q(J().querySelectorAll("[hx-trigger='revealed'],[data-hx-trigger='revealed']"), function(e2) {
                    it(e2);
                  });
                }
              }, 200);
            }
          }
          function it(t2) {
            if (!q(t2, "data-hx-revealed") && P(t2)) {
              t2.setAttribute("data-hx-revealed", "true");
              var e2 = Y(t2);
              if (e2.initHash) {
                ie(t2, "revealed");
              } else {
                t2.addEventListener("htmx:afterProcessNode", function(e3) {
                  ie(t2, "revealed");
                }, { once: true });
              }
            }
          }
          function at(e2, t2, r2) {
            var n2 = M(r2);
            for (var i2 = 0; i2 < n2.length; i2++) {
              var a2 = n2[i2].split(/:(.+)/);
              if (a2[0] === "connect") {
                ot(e2, a2[1], 0);
              }
              if (a2[0] === "send") {
                lt(e2);
              }
            }
          }
          function ot(s2, r2, n2) {
            if (!ee(s2)) {
              return;
            }
            if (r2.indexOf("/") == 0) {
              var e2 = location.hostname + (location.port ? ":" + location.port : "");
              if (location.protocol == "https:") {
                r2 = "wss://" + e2 + r2;
              } else if (location.protocol == "http:") {
                r2 = "ws://" + e2 + r2;
              }
            }
            var t2 = z.createWebSocket(r2);
            t2.onerror = function(e3) {
              ne(s2, "htmx:wsError", { error: e3, socket: t2 });
              st(s2);
            };
            t2.onclose = function(e3) {
              if ([1006, 1012, 1013].indexOf(e3.code) >= 0) {
                var t3 = ut(n2);
                setTimeout(function() {
                  ot(s2, r2, n2 + 1);
                }, t3);
              }
            };
            t2.onopen = function(e3) {
              n2 = 0;
            };
            Y(s2).webSocket = t2;
            t2.addEventListener("message", function(e3) {
              if (st(s2)) {
                return;
              }
              var t3 = e3.data;
              w(s2, function(e4) {
                t3 = e4.transformResponse(t3, null, s2);
              });
              var r3 = S(s2);
              var n3 = l(t3);
              var i2 = k(n3.children);
              for (var a2 = 0; a2 < i2.length; a2++) {
                var o2 = i2[a2];
                me(G(o2, "hx-swap-oob") || "true", o2, r3);
              }
              Bt(r3.tasks);
            });
          }
          function st(e2) {
            if (!ee(e2)) {
              Y(e2).webSocket.close();
              return true;
            }
          }
          function lt(u2) {
            var f2 = c(u2, function(e2) {
              return Y(e2).webSocket != null;
            });
            if (f2) {
              u2.addEventListener(ze(u2)[0].trigger, function(e2) {
                var t2 = Y(f2).webSocket;
                var r2 = Qt(u2, f2);
                var n2 = Jt(u2, "post");
                var i2 = n2.errors;
                var a2 = n2.values;
                var o2 = fr(u2);
                var s2 = te(a2, o2);
                var l2 = er(s2, u2);
                l2["HEADERS"] = r2;
                if (i2 && i2.length > 0) {
                  ie(u2, "htmx:validation:halted", i2);
                  return;
                }
                t2.send(JSON.stringify(l2));
                if (Ke(e2, u2)) {
                  e2.preventDefault();
                }
              });
            } else {
              ne(u2, "htmx:noWebSocketSourceError");
            }
          }
          function ut(e2) {
            var t2 = z.config.wsReconnectDelay;
            if (typeof t2 === "function") {
              return t2(e2);
            }
            if (t2 === "full-jitter") {
              var r2 = Math.min(e2, 6);
              var n2 = 1e3 * Math.pow(2, r2);
              return n2 * Math.random();
            }
            x('htmx.config.wsReconnectDelay must either be a function or the string "full-jitter"');
          }
          function ft(e2, t2, r2) {
            var n2 = M(r2);
            for (var i2 = 0; i2 < n2.length; i2++) {
              var a2 = n2[i2].split(/:(.+)/);
              if (a2[0] === "connect") {
                ct(e2, a2[1]);
              }
              if (a2[0] === "swap") {
                ht(e2, a2[1]);
              }
            }
          }
          function ct(t2, e2) {
            var r2 = z.createEventSource(e2);
            r2.onerror = function(e3) {
              ne(t2, "htmx:sseError", { error: e3, source: r2 });
              vt(t2);
            };
            Y(t2).sseEventSource = r2;
          }
          function ht(a2, o2) {
            var s2 = c(a2, gt);
            if (s2) {
              var l2 = Y(s2).sseEventSource;
              var u2 = function(e2) {
                if (vt(s2)) {
                  l2.removeEventListener(o2, u2);
                  return;
                }
                var t2 = e2.data;
                w(a2, function(e3) {
                  t2 = e3.transformResponse(t2, null, a2);
                });
                var r2 = rr(a2);
                var n2 = de(a2);
                var i2 = S(a2);
                Me(r2.swapStyle, a2, n2, t2, i2);
                Bt(i2.tasks);
                ie(a2, "htmx:sseMessage", e2);
              };
              Y(a2).sseListener = u2;
              l2.addEventListener(o2, u2);
            } else {
              ne(a2, "htmx:noSSESourceError");
            }
          }
          function dt(e2, t2, r2) {
            var n2 = c(e2, gt);
            if (n2) {
              var i2 = Y(n2).sseEventSource;
              var a2 = function() {
                if (!vt(n2)) {
                  if (ee(e2)) {
                    t2(e2);
                  } else {
                    i2.removeEventListener(r2, a2);
                  }
                }
              };
              Y(e2).sseListener = a2;
              i2.addEventListener(r2, a2);
            } else {
              ne(e2, "htmx:noSSESourceError");
            }
          }
          function vt(e2) {
            if (!ee(e2)) {
              Y(e2).sseEventSource.close();
              return true;
            }
          }
          function gt(e2) {
            return Y(e2).sseEventSource != null;
          }
          function pt(e2, t2, r2, n2) {
            var i2 = function() {
              if (!r2.loaded) {
                r2.loaded = true;
                t2(e2);
              }
            };
            if (n2) {
              setTimeout(i2, n2);
            } else {
              i2();
            }
          }
          function mt(t2, i2, e2) {
            var a2 = false;
            Q(R, function(r2) {
              if (q(t2, "hx-" + r2)) {
                var n2 = G(t2, "hx-" + r2);
                a2 = true;
                i2.path = n2;
                i2.verb = r2;
                e2.forEach(function(e3) {
                  xt(t2, e3, i2, function(e4, t3) {
                    ae(r2, n2, e4, t3);
                  });
                });
              }
            });
            return a2;
          }
          function xt(n2, e2, t2, r2) {
            if (e2.sseEvent) {
              dt(n2, r2, e2.sseEvent);
            } else if (e2.trigger === "revealed") {
              nt();
              et(n2, r2, t2, e2);
              it(n2);
            } else if (e2.trigger === "intersect") {
              var i2 = {};
              if (e2.root) {
                i2.root = re(n2, e2.root);
              }
              if (e2.threshold) {
                i2.threshold = parseFloat(e2.threshold);
              }
              var a2 = new IntersectionObserver(function(e3) {
                for (var t3 = 0; t3 < e3.length; t3++) {
                  var r3 = e3[t3];
                  if (r3.isIntersecting) {
                    ie(n2, "intersect");
                    break;
                  }
                }
              }, i2);
              a2.observe(n2);
              et(n2, r2, t2, e2);
            } else if (e2.trigger === "load") {
              if (!Qe(e2, Lt("load", { elt: n2 }))) {
                pt(n2, r2, t2, e2.delay);
              }
            } else if (e2.pollInterval) {
              t2.polling = true;
              Ge(n2, r2, e2);
            } else {
              et(n2, r2, t2, e2);
            }
          }
          function yt(e2) {
            if (e2.type === "text/javascript" || e2.type === "module" || e2.type === "") {
              var t2 = J().createElement("script");
              Q(e2.attributes, function(e3) {
                t2.setAttribute(e3.name, e3.value);
              });
              t2.textContent = e2.textContent;
              t2.async = false;
              if (z.config.inlineScriptNonce) {
                t2.nonce = z.config.inlineScriptNonce;
              }
              var r2 = e2.parentElement;
              try {
                r2.insertBefore(t2, e2);
              } catch (e3) {
                x(e3);
              } finally {
                if (e2.parentElement) {
                  e2.parentElement.removeChild(e2);
                }
              }
            }
          }
          function bt(e2) {
            if (h(e2, "script")) {
              yt(e2);
            }
            Q(f(e2, "script"), function(e3) {
              yt(e3);
            });
          }
          function wt() {
            return document.querySelector("[hx-boost], [data-hx-boost]");
          }
          function St(e2) {
            if (e2.querySelectorAll) {
              var t2 = wt() ? ", a, form" : "";
              var r2 = e2.querySelectorAll(O + t2 + ", [hx-sse], [data-hx-sse], [hx-ws], [data-hx-ws], [hx-ext], [data-hx-ext], [hx-trigger], [data-hx-trigger], [hx-on], [data-hx-on]");
              return r2;
            } else {
              return [];
            }
          }
          function Et(n2) {
            var e2 = function(e3) {
              var t2 = d(e3.target, "button, input[type='submit']");
              if (t2 !== null) {
                var r2 = Y(n2);
                r2.lastButtonClicked = t2;
              }
            };
            n2.addEventListener("click", e2);
            n2.addEventListener("focusin", e2);
            n2.addEventListener("focusout", function(e3) {
              var t2 = Y(n2);
              t2.lastButtonClicked = null;
            });
          }
          function Ct(e2) {
            var t2 = Ue(e2);
            var r2 = 0;
            for (let e3 = 0; e3 < t2.length; e3++) {
              const n2 = t2[e3];
              if (n2 === "{") {
                r2++;
              } else if (n2 === "}") {
                r2--;
              }
            }
            return r2;
          }
          function Rt(t2, e2, r2) {
            var n2 = Y(t2);
            n2.onHandlers = [];
            var i2 = new Function("event", r2 + "; return;");
            var a2 = t2.addEventListener(e2, function(e3) {
              return i2.call(t2, e3);
            });
            n2.onHandlers.push({ event: e2, listener: a2 });
            return { nodeData: n2, code: r2, func: i2, listener: a2 };
          }
          function Ot(e2) {
            var t2 = G(e2, "hx-on");
            if (t2) {
              var r2 = {};
              var n2 = t2.split("\n");
              var i2 = null;
              var a2 = 0;
              while (n2.length > 0) {
                var o2 = n2.shift();
                var s2 = o2.match(/^\s*([a-zA-Z:\-]+:)(.*)/);
                if (a2 === 0 && s2) {
                  o2.split(":");
                  i2 = s2[1].slice(0, -1);
                  r2[i2] = s2[2];
                } else {
                  r2[i2] += o2;
                }
                a2 += Ct(o2);
              }
              for (var l2 in r2) {
                Rt(e2, l2, r2[l2]);
              }
            }
          }
          function qt(t2) {
            if (t2.closest && t2.closest(z.config.disableSelector)) {
              return;
            }
            var r2 = Y(t2);
            if (r2.initHash !== Ce(t2)) {
              r2.initHash = Ce(t2);
              Re(t2);
              Ot(t2);
              ie(t2, "htmx:beforeProcessNode");
              if (t2.value) {
                r2.lastValue = t2.value;
              }
              var e2 = ze(t2);
              var n2 = mt(t2, r2, e2);
              if (!n2) {
                if (Z(t2, "hx-boost") === "true") {
                  Ze(t2, r2, e2);
                } else if (q(t2, "hx-trigger")) {
                  e2.forEach(function(e3) {
                    xt(t2, e3, r2, function() {
                    });
                  });
                }
              }
              if (t2.tagName === "FORM") {
                Et(t2);
              }
              var i2 = G(t2, "hx-sse");
              if (i2) {
                ft(t2, r2, i2);
              }
              var a2 = G(t2, "hx-ws");
              if (a2) {
                at(t2, r2, a2);
              }
              ie(t2, "htmx:afterProcessNode");
            }
          }
          function Tt(e2) {
            e2 = s(e2);
            qt(e2);
            Q(St(e2), function(e3) {
              qt(e3);
            });
          }
          function Ht(e2) {
            return e2.replace(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase();
          }
          function Lt(e2, t2) {
            var r2;
            if (window.CustomEvent && typeof window.CustomEvent === "function") {
              r2 = new CustomEvent(e2, { bubbles: true, cancelable: true, detail: t2 });
            } else {
              r2 = J().createEvent("CustomEvent");
              r2.initCustomEvent(e2, true, true, t2);
            }
            return r2;
          }
          function ne(e2, t2, r2) {
            ie(e2, t2, te({ error: t2 }, r2));
          }
          function At(e2) {
            return e2 === "htmx:afterProcessNode";
          }
          function w(e2, t2) {
            Q(wr(e2), function(e3) {
              try {
                t2(e3);
              } catch (e4) {
                x(e4);
              }
            });
          }
          function x(e2) {
            if (console.error) {
              console.error(e2);
            } else if (console.log) {
              console.log("ERROR: ", e2);
            }
          }
          function ie(e2, t2, r2) {
            e2 = s(e2);
            if (r2 == null) {
              r2 = {};
            }
            r2["elt"] = e2;
            var n2 = Lt(t2, r2);
            if (z.logger && !At(t2)) {
              z.logger(e2, t2, r2);
            }
            if (r2.error) {
              x(r2.error);
              ie(e2, "htmx:error", { errorInfo: r2 });
            }
            var i2 = e2.dispatchEvent(n2);
            var a2 = Ht(t2);
            if (i2 && a2 !== t2) {
              var o2 = Lt(a2, n2.detail);
              i2 = i2 && e2.dispatchEvent(o2);
            }
            w(e2, function(e3) {
              i2 = i2 && e3.onEvent(t2, n2) !== false;
            });
            return i2;
          }
          var Nt = location.pathname + location.search;
          function It() {
            var e2 = J().querySelector("[hx-history-elt],[data-hx-history-elt]");
            return e2 || J().body;
          }
          function kt(e2, t2, r2, n2) {
            if (!D()) {
              return;
            }
            e2 = X(e2);
            var i2 = y(localStorage.getItem("htmx-history-cache")) || [];
            for (var a2 = 0; a2 < i2.length; a2++) {
              if (i2[a2].url === e2) {
                i2.splice(a2, 1);
                break;
              }
            }
            var o2 = { url: e2, content: t2, title: r2, scroll: n2 };
            ie(J().body, "htmx:historyItemCreated", { item: o2, cache: i2 });
            i2.push(o2);
            while (i2.length > z.config.historyCacheSize) {
              i2.shift();
            }
            while (i2.length > 0) {
              try {
                localStorage.setItem("htmx-history-cache", JSON.stringify(i2));
                break;
              } catch (e3) {
                ne(J().body, "htmx:historyCacheError", { cause: e3, cache: i2 });
                i2.shift();
              }
            }
          }
          function Pt(e2) {
            if (!D()) {
              return null;
            }
            e2 = X(e2);
            var t2 = y(localStorage.getItem("htmx-history-cache")) || [];
            for (var r2 = 0; r2 < t2.length; r2++) {
              if (t2[r2].url === e2) {
                return t2[r2];
              }
            }
            return null;
          }
          function Mt(e2) {
            var t2 = z.config.requestClass;
            var r2 = e2.cloneNode(true);
            Q(f(r2, "." + t2), function(e3) {
              n(e3, t2);
            });
            return r2.innerHTML;
          }
          function Dt() {
            var e2 = It();
            var t2 = Nt || location.pathname + location.search;
            var r2 = J().querySelector('[hx-history="false" i],[data-hx-history="false" i]');
            if (!r2) {
              ie(J().body, "htmx:beforeHistorySave", { path: t2, historyElt: e2 });
              kt(t2, Mt(e2), J().title, window.scrollY);
            }
            if (z.config.historyEnabled)
              history.replaceState({ htmx: true }, J().title, window.location.href);
          }
          function Xt(e2) {
            if (z.config.getCacheBusterParam) {
              e2 = e2.replace(/org\.htmx\.cache-buster=[^&]*&?/, "");
              if (e2.endsWith("&") || e2.endsWith("?")) {
                e2 = e2.slice(0, -1);
              }
            }
            if (z.config.historyEnabled) {
              history.pushState({ htmx: true }, "", e2);
            }
            Nt = e2;
          }
          function Ft(e2) {
            if (z.config.historyEnabled)
              history.replaceState({ htmx: true }, "", e2);
            Nt = e2;
          }
          function Bt(e2) {
            Q(e2, function(e3) {
              e3.call();
            });
          }
          function jt(a2) {
            var e2 = new XMLHttpRequest();
            var o2 = { path: a2, xhr: e2 };
            ie(J().body, "htmx:historyCacheMiss", o2);
            e2.open("GET", a2, true);
            e2.setRequestHeader("HX-History-Restore-Request", "true");
            e2.onload = function() {
              if (this.status >= 200 && this.status < 400) {
                ie(J().body, "htmx:historyCacheMissLoad", o2);
                var e3 = l(this.response);
                e3 = e3.querySelector("[hx-history-elt],[data-hx-history-elt]") || e3;
                var t2 = It();
                var r2 = S(t2);
                var n2 = Pe(this.response);
                if (n2) {
                  var i2 = b("title");
                  if (i2) {
                    i2.innerHTML = n2;
                  } else {
                    window.document.title = n2;
                  }
                }
                Ne(t2, e3, r2);
                Bt(r2.tasks);
                Nt = a2;
                ie(J().body, "htmx:historyRestore", { path: a2, cacheMiss: true, serverResponse: this.response });
              } else {
                ne(J().body, "htmx:historyCacheMissLoadError", o2);
              }
            };
            e2.send();
          }
          function Ut(e2) {
            Dt();
            e2 = e2 || location.pathname + location.search;
            var t2 = Pt(e2);
            if (t2) {
              var r2 = l(t2.content);
              var n2 = It();
              var i2 = S(n2);
              Ne(n2, r2, i2);
              Bt(i2.tasks);
              document.title = t2.title;
              window.scrollTo(0, t2.scroll);
              Nt = e2;
              ie(J().body, "htmx:historyRestore", { path: e2, item: t2 });
            } else {
              if (z.config.refreshOnHistoryMiss) {
                window.location.reload(true);
              } else {
                jt(e2);
              }
            }
          }
          function Vt(e2) {
            var t2 = ce(e2, "hx-indicator");
            if (t2 == null) {
              t2 = [e2];
            }
            Q(t2, function(e3) {
              var t3 = Y(e3);
              t3.requestCount = (t3.requestCount || 0) + 1;
              e3.classList["add"].call(e3.classList, z.config.requestClass);
            });
            return t2;
          }
          function _t(e2) {
            Q(e2, function(e3) {
              var t2 = Y(e3);
              t2.requestCount = (t2.requestCount || 0) - 1;
              if (t2.requestCount === 0) {
                e3.classList["remove"].call(e3.classList, z.config.requestClass);
              }
            });
          }
          function Wt(e2, t2) {
            for (var r2 = 0; r2 < e2.length; r2++) {
              var n2 = e2[r2];
              if (n2.isSameNode(t2)) {
                return true;
              }
            }
            return false;
          }
          function zt(e2) {
            if (e2.name === "" || e2.name == null || e2.disabled) {
              return false;
            }
            if (e2.type === "button" || e2.type === "submit" || e2.tagName === "image" || e2.tagName === "reset" || e2.tagName === "file") {
              return false;
            }
            if (e2.type === "checkbox" || e2.type === "radio") {
              return e2.checked;
            }
            return true;
          }
          function $t(t2, r2, n2, e2, i2) {
            if (e2 == null || Wt(t2, e2)) {
              return;
            } else {
              t2.push(e2);
            }
            if (zt(e2)) {
              var a2 = $(e2, "name");
              var o2 = e2.value;
              if (e2.multiple) {
                o2 = k(e2.querySelectorAll("option:checked")).map(function(e3) {
                  return e3.value;
                });
              }
              if (e2.files) {
                o2 = k(e2.files);
              }
              if (a2 != null && o2 != null) {
                var s2 = r2[a2];
                if (s2 !== void 0) {
                  if (Array.isArray(s2)) {
                    if (Array.isArray(o2)) {
                      r2[a2] = s2.concat(o2);
                    } else {
                      s2.push(o2);
                    }
                  } else {
                    if (Array.isArray(o2)) {
                      r2[a2] = [s2].concat(o2);
                    } else {
                      r2[a2] = [s2, o2];
                    }
                  }
                } else {
                  r2[a2] = o2;
                }
              }
              if (i2) {
                Gt(e2, n2);
              }
            }
            if (h(e2, "form")) {
              var l2 = e2.elements;
              Q(l2, function(e3) {
                $t(t2, r2, n2, e3, i2);
              });
            }
          }
          function Gt(e2, t2) {
            if (e2.willValidate) {
              ie(e2, "htmx:validation:validate");
              if (!e2.checkValidity()) {
                t2.push({ elt: e2, message: e2.validationMessage, validity: e2.validity });
                ie(e2, "htmx:validation:failed", { message: e2.validationMessage, validity: e2.validity });
              }
            }
          }
          function Jt(e2, t2) {
            var r2 = [];
            var n2 = {};
            var i2 = {};
            var a2 = [];
            var o2 = Y(e2);
            var s2 = h(e2, "form") && e2.noValidate !== true || G(e2, "hx-validate") === "true";
            if (o2.lastButtonClicked) {
              s2 = s2 && o2.lastButtonClicked.formNoValidate !== true;
            }
            if (t2 !== "get") {
              $t(r2, i2, a2, d(e2, "form"), s2);
            }
            $t(r2, n2, a2, e2, s2);
            if (o2.lastButtonClicked) {
              var l2 = $(o2.lastButtonClicked, "name");
              if (l2) {
                n2[l2] = o2.lastButtonClicked.value;
              }
            }
            var u2 = ce(e2, "hx-include");
            Q(u2, function(e3) {
              $t(r2, n2, a2, e3, s2);
              if (!h(e3, "form")) {
                Q(e3.querySelectorAll(We), function(e4) {
                  $t(r2, n2, a2, e4, s2);
                });
              }
            });
            n2 = te(n2, i2);
            return { errors: a2, values: n2 };
          }
          function Zt(e2, t2, r2) {
            if (e2 !== "") {
              e2 += "&";
            }
            if (String(r2) === "[object Object]") {
              r2 = JSON.stringify(r2);
            }
            var n2 = encodeURIComponent(r2);
            e2 += encodeURIComponent(t2) + "=" + n2;
            return e2;
          }
          function Kt(e2) {
            var t2 = "";
            for (var r2 in e2) {
              if (e2.hasOwnProperty(r2)) {
                var n2 = e2[r2];
                if (Array.isArray(n2)) {
                  Q(n2, function(e3) {
                    t2 = Zt(t2, r2, e3);
                  });
                } else {
                  t2 = Zt(t2, r2, n2);
                }
              }
            }
            return t2;
          }
          function Yt(e2) {
            var t2 = new FormData();
            for (var r2 in e2) {
              if (e2.hasOwnProperty(r2)) {
                var n2 = e2[r2];
                if (Array.isArray(n2)) {
                  Q(n2, function(e3) {
                    t2.append(r2, e3);
                  });
                } else {
                  t2.append(r2, n2);
                }
              }
            }
            return t2;
          }
          function Qt(e2, t2, r2) {
            var n2 = { "HX-Request": "true", "HX-Trigger": $(e2, "id"), "HX-Trigger-Name": $(e2, "name"), "HX-Target": G(t2, "id"), "HX-Current-URL": J().location.href };
            or(e2, "hx-headers", false, n2);
            if (r2 !== void 0) {
              n2["HX-Prompt"] = r2;
            }
            if (Y(e2).boosted) {
              n2["HX-Boosted"] = "true";
            }
            return n2;
          }
          function er(t2, e2) {
            var r2 = Z(e2, "hx-params");
            if (r2) {
              if (r2 === "none") {
                return {};
              } else if (r2 === "*") {
                return t2;
              } else if (r2.indexOf("not ") === 0) {
                Q(r2.substr(4).split(","), function(e3) {
                  e3 = e3.trim();
                  delete t2[e3];
                });
                return t2;
              } else {
                var n2 = {};
                Q(r2.split(","), function(e3) {
                  e3 = e3.trim();
                  n2[e3] = t2[e3];
                });
                return n2;
              }
            } else {
              return t2;
            }
          }
          function tr(e2) {
            return $(e2, "href") && $(e2, "href").indexOf("#") >= 0;
          }
          function rr(e2, t2) {
            var r2 = t2 ? t2 : Z(e2, "hx-swap");
            var n2 = { swapStyle: Y(e2).boosted ? "innerHTML" : z.config.defaultSwapStyle, swapDelay: z.config.defaultSwapDelay, settleDelay: z.config.defaultSettleDelay };
            if (Y(e2).boosted && !tr(e2)) {
              n2["show"] = "top";
            }
            if (r2) {
              var i2 = M(r2);
              if (i2.length > 0) {
                n2["swapStyle"] = i2[0];
                for (var a2 = 1; a2 < i2.length; a2++) {
                  var o2 = i2[a2];
                  if (o2.indexOf("swap:") === 0) {
                    n2["swapDelay"] = v(o2.substr(5));
                  }
                  if (o2.indexOf("settle:") === 0) {
                    n2["settleDelay"] = v(o2.substr(7));
                  }
                  if (o2.indexOf("transition:") === 0) {
                    n2["transition"] = o2.substr(11) === "true";
                  }
                  if (o2.indexOf("scroll:") === 0) {
                    var s2 = o2.substr(7);
                    var l2 = s2.split(":");
                    var u2 = l2.pop();
                    var f2 = l2.length > 0 ? l2.join(":") : null;
                    n2["scroll"] = u2;
                    n2["scrollTarget"] = f2;
                  }
                  if (o2.indexOf("show:") === 0) {
                    var c2 = o2.substr(5);
                    var l2 = c2.split(":");
                    var h2 = l2.pop();
                    var f2 = l2.length > 0 ? l2.join(":") : null;
                    n2["show"] = h2;
                    n2["showTarget"] = f2;
                  }
                  if (o2.indexOf("focus-scroll:") === 0) {
                    var d2 = o2.substr("focus-scroll:".length);
                    n2["focusScroll"] = d2 == "true";
                  }
                }
              }
            }
            return n2;
          }
          function nr(e2) {
            return Z(e2, "hx-encoding") === "multipart/form-data" || h(e2, "form") && $(e2, "enctype") === "multipart/form-data";
          }
          function ir(t2, r2, n2) {
            var i2 = null;
            w(r2, function(e2) {
              if (i2 == null) {
                i2 = e2.encodeParameters(t2, n2, r2);
              }
            });
            if (i2 != null) {
              return i2;
            } else {
              if (nr(r2)) {
                return Yt(n2);
              } else {
                return Kt(n2);
              }
            }
          }
          function S(e2) {
            return { tasks: [], elts: [e2] };
          }
          function ar(e2, t2) {
            var r2 = e2[0];
            var n2 = e2[e2.length - 1];
            if (t2.scroll) {
              var i2 = null;
              if (t2.scrollTarget) {
                i2 = re(r2, t2.scrollTarget);
              }
              if (t2.scroll === "top" && (r2 || i2)) {
                i2 = i2 || r2;
                i2.scrollTop = 0;
              }
              if (t2.scroll === "bottom" && (n2 || i2)) {
                i2 = i2 || n2;
                i2.scrollTop = i2.scrollHeight;
              }
            }
            if (t2.show) {
              var i2 = null;
              if (t2.showTarget) {
                var a2 = t2.showTarget;
                if (t2.showTarget === "window") {
                  a2 = "body";
                }
                i2 = re(r2, a2);
              }
              if (t2.show === "top" && (r2 || i2)) {
                i2 = i2 || r2;
                i2.scrollIntoView({ block: "start", behavior: z.config.scrollBehavior });
              }
              if (t2.show === "bottom" && (n2 || i2)) {
                i2 = i2 || n2;
                i2.scrollIntoView({ block: "end", behavior: z.config.scrollBehavior });
              }
            }
          }
          function or(e2, t2, r2, n2) {
            if (n2 == null) {
              n2 = {};
            }
            if (e2 == null) {
              return n2;
            }
            var i2 = G(e2, t2);
            if (i2) {
              var a2 = i2.trim();
              var o2 = r2;
              if (a2 === "unset") {
                return null;
              }
              if (a2.indexOf("javascript:") === 0) {
                a2 = a2.substr(11);
                o2 = true;
              } else if (a2.indexOf("js:") === 0) {
                a2 = a2.substr(3);
                o2 = true;
              }
              if (a2.indexOf("{") !== 0) {
                a2 = "{" + a2 + "}";
              }
              var s2;
              if (o2) {
                s2 = sr(e2, function() {
                  return Function("return (" + a2 + ")")();
                }, {});
              } else {
                s2 = y(a2);
              }
              for (var l2 in s2) {
                if (s2.hasOwnProperty(l2)) {
                  if (n2[l2] == null) {
                    n2[l2] = s2[l2];
                  }
                }
              }
            }
            return or(u(e2), t2, r2, n2);
          }
          function sr(e2, t2, r2) {
            if (z.config.allowEval) {
              return t2();
            } else {
              ne(e2, "htmx:evalDisallowedError");
              return r2;
            }
          }
          function lr(e2, t2) {
            return or(e2, "hx-vars", true, t2);
          }
          function ur(e2, t2) {
            return or(e2, "hx-vals", false, t2);
          }
          function fr(e2) {
            return te(lr(e2), ur(e2));
          }
          function cr(t2, r2, n2) {
            if (n2 !== null) {
              try {
                t2.setRequestHeader(r2, n2);
              } catch (e2) {
                t2.setRequestHeader(r2, encodeURIComponent(n2));
                t2.setRequestHeader(r2 + "-URI-AutoEncoded", "true");
              }
            }
          }
          function hr(t2) {
            if (t2.responseURL && typeof URL !== "undefined") {
              try {
                var e2 = new URL(t2.responseURL);
                return e2.pathname + e2.search;
              } catch (e3) {
                ne(J().body, "htmx:badResponseUrl", { url: t2.responseURL });
              }
            }
          }
          function E(e2, t2) {
            return e2.getAllResponseHeaders().match(t2);
          }
          function dr(e2, t2, r2) {
            e2 = e2.toLowerCase();
            if (r2) {
              if (r2 instanceof Element || A(r2, "String")) {
                return ae(e2, t2, null, null, { targetOverride: s(r2), returnPromise: true });
              } else {
                return ae(e2, t2, s(r2.source), r2.event, { handler: r2.handler, headers: r2.headers, values: r2.values, targetOverride: s(r2.target), swapOverride: r2.swap, returnPromise: true });
              }
            } else {
              return ae(e2, t2, null, null, { returnPromise: true });
            }
          }
          function vr(e2) {
            var t2 = [];
            while (e2) {
              t2.push(e2);
              e2 = e2.parentElement;
            }
            return t2;
          }
          function ae(e2, t2, n2, r2, i2, M2) {
            var a2 = null;
            var o2 = null;
            i2 = i2 != null ? i2 : {};
            if (i2.returnPromise && typeof Promise !== "undefined") {
              var s2 = new Promise(function(e3, t3) {
                a2 = e3;
                o2 = t3;
              });
            }
            if (n2 == null) {
              n2 = J().body;
            }
            var D2 = i2.handler || pr;
            if (!ee(n2)) {
              return;
            }
            var l2 = i2.targetOverride || de(n2);
            if (l2 == null || l2 == fe) {
              ne(n2, "htmx:targetError", { target: G(n2, "hx-target") });
              return;
            }
            if (!M2) {
              var X2 = function() {
                return ae(e2, t2, n2, r2, i2, true);
              };
              var F2 = { target: l2, elt: n2, path: t2, verb: e2, triggeringEvent: r2, etc: i2, issueRequest: X2 };
              if (ie(n2, "htmx:confirm", F2) === false) {
                return;
              }
            }
            var u2 = n2;
            var f2 = Y(n2);
            var c2 = Z(n2, "hx-sync");
            var h2 = null;
            var d2 = false;
            if (c2) {
              var v2 = c2.split(":");
              var g2 = v2[0].trim();
              if (g2 === "this") {
                u2 = he(n2, "hx-sync");
              } else {
                u2 = re(n2, g2);
              }
              c2 = (v2[1] || "drop").trim();
              f2 = Y(u2);
              if (c2 === "drop" && f2.xhr && f2.abortable !== true) {
                return;
              } else if (c2 === "abort") {
                if (f2.xhr) {
                  return;
                } else {
                  d2 = true;
                }
              } else if (c2 === "replace") {
                ie(u2, "htmx:abort");
              } else if (c2.indexOf("queue") === 0) {
                var B2 = c2.split(" ");
                h2 = (B2[1] || "last").trim();
              }
            }
            if (f2.xhr) {
              if (f2.abortable) {
                ie(u2, "htmx:abort");
              } else {
                if (h2 == null) {
                  if (r2) {
                    var p2 = Y(r2);
                    if (p2 && p2.triggerSpec && p2.triggerSpec.queue) {
                      h2 = p2.triggerSpec.queue;
                    }
                  }
                  if (h2 == null) {
                    h2 = "last";
                  }
                }
                if (f2.queuedRequests == null) {
                  f2.queuedRequests = [];
                }
                if (h2 === "first" && f2.queuedRequests.length === 0) {
                  f2.queuedRequests.push(function() {
                    ae(e2, t2, n2, r2, i2);
                  });
                } else if (h2 === "all") {
                  f2.queuedRequests.push(function() {
                    ae(e2, t2, n2, r2, i2);
                  });
                } else if (h2 === "last") {
                  f2.queuedRequests = [];
                  f2.queuedRequests.push(function() {
                    ae(e2, t2, n2, r2, i2);
                  });
                }
                return;
              }
            }
            var m2 = new XMLHttpRequest();
            f2.xhr = m2;
            f2.abortable = d2;
            var x2 = function() {
              f2.xhr = null;
              f2.abortable = false;
              if (f2.queuedRequests != null && f2.queuedRequests.length > 0) {
                var e3 = f2.queuedRequests.shift();
                e3();
              }
            };
            var y2 = Z(n2, "hx-prompt");
            if (y2) {
              var b2 = prompt(y2);
              if (b2 === null || !ie(n2, "htmx:prompt", { prompt: b2, target: l2 })) {
                K(a2);
                x2();
                return s2;
              }
            }
            var w2 = Z(n2, "hx-confirm");
            if (w2) {
              if (!confirm(w2)) {
                K(a2);
                x2();
                return s2;
              }
            }
            var S2 = Qt(n2, l2, b2);
            if (i2.headers) {
              S2 = te(S2, i2.headers);
            }
            var E2 = Jt(n2, e2);
            var C2 = E2.errors;
            var R2 = E2.values;
            if (i2.values) {
              R2 = te(R2, i2.values);
            }
            var j2 = fr(n2);
            var O2 = te(R2, j2);
            var q2 = er(O2, n2);
            if (e2 !== "get" && !nr(n2)) {
              S2["Content-Type"] = "application/x-www-form-urlencoded";
            }
            if (z.config.getCacheBusterParam && e2 === "get") {
              q2["org.htmx.cache-buster"] = $(l2, "id") || "true";
            }
            if (t2 == null || t2 === "") {
              t2 = J().location.href;
            }
            var T2 = or(n2, "hx-request");
            var H2 = Y(n2).boosted;
            var L2 = { boosted: H2, parameters: q2, unfilteredParameters: O2, headers: S2, target: l2, verb: e2, errors: C2, withCredentials: i2.credentials || T2.credentials || z.config.withCredentials, timeout: i2.timeout || T2.timeout || z.config.timeout, path: t2, triggeringEvent: r2 };
            if (!ie(n2, "htmx:configRequest", L2)) {
              K(a2);
              x2();
              return s2;
            }
            t2 = L2.path;
            e2 = L2.verb;
            S2 = L2.headers;
            q2 = L2.parameters;
            C2 = L2.errors;
            if (C2 && C2.length > 0) {
              ie(n2, "htmx:validation:halted", L2);
              K(a2);
              x2();
              return s2;
            }
            var U2 = t2.split("#");
            var V2 = U2[0];
            var A2 = U2[1];
            var N2 = null;
            if (e2 === "get") {
              N2 = V2;
              var _2 = Object.keys(q2).length !== 0;
              if (_2) {
                if (N2.indexOf("?") < 0) {
                  N2 += "?";
                } else {
                  N2 += "&";
                }
                N2 += Kt(q2);
                if (A2) {
                  N2 += "#" + A2;
                }
              }
              m2.open("GET", N2, true);
            } else {
              m2.open(e2.toUpperCase(), t2, true);
            }
            m2.overrideMimeType("text/html");
            m2.withCredentials = L2.withCredentials;
            m2.timeout = L2.timeout;
            if (T2.noHeaders) {
            } else {
              for (var I2 in S2) {
                if (S2.hasOwnProperty(I2)) {
                  var W2 = S2[I2];
                  cr(m2, I2, W2);
                }
              }
            }
            var k2 = { xhr: m2, target: l2, requestConfig: L2, etc: i2, boosted: H2, pathInfo: { requestPath: t2, finalRequestPath: N2 || t2, anchor: A2 } };
            m2.onload = function() {
              try {
                var e3 = vr(n2);
                k2.pathInfo.responsePath = hr(m2);
                D2(n2, k2);
                _t(P2);
                ie(n2, "htmx:afterRequest", k2);
                ie(n2, "htmx:afterOnLoad", k2);
                if (!ee(n2)) {
                  var t3 = null;
                  while (e3.length > 0 && t3 == null) {
                    var r3 = e3.shift();
                    if (ee(r3)) {
                      t3 = r3;
                    }
                  }
                  if (t3) {
                    ie(t3, "htmx:afterRequest", k2);
                    ie(t3, "htmx:afterOnLoad", k2);
                  }
                }
                K(a2);
                x2();
              } catch (e4) {
                ne(n2, "htmx:onLoadError", te({ error: e4 }, k2));
                throw e4;
              }
            };
            m2.onerror = function() {
              _t(P2);
              ne(n2, "htmx:afterRequest", k2);
              ne(n2, "htmx:sendError", k2);
              K(o2);
              x2();
            };
            m2.onabort = function() {
              _t(P2);
              ne(n2, "htmx:afterRequest", k2);
              ne(n2, "htmx:sendAbort", k2);
              K(o2);
              x2();
            };
            m2.ontimeout = function() {
              _t(P2);
              ne(n2, "htmx:afterRequest", k2);
              ne(n2, "htmx:timeout", k2);
              K(o2);
              x2();
            };
            if (!ie(n2, "htmx:beforeRequest", k2)) {
              K(a2);
              x2();
              return s2;
            }
            var P2 = Vt(n2);
            Q(["loadstart", "loadend", "progress", "abort"], function(t3) {
              Q([m2, m2.upload], function(e3) {
                e3.addEventListener(t3, function(e4) {
                  ie(n2, "htmx:xhr:" + t3, { lengthComputable: e4.lengthComputable, loaded: e4.loaded, total: e4.total });
                });
              });
            });
            ie(n2, "htmx:beforeSend", k2);
            m2.send(e2 === "get" ? null : ir(m2, n2, q2));
            return s2;
          }
          function gr(e2, t2) {
            var r2 = t2.xhr;
            var n2 = null;
            var i2 = null;
            if (E(r2, /HX-Push:/i)) {
              n2 = r2.getResponseHeader("HX-Push");
              i2 = "push";
            } else if (E(r2, /HX-Push-Url:/i)) {
              n2 = r2.getResponseHeader("HX-Push-Url");
              i2 = "push";
            } else if (E(r2, /HX-Replace-Url:/i)) {
              n2 = r2.getResponseHeader("HX-Replace-Url");
              i2 = "replace";
            }
            if (n2) {
              if (n2 === "false") {
                return {};
              } else {
                return { type: i2, path: n2 };
              }
            }
            var a2 = t2.pathInfo.finalRequestPath;
            var o2 = t2.pathInfo.responsePath;
            var s2 = Z(e2, "hx-push-url");
            var l2 = Z(e2, "hx-replace-url");
            var u2 = Y(e2).boosted;
            var f2 = null;
            var c2 = null;
            if (s2) {
              f2 = "push";
              c2 = s2;
            } else if (l2) {
              f2 = "replace";
              c2 = l2;
            } else if (u2) {
              f2 = "push";
              c2 = o2 || a2;
            }
            if (c2) {
              if (c2 === "false") {
                return {};
              }
              if (c2 === "true") {
                c2 = o2 || a2;
              }
              if (t2.pathInfo.anchor && c2.indexOf("#") === -1) {
                c2 = c2 + "#" + t2.pathInfo.anchor;
              }
              return { type: f2, path: c2 };
            } else {
              return {};
            }
          }
          function pr(s2, l2) {
            var u2 = l2.xhr;
            var f2 = l2.target;
            var e2 = l2.etc;
            if (!ie(s2, "htmx:beforeOnLoad", l2))
              return;
            if (E(u2, /HX-Trigger:/i)) {
              De(u2, "HX-Trigger", s2);
            }
            if (E(u2, /HX-Location:/i)) {
              Dt();
              var t2 = u2.getResponseHeader("HX-Location");
              var c2;
              if (t2.indexOf("{") === 0) {
                c2 = y(t2);
                t2 = c2["path"];
                delete c2["path"];
              }
              dr("GET", t2, c2).then(function() {
                Xt(t2);
              });
              return;
            }
            if (E(u2, /HX-Redirect:/i)) {
              location.href = u2.getResponseHeader("HX-Redirect");
              return;
            }
            if (E(u2, /HX-Refresh:/i)) {
              if ("true" === u2.getResponseHeader("HX-Refresh")) {
                location.reload();
                return;
              }
            }
            if (E(u2, /HX-Retarget:/i)) {
              l2.target = J().querySelector(u2.getResponseHeader("HX-Retarget"));
            }
            var h2 = gr(s2, l2);
            var r2 = u2.status >= 200 && u2.status < 400 && u2.status !== 204;
            var d2 = u2.response;
            var n2 = u2.status >= 400;
            var i2 = te({ shouldSwap: r2, serverResponse: d2, isError: n2 }, l2);
            if (!ie(f2, "htmx:beforeSwap", i2))
              return;
            f2 = i2.target;
            d2 = i2.serverResponse;
            n2 = i2.isError;
            l2.target = f2;
            l2.failed = n2;
            l2.successful = !n2;
            if (i2.shouldSwap) {
              if (u2.status === 286) {
                $e(s2);
              }
              w(s2, function(e3) {
                d2 = e3.transformResponse(d2, u2, s2);
              });
              if (h2.type) {
                Dt();
              }
              var a2 = e2.swapOverride;
              if (E(u2, /HX-Reswap:/i)) {
                a2 = u2.getResponseHeader("HX-Reswap");
              }
              var c2 = rr(s2, a2);
              f2.classList.add(z.config.swappingClass);
              var v2 = null;
              var g2 = null;
              var o2 = function() {
                try {
                  var e3 = document.activeElement;
                  var t3 = {};
                  try {
                    t3 = { elt: e3, start: e3 ? e3.selectionStart : null, end: e3 ? e3.selectionEnd : null };
                  } catch (e4) {
                  }
                  var n3 = S(f2);
                  Me(c2.swapStyle, f2, s2, d2, n3);
                  if (t3.elt && !ee(t3.elt) && t3.elt.id) {
                    var r3 = document.getElementById(t3.elt.id);
                    var i3 = { preventScroll: c2.focusScroll !== void 0 ? !c2.focusScroll : !z.config.defaultFocusScroll };
                    if (r3) {
                      if (t3.start && r3.setSelectionRange) {
                        try {
                          r3.setSelectionRange(t3.start, t3.end);
                        } catch (e4) {
                        }
                      }
                      r3.focus(i3);
                    }
                  }
                  f2.classList.remove(z.config.swappingClass);
                  Q(n3.elts, function(e4) {
                    if (e4.classList) {
                      e4.classList.add(z.config.settlingClass);
                    }
                    ie(e4, "htmx:afterSwap", l2);
                  });
                  if (E(u2, /HX-Trigger-After-Swap:/i)) {
                    var a3 = s2;
                    if (!ee(s2)) {
                      a3 = J().body;
                    }
                    De(u2, "HX-Trigger-After-Swap", a3);
                  }
                  var o3 = function() {
                    Q(n3.tasks, function(e5) {
                      e5.call();
                    });
                    Q(n3.elts, function(e5) {
                      if (e5.classList) {
                        e5.classList.remove(z.config.settlingClass);
                      }
                      ie(e5, "htmx:afterSettle", l2);
                    });
                    if (h2.type) {
                      if (h2.type === "push") {
                        Xt(h2.path);
                        ie(J().body, "htmx:pushedIntoHistory", { path: h2.path });
                      } else {
                        Ft(h2.path);
                        ie(J().body, "htmx:replacedInHistory", { path: h2.path });
                      }
                    }
                    if (l2.pathInfo.anchor) {
                      var e4 = b("#" + l2.pathInfo.anchor);
                      if (e4) {
                        e4.scrollIntoView({ block: "start", behavior: "auto" });
                      }
                    }
                    if (n3.title) {
                      var t4 = b("title");
                      if (t4) {
                        t4.innerHTML = n3.title;
                      } else {
                        window.document.title = n3.title;
                      }
                    }
                    ar(n3.elts, c2);
                    if (E(u2, /HX-Trigger-After-Settle:/i)) {
                      var r4 = s2;
                      if (!ee(s2)) {
                        r4 = J().body;
                      }
                      De(u2, "HX-Trigger-After-Settle", r4);
                    }
                    K(v2);
                  };
                  if (c2.settleDelay > 0) {
                    setTimeout(o3, c2.settleDelay);
                  } else {
                    o3();
                  }
                } catch (e4) {
                  ne(s2, "htmx:swapError", l2);
                  K(g2);
                  throw e4;
                }
              };
              var p2 = z.config.globalViewTransitions;
              if (c2.hasOwnProperty("transition")) {
                p2 = c2.transition;
              }
              if (p2 && ie(s2, "htmx:beforeTransition", l2) && typeof Promise !== "undefined" && document.startViewTransition) {
                var m2 = new Promise(function(e3, t3) {
                  v2 = e3;
                  g2 = t3;
                });
                var x2 = o2;
                o2 = function() {
                  document.startViewTransition(function() {
                    x2();
                    return m2;
                  });
                };
              }
              if (c2.swapDelay > 0) {
                setTimeout(o2, c2.swapDelay);
              } else {
                o2();
              }
            }
            if (n2) {
              ne(s2, "htmx:responseError", te({ error: "Response Status Error Code " + u2.status + " from " + l2.pathInfo.requestPath }, l2));
            }
          }
          var mr = {};
          function xr() {
            return { init: function(e2) {
              return null;
            }, onEvent: function(e2, t2) {
              return true;
            }, transformResponse: function(e2, t2, r2) {
              return e2;
            }, isInlineSwap: function(e2) {
              return false;
            }, handleSwap: function(e2, t2, r2, n2) {
              return false;
            }, encodeParameters: function(e2, t2, r2) {
              return null;
            } };
          }
          function yr(e2, t2) {
            if (t2.init) {
              t2.init(C);
            }
            mr[e2] = te(xr(), t2);
          }
          function br(e2) {
            delete mr[e2];
          }
          function wr(e2, r2, n2) {
            if (e2 == void 0) {
              return r2;
            }
            if (r2 == void 0) {
              r2 = [];
            }
            if (n2 == void 0) {
              n2 = [];
            }
            var t2 = G(e2, "hx-ext");
            if (t2) {
              Q(t2.split(","), function(e3) {
                e3 = e3.replace(/ /g, "");
                if (e3.slice(0, 7) == "ignore:") {
                  n2.push(e3.slice(7));
                  return;
                }
                if (n2.indexOf(e3) < 0) {
                  var t3 = mr[e3];
                  if (t3 && r2.indexOf(t3) < 0) {
                    r2.push(t3);
                  }
                }
              });
            }
            return wr(u(e2), r2, n2);
          }
          function Sr(e2) {
            if (J().readyState !== "loading") {
              e2();
            } else {
              J().addEventListener("DOMContentLoaded", e2);
            }
          }
          function Er() {
            if (z.config.includeIndicatorStyles !== false) {
              J().head.insertAdjacentHTML("beforeend", "<style>                      ." + z.config.indicatorClass + "{opacity:0;transition: opacity 200ms ease-in;}                      ." + z.config.requestClass + " ." + z.config.indicatorClass + "{opacity:1}                      ." + z.config.requestClass + "." + z.config.indicatorClass + "{opacity:1}                    </style>");
            }
          }
          function Cr() {
            var e2 = J().querySelector('meta[name="htmx-config"]');
            if (e2) {
              return y(e2.content);
            } else {
              return null;
            }
          }
          function Rr() {
            var e2 = Cr();
            if (e2) {
              z.config = te(z.config, e2);
            }
          }
          Sr(function() {
            Rr();
            Er();
            var e2 = J().body;
            Tt(e2);
            var t2 = J().querySelectorAll("[hx-trigger='restored'],[data-hx-trigger='restored']");
            e2.addEventListener("htmx:abort", function(e3) {
              var t3 = e3.target;
              var r3 = Y(t3);
              if (r3 && r3.xhr) {
                r3.xhr.abort();
              }
            });
            var r2 = window.onpopstate;
            window.onpopstate = function(e3) {
              if (e3.state && e3.state.htmx) {
                Ut();
                Q(t2, function(e4) {
                  ie(e4, "htmx:restored", { document: J(), triggerEvent: ie });
                });
              } else {
                if (r2) {
                  r2(e3);
                }
              }
            };
            setTimeout(function() {
              ie(e2, "htmx:load", {});
              e2 = null;
            }, 0);
          });
          return z;
        }();
      });
    }
  });

  // node_modules/alpinejs/dist/module.esm.js
  var flushPending = false;
  var flushing = false;
  var queue = [];
  var lastFlushedIndex = -1;
  function scheduler(callback) {
    queueJob(callback);
  }
  function queueJob(job) {
    if (!queue.includes(job))
      queue.push(job);
    queueFlush();
  }
  function dequeueJob(job) {
    let index = queue.indexOf(job);
    if (index !== -1 && index > lastFlushedIndex)
      queue.splice(index, 1);
  }
  function queueFlush() {
    if (!flushing && !flushPending) {
      flushPending = true;
      queueMicrotask(flushJobs);
    }
  }
  function flushJobs() {
    flushPending = false;
    flushing = true;
    for (let i2 = 0; i2 < queue.length; i2++) {
      queue[i2]();
      lastFlushedIndex = i2;
    }
    queue.length = 0;
    lastFlushedIndex = -1;
    flushing = false;
  }
  var reactive;
  var effect;
  var release;
  var raw;
  var shouldSchedule = true;
  function disableEffectScheduling(callback) {
    shouldSchedule = false;
    callback();
    shouldSchedule = true;
  }
  function setReactivityEngine(engine) {
    reactive = engine.reactive;
    release = engine.release;
    effect = (callback) => engine.effect(callback, { scheduler: (task) => {
      if (shouldSchedule) {
        scheduler(task);
      } else {
        task();
      }
    } });
    raw = engine.raw;
  }
  function overrideEffect(override) {
    effect = override;
  }
  function elementBoundEffect(el) {
    let cleanup2 = () => {
    };
    let wrappedEffect = (callback) => {
      let effectReference = effect(callback);
      if (!el._x_effects) {
        el._x_effects = /* @__PURE__ */ new Set();
        el._x_runEffects = () => {
          el._x_effects.forEach((i2) => i2());
        };
      }
      el._x_effects.add(effectReference);
      cleanup2 = () => {
        if (effectReference === void 0)
          return;
        el._x_effects.delete(effectReference);
        release(effectReference);
      };
      return effectReference;
    };
    return [wrappedEffect, () => {
      cleanup2();
    }];
  }
  var onAttributeAddeds = [];
  var onElRemoveds = [];
  var onElAddeds = [];
  function onElAdded(callback) {
    onElAddeds.push(callback);
  }
  function onElRemoved(el, callback) {
    if (typeof callback === "function") {
      if (!el._x_cleanups)
        el._x_cleanups = [];
      el._x_cleanups.push(callback);
    } else {
      callback = el;
      onElRemoveds.push(callback);
    }
  }
  function onAttributesAdded(callback) {
    onAttributeAddeds.push(callback);
  }
  function onAttributeRemoved(el, name, callback) {
    if (!el._x_attributeCleanups)
      el._x_attributeCleanups = {};
    if (!el._x_attributeCleanups[name])
      el._x_attributeCleanups[name] = [];
    el._x_attributeCleanups[name].push(callback);
  }
  function cleanupAttributes(el, names) {
    if (!el._x_attributeCleanups)
      return;
    Object.entries(el._x_attributeCleanups).forEach(([name, value]) => {
      if (names === void 0 || names.includes(name)) {
        value.forEach((i2) => i2());
        delete el._x_attributeCleanups[name];
      }
    });
  }
  var observer = new MutationObserver(onMutate);
  var currentlyObserving = false;
  function startObservingMutations() {
    observer.observe(document, { subtree: true, childList: true, attributes: true, attributeOldValue: true });
    currentlyObserving = true;
  }
  function stopObservingMutations() {
    flushObserver();
    observer.disconnect();
    currentlyObserving = false;
  }
  var recordQueue = [];
  var willProcessRecordQueue = false;
  function flushObserver() {
    recordQueue = recordQueue.concat(observer.takeRecords());
    if (recordQueue.length && !willProcessRecordQueue) {
      willProcessRecordQueue = true;
      queueMicrotask(() => {
        processRecordQueue();
        willProcessRecordQueue = false;
      });
    }
  }
  function processRecordQueue() {
    onMutate(recordQueue);
    recordQueue.length = 0;
  }
  function mutateDom(callback) {
    if (!currentlyObserving)
      return callback();
    stopObservingMutations();
    let result = callback();
    startObservingMutations();
    return result;
  }
  var isCollecting = false;
  var deferredMutations = [];
  function deferMutations() {
    isCollecting = true;
  }
  function flushAndStopDeferringMutations() {
    isCollecting = false;
    onMutate(deferredMutations);
    deferredMutations = [];
  }
  function onMutate(mutations) {
    if (isCollecting) {
      deferredMutations = deferredMutations.concat(mutations);
      return;
    }
    let addedNodes = [];
    let removedNodes = [];
    let addedAttributes = /* @__PURE__ */ new Map();
    let removedAttributes = /* @__PURE__ */ new Map();
    for (let i2 = 0; i2 < mutations.length; i2++) {
      if (mutations[i2].target._x_ignoreMutationObserver)
        continue;
      if (mutations[i2].type === "childList") {
        mutations[i2].addedNodes.forEach((node) => node.nodeType === 1 && addedNodes.push(node));
        mutations[i2].removedNodes.forEach((node) => node.nodeType === 1 && removedNodes.push(node));
      }
      if (mutations[i2].type === "attributes") {
        let el = mutations[i2].target;
        let name = mutations[i2].attributeName;
        let oldValue = mutations[i2].oldValue;
        let add2 = () => {
          if (!addedAttributes.has(el))
            addedAttributes.set(el, []);
          addedAttributes.get(el).push({ name, value: el.getAttribute(name) });
        };
        let remove = () => {
          if (!removedAttributes.has(el))
            removedAttributes.set(el, []);
          removedAttributes.get(el).push(name);
        };
        if (el.hasAttribute(name) && oldValue === null) {
          add2();
        } else if (el.hasAttribute(name)) {
          remove();
          add2();
        } else {
          remove();
        }
      }
    }
    removedAttributes.forEach((attrs, el) => {
      cleanupAttributes(el, attrs);
    });
    addedAttributes.forEach((attrs, el) => {
      onAttributeAddeds.forEach((i2) => i2(el, attrs));
    });
    for (let node of removedNodes) {
      if (addedNodes.includes(node))
        continue;
      onElRemoveds.forEach((i2) => i2(node));
      if (node._x_cleanups) {
        while (node._x_cleanups.length)
          node._x_cleanups.pop()();
      }
    }
    addedNodes.forEach((node) => {
      node._x_ignoreSelf = true;
      node._x_ignore = true;
    });
    for (let node of addedNodes) {
      if (removedNodes.includes(node))
        continue;
      if (!node.isConnected)
        continue;
      delete node._x_ignoreSelf;
      delete node._x_ignore;
      onElAddeds.forEach((i2) => i2(node));
      node._x_ignore = true;
      node._x_ignoreSelf = true;
    }
    addedNodes.forEach((node) => {
      delete node._x_ignoreSelf;
      delete node._x_ignore;
    });
    addedNodes = null;
    removedNodes = null;
    addedAttributes = null;
    removedAttributes = null;
  }
  function scope(node) {
    return mergeProxies(closestDataStack(node));
  }
  function addScopeToNode(node, data2, referenceNode) {
    node._x_dataStack = [data2, ...closestDataStack(referenceNode || node)];
    return () => {
      node._x_dataStack = node._x_dataStack.filter((i2) => i2 !== data2);
    };
  }
  function refreshScope(element, scope2) {
    let existingScope = element._x_dataStack[0];
    Object.entries(scope2).forEach(([key, value]) => {
      existingScope[key] = value;
    });
  }
  function closestDataStack(node) {
    if (node._x_dataStack)
      return node._x_dataStack;
    if (typeof ShadowRoot === "function" && node instanceof ShadowRoot) {
      return closestDataStack(node.host);
    }
    if (!node.parentNode) {
      return [];
    }
    return closestDataStack(node.parentNode);
  }
  function mergeProxies(objects) {
    let thisProxy = new Proxy({}, {
      ownKeys: () => {
        return Array.from(new Set(objects.flatMap((i2) => Object.keys(i2))));
      },
      has: (target, name) => {
        return objects.some((obj) => obj.hasOwnProperty(name));
      },
      get: (target, name) => {
        return (objects.find((obj) => {
          if (obj.hasOwnProperty(name)) {
            let descriptor = Object.getOwnPropertyDescriptor(obj, name);
            if (descriptor.get && descriptor.get._x_alreadyBound || descriptor.set && descriptor.set._x_alreadyBound) {
              return true;
            }
            if ((descriptor.get || descriptor.set) && descriptor.enumerable) {
              let getter = descriptor.get;
              let setter = descriptor.set;
              let property = descriptor;
              getter = getter && getter.bind(thisProxy);
              setter = setter && setter.bind(thisProxy);
              if (getter)
                getter._x_alreadyBound = true;
              if (setter)
                setter._x_alreadyBound = true;
              Object.defineProperty(obj, name, {
                ...property,
                get: getter,
                set: setter
              });
            }
            return true;
          }
          return false;
        }) || {})[name];
      },
      set: (target, name, value) => {
        let closestObjectWithKey = objects.find((obj) => obj.hasOwnProperty(name));
        if (closestObjectWithKey) {
          closestObjectWithKey[name] = value;
        } else {
          objects[objects.length - 1][name] = value;
        }
        return true;
      }
    });
    return thisProxy;
  }
  function initInterceptors(data2) {
    let isObject2 = (val) => typeof val === "object" && !Array.isArray(val) && val !== null;
    let recurse = (obj, basePath = "") => {
      Object.entries(Object.getOwnPropertyDescriptors(obj)).forEach(([key, { value, enumerable }]) => {
        if (enumerable === false || value === void 0)
          return;
        let path = basePath === "" ? key : `${basePath}.${key}`;
        if (typeof value === "object" && value !== null && value._x_interceptor) {
          obj[key] = value.initialize(data2, path, key);
        } else {
          if (isObject2(value) && value !== obj && !(value instanceof Element)) {
            recurse(value, path);
          }
        }
      });
    };
    return recurse(data2);
  }
  function interceptor(callback, mutateObj = () => {
  }) {
    let obj = {
      initialValue: void 0,
      _x_interceptor: true,
      initialize(data2, path, key) {
        return callback(this.initialValue, () => get(data2, path), (value) => set(data2, path, value), path, key);
      }
    };
    mutateObj(obj);
    return (initialValue) => {
      if (typeof initialValue === "object" && initialValue !== null && initialValue._x_interceptor) {
        let initialize = obj.initialize.bind(obj);
        obj.initialize = (data2, path, key) => {
          let innerValue = initialValue.initialize(data2, path, key);
          obj.initialValue = innerValue;
          return initialize(data2, path, key);
        };
      } else {
        obj.initialValue = initialValue;
      }
      return obj;
    };
  }
  function get(obj, path) {
    return path.split(".").reduce((carry, segment) => carry[segment], obj);
  }
  function set(obj, path, value) {
    if (typeof path === "string")
      path = path.split(".");
    if (path.length === 1)
      obj[path[0]] = value;
    else if (path.length === 0)
      throw error;
    else {
      if (obj[path[0]])
        return set(obj[path[0]], path.slice(1), value);
      else {
        obj[path[0]] = {};
        return set(obj[path[0]], path.slice(1), value);
      }
    }
  }
  var magics = {};
  function magic(name, callback) {
    magics[name] = callback;
  }
  function injectMagics(obj, el) {
    Object.entries(magics).forEach(([name, callback]) => {
      Object.defineProperty(obj, `$${name}`, {
        get() {
          let [utilities, cleanup2] = getElementBoundUtilities(el);
          utilities = { interceptor, ...utilities };
          onElRemoved(el, cleanup2);
          return callback(el, utilities);
        },
        enumerable: false
      });
    });
    return obj;
  }
  function tryCatch(el, expression, callback, ...args) {
    try {
      return callback(...args);
    } catch (e2) {
      handleError(e2, el, expression);
    }
  }
  function handleError(error2, el, expression = void 0) {
    Object.assign(error2, { el, expression });
    console.warn(`Alpine Expression Error: ${error2.message}

${expression ? 'Expression: "' + expression + '"\n\n' : ""}`, el);
    setTimeout(() => {
      throw error2;
    }, 0);
  }
  var shouldAutoEvaluateFunctions = true;
  function dontAutoEvaluateFunctions(callback) {
    let cache = shouldAutoEvaluateFunctions;
    shouldAutoEvaluateFunctions = false;
    callback();
    shouldAutoEvaluateFunctions = cache;
  }
  function evaluate(el, expression, extras = {}) {
    let result;
    evaluateLater(el, expression)((value) => result = value, extras);
    return result;
  }
  function evaluateLater(...args) {
    return theEvaluatorFunction(...args);
  }
  var theEvaluatorFunction = normalEvaluator;
  function setEvaluator(newEvaluator) {
    theEvaluatorFunction = newEvaluator;
  }
  function normalEvaluator(el, expression) {
    let overriddenMagics = {};
    injectMagics(overriddenMagics, el);
    let dataStack = [overriddenMagics, ...closestDataStack(el)];
    let evaluator = typeof expression === "function" ? generateEvaluatorFromFunction(dataStack, expression) : generateEvaluatorFromString(dataStack, expression, el);
    return tryCatch.bind(null, el, expression, evaluator);
  }
  function generateEvaluatorFromFunction(dataStack, func) {
    return (receiver = () => {
    }, { scope: scope2 = {}, params = [] } = {}) => {
      let result = func.apply(mergeProxies([scope2, ...dataStack]), params);
      runIfTypeOfFunction(receiver, result);
    };
  }
  var evaluatorMemo = {};
  function generateFunctionFromString(expression, el) {
    if (evaluatorMemo[expression]) {
      return evaluatorMemo[expression];
    }
    let AsyncFunction = Object.getPrototypeOf(async function() {
    }).constructor;
    let rightSideSafeExpression = /^[\n\s]*if.*\(.*\)/.test(expression) || /^(let|const)\s/.test(expression) ? `(async()=>{ ${expression} })()` : expression;
    const safeAsyncFunction = () => {
      try {
        return new AsyncFunction(["__self", "scope"], `with (scope) { __self.result = ${rightSideSafeExpression} }; __self.finished = true; return __self.result;`);
      } catch (error2) {
        handleError(error2, el, expression);
        return Promise.resolve();
      }
    };
    let func = safeAsyncFunction();
    evaluatorMemo[expression] = func;
    return func;
  }
  function generateEvaluatorFromString(dataStack, expression, el) {
    let func = generateFunctionFromString(expression, el);
    return (receiver = () => {
    }, { scope: scope2 = {}, params = [] } = {}) => {
      func.result = void 0;
      func.finished = false;
      let completeScope = mergeProxies([scope2, ...dataStack]);
      if (typeof func === "function") {
        let promise = func(func, completeScope).catch((error2) => handleError(error2, el, expression));
        if (func.finished) {
          runIfTypeOfFunction(receiver, func.result, completeScope, params, el);
          func.result = void 0;
        } else {
          promise.then((result) => {
            runIfTypeOfFunction(receiver, result, completeScope, params, el);
          }).catch((error2) => handleError(error2, el, expression)).finally(() => func.result = void 0);
        }
      }
    };
  }
  function runIfTypeOfFunction(receiver, value, scope2, params, el) {
    if (shouldAutoEvaluateFunctions && typeof value === "function") {
      let result = value.apply(scope2, params);
      if (result instanceof Promise) {
        result.then((i2) => runIfTypeOfFunction(receiver, i2, scope2, params)).catch((error2) => handleError(error2, el, value));
      } else {
        receiver(result);
      }
    } else if (typeof value === "object" && value instanceof Promise) {
      value.then((i2) => receiver(i2));
    } else {
      receiver(value);
    }
  }
  var prefixAsString = "x-";
  function prefix(subject = "") {
    return prefixAsString + subject;
  }
  function setPrefix(newPrefix) {
    prefixAsString = newPrefix;
  }
  var directiveHandlers = {};
  function directive(name, callback) {
    directiveHandlers[name] = callback;
    return {
      before(directive2) {
        if (!directiveHandlers[directive2]) {
          console.warn(
            "Cannot find directive `${directive}`. `${name}` will use the default order of execution"
          );
          return;
        }
        const pos = directiveOrder.indexOf(directive2);
        directiveOrder.splice(pos >= 0 ? pos : directiveOrder.indexOf("DEFAULT"), 0, name);
      }
    };
  }
  function directives(el, attributes, originalAttributeOverride) {
    attributes = Array.from(attributes);
    if (el._x_virtualDirectives) {
      let vAttributes = Object.entries(el._x_virtualDirectives).map(([name, value]) => ({ name, value }));
      let staticAttributes = attributesOnly(vAttributes);
      vAttributes = vAttributes.map((attribute) => {
        if (staticAttributes.find((attr) => attr.name === attribute.name)) {
          return {
            name: `x-bind:${attribute.name}`,
            value: `"${attribute.value}"`
          };
        }
        return attribute;
      });
      attributes = attributes.concat(vAttributes);
    }
    let transformedAttributeMap = {};
    let directives2 = attributes.map(toTransformedAttributes((newName, oldName) => transformedAttributeMap[newName] = oldName)).filter(outNonAlpineAttributes).map(toParsedDirectives(transformedAttributeMap, originalAttributeOverride)).sort(byPriority);
    return directives2.map((directive2) => {
      return getDirectiveHandler(el, directive2);
    });
  }
  function attributesOnly(attributes) {
    return Array.from(attributes).map(toTransformedAttributes()).filter((attr) => !outNonAlpineAttributes(attr));
  }
  var isDeferringHandlers = false;
  var directiveHandlerStacks = /* @__PURE__ */ new Map();
  var currentHandlerStackKey = Symbol();
  function deferHandlingDirectives(callback) {
    isDeferringHandlers = true;
    let key = Symbol();
    currentHandlerStackKey = key;
    directiveHandlerStacks.set(key, []);
    let flushHandlers = () => {
      while (directiveHandlerStacks.get(key).length)
        directiveHandlerStacks.get(key).shift()();
      directiveHandlerStacks.delete(key);
    };
    let stopDeferring = () => {
      isDeferringHandlers = false;
      flushHandlers();
    };
    callback(flushHandlers);
    stopDeferring();
  }
  function getElementBoundUtilities(el) {
    let cleanups = [];
    let cleanup2 = (callback) => cleanups.push(callback);
    let [effect3, cleanupEffect] = elementBoundEffect(el);
    cleanups.push(cleanupEffect);
    let utilities = {
      Alpine: alpine_default,
      effect: effect3,
      cleanup: cleanup2,
      evaluateLater: evaluateLater.bind(evaluateLater, el),
      evaluate: evaluate.bind(evaluate, el)
    };
    let doCleanup = () => cleanups.forEach((i2) => i2());
    return [utilities, doCleanup];
  }
  function getDirectiveHandler(el, directive2) {
    let noop = () => {
    };
    let handler3 = directiveHandlers[directive2.type] || noop;
    let [utilities, cleanup2] = getElementBoundUtilities(el);
    onAttributeRemoved(el, directive2.original, cleanup2);
    let fullHandler = () => {
      if (el._x_ignore || el._x_ignoreSelf)
        return;
      handler3.inline && handler3.inline(el, directive2, utilities);
      handler3 = handler3.bind(handler3, el, directive2, utilities);
      isDeferringHandlers ? directiveHandlerStacks.get(currentHandlerStackKey).push(handler3) : handler3();
    };
    fullHandler.runCleanups = cleanup2;
    return fullHandler;
  }
  var startingWith = (subject, replacement) => ({ name, value }) => {
    if (name.startsWith(subject))
      name = name.replace(subject, replacement);
    return { name, value };
  };
  var into = (i2) => i2;
  function toTransformedAttributes(callback = () => {
  }) {
    return ({ name, value }) => {
      let { name: newName, value: newValue } = attributeTransformers.reduce((carry, transform) => {
        return transform(carry);
      }, { name, value });
      if (newName !== name)
        callback(newName, name);
      return { name: newName, value: newValue };
    };
  }
  var attributeTransformers = [];
  function mapAttributes(callback) {
    attributeTransformers.push(callback);
  }
  function outNonAlpineAttributes({ name }) {
    return alpineAttributeRegex().test(name);
  }
  var alpineAttributeRegex = () => new RegExp(`^${prefixAsString}([^:^.]+)\\b`);
  function toParsedDirectives(transformedAttributeMap, originalAttributeOverride) {
    return ({ name, value }) => {
      let typeMatch = name.match(alpineAttributeRegex());
      let valueMatch = name.match(/:([a-zA-Z0-9\-:]+)/);
      let modifiers = name.match(/\.[^.\]]+(?=[^\]]*$)/g) || [];
      let original = originalAttributeOverride || transformedAttributeMap[name] || name;
      return {
        type: typeMatch ? typeMatch[1] : null,
        value: valueMatch ? valueMatch[1] : null,
        modifiers: modifiers.map((i2) => i2.replace(".", "")),
        expression: value,
        original
      };
    };
  }
  var DEFAULT = "DEFAULT";
  var directiveOrder = [
    "ignore",
    "ref",
    "data",
    "id",
    "bind",
    "init",
    "for",
    "model",
    "modelable",
    "transition",
    "show",
    "if",
    DEFAULT,
    "teleport"
  ];
  function byPriority(a2, b2) {
    let typeA = directiveOrder.indexOf(a2.type) === -1 ? DEFAULT : a2.type;
    let typeB = directiveOrder.indexOf(b2.type) === -1 ? DEFAULT : b2.type;
    return directiveOrder.indexOf(typeA) - directiveOrder.indexOf(typeB);
  }
  function dispatch(el, name, detail = {}) {
    el.dispatchEvent(
      new CustomEvent(name, {
        detail,
        bubbles: true,
        // Allows events to pass the shadow DOM barrier.
        composed: true,
        cancelable: true
      })
    );
  }
  function walk(el, callback) {
    if (typeof ShadowRoot === "function" && el instanceof ShadowRoot) {
      Array.from(el.children).forEach((el2) => walk(el2, callback));
      return;
    }
    let skip = false;
    callback(el, () => skip = true);
    if (skip)
      return;
    let node = el.firstElementChild;
    while (node) {
      walk(node, callback, false);
      node = node.nextElementSibling;
    }
  }
  function warn(message, ...args) {
    console.warn(`Alpine Warning: ${message}`, ...args);
  }
  function start() {
    if (!document.body)
      warn("Unable to initialize. Trying to load Alpine before `<body>` is available. Did you forget to add `defer` in Alpine's `<script>` tag?");
    dispatch(document, "alpine:init");
    dispatch(document, "alpine:initializing");
    startObservingMutations();
    onElAdded((el) => initTree(el, walk));
    onElRemoved((el) => destroyTree(el));
    onAttributesAdded((el, attrs) => {
      directives(el, attrs).forEach((handle) => handle());
    });
    let outNestedComponents = (el) => !closestRoot(el.parentElement, true);
    Array.from(document.querySelectorAll(allSelectors())).filter(outNestedComponents).forEach((el) => {
      initTree(el);
    });
    dispatch(document, "alpine:initialized");
  }
  var rootSelectorCallbacks = [];
  var initSelectorCallbacks = [];
  function rootSelectors() {
    return rootSelectorCallbacks.map((fn) => fn());
  }
  function allSelectors() {
    return rootSelectorCallbacks.concat(initSelectorCallbacks).map((fn) => fn());
  }
  function addRootSelector(selectorCallback) {
    rootSelectorCallbacks.push(selectorCallback);
  }
  function addInitSelector(selectorCallback) {
    initSelectorCallbacks.push(selectorCallback);
  }
  function closestRoot(el, includeInitSelectors = false) {
    return findClosest(el, (element) => {
      const selectors = includeInitSelectors ? allSelectors() : rootSelectors();
      if (selectors.some((selector) => element.matches(selector)))
        return true;
    });
  }
  function findClosest(el, callback) {
    if (!el)
      return;
    if (callback(el))
      return el;
    if (el._x_teleportBack)
      el = el._x_teleportBack;
    if (!el.parentElement)
      return;
    return findClosest(el.parentElement, callback);
  }
  function isRoot(el) {
    return rootSelectors().some((selector) => el.matches(selector));
  }
  var initInterceptors2 = [];
  function interceptInit(callback) {
    initInterceptors2.push(callback);
  }
  function initTree(el, walker = walk, intercept = () => {
  }) {
    deferHandlingDirectives(() => {
      walker(el, (el2, skip) => {
        intercept(el2, skip);
        initInterceptors2.forEach((i2) => i2(el2, skip));
        directives(el2, el2.attributes).forEach((handle) => handle());
        el2._x_ignore && skip();
      });
    });
  }
  function destroyTree(root) {
    walk(root, (el) => cleanupAttributes(el));
  }
  var tickStack = [];
  var isHolding = false;
  function nextTick(callback = () => {
  }) {
    queueMicrotask(() => {
      isHolding || setTimeout(() => {
        releaseNextTicks();
      });
    });
    return new Promise((res) => {
      tickStack.push(() => {
        callback();
        res();
      });
    });
  }
  function releaseNextTicks() {
    isHolding = false;
    while (tickStack.length)
      tickStack.shift()();
  }
  function holdNextTicks() {
    isHolding = true;
  }
  function setClasses(el, value) {
    if (Array.isArray(value)) {
      return setClassesFromString(el, value.join(" "));
    } else if (typeof value === "object" && value !== null) {
      return setClassesFromObject(el, value);
    } else if (typeof value === "function") {
      return setClasses(el, value());
    }
    return setClassesFromString(el, value);
  }
  function setClassesFromString(el, classString) {
    let split = (classString2) => classString2.split(" ").filter(Boolean);
    let missingClasses = (classString2) => classString2.split(" ").filter((i2) => !el.classList.contains(i2)).filter(Boolean);
    let addClassesAndReturnUndo = (classes) => {
      el.classList.add(...classes);
      return () => {
        el.classList.remove(...classes);
      };
    };
    classString = classString === true ? classString = "" : classString || "";
    return addClassesAndReturnUndo(missingClasses(classString));
  }
  function setClassesFromObject(el, classObject) {
    let split = (classString) => classString.split(" ").filter(Boolean);
    let forAdd = Object.entries(classObject).flatMap(([classString, bool]) => bool ? split(classString) : false).filter(Boolean);
    let forRemove = Object.entries(classObject).flatMap(([classString, bool]) => !bool ? split(classString) : false).filter(Boolean);
    let added = [];
    let removed = [];
    forRemove.forEach((i2) => {
      if (el.classList.contains(i2)) {
        el.classList.remove(i2);
        removed.push(i2);
      }
    });
    forAdd.forEach((i2) => {
      if (!el.classList.contains(i2)) {
        el.classList.add(i2);
        added.push(i2);
      }
    });
    return () => {
      removed.forEach((i2) => el.classList.add(i2));
      added.forEach((i2) => el.classList.remove(i2));
    };
  }
  function setStyles(el, value) {
    if (typeof value === "object" && value !== null) {
      return setStylesFromObject(el, value);
    }
    return setStylesFromString(el, value);
  }
  function setStylesFromObject(el, value) {
    let previousStyles = {};
    Object.entries(value).forEach(([key, value2]) => {
      previousStyles[key] = el.style[key];
      if (!key.startsWith("--")) {
        key = kebabCase(key);
      }
      el.style.setProperty(key, value2);
    });
    setTimeout(() => {
      if (el.style.length === 0) {
        el.removeAttribute("style");
      }
    });
    return () => {
      setStyles(el, previousStyles);
    };
  }
  function setStylesFromString(el, value) {
    let cache = el.getAttribute("style", value);
    el.setAttribute("style", value);
    return () => {
      el.setAttribute("style", cache || "");
    };
  }
  function kebabCase(subject) {
    return subject.replace(/([a-z])([A-Z])/g, "$1-$2").toLowerCase();
  }
  function once(callback, fallback = () => {
  }) {
    let called = false;
    return function() {
      if (!called) {
        called = true;
        callback.apply(this, arguments);
      } else {
        fallback.apply(this, arguments);
      }
    };
  }
  directive("transition", (el, { value, modifiers, expression }, { evaluate: evaluate2 }) => {
    if (typeof expression === "function")
      expression = evaluate2(expression);
    if (!expression) {
      registerTransitionsFromHelper(el, modifiers, value);
    } else {
      registerTransitionsFromClassString(el, expression, value);
    }
  });
  function registerTransitionsFromClassString(el, classString, stage) {
    registerTransitionObject(el, setClasses, "");
    let directiveStorageMap = {
      "enter": (classes) => {
        el._x_transition.enter.during = classes;
      },
      "enter-start": (classes) => {
        el._x_transition.enter.start = classes;
      },
      "enter-end": (classes) => {
        el._x_transition.enter.end = classes;
      },
      "leave": (classes) => {
        el._x_transition.leave.during = classes;
      },
      "leave-start": (classes) => {
        el._x_transition.leave.start = classes;
      },
      "leave-end": (classes) => {
        el._x_transition.leave.end = classes;
      }
    };
    directiveStorageMap[stage](classString);
  }
  function registerTransitionsFromHelper(el, modifiers, stage) {
    registerTransitionObject(el, setStyles);
    let doesntSpecify = !modifiers.includes("in") && !modifiers.includes("out") && !stage;
    let transitioningIn = doesntSpecify || modifiers.includes("in") || ["enter"].includes(stage);
    let transitioningOut = doesntSpecify || modifiers.includes("out") || ["leave"].includes(stage);
    if (modifiers.includes("in") && !doesntSpecify) {
      modifiers = modifiers.filter((i2, index) => index < modifiers.indexOf("out"));
    }
    if (modifiers.includes("out") && !doesntSpecify) {
      modifiers = modifiers.filter((i2, index) => index > modifiers.indexOf("out"));
    }
    let wantsAll = !modifiers.includes("opacity") && !modifiers.includes("scale");
    let wantsOpacity = wantsAll || modifiers.includes("opacity");
    let wantsScale = wantsAll || modifiers.includes("scale");
    let opacityValue = wantsOpacity ? 0 : 1;
    let scaleValue = wantsScale ? modifierValue(modifiers, "scale", 95) / 100 : 1;
    let delay = modifierValue(modifiers, "delay", 0);
    let origin = modifierValue(modifiers, "origin", "center");
    let property = "opacity, transform";
    let durationIn = modifierValue(modifiers, "duration", 150) / 1e3;
    let durationOut = modifierValue(modifiers, "duration", 75) / 1e3;
    let easing = `cubic-bezier(0.4, 0.0, 0.2, 1)`;
    if (transitioningIn) {
      el._x_transition.enter.during = {
        transformOrigin: origin,
        transitionDelay: delay,
        transitionProperty: property,
        transitionDuration: `${durationIn}s`,
        transitionTimingFunction: easing
      };
      el._x_transition.enter.start = {
        opacity: opacityValue,
        transform: `scale(${scaleValue})`
      };
      el._x_transition.enter.end = {
        opacity: 1,
        transform: `scale(1)`
      };
    }
    if (transitioningOut) {
      el._x_transition.leave.during = {
        transformOrigin: origin,
        transitionDelay: delay,
        transitionProperty: property,
        transitionDuration: `${durationOut}s`,
        transitionTimingFunction: easing
      };
      el._x_transition.leave.start = {
        opacity: 1,
        transform: `scale(1)`
      };
      el._x_transition.leave.end = {
        opacity: opacityValue,
        transform: `scale(${scaleValue})`
      };
    }
  }
  function registerTransitionObject(el, setFunction, defaultValue = {}) {
    if (!el._x_transition)
      el._x_transition = {
        enter: { during: defaultValue, start: defaultValue, end: defaultValue },
        leave: { during: defaultValue, start: defaultValue, end: defaultValue },
        in(before = () => {
        }, after = () => {
        }) {
          transition(el, setFunction, {
            during: this.enter.during,
            start: this.enter.start,
            end: this.enter.end
          }, before, after);
        },
        out(before = () => {
        }, after = () => {
        }) {
          transition(el, setFunction, {
            during: this.leave.during,
            start: this.leave.start,
            end: this.leave.end
          }, before, after);
        }
      };
  }
  window.Element.prototype._x_toggleAndCascadeWithTransitions = function(el, value, show, hide) {
    const nextTick2 = document.visibilityState === "visible" ? requestAnimationFrame : setTimeout;
    let clickAwayCompatibleShow = () => nextTick2(show);
    if (value) {
      if (el._x_transition && (el._x_transition.enter || el._x_transition.leave)) {
        el._x_transition.enter && (Object.entries(el._x_transition.enter.during).length || Object.entries(el._x_transition.enter.start).length || Object.entries(el._x_transition.enter.end).length) ? el._x_transition.in(show) : clickAwayCompatibleShow();
      } else {
        el._x_transition ? el._x_transition.in(show) : clickAwayCompatibleShow();
      }
      return;
    }
    el._x_hidePromise = el._x_transition ? new Promise((resolve, reject) => {
      el._x_transition.out(() => {
      }, () => resolve(hide));
      el._x_transitioning.beforeCancel(() => reject({ isFromCancelledTransition: true }));
    }) : Promise.resolve(hide);
    queueMicrotask(() => {
      let closest = closestHide(el);
      if (closest) {
        if (!closest._x_hideChildren)
          closest._x_hideChildren = [];
        closest._x_hideChildren.push(el);
      } else {
        nextTick2(() => {
          let hideAfterChildren = (el2) => {
            let carry = Promise.all([
              el2._x_hidePromise,
              ...(el2._x_hideChildren || []).map(hideAfterChildren)
            ]).then(([i2]) => i2());
            delete el2._x_hidePromise;
            delete el2._x_hideChildren;
            return carry;
          };
          hideAfterChildren(el).catch((e2) => {
            if (!e2.isFromCancelledTransition)
              throw e2;
          });
        });
      }
    });
  };
  function closestHide(el) {
    let parent = el.parentNode;
    if (!parent)
      return;
    return parent._x_hidePromise ? parent : closestHide(parent);
  }
  function transition(el, setFunction, { during, start: start2, end } = {}, before = () => {
  }, after = () => {
  }) {
    if (el._x_transitioning)
      el._x_transitioning.cancel();
    if (Object.keys(during).length === 0 && Object.keys(start2).length === 0 && Object.keys(end).length === 0) {
      before();
      after();
      return;
    }
    let undoStart, undoDuring, undoEnd;
    performTransition(el, {
      start() {
        undoStart = setFunction(el, start2);
      },
      during() {
        undoDuring = setFunction(el, during);
      },
      before,
      end() {
        undoStart();
        undoEnd = setFunction(el, end);
      },
      after,
      cleanup() {
        undoDuring();
        undoEnd();
      }
    });
  }
  function performTransition(el, stages) {
    let interrupted, reachedBefore, reachedEnd;
    let finish = once(() => {
      mutateDom(() => {
        interrupted = true;
        if (!reachedBefore)
          stages.before();
        if (!reachedEnd) {
          stages.end();
          releaseNextTicks();
        }
        stages.after();
        if (el.isConnected)
          stages.cleanup();
        delete el._x_transitioning;
      });
    });
    el._x_transitioning = {
      beforeCancels: [],
      beforeCancel(callback) {
        this.beforeCancels.push(callback);
      },
      cancel: once(function() {
        while (this.beforeCancels.length) {
          this.beforeCancels.shift()();
        }
        ;
        finish();
      }),
      finish
    };
    mutateDom(() => {
      stages.start();
      stages.during();
    });
    holdNextTicks();
    requestAnimationFrame(() => {
      if (interrupted)
        return;
      let duration = Number(getComputedStyle(el).transitionDuration.replace(/,.*/, "").replace("s", "")) * 1e3;
      let delay = Number(getComputedStyle(el).transitionDelay.replace(/,.*/, "").replace("s", "")) * 1e3;
      if (duration === 0)
        duration = Number(getComputedStyle(el).animationDuration.replace("s", "")) * 1e3;
      mutateDom(() => {
        stages.before();
      });
      reachedBefore = true;
      requestAnimationFrame(() => {
        if (interrupted)
          return;
        mutateDom(() => {
          stages.end();
        });
        releaseNextTicks();
        setTimeout(el._x_transitioning.finish, duration + delay);
        reachedEnd = true;
      });
    });
  }
  function modifierValue(modifiers, key, fallback) {
    if (modifiers.indexOf(key) === -1)
      return fallback;
    const rawValue = modifiers[modifiers.indexOf(key) + 1];
    if (!rawValue)
      return fallback;
    if (key === "scale") {
      if (isNaN(rawValue))
        return fallback;
    }
    if (key === "duration") {
      let match = rawValue.match(/([0-9]+)ms/);
      if (match)
        return match[1];
    }
    if (key === "origin") {
      if (["top", "right", "left", "center", "bottom"].includes(modifiers[modifiers.indexOf(key) + 2])) {
        return [rawValue, modifiers[modifiers.indexOf(key) + 2]].join(" ");
      }
    }
    return rawValue;
  }
  var isCloning = false;
  function skipDuringClone(callback, fallback = () => {
  }) {
    return (...args) => isCloning ? fallback(...args) : callback(...args);
  }
  function onlyDuringClone(callback) {
    return (...args) => isCloning && callback(...args);
  }
  function clone(oldEl, newEl) {
    if (!newEl._x_dataStack)
      newEl._x_dataStack = oldEl._x_dataStack;
    isCloning = true;
    dontRegisterReactiveSideEffects(() => {
      cloneTree(newEl);
    });
    isCloning = false;
  }
  function cloneTree(el) {
    let hasRunThroughFirstEl = false;
    let shallowWalker = (el2, callback) => {
      walk(el2, (el3, skip) => {
        if (hasRunThroughFirstEl && isRoot(el3))
          return skip();
        hasRunThroughFirstEl = true;
        callback(el3, skip);
      });
    };
    initTree(el, shallowWalker);
  }
  function dontRegisterReactiveSideEffects(callback) {
    let cache = effect;
    overrideEffect((callback2, el) => {
      let storedEffect = cache(callback2);
      release(storedEffect);
      return () => {
      };
    });
    callback();
    overrideEffect(cache);
  }
  function bind(el, name, value, modifiers = []) {
    if (!el._x_bindings)
      el._x_bindings = reactive({});
    el._x_bindings[name] = value;
    name = modifiers.includes("camel") ? camelCase(name) : name;
    switch (name) {
      case "value":
        bindInputValue(el, value);
        break;
      case "style":
        bindStyles(el, value);
        break;
      case "class":
        bindClasses(el, value);
        break;
      default:
        bindAttribute(el, name, value);
        break;
    }
  }
  function bindInputValue(el, value) {
    if (el.type === "radio") {
      if (el.attributes.value === void 0) {
        el.value = value;
      }
      if (window.fromModel) {
        el.checked = checkedAttrLooseCompare(el.value, value);
      }
    } else if (el.type === "checkbox") {
      if (Number.isInteger(value)) {
        el.value = value;
      } else if (!Number.isInteger(value) && !Array.isArray(value) && typeof value !== "boolean" && ![null, void 0].includes(value)) {
        el.value = String(value);
      } else {
        if (Array.isArray(value)) {
          el.checked = value.some((val) => checkedAttrLooseCompare(val, el.value));
        } else {
          el.checked = !!value;
        }
      }
    } else if (el.tagName === "SELECT") {
      updateSelect(el, value);
    } else {
      if (el.value === value)
        return;
      el.value = value;
    }
  }
  function bindClasses(el, value) {
    if (el._x_undoAddedClasses)
      el._x_undoAddedClasses();
    el._x_undoAddedClasses = setClasses(el, value);
  }
  function bindStyles(el, value) {
    if (el._x_undoAddedStyles)
      el._x_undoAddedStyles();
    el._x_undoAddedStyles = setStyles(el, value);
  }
  function bindAttribute(el, name, value) {
    if ([null, void 0, false].includes(value) && attributeShouldntBePreservedIfFalsy(name)) {
      el.removeAttribute(name);
    } else {
      if (isBooleanAttr(name))
        value = name;
      setIfChanged(el, name, value);
    }
  }
  function setIfChanged(el, attrName, value) {
    if (el.getAttribute(attrName) != value) {
      el.setAttribute(attrName, value);
    }
  }
  function updateSelect(el, value) {
    const arrayWrappedValue = [].concat(value).map((value2) => {
      return value2 + "";
    });
    Array.from(el.options).forEach((option) => {
      option.selected = arrayWrappedValue.includes(option.value);
    });
  }
  function camelCase(subject) {
    return subject.toLowerCase().replace(/-(\w)/g, (match, char) => char.toUpperCase());
  }
  function checkedAttrLooseCompare(valueA, valueB) {
    return valueA == valueB;
  }
  function isBooleanAttr(attrName) {
    const booleanAttributes = [
      "disabled",
      "checked",
      "required",
      "readonly",
      "hidden",
      "open",
      "selected",
      "autofocus",
      "itemscope",
      "multiple",
      "novalidate",
      "allowfullscreen",
      "allowpaymentrequest",
      "formnovalidate",
      "autoplay",
      "controls",
      "loop",
      "muted",
      "playsinline",
      "default",
      "ismap",
      "reversed",
      "async",
      "defer",
      "nomodule"
    ];
    return booleanAttributes.includes(attrName);
  }
  function attributeShouldntBePreservedIfFalsy(name) {
    return !["aria-pressed", "aria-checked", "aria-expanded", "aria-selected"].includes(name);
  }
  function getBinding(el, name, fallback) {
    if (el._x_bindings && el._x_bindings[name] !== void 0)
      return el._x_bindings[name];
    let attr = el.getAttribute(name);
    if (attr === null)
      return typeof fallback === "function" ? fallback() : fallback;
    if (attr === "")
      return true;
    if (isBooleanAttr(name)) {
      return !![name, "true"].includes(attr);
    }
    return attr;
  }
  function debounce(func, wait) {
    var timeout;
    return function() {
      var context = this, args = arguments;
      var later = function() {
        timeout = null;
        func.apply(context, args);
      };
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
    };
  }
  function throttle(func, limit) {
    let inThrottle;
    return function() {
      let context = this, args = arguments;
      if (!inThrottle) {
        func.apply(context, args);
        inThrottle = true;
        setTimeout(() => inThrottle = false, limit);
      }
    };
  }
  function plugin(callback) {
    callback(alpine_default);
  }
  var stores = {};
  var isReactive = false;
  function store(name, value) {
    if (!isReactive) {
      stores = reactive(stores);
      isReactive = true;
    }
    if (value === void 0) {
      return stores[name];
    }
    stores[name] = value;
    if (typeof value === "object" && value !== null && value.hasOwnProperty("init") && typeof value.init === "function") {
      stores[name].init();
    }
    initInterceptors(stores[name]);
  }
  function getStores() {
    return stores;
  }
  var binds = {};
  function bind2(name, bindings) {
    let getBindings = typeof bindings !== "function" ? () => bindings : bindings;
    if (name instanceof Element) {
      applyBindingsObject(name, getBindings());
    } else {
      binds[name] = getBindings;
    }
  }
  function injectBindingProviders(obj) {
    Object.entries(binds).forEach(([name, callback]) => {
      Object.defineProperty(obj, name, {
        get() {
          return (...args) => {
            return callback(...args);
          };
        }
      });
    });
    return obj;
  }
  function applyBindingsObject(el, obj, original) {
    let cleanupRunners = [];
    while (cleanupRunners.length)
      cleanupRunners.pop()();
    let attributes = Object.entries(obj).map(([name, value]) => ({ name, value }));
    let staticAttributes = attributesOnly(attributes);
    attributes = attributes.map((attribute) => {
      if (staticAttributes.find((attr) => attr.name === attribute.name)) {
        return {
          name: `x-bind:${attribute.name}`,
          value: `"${attribute.value}"`
        };
      }
      return attribute;
    });
    directives(el, attributes, original).map((handle) => {
      cleanupRunners.push(handle.runCleanups);
      handle();
    });
  }
  var datas = {};
  function data(name, callback) {
    datas[name] = callback;
  }
  function injectDataProviders(obj, context) {
    Object.entries(datas).forEach(([name, callback]) => {
      Object.defineProperty(obj, name, {
        get() {
          return (...args) => {
            return callback.bind(context)(...args);
          };
        },
        enumerable: false
      });
    });
    return obj;
  }
  var Alpine = {
    get reactive() {
      return reactive;
    },
    get release() {
      return release;
    },
    get effect() {
      return effect;
    },
    get raw() {
      return raw;
    },
    version: "3.12.0",
    flushAndStopDeferringMutations,
    dontAutoEvaluateFunctions,
    disableEffectScheduling,
    startObservingMutations,
    stopObservingMutations,
    setReactivityEngine,
    closestDataStack,
    skipDuringClone,
    onlyDuringClone,
    addRootSelector,
    addInitSelector,
    addScopeToNode,
    deferMutations,
    mapAttributes,
    evaluateLater,
    interceptInit,
    setEvaluator,
    mergeProxies,
    findClosest,
    closestRoot,
    destroyTree,
    interceptor,
    // INTERNAL: not public API and is subject to change without major release.
    transition,
    // INTERNAL
    setStyles,
    // INTERNAL
    mutateDom,
    directive,
    throttle,
    debounce,
    evaluate,
    initTree,
    nextTick,
    prefixed: prefix,
    prefix: setPrefix,
    plugin,
    magic,
    store,
    start,
    clone,
    bound: getBinding,
    $data: scope,
    walk,
    data,
    bind: bind2
  };
  var alpine_default = Alpine;
  function makeMap(str, expectsLowerCase) {
    const map = /* @__PURE__ */ Object.create(null);
    const list = str.split(",");
    for (let i2 = 0; i2 < list.length; i2++) {
      map[list[i2]] = true;
    }
    return expectsLowerCase ? (val) => !!map[val.toLowerCase()] : (val) => !!map[val];
  }
  var specialBooleanAttrs = `itemscope,allowfullscreen,formnovalidate,ismap,nomodule,novalidate,readonly`;
  var isBooleanAttr2 = /* @__PURE__ */ makeMap(specialBooleanAttrs + `,async,autofocus,autoplay,controls,default,defer,disabled,hidden,loop,open,required,reversed,scoped,seamless,checked,muted,multiple,selected`);
  var EMPTY_OBJ = true ? Object.freeze({}) : {};
  var EMPTY_ARR = true ? Object.freeze([]) : [];
  var extend = Object.assign;
  var hasOwnProperty = Object.prototype.hasOwnProperty;
  var hasOwn = (val, key) => hasOwnProperty.call(val, key);
  var isArray = Array.isArray;
  var isMap = (val) => toTypeString(val) === "[object Map]";
  var isString = (val) => typeof val === "string";
  var isSymbol = (val) => typeof val === "symbol";
  var isObject = (val) => val !== null && typeof val === "object";
  var objectToString = Object.prototype.toString;
  var toTypeString = (value) => objectToString.call(value);
  var toRawType = (value) => {
    return toTypeString(value).slice(8, -1);
  };
  var isIntegerKey = (key) => isString(key) && key !== "NaN" && key[0] !== "-" && "" + parseInt(key, 10) === key;
  var cacheStringFunction = (fn) => {
    const cache = /* @__PURE__ */ Object.create(null);
    return (str) => {
      const hit = cache[str];
      return hit || (cache[str] = fn(str));
    };
  };
  var camelizeRE = /-(\w)/g;
  var camelize = cacheStringFunction((str) => {
    return str.replace(camelizeRE, (_2, c2) => c2 ? c2.toUpperCase() : "");
  });
  var hyphenateRE = /\B([A-Z])/g;
  var hyphenate = cacheStringFunction((str) => str.replace(hyphenateRE, "-$1").toLowerCase());
  var capitalize = cacheStringFunction((str) => str.charAt(0).toUpperCase() + str.slice(1));
  var toHandlerKey = cacheStringFunction((str) => str ? `on${capitalize(str)}` : ``);
  var hasChanged = (value, oldValue) => value !== oldValue && (value === value || oldValue === oldValue);
  var targetMap = /* @__PURE__ */ new WeakMap();
  var effectStack = [];
  var activeEffect;
  var ITERATE_KEY = Symbol(true ? "iterate" : "");
  var MAP_KEY_ITERATE_KEY = Symbol(true ? "Map key iterate" : "");
  function isEffect(fn) {
    return fn && fn._isEffect === true;
  }
  function effect2(fn, options = EMPTY_OBJ) {
    if (isEffect(fn)) {
      fn = fn.raw;
    }
    const effect3 = createReactiveEffect(fn, options);
    if (!options.lazy) {
      effect3();
    }
    return effect3;
  }
  function stop(effect3) {
    if (effect3.active) {
      cleanup(effect3);
      if (effect3.options.onStop) {
        effect3.options.onStop();
      }
      effect3.active = false;
    }
  }
  var uid = 0;
  function createReactiveEffect(fn, options) {
    const effect3 = function reactiveEffect() {
      if (!effect3.active) {
        return fn();
      }
      if (!effectStack.includes(effect3)) {
        cleanup(effect3);
        try {
          enableTracking();
          effectStack.push(effect3);
          activeEffect = effect3;
          return fn();
        } finally {
          effectStack.pop();
          resetTracking();
          activeEffect = effectStack[effectStack.length - 1];
        }
      }
    };
    effect3.id = uid++;
    effect3.allowRecurse = !!options.allowRecurse;
    effect3._isEffect = true;
    effect3.active = true;
    effect3.raw = fn;
    effect3.deps = [];
    effect3.options = options;
    return effect3;
  }
  function cleanup(effect3) {
    const { deps } = effect3;
    if (deps.length) {
      for (let i2 = 0; i2 < deps.length; i2++) {
        deps[i2].delete(effect3);
      }
      deps.length = 0;
    }
  }
  var shouldTrack = true;
  var trackStack = [];
  function pauseTracking() {
    trackStack.push(shouldTrack);
    shouldTrack = false;
  }
  function enableTracking() {
    trackStack.push(shouldTrack);
    shouldTrack = true;
  }
  function resetTracking() {
    const last = trackStack.pop();
    shouldTrack = last === void 0 ? true : last;
  }
  function track(target, type, key) {
    if (!shouldTrack || activeEffect === void 0) {
      return;
    }
    let depsMap = targetMap.get(target);
    if (!depsMap) {
      targetMap.set(target, depsMap = /* @__PURE__ */ new Map());
    }
    let dep = depsMap.get(key);
    if (!dep) {
      depsMap.set(key, dep = /* @__PURE__ */ new Set());
    }
    if (!dep.has(activeEffect)) {
      dep.add(activeEffect);
      activeEffect.deps.push(dep);
      if (activeEffect.options.onTrack) {
        activeEffect.options.onTrack({
          effect: activeEffect,
          target,
          type,
          key
        });
      }
    }
  }
  function trigger(target, type, key, newValue, oldValue, oldTarget) {
    const depsMap = targetMap.get(target);
    if (!depsMap) {
      return;
    }
    const effects = /* @__PURE__ */ new Set();
    const add2 = (effectsToAdd) => {
      if (effectsToAdd) {
        effectsToAdd.forEach((effect3) => {
          if (effect3 !== activeEffect || effect3.allowRecurse) {
            effects.add(effect3);
          }
        });
      }
    };
    if (type === "clear") {
      depsMap.forEach(add2);
    } else if (key === "length" && isArray(target)) {
      depsMap.forEach((dep, key2) => {
        if (key2 === "length" || key2 >= newValue) {
          add2(dep);
        }
      });
    } else {
      if (key !== void 0) {
        add2(depsMap.get(key));
      }
      switch (type) {
        case "add":
          if (!isArray(target)) {
            add2(depsMap.get(ITERATE_KEY));
            if (isMap(target)) {
              add2(depsMap.get(MAP_KEY_ITERATE_KEY));
            }
          } else if (isIntegerKey(key)) {
            add2(depsMap.get("length"));
          }
          break;
        case "delete":
          if (!isArray(target)) {
            add2(depsMap.get(ITERATE_KEY));
            if (isMap(target)) {
              add2(depsMap.get(MAP_KEY_ITERATE_KEY));
            }
          }
          break;
        case "set":
          if (isMap(target)) {
            add2(depsMap.get(ITERATE_KEY));
          }
          break;
      }
    }
    const run = (effect3) => {
      if (effect3.options.onTrigger) {
        effect3.options.onTrigger({
          effect: effect3,
          target,
          key,
          type,
          newValue,
          oldValue,
          oldTarget
        });
      }
      if (effect3.options.scheduler) {
        effect3.options.scheduler(effect3);
      } else {
        effect3();
      }
    };
    effects.forEach(run);
  }
  var isNonTrackableKeys = /* @__PURE__ */ makeMap(`__proto__,__v_isRef,__isVue`);
  var builtInSymbols = new Set(Object.getOwnPropertyNames(Symbol).map((key) => Symbol[key]).filter(isSymbol));
  var get2 = /* @__PURE__ */ createGetter();
  var shallowGet = /* @__PURE__ */ createGetter(false, true);
  var readonlyGet = /* @__PURE__ */ createGetter(true);
  var shallowReadonlyGet = /* @__PURE__ */ createGetter(true, true);
  var arrayInstrumentations = {};
  ["includes", "indexOf", "lastIndexOf"].forEach((key) => {
    const method = Array.prototype[key];
    arrayInstrumentations[key] = function(...args) {
      const arr = toRaw(this);
      for (let i2 = 0, l2 = this.length; i2 < l2; i2++) {
        track(arr, "get", i2 + "");
      }
      const res = method.apply(arr, args);
      if (res === -1 || res === false) {
        return method.apply(arr, args.map(toRaw));
      } else {
        return res;
      }
    };
  });
  ["push", "pop", "shift", "unshift", "splice"].forEach((key) => {
    const method = Array.prototype[key];
    arrayInstrumentations[key] = function(...args) {
      pauseTracking();
      const res = method.apply(this, args);
      resetTracking();
      return res;
    };
  });
  function createGetter(isReadonly = false, shallow = false) {
    return function get3(target, key, receiver) {
      if (key === "__v_isReactive") {
        return !isReadonly;
      } else if (key === "__v_isReadonly") {
        return isReadonly;
      } else if (key === "__v_raw" && receiver === (isReadonly ? shallow ? shallowReadonlyMap : readonlyMap : shallow ? shallowReactiveMap : reactiveMap).get(target)) {
        return target;
      }
      const targetIsArray = isArray(target);
      if (!isReadonly && targetIsArray && hasOwn(arrayInstrumentations, key)) {
        return Reflect.get(arrayInstrumentations, key, receiver);
      }
      const res = Reflect.get(target, key, receiver);
      if (isSymbol(key) ? builtInSymbols.has(key) : isNonTrackableKeys(key)) {
        return res;
      }
      if (!isReadonly) {
        track(target, "get", key);
      }
      if (shallow) {
        return res;
      }
      if (isRef(res)) {
        const shouldUnwrap = !targetIsArray || !isIntegerKey(key);
        return shouldUnwrap ? res.value : res;
      }
      if (isObject(res)) {
        return isReadonly ? readonly(res) : reactive2(res);
      }
      return res;
    };
  }
  var set2 = /* @__PURE__ */ createSetter();
  var shallowSet = /* @__PURE__ */ createSetter(true);
  function createSetter(shallow = false) {
    return function set3(target, key, value, receiver) {
      let oldValue = target[key];
      if (!shallow) {
        value = toRaw(value);
        oldValue = toRaw(oldValue);
        if (!isArray(target) && isRef(oldValue) && !isRef(value)) {
          oldValue.value = value;
          return true;
        }
      }
      const hadKey = isArray(target) && isIntegerKey(key) ? Number(key) < target.length : hasOwn(target, key);
      const result = Reflect.set(target, key, value, receiver);
      if (target === toRaw(receiver)) {
        if (!hadKey) {
          trigger(target, "add", key, value);
        } else if (hasChanged(value, oldValue)) {
          trigger(target, "set", key, value, oldValue);
        }
      }
      return result;
    };
  }
  function deleteProperty(target, key) {
    const hadKey = hasOwn(target, key);
    const oldValue = target[key];
    const result = Reflect.deleteProperty(target, key);
    if (result && hadKey) {
      trigger(target, "delete", key, void 0, oldValue);
    }
    return result;
  }
  function has(target, key) {
    const result = Reflect.has(target, key);
    if (!isSymbol(key) || !builtInSymbols.has(key)) {
      track(target, "has", key);
    }
    return result;
  }
  function ownKeys(target) {
    track(target, "iterate", isArray(target) ? "length" : ITERATE_KEY);
    return Reflect.ownKeys(target);
  }
  var mutableHandlers = {
    get: get2,
    set: set2,
    deleteProperty,
    has,
    ownKeys
  };
  var readonlyHandlers = {
    get: readonlyGet,
    set(target, key) {
      if (true) {
        console.warn(`Set operation on key "${String(key)}" failed: target is readonly.`, target);
      }
      return true;
    },
    deleteProperty(target, key) {
      if (true) {
        console.warn(`Delete operation on key "${String(key)}" failed: target is readonly.`, target);
      }
      return true;
    }
  };
  var shallowReactiveHandlers = extend({}, mutableHandlers, {
    get: shallowGet,
    set: shallowSet
  });
  var shallowReadonlyHandlers = extend({}, readonlyHandlers, {
    get: shallowReadonlyGet
  });
  var toReactive = (value) => isObject(value) ? reactive2(value) : value;
  var toReadonly = (value) => isObject(value) ? readonly(value) : value;
  var toShallow = (value) => value;
  var getProto = (v2) => Reflect.getPrototypeOf(v2);
  function get$1(target, key, isReadonly = false, isShallow = false) {
    target = target[
      "__v_raw"
      /* RAW */
    ];
    const rawTarget = toRaw(target);
    const rawKey = toRaw(key);
    if (key !== rawKey) {
      !isReadonly && track(rawTarget, "get", key);
    }
    !isReadonly && track(rawTarget, "get", rawKey);
    const { has: has2 } = getProto(rawTarget);
    const wrap = isShallow ? toShallow : isReadonly ? toReadonly : toReactive;
    if (has2.call(rawTarget, key)) {
      return wrap(target.get(key));
    } else if (has2.call(rawTarget, rawKey)) {
      return wrap(target.get(rawKey));
    } else if (target !== rawTarget) {
      target.get(key);
    }
  }
  function has$1(key, isReadonly = false) {
    const target = this[
      "__v_raw"
      /* RAW */
    ];
    const rawTarget = toRaw(target);
    const rawKey = toRaw(key);
    if (key !== rawKey) {
      !isReadonly && track(rawTarget, "has", key);
    }
    !isReadonly && track(rawTarget, "has", rawKey);
    return key === rawKey ? target.has(key) : target.has(key) || target.has(rawKey);
  }
  function size(target, isReadonly = false) {
    target = target[
      "__v_raw"
      /* RAW */
    ];
    !isReadonly && track(toRaw(target), "iterate", ITERATE_KEY);
    return Reflect.get(target, "size", target);
  }
  function add(value) {
    value = toRaw(value);
    const target = toRaw(this);
    const proto = getProto(target);
    const hadKey = proto.has.call(target, value);
    if (!hadKey) {
      target.add(value);
      trigger(target, "add", value, value);
    }
    return this;
  }
  function set$1(key, value) {
    value = toRaw(value);
    const target = toRaw(this);
    const { has: has2, get: get3 } = getProto(target);
    let hadKey = has2.call(target, key);
    if (!hadKey) {
      key = toRaw(key);
      hadKey = has2.call(target, key);
    } else if (true) {
      checkIdentityKeys(target, has2, key);
    }
    const oldValue = get3.call(target, key);
    target.set(key, value);
    if (!hadKey) {
      trigger(target, "add", key, value);
    } else if (hasChanged(value, oldValue)) {
      trigger(target, "set", key, value, oldValue);
    }
    return this;
  }
  function deleteEntry(key) {
    const target = toRaw(this);
    const { has: has2, get: get3 } = getProto(target);
    let hadKey = has2.call(target, key);
    if (!hadKey) {
      key = toRaw(key);
      hadKey = has2.call(target, key);
    } else if (true) {
      checkIdentityKeys(target, has2, key);
    }
    const oldValue = get3 ? get3.call(target, key) : void 0;
    const result = target.delete(key);
    if (hadKey) {
      trigger(target, "delete", key, void 0, oldValue);
    }
    return result;
  }
  function clear() {
    const target = toRaw(this);
    const hadItems = target.size !== 0;
    const oldTarget = true ? isMap(target) ? new Map(target) : new Set(target) : void 0;
    const result = target.clear();
    if (hadItems) {
      trigger(target, "clear", void 0, void 0, oldTarget);
    }
    return result;
  }
  function createForEach(isReadonly, isShallow) {
    return function forEach(callback, thisArg) {
      const observed = this;
      const target = observed[
        "__v_raw"
        /* RAW */
      ];
      const rawTarget = toRaw(target);
      const wrap = isShallow ? toShallow : isReadonly ? toReadonly : toReactive;
      !isReadonly && track(rawTarget, "iterate", ITERATE_KEY);
      return target.forEach((value, key) => {
        return callback.call(thisArg, wrap(value), wrap(key), observed);
      });
    };
  }
  function createIterableMethod(method, isReadonly, isShallow) {
    return function(...args) {
      const target = this[
        "__v_raw"
        /* RAW */
      ];
      const rawTarget = toRaw(target);
      const targetIsMap = isMap(rawTarget);
      const isPair = method === "entries" || method === Symbol.iterator && targetIsMap;
      const isKeyOnly = method === "keys" && targetIsMap;
      const innerIterator = target[method](...args);
      const wrap = isShallow ? toShallow : isReadonly ? toReadonly : toReactive;
      !isReadonly && track(rawTarget, "iterate", isKeyOnly ? MAP_KEY_ITERATE_KEY : ITERATE_KEY);
      return {
        // iterator protocol
        next() {
          const { value, done } = innerIterator.next();
          return done ? { value, done } : {
            value: isPair ? [wrap(value[0]), wrap(value[1])] : wrap(value),
            done
          };
        },
        // iterable protocol
        [Symbol.iterator]() {
          return this;
        }
      };
    };
  }
  function createReadonlyMethod(type) {
    return function(...args) {
      if (true) {
        const key = args[0] ? `on key "${args[0]}" ` : ``;
        console.warn(`${capitalize(type)} operation ${key}failed: target is readonly.`, toRaw(this));
      }
      return type === "delete" ? false : this;
    };
  }
  var mutableInstrumentations = {
    get(key) {
      return get$1(this, key);
    },
    get size() {
      return size(this);
    },
    has: has$1,
    add,
    set: set$1,
    delete: deleteEntry,
    clear,
    forEach: createForEach(false, false)
  };
  var shallowInstrumentations = {
    get(key) {
      return get$1(this, key, false, true);
    },
    get size() {
      return size(this);
    },
    has: has$1,
    add,
    set: set$1,
    delete: deleteEntry,
    clear,
    forEach: createForEach(false, true)
  };
  var readonlyInstrumentations = {
    get(key) {
      return get$1(this, key, true);
    },
    get size() {
      return size(this, true);
    },
    has(key) {
      return has$1.call(this, key, true);
    },
    add: createReadonlyMethod(
      "add"
      /* ADD */
    ),
    set: createReadonlyMethod(
      "set"
      /* SET */
    ),
    delete: createReadonlyMethod(
      "delete"
      /* DELETE */
    ),
    clear: createReadonlyMethod(
      "clear"
      /* CLEAR */
    ),
    forEach: createForEach(true, false)
  };
  var shallowReadonlyInstrumentations = {
    get(key) {
      return get$1(this, key, true, true);
    },
    get size() {
      return size(this, true);
    },
    has(key) {
      return has$1.call(this, key, true);
    },
    add: createReadonlyMethod(
      "add"
      /* ADD */
    ),
    set: createReadonlyMethod(
      "set"
      /* SET */
    ),
    delete: createReadonlyMethod(
      "delete"
      /* DELETE */
    ),
    clear: createReadonlyMethod(
      "clear"
      /* CLEAR */
    ),
    forEach: createForEach(true, true)
  };
  var iteratorMethods = ["keys", "values", "entries", Symbol.iterator];
  iteratorMethods.forEach((method) => {
    mutableInstrumentations[method] = createIterableMethod(method, false, false);
    readonlyInstrumentations[method] = createIterableMethod(method, true, false);
    shallowInstrumentations[method] = createIterableMethod(method, false, true);
    shallowReadonlyInstrumentations[method] = createIterableMethod(method, true, true);
  });
  function createInstrumentationGetter(isReadonly, shallow) {
    const instrumentations = shallow ? isReadonly ? shallowReadonlyInstrumentations : shallowInstrumentations : isReadonly ? readonlyInstrumentations : mutableInstrumentations;
    return (target, key, receiver) => {
      if (key === "__v_isReactive") {
        return !isReadonly;
      } else if (key === "__v_isReadonly") {
        return isReadonly;
      } else if (key === "__v_raw") {
        return target;
      }
      return Reflect.get(hasOwn(instrumentations, key) && key in target ? instrumentations : target, key, receiver);
    };
  }
  var mutableCollectionHandlers = {
    get: createInstrumentationGetter(false, false)
  };
  var shallowCollectionHandlers = {
    get: createInstrumentationGetter(false, true)
  };
  var readonlyCollectionHandlers = {
    get: createInstrumentationGetter(true, false)
  };
  var shallowReadonlyCollectionHandlers = {
    get: createInstrumentationGetter(true, true)
  };
  function checkIdentityKeys(target, has2, key) {
    const rawKey = toRaw(key);
    if (rawKey !== key && has2.call(target, rawKey)) {
      const type = toRawType(target);
      console.warn(`Reactive ${type} contains both the raw and reactive versions of the same object${type === `Map` ? ` as keys` : ``}, which can lead to inconsistencies. Avoid differentiating between the raw and reactive versions of an object and only use the reactive version if possible.`);
    }
  }
  var reactiveMap = /* @__PURE__ */ new WeakMap();
  var shallowReactiveMap = /* @__PURE__ */ new WeakMap();
  var readonlyMap = /* @__PURE__ */ new WeakMap();
  var shallowReadonlyMap = /* @__PURE__ */ new WeakMap();
  function targetTypeMap(rawType) {
    switch (rawType) {
      case "Object":
      case "Array":
        return 1;
      case "Map":
      case "Set":
      case "WeakMap":
      case "WeakSet":
        return 2;
      default:
        return 0;
    }
  }
  function getTargetType(value) {
    return value[
      "__v_skip"
      /* SKIP */
    ] || !Object.isExtensible(value) ? 0 : targetTypeMap(toRawType(value));
  }
  function reactive2(target) {
    if (target && target[
      "__v_isReadonly"
      /* IS_READONLY */
    ]) {
      return target;
    }
    return createReactiveObject(target, false, mutableHandlers, mutableCollectionHandlers, reactiveMap);
  }
  function readonly(target) {
    return createReactiveObject(target, true, readonlyHandlers, readonlyCollectionHandlers, readonlyMap);
  }
  function createReactiveObject(target, isReadonly, baseHandlers, collectionHandlers, proxyMap) {
    if (!isObject(target)) {
      if (true) {
        console.warn(`value cannot be made reactive: ${String(target)}`);
      }
      return target;
    }
    if (target[
      "__v_raw"
      /* RAW */
    ] && !(isReadonly && target[
      "__v_isReactive"
      /* IS_REACTIVE */
    ])) {
      return target;
    }
    const existingProxy = proxyMap.get(target);
    if (existingProxy) {
      return existingProxy;
    }
    const targetType = getTargetType(target);
    if (targetType === 0) {
      return target;
    }
    const proxy = new Proxy(target, targetType === 2 ? collectionHandlers : baseHandlers);
    proxyMap.set(target, proxy);
    return proxy;
  }
  function toRaw(observed) {
    return observed && toRaw(observed[
      "__v_raw"
      /* RAW */
    ]) || observed;
  }
  function isRef(r2) {
    return Boolean(r2 && r2.__v_isRef === true);
  }
  magic("nextTick", () => nextTick);
  magic("dispatch", (el) => dispatch.bind(dispatch, el));
  magic("watch", (el, { evaluateLater: evaluateLater2, effect: effect3 }) => (key, callback) => {
    let evaluate2 = evaluateLater2(key);
    let firstTime = true;
    let oldValue;
    let effectReference = effect3(() => evaluate2((value) => {
      JSON.stringify(value);
      if (!firstTime) {
        queueMicrotask(() => {
          callback(value, oldValue);
          oldValue = value;
        });
      } else {
        oldValue = value;
      }
      firstTime = false;
    }));
    el._x_effects.delete(effectReference);
  });
  magic("store", getStores);
  magic("data", (el) => scope(el));
  magic("root", (el) => closestRoot(el));
  magic("refs", (el) => {
    if (el._x_refs_proxy)
      return el._x_refs_proxy;
    el._x_refs_proxy = mergeProxies(getArrayOfRefObject(el));
    return el._x_refs_proxy;
  });
  function getArrayOfRefObject(el) {
    let refObjects = [];
    let currentEl = el;
    while (currentEl) {
      if (currentEl._x_refs)
        refObjects.push(currentEl._x_refs);
      currentEl = currentEl.parentNode;
    }
    return refObjects;
  }
  var globalIdMemo = {};
  function findAndIncrementId(name) {
    if (!globalIdMemo[name])
      globalIdMemo[name] = 0;
    return ++globalIdMemo[name];
  }
  function closestIdRoot(el, name) {
    return findClosest(el, (element) => {
      if (element._x_ids && element._x_ids[name])
        return true;
    });
  }
  function setIdRoot(el, name) {
    if (!el._x_ids)
      el._x_ids = {};
    if (!el._x_ids[name])
      el._x_ids[name] = findAndIncrementId(name);
  }
  magic("id", (el) => (name, key = null) => {
    let root = closestIdRoot(el, name);
    let id = root ? root._x_ids[name] : findAndIncrementId(name);
    return key ? `${name}-${id}-${key}` : `${name}-${id}`;
  });
  magic("el", (el) => el);
  warnMissingPluginMagic("Focus", "focus", "focus");
  warnMissingPluginMagic("Persist", "persist", "persist");
  function warnMissingPluginMagic(name, magicName, slug) {
    magic(magicName, (el) => warn(`You can't use [$${directiveName}] without first installing the "${name}" plugin here: https://alpinejs.dev/plugins/${slug}`, el));
  }
  function entangle({ get: outerGet, set: outerSet }, { get: innerGet, set: innerSet }) {
    let firstRun = true;
    let outerHash, innerHash, outerHashLatest, innerHashLatest;
    let reference = effect(() => {
      let outer, inner;
      if (firstRun) {
        outer = outerGet();
        innerSet(outer);
        inner = innerGet();
        firstRun = false;
      } else {
        outer = outerGet();
        inner = innerGet();
        outerHashLatest = JSON.stringify(outer);
        innerHashLatest = JSON.stringify(inner);
        if (outerHashLatest !== outerHash) {
          inner = innerGet();
          innerSet(outer);
          inner = outer;
        } else {
          outerSet(inner);
          outer = inner;
        }
      }
      outerHash = JSON.stringify(outer);
      innerHash = JSON.stringify(inner);
    });
    return () => {
      release(reference);
    };
  }
  directive("modelable", (el, { expression }, { effect: effect3, evaluateLater: evaluateLater2, cleanup: cleanup2 }) => {
    let func = evaluateLater2(expression);
    let innerGet = () => {
      let result;
      func((i2) => result = i2);
      return result;
    };
    let evaluateInnerSet = evaluateLater2(`${expression} = __placeholder`);
    let innerSet = (val) => evaluateInnerSet(() => {
    }, { scope: { "__placeholder": val } });
    let initialValue = innerGet();
    innerSet(initialValue);
    queueMicrotask(() => {
      if (!el._x_model)
        return;
      el._x_removeModelListeners["default"]();
      let outerGet = el._x_model.get;
      let outerSet = el._x_model.set;
      let releaseEntanglement = entangle(
        {
          get() {
            return outerGet();
          },
          set(value) {
            outerSet(value);
          }
        },
        {
          get() {
            return innerGet();
          },
          set(value) {
            innerSet(value);
          }
        }
      );
      cleanup2(releaseEntanglement);
    });
  });
  var teleportContainerDuringClone = document.createElement("div");
  directive("teleport", (el, { modifiers, expression }, { cleanup: cleanup2 }) => {
    if (el.tagName.toLowerCase() !== "template")
      warn("x-teleport can only be used on a <template> tag", el);
    let target = skipDuringClone(() => {
      return document.querySelector(expression);
    }, () => {
      return teleportContainerDuringClone;
    })();
    if (!target)
      warn(`Cannot find x-teleport element for selector: "${expression}"`);
    let clone2 = el.content.cloneNode(true).firstElementChild;
    el._x_teleport = clone2;
    clone2._x_teleportBack = el;
    if (el._x_forwardEvents) {
      el._x_forwardEvents.forEach((eventName) => {
        clone2.addEventListener(eventName, (e2) => {
          e2.stopPropagation();
          el.dispatchEvent(new e2.constructor(e2.type, e2));
        });
      });
    }
    addScopeToNode(clone2, {}, el);
    mutateDom(() => {
      if (modifiers.includes("prepend")) {
        target.parentNode.insertBefore(clone2, target);
      } else if (modifiers.includes("append")) {
        target.parentNode.insertBefore(clone2, target.nextSibling);
      } else {
        target.appendChild(clone2);
      }
      initTree(clone2);
      clone2._x_ignore = true;
    });
    cleanup2(() => clone2.remove());
  });
  var handler = () => {
  };
  handler.inline = (el, { modifiers }, { cleanup: cleanup2 }) => {
    modifiers.includes("self") ? el._x_ignoreSelf = true : el._x_ignore = true;
    cleanup2(() => {
      modifiers.includes("self") ? delete el._x_ignoreSelf : delete el._x_ignore;
    });
  };
  directive("ignore", handler);
  directive("effect", (el, { expression }, { effect: effect3 }) => effect3(evaluateLater(el, expression)));
  function on(el, event, modifiers, callback) {
    let listenerTarget = el;
    let handler3 = (e2) => callback(e2);
    let options = {};
    let wrapHandler = (callback2, wrapper) => (e2) => wrapper(callback2, e2);
    if (modifiers.includes("dot"))
      event = dotSyntax(event);
    if (modifiers.includes("camel"))
      event = camelCase2(event);
    if (modifiers.includes("passive"))
      options.passive = true;
    if (modifiers.includes("capture"))
      options.capture = true;
    if (modifiers.includes("window"))
      listenerTarget = window;
    if (modifiers.includes("document"))
      listenerTarget = document;
    if (modifiers.includes("prevent"))
      handler3 = wrapHandler(handler3, (next, e2) => {
        e2.preventDefault();
        next(e2);
      });
    if (modifiers.includes("stop"))
      handler3 = wrapHandler(handler3, (next, e2) => {
        e2.stopPropagation();
        next(e2);
      });
    if (modifiers.includes("self"))
      handler3 = wrapHandler(handler3, (next, e2) => {
        e2.target === el && next(e2);
      });
    if (modifiers.includes("away") || modifiers.includes("outside")) {
      listenerTarget = document;
      handler3 = wrapHandler(handler3, (next, e2) => {
        if (el.contains(e2.target))
          return;
        if (e2.target.isConnected === false)
          return;
        if (el.offsetWidth < 1 && el.offsetHeight < 1)
          return;
        if (el._x_isShown === false)
          return;
        next(e2);
      });
    }
    if (modifiers.includes("once")) {
      handler3 = wrapHandler(handler3, (next, e2) => {
        next(e2);
        listenerTarget.removeEventListener(event, handler3, options);
      });
    }
    handler3 = wrapHandler(handler3, (next, e2) => {
      if (isKeyEvent(event)) {
        if (isListeningForASpecificKeyThatHasntBeenPressed(e2, modifiers)) {
          return;
        }
      }
      next(e2);
    });
    if (modifiers.includes("debounce")) {
      let nextModifier = modifiers[modifiers.indexOf("debounce") + 1] || "invalid-wait";
      let wait = isNumeric(nextModifier.split("ms")[0]) ? Number(nextModifier.split("ms")[0]) : 250;
      handler3 = debounce(handler3, wait);
    }
    if (modifiers.includes("throttle")) {
      let nextModifier = modifiers[modifiers.indexOf("throttle") + 1] || "invalid-wait";
      let wait = isNumeric(nextModifier.split("ms")[0]) ? Number(nextModifier.split("ms")[0]) : 250;
      handler3 = throttle(handler3, wait);
    }
    listenerTarget.addEventListener(event, handler3, options);
    return () => {
      listenerTarget.removeEventListener(event, handler3, options);
    };
  }
  function dotSyntax(subject) {
    return subject.replace(/-/g, ".");
  }
  function camelCase2(subject) {
    return subject.toLowerCase().replace(/-(\w)/g, (match, char) => char.toUpperCase());
  }
  function isNumeric(subject) {
    return !Array.isArray(subject) && !isNaN(subject);
  }
  function kebabCase2(subject) {
    if ([" ", "_"].includes(
      subject
    ))
      return subject;
    return subject.replace(/([a-z])([A-Z])/g, "$1-$2").replace(/[_\s]/, "-").toLowerCase();
  }
  function isKeyEvent(event) {
    return ["keydown", "keyup"].includes(event);
  }
  function isListeningForASpecificKeyThatHasntBeenPressed(e2, modifiers) {
    let keyModifiers = modifiers.filter((i2) => {
      return !["window", "document", "prevent", "stop", "once", "capture"].includes(i2);
    });
    if (keyModifiers.includes("debounce")) {
      let debounceIndex = keyModifiers.indexOf("debounce");
      keyModifiers.splice(debounceIndex, isNumeric((keyModifiers[debounceIndex + 1] || "invalid-wait").split("ms")[0]) ? 2 : 1);
    }
    if (keyModifiers.includes("throttle")) {
      let debounceIndex = keyModifiers.indexOf("throttle");
      keyModifiers.splice(debounceIndex, isNumeric((keyModifiers[debounceIndex + 1] || "invalid-wait").split("ms")[0]) ? 2 : 1);
    }
    if (keyModifiers.length === 0)
      return false;
    if (keyModifiers.length === 1 && keyToModifiers(e2.key).includes(keyModifiers[0]))
      return false;
    const systemKeyModifiers = ["ctrl", "shift", "alt", "meta", "cmd", "super"];
    const selectedSystemKeyModifiers = systemKeyModifiers.filter((modifier) => keyModifiers.includes(modifier));
    keyModifiers = keyModifiers.filter((i2) => !selectedSystemKeyModifiers.includes(i2));
    if (selectedSystemKeyModifiers.length > 0) {
      const activelyPressedKeyModifiers = selectedSystemKeyModifiers.filter((modifier) => {
        if (modifier === "cmd" || modifier === "super")
          modifier = "meta";
        return e2[`${modifier}Key`];
      });
      if (activelyPressedKeyModifiers.length === selectedSystemKeyModifiers.length) {
        if (keyToModifiers(e2.key).includes(keyModifiers[0]))
          return false;
      }
    }
    return true;
  }
  function keyToModifiers(key) {
    if (!key)
      return [];
    key = kebabCase2(key);
    let modifierToKeyMap = {
      "ctrl": "control",
      "slash": "/",
      "space": " ",
      "spacebar": " ",
      "cmd": "meta",
      "esc": "escape",
      "up": "arrow-up",
      "down": "arrow-down",
      "left": "arrow-left",
      "right": "arrow-right",
      "period": ".",
      "equal": "=",
      "minus": "-",
      "underscore": "_"
    };
    modifierToKeyMap[key] = key;
    return Object.keys(modifierToKeyMap).map((modifier) => {
      if (modifierToKeyMap[modifier] === key)
        return modifier;
    }).filter((modifier) => modifier);
  }
  directive("model", (el, { modifiers, expression }, { effect: effect3, cleanup: cleanup2 }) => {
    let scopeTarget = el;
    if (modifiers.includes("parent")) {
      scopeTarget = el.parentNode;
    }
    let evaluateGet = evaluateLater(scopeTarget, expression);
    let evaluateSet;
    if (typeof expression === "string") {
      evaluateSet = evaluateLater(scopeTarget, `${expression} = __placeholder`);
    } else if (typeof expression === "function" && typeof expression() === "string") {
      evaluateSet = evaluateLater(scopeTarget, `${expression()} = __placeholder`);
    } else {
      evaluateSet = () => {
      };
    }
    let getValue = () => {
      let result;
      evaluateGet((value) => result = value);
      return isGetterSetter(result) ? result.get() : result;
    };
    let setValue = (value) => {
      let result;
      evaluateGet((value2) => result = value2);
      if (isGetterSetter(result)) {
        result.set(value);
      } else {
        evaluateSet(() => {
        }, {
          scope: { "__placeholder": value }
        });
      }
    };
    if (modifiers.includes("fill") && el.hasAttribute("value") && (getValue() === null || getValue() === "")) {
      setValue(el.value);
    }
    if (typeof expression === "string" && el.type === "radio") {
      mutateDom(() => {
        if (!el.hasAttribute("name"))
          el.setAttribute("name", expression);
      });
    }
    var event = el.tagName.toLowerCase() === "select" || ["checkbox", "radio"].includes(el.type) || modifiers.includes("lazy") ? "change" : "input";
    let removeListener = isCloning ? () => {
    } : on(el, event, modifiers, (e2) => {
      setValue(getInputValue(el, modifiers, e2, getValue()));
    });
    if (!el._x_removeModelListeners)
      el._x_removeModelListeners = {};
    el._x_removeModelListeners["default"] = removeListener;
    cleanup2(() => el._x_removeModelListeners["default"]());
    if (el.form) {
      let removeResetListener = on(el.form, "reset", [], (e2) => {
        nextTick(() => el._x_model && el._x_model.set(el.value));
      });
      cleanup2(() => removeResetListener());
    }
    el._x_model = {
      get() {
        return getValue();
      },
      set(value) {
        setValue(value);
      }
    };
    el._x_forceModelUpdate = (value) => {
      value = value === void 0 ? getValue() : value;
      if (value === void 0 && typeof expression === "string" && expression.match(/\./))
        value = "";
      window.fromModel = true;
      mutateDom(() => bind(el, "value", value));
      delete window.fromModel;
    };
    effect3(() => {
      let value = getValue();
      if (modifiers.includes("unintrusive") && document.activeElement.isSameNode(el))
        return;
      el._x_forceModelUpdate(value);
    });
  });
  function getInputValue(el, modifiers, event, currentValue) {
    return mutateDom(() => {
      if (event instanceof CustomEvent && event.detail !== void 0) {
        return typeof event.detail != "undefined" ? event.detail : event.target.value;
      } else if (el.type === "checkbox") {
        if (Array.isArray(currentValue)) {
          let newValue = modifiers.includes("number") ? safeParseNumber(event.target.value) : event.target.value;
          return event.target.checked ? currentValue.concat([newValue]) : currentValue.filter((el2) => !checkedAttrLooseCompare2(el2, newValue));
        } else {
          return event.target.checked;
        }
      } else if (el.tagName.toLowerCase() === "select" && el.multiple) {
        return modifiers.includes("number") ? Array.from(event.target.selectedOptions).map((option) => {
          let rawValue = option.value || option.text;
          return safeParseNumber(rawValue);
        }) : Array.from(event.target.selectedOptions).map((option) => {
          return option.value || option.text;
        });
      } else {
        let rawValue = event.target.value;
        return modifiers.includes("number") ? safeParseNumber(rawValue) : modifiers.includes("trim") ? rawValue.trim() : rawValue;
      }
    });
  }
  function safeParseNumber(rawValue) {
    let number = rawValue ? parseFloat(rawValue) : null;
    return isNumeric2(number) ? number : rawValue;
  }
  function checkedAttrLooseCompare2(valueA, valueB) {
    return valueA == valueB;
  }
  function isNumeric2(subject) {
    return !Array.isArray(subject) && !isNaN(subject);
  }
  function isGetterSetter(value) {
    return value !== null && typeof value === "object" && typeof value.get === "function" && typeof value.set === "function";
  }
  directive("cloak", (el) => queueMicrotask(() => mutateDom(() => el.removeAttribute(prefix("cloak")))));
  addInitSelector(() => `[${prefix("init")}]`);
  directive("init", skipDuringClone((el, { expression }, { evaluate: evaluate2 }) => {
    if (typeof expression === "string") {
      return !!expression.trim() && evaluate2(expression, {}, false);
    }
    return evaluate2(expression, {}, false);
  }));
  directive("text", (el, { expression }, { effect: effect3, evaluateLater: evaluateLater2 }) => {
    let evaluate2 = evaluateLater2(expression);
    effect3(() => {
      evaluate2((value) => {
        mutateDom(() => {
          el.textContent = value;
        });
      });
    });
  });
  directive("html", (el, { expression }, { effect: effect3, evaluateLater: evaluateLater2 }) => {
    let evaluate2 = evaluateLater2(expression);
    effect3(() => {
      evaluate2((value) => {
        mutateDom(() => {
          el.innerHTML = value;
          el._x_ignoreSelf = true;
          initTree(el);
          delete el._x_ignoreSelf;
        });
      });
    });
  });
  mapAttributes(startingWith(":", into(prefix("bind:"))));
  directive("bind", (el, { value, modifiers, expression, original }, { effect: effect3 }) => {
    if (!value) {
      let bindingProviders = {};
      injectBindingProviders(bindingProviders);
      let getBindings = evaluateLater(el, expression);
      getBindings((bindings) => {
        applyBindingsObject(el, bindings, original);
      }, { scope: bindingProviders });
      return;
    }
    if (value === "key")
      return storeKeyForXFor(el, expression);
    let evaluate2 = evaluateLater(el, expression);
    effect3(() => evaluate2((result) => {
      if (result === void 0 && typeof expression === "string" && expression.match(/\./)) {
        result = "";
      }
      mutateDom(() => bind(el, value, result, modifiers));
    }));
  });
  function storeKeyForXFor(el, expression) {
    el._x_keyExpression = expression;
  }
  addRootSelector(() => `[${prefix("data")}]`);
  directive("data", skipDuringClone((el, { expression }, { cleanup: cleanup2 }) => {
    expression = expression === "" ? "{}" : expression;
    let magicContext = {};
    injectMagics(magicContext, el);
    let dataProviderContext = {};
    injectDataProviders(dataProviderContext, magicContext);
    let data2 = evaluate(el, expression, { scope: dataProviderContext });
    if (data2 === void 0 || data2 === true)
      data2 = {};
    injectMagics(data2, el);
    let reactiveData = reactive(data2);
    initInterceptors(reactiveData);
    let undo = addScopeToNode(el, reactiveData);
    reactiveData["init"] && evaluate(el, reactiveData["init"]);
    cleanup2(() => {
      reactiveData["destroy"] && evaluate(el, reactiveData["destroy"]);
      undo();
    });
  }));
  directive("show", (el, { modifiers, expression }, { effect: effect3 }) => {
    let evaluate2 = evaluateLater(el, expression);
    if (!el._x_doHide)
      el._x_doHide = () => {
        mutateDom(() => {
          el.style.setProperty("display", "none", modifiers.includes("important") ? "important" : void 0);
        });
      };
    if (!el._x_doShow)
      el._x_doShow = () => {
        mutateDom(() => {
          if (el.style.length === 1 && el.style.display === "none") {
            el.removeAttribute("style");
          } else {
            el.style.removeProperty("display");
          }
        });
      };
    let hide = () => {
      el._x_doHide();
      el._x_isShown = false;
    };
    let show = () => {
      el._x_doShow();
      el._x_isShown = true;
    };
    let clickAwayCompatibleShow = () => setTimeout(show);
    let toggle = once(
      (value) => value ? show() : hide(),
      (value) => {
        if (typeof el._x_toggleAndCascadeWithTransitions === "function") {
          el._x_toggleAndCascadeWithTransitions(el, value, show, hide);
        } else {
          value ? clickAwayCompatibleShow() : hide();
        }
      }
    );
    let oldValue;
    let firstTime = true;
    effect3(() => evaluate2((value) => {
      if (!firstTime && value === oldValue)
        return;
      if (modifiers.includes("immediate"))
        value ? clickAwayCompatibleShow() : hide();
      toggle(value);
      oldValue = value;
      firstTime = false;
    }));
  });
  directive("for", (el, { expression }, { effect: effect3, cleanup: cleanup2 }) => {
    let iteratorNames = parseForExpression(expression);
    let evaluateItems = evaluateLater(el, iteratorNames.items);
    let evaluateKey = evaluateLater(
      el,
      // the x-bind:key expression is stored for our use instead of evaluated.
      el._x_keyExpression || "index"
    );
    el._x_prevKeys = [];
    el._x_lookup = {};
    effect3(() => loop(el, iteratorNames, evaluateItems, evaluateKey));
    cleanup2(() => {
      Object.values(el._x_lookup).forEach((el2) => el2.remove());
      delete el._x_prevKeys;
      delete el._x_lookup;
    });
  });
  function loop(el, iteratorNames, evaluateItems, evaluateKey) {
    let isObject2 = (i2) => typeof i2 === "object" && !Array.isArray(i2);
    let templateEl = el;
    evaluateItems((items) => {
      if (isNumeric3(items) && items >= 0) {
        items = Array.from(Array(items).keys(), (i2) => i2 + 1);
      }
      if (items === void 0)
        items = [];
      let lookup = el._x_lookup;
      let prevKeys = el._x_prevKeys;
      let scopes = [];
      let keys = [];
      if (isObject2(items)) {
        items = Object.entries(items).map(([key, value]) => {
          let scope2 = getIterationScopeVariables(iteratorNames, value, key, items);
          evaluateKey((value2) => keys.push(value2), { scope: { index: key, ...scope2 } });
          scopes.push(scope2);
        });
      } else {
        for (let i2 = 0; i2 < items.length; i2++) {
          let scope2 = getIterationScopeVariables(iteratorNames, items[i2], i2, items);
          evaluateKey((value) => keys.push(value), { scope: { index: i2, ...scope2 } });
          scopes.push(scope2);
        }
      }
      let adds = [];
      let moves = [];
      let removes = [];
      let sames = [];
      for (let i2 = 0; i2 < prevKeys.length; i2++) {
        let key = prevKeys[i2];
        if (keys.indexOf(key) === -1)
          removes.push(key);
      }
      prevKeys = prevKeys.filter((key) => !removes.includes(key));
      let lastKey = "template";
      for (let i2 = 0; i2 < keys.length; i2++) {
        let key = keys[i2];
        let prevIndex = prevKeys.indexOf(key);
        if (prevIndex === -1) {
          prevKeys.splice(i2, 0, key);
          adds.push([lastKey, i2]);
        } else if (prevIndex !== i2) {
          let keyInSpot = prevKeys.splice(i2, 1)[0];
          let keyForSpot = prevKeys.splice(prevIndex - 1, 1)[0];
          prevKeys.splice(i2, 0, keyForSpot);
          prevKeys.splice(prevIndex, 0, keyInSpot);
          moves.push([keyInSpot, keyForSpot]);
        } else {
          sames.push(key);
        }
        lastKey = key;
      }
      for (let i2 = 0; i2 < removes.length; i2++) {
        let key = removes[i2];
        if (!!lookup[key]._x_effects) {
          lookup[key]._x_effects.forEach(dequeueJob);
        }
        lookup[key].remove();
        lookup[key] = null;
        delete lookup[key];
      }
      for (let i2 = 0; i2 < moves.length; i2++) {
        let [keyInSpot, keyForSpot] = moves[i2];
        let elInSpot = lookup[keyInSpot];
        let elForSpot = lookup[keyForSpot];
        let marker = document.createElement("div");
        mutateDom(() => {
          elForSpot.after(marker);
          elInSpot.after(elForSpot);
          elForSpot._x_currentIfEl && elForSpot.after(elForSpot._x_currentIfEl);
          marker.before(elInSpot);
          elInSpot._x_currentIfEl && elInSpot.after(elInSpot._x_currentIfEl);
          marker.remove();
        });
        refreshScope(elForSpot, scopes[keys.indexOf(keyForSpot)]);
      }
      for (let i2 = 0; i2 < adds.length; i2++) {
        let [lastKey2, index] = adds[i2];
        let lastEl = lastKey2 === "template" ? templateEl : lookup[lastKey2];
        if (lastEl._x_currentIfEl)
          lastEl = lastEl._x_currentIfEl;
        let scope2 = scopes[index];
        let key = keys[index];
        let clone2 = document.importNode(templateEl.content, true).firstElementChild;
        addScopeToNode(clone2, reactive(scope2), templateEl);
        mutateDom(() => {
          lastEl.after(clone2);
          initTree(clone2);
        });
        if (typeof key === "object") {
          warn("x-for key cannot be an object, it must be a string or an integer", templateEl);
        }
        lookup[key] = clone2;
      }
      for (let i2 = 0; i2 < sames.length; i2++) {
        refreshScope(lookup[sames[i2]], scopes[keys.indexOf(sames[i2])]);
      }
      templateEl._x_prevKeys = keys;
    });
  }
  function parseForExpression(expression) {
    let forIteratorRE = /,([^,\}\]]*)(?:,([^,\}\]]*))?$/;
    let stripParensRE = /^\s*\(|\)\s*$/g;
    let forAliasRE = /([\s\S]*?)\s+(?:in|of)\s+([\s\S]*)/;
    let inMatch = expression.match(forAliasRE);
    if (!inMatch)
      return;
    let res = {};
    res.items = inMatch[2].trim();
    let item = inMatch[1].replace(stripParensRE, "").trim();
    let iteratorMatch = item.match(forIteratorRE);
    if (iteratorMatch) {
      res.item = item.replace(forIteratorRE, "").trim();
      res.index = iteratorMatch[1].trim();
      if (iteratorMatch[2]) {
        res.collection = iteratorMatch[2].trim();
      }
    } else {
      res.item = item;
    }
    return res;
  }
  function getIterationScopeVariables(iteratorNames, item, index, items) {
    let scopeVariables = {};
    if (/^\[.*\]$/.test(iteratorNames.item) && Array.isArray(item)) {
      let names = iteratorNames.item.replace("[", "").replace("]", "").split(",").map((i2) => i2.trim());
      names.forEach((name, i2) => {
        scopeVariables[name] = item[i2];
      });
    } else if (/^\{.*\}$/.test(iteratorNames.item) && !Array.isArray(item) && typeof item === "object") {
      let names = iteratorNames.item.replace("{", "").replace("}", "").split(",").map((i2) => i2.trim());
      names.forEach((name) => {
        scopeVariables[name] = item[name];
      });
    } else {
      scopeVariables[iteratorNames.item] = item;
    }
    if (iteratorNames.index)
      scopeVariables[iteratorNames.index] = index;
    if (iteratorNames.collection)
      scopeVariables[iteratorNames.collection] = items;
    return scopeVariables;
  }
  function isNumeric3(subject) {
    return !Array.isArray(subject) && !isNaN(subject);
  }
  function handler2() {
  }
  handler2.inline = (el, { expression }, { cleanup: cleanup2 }) => {
    let root = closestRoot(el);
    if (!root._x_refs)
      root._x_refs = {};
    root._x_refs[expression] = el;
    cleanup2(() => delete root._x_refs[expression]);
  };
  directive("ref", handler2);
  directive("if", (el, { expression }, { effect: effect3, cleanup: cleanup2 }) => {
    let evaluate2 = evaluateLater(el, expression);
    let show = () => {
      if (el._x_currentIfEl)
        return el._x_currentIfEl;
      let clone2 = el.content.cloneNode(true).firstElementChild;
      addScopeToNode(clone2, {}, el);
      mutateDom(() => {
        el.after(clone2);
        initTree(clone2);
      });
      el._x_currentIfEl = clone2;
      el._x_undoIf = () => {
        walk(clone2, (node) => {
          if (!!node._x_effects) {
            node._x_effects.forEach(dequeueJob);
          }
        });
        clone2.remove();
        delete el._x_currentIfEl;
      };
      return clone2;
    };
    let hide = () => {
      if (!el._x_undoIf)
        return;
      el._x_undoIf();
      delete el._x_undoIf;
    };
    effect3(() => evaluate2((value) => {
      value ? show() : hide();
    }));
    cleanup2(() => el._x_undoIf && el._x_undoIf());
  });
  directive("id", (el, { expression }, { evaluate: evaluate2 }) => {
    let names = evaluate2(expression);
    names.forEach((name) => setIdRoot(el, name));
  });
  mapAttributes(startingWith("@", into(prefix("on:"))));
  directive("on", skipDuringClone((el, { value, modifiers, expression }, { cleanup: cleanup2 }) => {
    let evaluate2 = expression ? evaluateLater(el, expression) : () => {
    };
    if (el.tagName.toLowerCase() === "template") {
      if (!el._x_forwardEvents)
        el._x_forwardEvents = [];
      if (!el._x_forwardEvents.includes(value))
        el._x_forwardEvents.push(value);
    }
    let removeListener = on(el, value, modifiers, (e2) => {
      evaluate2(() => {
      }, { scope: { "$event": e2 }, params: [e2] });
    });
    cleanup2(() => removeListener());
  }));
  warnMissingPluginDirective("Collapse", "collapse", "collapse");
  warnMissingPluginDirective("Intersect", "intersect", "intersect");
  warnMissingPluginDirective("Focus", "trap", "focus");
  warnMissingPluginDirective("Mask", "mask", "mask");
  function warnMissingPluginDirective(name, directiveName2, slug) {
    directive(directiveName2, (el) => warn(`You can't use [x-${directiveName2}] without first installing the "${name}" plugin here: https://alpinejs.dev/plugins/${slug}`, el));
  }
  alpine_default.setEvaluator(normalEvaluator);
  alpine_default.setReactivityEngine({ reactive: reactive2, effect: effect2, release: stop, raw: toRaw });
  var src_default = alpine_default;
  var module_default = src_default;

  // Assets/ts/alpine-table.ts
  var TableSetting = Object.freeze({
    CurrentPage: "currentPage",
    PerPage: "perPage",
    SearchQuery: "searchQuery",
    Sort: "sort"
  });
  var alpineTable = (key, src) => ({
    // from html placeholder for the table
    key,
    src,
    // internal state
    rows: [],
    filteredRows: [],
    filteredRowTotal: 0,
    sortColumns: [],
    currentPage: 0,
    perPage: 10,
    maxPage: 0,
    searchQuery: "",
    debounceTimer: 0,
    fetchSetting(name) {
      return sessionStorage.getItem(`${this.key}_${name}`);
    },
    saveSetting(name, value) {
      sessionStorage.setItem(`${this.key}_${name}`, value.toString());
    },
    defaultCompare(a2, b2) {
      if (a2._index > b2._index) {
        return 1;
      }
      return a2._index < b2._index ? -1 : 0;
    },
    compare(a2, b2) {
      let i2 = 0;
      const len = this.length;
      for (; i2 < len; i2 += 1) {
        const sort = this[i2];
        const aa = a2[sort.property];
        const bb = b2[sort.property];
        if (aa === null) {
          return 1;
        }
        if (bb === null) {
          return -1;
        }
        if (aa < bb) {
          return sort.sortOrder === "asc" /* Asc */ ? -1 : 1;
        }
        if (aa > bb) {
          return sort.sortOrder === "asc" /* Asc */ ? 1 : -1;
        }
      }
      return 0;
    },
    filterArray(obj) {
      const tokens = (this || "").split(" ");
      return Array.from(Object.values(obj)).some((x2) => {
        if (x2.indexOf("_") < 0 && Object.prototype.hasOwnProperty.call(obj, x2)) {
          const objVal = `${obj[x2]}`.toLowerCase();
          if (tokens.every((y2) => objVal.indexOf(y2) > -1)) {
            return true;
          }
        }
        return false;
      });
    },
    filterData() {
      const filteredData = this.searchQuery ? this.rows?.filter(this.filterArray.bind(this.searchQuery.toLowerCase())) ?? [] : [...this.rows ?? []];
      filteredData.sort(this.sortColumns?.length ? this.compare.bind(this.sortColumns) : this.defaultCompare);
      this.filteredRowTotal = filteredData.length;
      this.maxPage = Math.max(Math.ceil(this.filteredRowTotal / this.perPage) - 1, 0);
      this.filteredRows = filteredData.slice(this.perPage * this.currentPage, this.perPage * this.currentPage + this.perPage);
      this.$nextTick(() => {
        this.$root.dispatchEvent(new CustomEvent("alpine-table-updated", { bubbles: true, composed: true }));
      });
    },
    onSearchQueryInput(event) {
      const val = event?.target?.value;
      if (this.debounceTimer) {
        clearTimeout(this.debounceTimer);
      }
      this.debounceTimer = window.setTimeout(() => {
        if (this.searchQuery !== val) {
          this.currentPage = 0;
          this.saveSetting(TableSetting.SearchQuery, val);
        }
        this.searchQuery = val;
        this.filterData();
      }, 250);
    },
    onPerPageInput(event) {
      const newVal = parseInt(event?.target?.value ?? "10", 10) ?? 10;
      if (this.perPage !== newVal) {
        this.currentPage = 0;
        this.saveSetting(TableSetting.PerPage, newVal);
      }
      this.perPage = newVal;
      this.filterData();
    },
    onFirstPageClick() {
      this.setPage(0);
    },
    onLastPageClick() {
      this.setPage(this.maxPage);
    },
    onPreviousPageClick() {
      this.setPage(Math.max(this.currentPage - 1, 0));
    },
    onNextPageClick() {
      this.setPage(Math.min(this.currentPage + 1, this.maxPage));
    },
    setPage(page) {
      this.currentPage = page;
      this.saveSetting(TableSetting.CurrentPage, this.currentPage);
      this.filterData();
    },
    onSortClick(property) {
      if (!property) {
        return;
      }
      const index = this.sortColumns.findIndex((x2) => x2.property === property);
      if (index === -1) {
        this.sortColumns.push({ property, sortOrder: "asc" /* Asc */ });
      } else if (this.sortColumns[index].sortOrder === "asc" /* Asc */) {
        this.sortColumns[index].sortOrder = "desc" /* Desc */;
      } else {
        this.sortColumns = this.sortColumns.filter((x2) => x2.property !== property);
      }
      this.saveSetting(TableSetting.Sort, JSON.stringify(this.sortColumns));
      this.filterData();
    },
    sortClass(property) {
      const index = property ? this.sortColumns.findIndex((x2) => x2.property === property) : -1;
      if (index === -1) {
        return "";
      }
      return this.sortColumns[index].sortOrder === "asc" /* Asc */ ? "alpine-sort-asc" : "alpine-sort-desc";
    },
    get startRowNumber() {
      return this.filteredRowTotal ? this.currentPage * this.perPage + 1 : 0;
    },
    get endRowNumber() {
      return this.filteredRowTotal ? Math.min((this.currentPage + 1) * this.perPage, this.filteredRowTotal) : 0;
    },
    get isFirstPage() {
      return this.currentPage === 0;
    },
    get isLastPage() {
      return this.currentPage === this.maxPage;
    },
    get is10PerPage() {
      return this.perPage === 10;
    },
    get is20PerPage() {
      return this.perPage === 20;
    },
    get is50PerPage() {
      return this.perPage === 50;
    },
    get is100PerPage() {
      return this.perPage === 100;
    },
    async fetchData() {
      if (!this.src.length) {
        return;
      }
      try {
        this.rows = (await fetch(this.src, { headers: { "X-Requested-With": "XMLHttpRequest" } }).then((res) => res.json())).map((x2, index) => ({ ...x2, _index: index })) ?? [];
      } catch {
        this.rows = [];
      }
      this.filterData();
    },
    async init() {
      this.perPage = parseInt(this.fetchSetting(TableSetting.PerPage) ?? "10", 10);
      this.currentPage = parseInt(this.fetchSetting(TableSetting.CurrentPage) ?? "0", 10);
      this.searchQuery = this.fetchSetting(TableSetting.SearchQuery) ?? "";
      this.sortColumns = JSON.parse(this.fetchSetting(TableSetting.Sort) ?? "[]");
      await this.fetchData();
    }
  });
  var alpine_table_default = alpineTable;

  // Assets/ts/eventHandlers/onHtmxConfirm.ts
  var onHtmxConfirm = async (e2) => {
    const { elt } = e2.detail;
    console.log(elt);
    if (elt.hasAttribute("hx-confirm-content")) {
      e2.preventDefault();
    } else if (elt.hasAttribute("hx-alert-content")) {
      e2.preventDefault();
      elt.focus();
    }
  };
  var onHtmxConfirm_default = onHtmxConfirm;

  // Assets/ts/eventHandlers/onTableUpdated.ts
  var htmx = __toESM(require_htmx_min());
  var onTableUpdated = (e2) => {
    if (e2?.target) {
      htmx.process(e2.target);
    }
  };
  var onTableUpdated_default = onTableUpdated;

  // Assets/ts/eventHandlers/onSidebarToggled.ts
  var onSidebarToggled = () => {
    document.body.classList.toggle("sidebar-toggled");
  };
  var onSidebarToggled_default = onSidebarToggled;

  // Assets/ts/index.ts
  document.getElementById("sidebar-button").addEventListener("click", onSidebarToggled_default);
  document.body.addEventListener("alpine-table-updated", onTableUpdated_default);
  document.body.addEventListener("htmx:confirm", onHtmxConfirm_default);
  module_default.data("table", alpine_table_default);
  module_default.start();
})();
//# sourceMappingURL=index.js.map
