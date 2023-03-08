(function() {
  const t = document.createElement("link").relList;
  if (t && t.supports && t.supports("modulepreload"))
    return;
  for (const o of document.querySelectorAll('link[rel="modulepreload"]'))
    i(o);
  new MutationObserver((o) => {
    for (const s of o)
      if (s.type === "childList")
        for (const a of s.addedNodes)
          a.tagName === "LINK" && a.rel === "modulepreload" && i(a);
  }).observe(document, { childList: !0, subtree: !0 });
  function e(o) {
    const s = {};
    return o.integrity && (s.integrity = o.integrity), o.referrerpolicy && (s.referrerPolicy = o.referrerpolicy), o.crossorigin === "use-credentials" ? s.credentials = "include" : o.crossorigin === "anonymous" ? s.credentials = "omit" : s.credentials = "same-origin", s;
  }
  function i(o) {
    if (o.ep)
      return;
    o.ep = !0;
    const s = e(o);
    fetch(o.href, s);
  }
})();
const q = window, dt = q.ShadowRoot && (q.ShadyCSS === void 0 || q.ShadyCSS.nativeShadow) && "adoptedStyleSheets" in Document.prototype && "replace" in CSSStyleSheet.prototype, ht = Symbol(), mt = /* @__PURE__ */ new WeakMap();
let St = class {
  constructor(t, e, i) {
    if (this._$cssResult$ = !0, i !== ht)
      throw Error("CSSResult is not constructable. Use `unsafeCSS` or `css` instead.");
    this.cssText = t, this.t = e;
  }
  get styleSheet() {
    let t = this.o;
    const e = this.t;
    if (dt && t === void 0) {
      const i = e !== void 0 && e.length === 1;
      i && (t = mt.get(e)), t === void 0 && ((this.o = t = new CSSStyleSheet()).replaceSync(this.cssText), i && mt.set(e, t));
    }
    return t;
  }
  toString() {
    return this.cssText;
  }
};
const B = (r) => new St(typeof r == "string" ? r : r + "", void 0, ht), X = (r, ...t) => {
  const e = r.length === 1 ? r[0] : t.reduce((i, o, s) => i + ((a) => {
    if (a._$cssResult$ === !0)
      return a.cssText;
    if (typeof a == "number")
      return a;
    throw Error("Value passed to 'css' function must be a 'css' function result: " + a + ". Use 'unsafeCSS' to pass non-literal values, but take care to ensure page security.");
  })(o) + r[s + 1], r[0]);
  return new St(e, r, ht);
}, Ut = (r, t) => {
  dt ? r.adoptedStyleSheets = t.map((e) => e instanceof CSSStyleSheet ? e : e.styleSheet) : t.forEach((e) => {
    const i = document.createElement("style"), o = q.litNonce;
    o !== void 0 && i.setAttribute("nonce", o), i.textContent = e.cssText, r.appendChild(i);
  });
}, ft = dt ? (r) => r : (r) => r instanceof CSSStyleSheet ? ((t) => {
  let e = "";
  for (const i of t.cssRules)
    e += i.cssText;
  return B(e);
})(r) : r;
var it;
const V = window, vt = V.trustedTypes, Tt = vt ? vt.emptyScript : "", yt = V.reactiveElementPolyfillSupport, ct = { toAttribute(r, t) {
  switch (t) {
    case Boolean:
      r = r ? Tt : null;
      break;
    case Object:
    case Array:
      r = r == null ? r : JSON.stringify(r);
  }
  return r;
}, fromAttribute(r, t) {
  let e = r;
  switch (t) {
    case Boolean:
      e = r !== null;
      break;
    case Number:
      e = r === null ? null : Number(r);
      break;
    case Object:
    case Array:
      try {
        e = JSON.parse(r);
      } catch {
        e = null;
      }
  }
  return e;
} }, kt = (r, t) => t !== r && (t == t || r == r), ot = { attribute: !0, type: String, converter: ct, reflect: !1, hasChanged: kt };
let P = class extends HTMLElement {
  constructor() {
    super(), this._$Ei = /* @__PURE__ */ new Map(), this.isUpdatePending = !1, this.hasUpdated = !1, this._$El = null, this.u();
  }
  static addInitializer(t) {
    var e;
    this.finalize(), ((e = this.h) !== null && e !== void 0 ? e : this.h = []).push(t);
  }
  static get observedAttributes() {
    this.finalize();
    const t = [];
    return this.elementProperties.forEach((e, i) => {
      const o = this._$Ep(i, e);
      o !== void 0 && (this._$Ev.set(o, i), t.push(o));
    }), t;
  }
  static createProperty(t, e = ot) {
    if (e.state && (e.attribute = !1), this.finalize(), this.elementProperties.set(t, e), !e.noAccessor && !this.prototype.hasOwnProperty(t)) {
      const i = typeof t == "symbol" ? Symbol() : "__" + t, o = this.getPropertyDescriptor(t, i, e);
      o !== void 0 && Object.defineProperty(this.prototype, t, o);
    }
  }
  static getPropertyDescriptor(t, e, i) {
    return { get() {
      return this[e];
    }, set(o) {
      const s = this[t];
      this[e] = o, this.requestUpdate(t, s, i);
    }, configurable: !0, enumerable: !0 };
  }
  static getPropertyOptions(t) {
    return this.elementProperties.get(t) || ot;
  }
  static finalize() {
    if (this.hasOwnProperty("finalized"))
      return !1;
    this.finalized = !0;
    const t = Object.getPrototypeOf(this);
    if (t.finalize(), t.h !== void 0 && (this.h = [...t.h]), this.elementProperties = new Map(t.elementProperties), this._$Ev = /* @__PURE__ */ new Map(), this.hasOwnProperty("properties")) {
      const e = this.properties, i = [...Object.getOwnPropertyNames(e), ...Object.getOwnPropertySymbols(e)];
      for (const o of i)
        this.createProperty(o, e[o]);
    }
    return this.elementStyles = this.finalizeStyles(this.styles), !0;
  }
  static finalizeStyles(t) {
    const e = [];
    if (Array.isArray(t)) {
      const i = new Set(t.flat(1 / 0).reverse());
      for (const o of i)
        e.unshift(ft(o));
    } else
      t !== void 0 && e.push(ft(t));
    return e;
  }
  static _$Ep(t, e) {
    const i = e.attribute;
    return i === !1 ? void 0 : typeof i == "string" ? i : typeof t == "string" ? t.toLowerCase() : void 0;
  }
  u() {
    var t;
    this._$E_ = new Promise((e) => this.enableUpdating = e), this._$AL = /* @__PURE__ */ new Map(), this._$Eg(), this.requestUpdate(), (t = this.constructor.h) === null || t === void 0 || t.forEach((e) => e(this));
  }
  addController(t) {
    var e, i;
    ((e = this._$ES) !== null && e !== void 0 ? e : this._$ES = []).push(t), this.renderRoot !== void 0 && this.isConnected && ((i = t.hostConnected) === null || i === void 0 || i.call(t));
  }
  removeController(t) {
    var e;
    (e = this._$ES) === null || e === void 0 || e.splice(this._$ES.indexOf(t) >>> 0, 1);
  }
  _$Eg() {
    this.constructor.elementProperties.forEach((t, e) => {
      this.hasOwnProperty(e) && (this._$Ei.set(e, this[e]), delete this[e]);
    });
  }
  createRenderRoot() {
    var t;
    const e = (t = this.shadowRoot) !== null && t !== void 0 ? t : this.attachShadow(this.constructor.shadowRootOptions);
    return Ut(e, this.constructor.elementStyles), e;
  }
  connectedCallback() {
    var t;
    this.renderRoot === void 0 && (this.renderRoot = this.createRenderRoot()), this.enableUpdating(!0), (t = this._$ES) === null || t === void 0 || t.forEach((e) => {
      var i;
      return (i = e.hostConnected) === null || i === void 0 ? void 0 : i.call(e);
    });
  }
  enableUpdating(t) {
  }
  disconnectedCallback() {
    var t;
    (t = this._$ES) === null || t === void 0 || t.forEach((e) => {
      var i;
      return (i = e.hostDisconnected) === null || i === void 0 ? void 0 : i.call(e);
    });
  }
  attributeChangedCallback(t, e, i) {
    this._$AK(t, i);
  }
  _$EO(t, e, i = ot) {
    var o;
    const s = this.constructor._$Ep(t, i);
    if (s !== void 0 && i.reflect === !0) {
      const a = (((o = i.converter) === null || o === void 0 ? void 0 : o.toAttribute) !== void 0 ? i.converter : ct).toAttribute(e, i.type);
      this._$El = t, a == null ? this.removeAttribute(s) : this.setAttribute(s, a), this._$El = null;
    }
  }
  _$AK(t, e) {
    var i;
    const o = this.constructor, s = o._$Ev.get(t);
    if (s !== void 0 && this._$El !== s) {
      const a = o.getPropertyOptions(s), h = typeof a.converter == "function" ? { fromAttribute: a.converter } : ((i = a.converter) === null || i === void 0 ? void 0 : i.fromAttribute) !== void 0 ? a.converter : ct;
      this._$El = s, this[s] = h.fromAttribute(e, a.type), this._$El = null;
    }
  }
  requestUpdate(t, e, i) {
    let o = !0;
    t !== void 0 && (((i = i || this.constructor.getPropertyOptions(t)).hasChanged || kt)(this[t], e) ? (this._$AL.has(t) || this._$AL.set(t, e), i.reflect === !0 && this._$El !== t && (this._$EC === void 0 && (this._$EC = /* @__PURE__ */ new Map()), this._$EC.set(t, i))) : o = !1), !this.isUpdatePending && o && (this._$E_ = this._$Ej());
  }
  async _$Ej() {
    this.isUpdatePending = !0;
    try {
      await this._$E_;
    } catch (e) {
      Promise.reject(e);
    }
    const t = this.scheduleUpdate();
    return t != null && await t, !this.isUpdatePending;
  }
  scheduleUpdate() {
    return this.performUpdate();
  }
  performUpdate() {
    var t;
    if (!this.isUpdatePending)
      return;
    this.hasUpdated, this._$Ei && (this._$Ei.forEach((o, s) => this[s] = o), this._$Ei = void 0);
    let e = !1;
    const i = this._$AL;
    try {
      e = this.shouldUpdate(i), e ? (this.willUpdate(i), (t = this._$ES) === null || t === void 0 || t.forEach((o) => {
        var s;
        return (s = o.hostUpdate) === null || s === void 0 ? void 0 : s.call(o);
      }), this.update(i)) : this._$Ek();
    } catch (o) {
      throw e = !1, this._$Ek(), o;
    }
    e && this._$AE(i);
  }
  willUpdate(t) {
  }
  _$AE(t) {
    var e;
    (e = this._$ES) === null || e === void 0 || e.forEach((i) => {
      var o;
      return (o = i.hostUpdated) === null || o === void 0 ? void 0 : o.call(i);
    }), this.hasUpdated || (this.hasUpdated = !0, this.firstUpdated(t)), this.updated(t);
  }
  _$Ek() {
    this._$AL = /* @__PURE__ */ new Map(), this.isUpdatePending = !1;
  }
  get updateComplete() {
    return this.getUpdateComplete();
  }
  getUpdateComplete() {
    return this._$E_;
  }
  shouldUpdate(t) {
    return !0;
  }
  update(t) {
    this._$EC !== void 0 && (this._$EC.forEach((e, i) => this._$EO(i, this[i], e)), this._$EC = void 0), this._$Ek();
  }
  updated(t) {
  }
  firstUpdated(t) {
  }
};
P.finalized = !0, P.elementProperties = /* @__PURE__ */ new Map(), P.elementStyles = [], P.shadowRootOptions = { mode: "open" }, yt == null || yt({ ReactiveElement: P }), ((it = V.reactiveElementVersions) !== null && it !== void 0 ? it : V.reactiveElementVersions = []).push("1.5.0");
var st;
const W = window, k = W.trustedTypes, bt = k ? k.createPolicy("lit-html", { createHTML: (r) => r }) : void 0, _ = `lit$${(Math.random() + "").slice(9)}$`, Et = "?" + _, Nt = `<${Et}>`, E = document, M = (r = "") => E.createComment(r), j = (r) => r === null || typeof r != "object" && typeof r != "function", Ot = Array.isArray, Rt = (r) => Ot(r) || typeof (r == null ? void 0 : r[Symbol.iterator]) == "function", N = /<(?:(!--|\/[^a-zA-Z])|(\/?[a-zA-Z][^>\s]*)|(\/?$))/g, $t = /-->/g, xt = />/g, A = RegExp(`>|[ 	
\f\r](?:([^\\s"'>=/]+)([ 	
\f\r]*=[ 	
\f\r]*(?:[^ 	
\f\r"'\`<>=]|("|')|))|$)`, "g"), _t = /'/g, wt = /"/g, Dt = /^(?:script|style|textarea|title)$/i, Ht = (r) => (t, ...e) => ({ _$litType$: r, strings: t, values: e }), c = Ht(1), O = Symbol.for("lit-noChange"), g = Symbol.for("lit-nothing"), At = /* @__PURE__ */ new WeakMap(), S = E.createTreeWalker(E, 129, null, !1), Mt = (r, t) => {
  const e = r.length - 1, i = [];
  let o, s = t === 2 ? "<svg>" : "", a = N;
  for (let n = 0; n < e; n++) {
    const l = r[n];
    let $, d, p = -1, y = 0;
    for (; y < l.length && (a.lastIndex = y, d = a.exec(l), d !== null); )
      y = a.lastIndex, a === N ? d[1] === "!--" ? a = $t : d[1] !== void 0 ? a = xt : d[2] !== void 0 ? (Dt.test(d[2]) && (o = RegExp("</" + d[2], "g")), a = A) : d[3] !== void 0 && (a = A) : a === A ? d[0] === ">" ? (a = o ?? N, p = -1) : d[1] === void 0 ? p = -2 : (p = a.lastIndex - d[2].length, $ = d[1], a = d[3] === void 0 ? A : d[3] === '"' ? wt : _t) : a === wt || a === _t ? a = A : a === $t || a === xt ? a = N : (a = A, o = void 0);
    const K = a === A && r[n + 1].startsWith("/>") ? " " : "";
    s += a === N ? l + Nt : p >= 0 ? (i.push($), l.slice(0, p) + "$lit$" + l.slice(p) + _ + K) : l + _ + (p === -2 ? (i.push(void 0), n) : K);
  }
  const h = s + (r[e] || "<?>") + (t === 2 ? "</svg>" : "");
  if (!Array.isArray(r) || !r.hasOwnProperty("raw"))
    throw Error("invalid template strings array");
  return [bt !== void 0 ? bt.createHTML(h) : h, i];
};
class I {
  constructor({ strings: t, _$litType$: e }, i) {
    let o;
    this.parts = [];
    let s = 0, a = 0;
    const h = t.length - 1, n = this.parts, [l, $] = Mt(t, e);
    if (this.el = I.createElement(l, i), S.currentNode = this.el.content, e === 2) {
      const d = this.el.content, p = d.firstChild;
      p.remove(), d.append(...p.childNodes);
    }
    for (; (o = S.nextNode()) !== null && n.length < h; ) {
      if (o.nodeType === 1) {
        if (o.hasAttributes()) {
          const d = [];
          for (const p of o.getAttributeNames())
            if (p.endsWith("$lit$") || p.startsWith(_)) {
              const y = $[a++];
              if (d.push(p), y !== void 0) {
                const K = o.getAttribute(y.toLowerCase() + "$lit$").split(_), Q = /([.?@])?(.*)/.exec(y);
                n.push({ type: 1, index: s, name: Q[2], strings: K, ctor: Q[1] === "." ? It : Q[1] === "?" ? Bt : Q[1] === "@" ? Gt : Y });
              } else
                n.push({ type: 6, index: s });
            }
          for (const p of d)
            o.removeAttribute(p);
        }
        if (Dt.test(o.tagName)) {
          const d = o.textContent.split(_), p = d.length - 1;
          if (p > 0) {
            o.textContent = k ? k.emptyScript : "";
            for (let y = 0; y < p; y++)
              o.append(d[y], M()), S.nextNode(), n.push({ type: 2, index: ++s });
            o.append(d[p], M());
          }
        }
      } else if (o.nodeType === 8)
        if (o.data === Et)
          n.push({ type: 2, index: s });
        else {
          let d = -1;
          for (; (d = o.data.indexOf(_, d + 1)) !== -1; )
            n.push({ type: 7, index: s }), d += _.length - 1;
        }
      s++;
    }
  }
  static createElement(t, e) {
    const i = E.createElement("template");
    return i.innerHTML = t, i;
  }
}
function D(r, t, e = r, i) {
  var o, s, a, h;
  if (t === O)
    return t;
  let n = i !== void 0 ? (o = e._$Co) === null || o === void 0 ? void 0 : o[i] : e._$Cl;
  const l = j(t) ? void 0 : t._$litDirective$;
  return (n == null ? void 0 : n.constructor) !== l && ((s = n == null ? void 0 : n._$AO) === null || s === void 0 || s.call(n, !1), l === void 0 ? n = void 0 : (n = new l(r), n._$AT(r, e, i)), i !== void 0 ? ((a = (h = e)._$Co) !== null && a !== void 0 ? a : h._$Co = [])[i] = n : e._$Cl = n), n !== void 0 && (t = D(r, n._$AS(r, t.values), n, i)), t;
}
class jt {
  constructor(t, e) {
    this.u = [], this._$AN = void 0, this._$AD = t, this._$AM = e;
  }
  get parentNode() {
    return this._$AM.parentNode;
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  v(t) {
    var e;
    const { el: { content: i }, parts: o } = this._$AD, s = ((e = t == null ? void 0 : t.creationScope) !== null && e !== void 0 ? e : E).importNode(i, !0);
    S.currentNode = s;
    let a = S.nextNode(), h = 0, n = 0, l = o[0];
    for (; l !== void 0; ) {
      if (h === l.index) {
        let $;
        l.type === 2 ? $ = new G(a, a.nextSibling, this, t) : l.type === 1 ? $ = new l.ctor(a, l.name, l.strings, this, t) : l.type === 6 && ($ = new Ft(a, this, t)), this.u.push($), l = o[++n];
      }
      h !== (l == null ? void 0 : l.index) && (a = S.nextNode(), h++);
    }
    return s;
  }
  p(t) {
    let e = 0;
    for (const i of this.u)
      i !== void 0 && (i.strings !== void 0 ? (i._$AI(t, i, e), e += i.strings.length - 2) : i._$AI(t[e])), e++;
  }
}
class G {
  constructor(t, e, i, o) {
    var s;
    this.type = 2, this._$AH = g, this._$AN = void 0, this._$AA = t, this._$AB = e, this._$AM = i, this.options = o, this._$Cm = (s = o == null ? void 0 : o.isConnected) === null || s === void 0 || s;
  }
  get _$AU() {
    var t, e;
    return (e = (t = this._$AM) === null || t === void 0 ? void 0 : t._$AU) !== null && e !== void 0 ? e : this._$Cm;
  }
  get parentNode() {
    let t = this._$AA.parentNode;
    const e = this._$AM;
    return e !== void 0 && t.nodeType === 11 && (t = e.parentNode), t;
  }
  get startNode() {
    return this._$AA;
  }
  get endNode() {
    return this._$AB;
  }
  _$AI(t, e = this) {
    t = D(this, t, e), j(t) ? t === g || t == null || t === "" ? (this._$AH !== g && this._$AR(), this._$AH = g) : t !== this._$AH && t !== O && this.g(t) : t._$litType$ !== void 0 ? this.$(t) : t.nodeType !== void 0 ? this.T(t) : Rt(t) ? this.k(t) : this.g(t);
  }
  O(t, e = this._$AB) {
    return this._$AA.parentNode.insertBefore(t, e);
  }
  T(t) {
    this._$AH !== t && (this._$AR(), this._$AH = this.O(t));
  }
  g(t) {
    this._$AH !== g && j(this._$AH) ? this._$AA.nextSibling.data = t : this.T(E.createTextNode(t)), this._$AH = t;
  }
  $(t) {
    var e;
    const { values: i, _$litType$: o } = t, s = typeof o == "number" ? this._$AC(t) : (o.el === void 0 && (o.el = I.createElement(o.h, this.options)), o);
    if (((e = this._$AH) === null || e === void 0 ? void 0 : e._$AD) === s)
      this._$AH.p(i);
    else {
      const a = new jt(s, this), h = a.v(this.options);
      a.p(i), this.T(h), this._$AH = a;
    }
  }
  _$AC(t) {
    let e = At.get(t.strings);
    return e === void 0 && At.set(t.strings, e = new I(t)), e;
  }
  k(t) {
    Ot(this._$AH) || (this._$AH = [], this._$AR());
    const e = this._$AH;
    let i, o = 0;
    for (const s of t)
      o === e.length ? e.push(i = new G(this.O(M()), this.O(M()), this, this.options)) : i = e[o], i._$AI(s), o++;
    o < e.length && (this._$AR(i && i._$AB.nextSibling, o), e.length = o);
  }
  _$AR(t = this._$AA.nextSibling, e) {
    var i;
    for ((i = this._$AP) === null || i === void 0 || i.call(this, !1, !0, e); t && t !== this._$AB; ) {
      const o = t.nextSibling;
      t.remove(), t = o;
    }
  }
  setConnected(t) {
    var e;
    this._$AM === void 0 && (this._$Cm = t, (e = this._$AP) === null || e === void 0 || e.call(this, t));
  }
}
class Y {
  constructor(t, e, i, o, s) {
    this.type = 1, this._$AH = g, this._$AN = void 0, this.element = t, this.name = e, this._$AM = o, this.options = s, i.length > 2 || i[0] !== "" || i[1] !== "" ? (this._$AH = Array(i.length - 1).fill(new String()), this.strings = i) : this._$AH = g;
  }
  get tagName() {
    return this.element.tagName;
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  _$AI(t, e = this, i, o) {
    const s = this.strings;
    let a = !1;
    if (s === void 0)
      t = D(this, t, e, 0), a = !j(t) || t !== this._$AH && t !== O, a && (this._$AH = t);
    else {
      const h = t;
      let n, l;
      for (t = s[0], n = 0; n < s.length - 1; n++)
        l = D(this, h[i + n], e, n), l === O && (l = this._$AH[n]), a || (a = !j(l) || l !== this._$AH[n]), l === g ? t = g : t !== g && (t += (l ?? "") + s[n + 1]), this._$AH[n] = l;
    }
    a && !o && this.j(t);
  }
  j(t) {
    t === g ? this.element.removeAttribute(this.name) : this.element.setAttribute(this.name, t ?? "");
  }
}
class It extends Y {
  constructor() {
    super(...arguments), this.type = 3;
  }
  j(t) {
    this.element[this.name] = t === g ? void 0 : t;
  }
}
const Lt = k ? k.emptyScript : "";
class Bt extends Y {
  constructor() {
    super(...arguments), this.type = 4;
  }
  j(t) {
    t && t !== g ? this.element.setAttribute(this.name, Lt) : this.element.removeAttribute(this.name);
  }
}
class Gt extends Y {
  constructor(t, e, i, o, s) {
    super(t, e, i, o, s), this.type = 5;
  }
  _$AI(t, e = this) {
    var i;
    if ((t = (i = D(this, t, e, 0)) !== null && i !== void 0 ? i : g) === O)
      return;
    const o = this._$AH, s = t === g && o !== g || t.capture !== o.capture || t.once !== o.once || t.passive !== o.passive, a = t !== g && (o === g || s);
    s && this.element.removeEventListener(this.name, this, o), a && this.element.addEventListener(this.name, this, t), this._$AH = t;
  }
  handleEvent(t) {
    var e, i;
    typeof this._$AH == "function" ? this._$AH.call((i = (e = this.options) === null || e === void 0 ? void 0 : e.host) !== null && i !== void 0 ? i : this.element, t) : this._$AH.handleEvent(t);
  }
}
class Ft {
  constructor(t, e, i) {
    this.element = t, this.type = 6, this._$AN = void 0, this._$AM = e, this.options = i;
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  _$AI(t) {
    D(this, t);
  }
}
const Ct = W.litHtmlPolyfillSupport;
Ct == null || Ct(I, G), ((st = W.litHtmlVersions) !== null && st !== void 0 ? st : W.litHtmlVersions = []).push("2.5.0");
const Kt = (r, t, e) => {
  var i, o;
  const s = (i = e == null ? void 0 : e.renderBefore) !== null && i !== void 0 ? i : t;
  let a = s._$litPart$;
  if (a === void 0) {
    const h = (o = e == null ? void 0 : e.renderBefore) !== null && o !== void 0 ? o : null;
    s._$litPart$ = a = new G(t.insertBefore(M(), h), h, void 0, e ?? {});
  }
  return a._$AI(r), a;
};
var at, nt;
class b extends P {
  constructor() {
    super(...arguments), this.renderOptions = { host: this }, this._$Do = void 0;
  }
  createRenderRoot() {
    var t, e;
    const i = super.createRenderRoot();
    return (t = (e = this.renderOptions).renderBefore) !== null && t !== void 0 || (e.renderBefore = i.firstChild), i;
  }
  update(t) {
    const e = this.render();
    this.hasUpdated || (this.renderOptions.isConnected = this.isConnected), super.update(t), this._$Do = Kt(e, this.renderRoot, this.renderOptions);
  }
  connectedCallback() {
    var t;
    super.connectedCallback(), (t = this._$Do) === null || t === void 0 || t.setConnected(!0);
  }
  disconnectedCallback() {
    var t;
    super.disconnectedCallback(), (t = this._$Do) === null || t === void 0 || t.setConnected(!1);
  }
  render() {
    return O;
  }
}
b.finalized = !0, b._$litElement$ = !0, (at = globalThis.litElementHydrateSupport) === null || at === void 0 || at.call(globalThis, { LitElement: b });
const Pt = globalThis.litElementPolyfillSupport;
Pt == null || Pt({ LitElement: b });
((nt = globalThis.litElementVersions) !== null && nt !== void 0 ? nt : globalThis.litElementVersions = []).push("3.2.2");
const T = (r) => (t) => typeof t == "function" ? ((e, i) => (customElements.define(e, i), i))(r, t) : ((e, i) => {
  const { kind: o, elements: s } = i;
  return { kind: o, elements: s, finisher(a) {
    customElements.define(e, a);
  } };
})(r, t);
const Qt = (r, t) => t.kind === "method" && t.descriptor && !("value" in t.descriptor) ? { ...t, finisher(e) {
  e.createProperty(t.key, r);
} } : { kind: "field", key: Symbol(), placement: "own", descriptor: {}, originalKey: t.key, initializer() {
  typeof t.initializer == "function" && (this[t.key] = t.initializer.call(this));
}, finisher(e) {
  e.createProperty(t.key, r);
} };
function m(r) {
  return (t, e) => e !== void 0 ? ((i, o, s) => {
    o.constructor.createProperty(s, i);
  })(r, t, e) : Qt(r, t);
}
function v(r) {
  return m({ ...r, state: !0 });
}
var lt;
((lt = window.HTMLSlotElement) === null || lt === void 0 ? void 0 : lt.prototype.assignedElements) != null;
var qt = Object.defineProperty, Vt = Object.getOwnPropertyDescriptor, pt = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? Vt(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && qt(t, e, o), o;
};
let J = class extends b {
  constructor() {
    super(...arguments), this.key = "", this.isActive = !1;
  }
  render() {
    return c`<slot></slot>`;
  }
};
pt([
  m()
], J.prototype, "key", 2);
pt([
  m({ type: Boolean, attribute: "is-active" })
], J.prototype, "isActive", 2);
J = pt([
  T("lit-tab")
], J);
const Z = `:root{--bg-color: #ffffff;--bg-secondary-color: #f3f3f6;--color-primary: #14854F;--color-lightGrey: #d2d6dd;--color-grey: #747681;--color-darkGrey: #3f4144;--color-error: #d43939;--color-success: #28bd14;--grid-maxWidth: 120rem;--grid-gutter: 2rem;--font-size: 1.6rem;--font-color: #333333;--font-family-sans: -apple-system, BlinkMacSystemFont, Avenir, "Avenir Next", "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif;--font-family-mono: monaco, "Consolas", "Lucida Console", monospace}html{box-sizing:border-box;font-size:62.5%;line-height:1.15;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}*,*:before,*:after{box-sizing:inherit}body{background-color:var(--bg-color);line-height:1.6;font-size:var(--font-size);color:var(--font-color);font-family:Segoe UI,Helvetica Neue,sans-serif;font-family:var(--font-family-sans);margin:0;padding:0}h1,h2,h3,h4,h5,h6{font-weight:500;margin:.35em 0 .7em}h1{font-size:2em}h2{font-size:1.75em}h3{font-size:1.5em}h4{font-size:1.25em}h5{font-size:1em}h6{font-size:.85em}a{color:var(--color-primary);text-decoration:none}a:hover:not(.button){opacity:.75}button{font-family:inherit}p{margin-top:0}blockquote{background-color:var(--bg-secondary-color);padding:1.5rem 2rem;border-left:3px solid var(--color-lightGrey)}dl dt{font-weight:700}hr{border:none;background-color:var(--color-lightGrey);height:1px;margin:1rem 0}table{width:100%;border:none;border-collapse:collapse;border-spacing:0;text-align:left}table.striped tr:nth-of-type(2n){background-color:var(--bg-secondary-color)}td,th{vertical-align:middle;padding:1.2rem .4rem}thead{border-bottom:2px solid var(--color-lightGrey)}tfoot{border-top:2px solid var(--color-lightGrey)}code,kbd,pre,samp,tt{font-family:var(--font-family-mono)}code,kbd{padding:0 .4rem;font-size:90%;white-space:pre-wrap;border-radius:4px;padding:.2em .4em;background-color:var(--bg-secondary-color);color:var(--color-error)}pre{background-color:var(--bg-secondary-color);font-size:1em;padding:1rem;overflow-x:auto}pre code{background:none;padding:0}abbr[title]{border-bottom:none;text-decoration:underline;text-decoration:underline dotted}img{max-width:100%}fieldset{border:1px solid var(--color-lightGrey)}iframe{border:0}.container{max-width:var(--grid-maxWidth);margin:0 auto;width:96%;padding:0 calc(var(--grid-gutter) / 2)}.row{display:flex;flex-flow:row wrap;justify-content:flex-start;margin-left:calc(var(--grid-gutter) / -2);margin-right:calc(var(--grid-gutter) / -2)}.row.reverse{flex-direction:row-reverse}.col{flex:1}.col,[class*=" col-"],[class^=col-]{margin:0 calc(var(--grid-gutter) / 2) calc(var(--grid-gutter) / 2)}.col-1{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}@media screen and (max-width: 599px){.container{width:100%}.col,[class*=col-],[class^=col-]{flex:0 1 100%;max-width:100%}}@media screen and (min-width: 900px){.col-1-md{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2-md{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3-md{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4-md{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5-md{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6-md{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7-md{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8-md{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9-md{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10-md{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11-md{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12-md{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}}@media screen and (min-width: 1200px){.col-1-lg{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2-lg{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3-lg{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4-lg{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5-lg{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6-lg{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7-lg{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8-lg{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9-lg{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10-lg{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11-lg{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12-lg{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}}fieldset{padding:.5rem 2rem}legend{text-transform:uppercase;font-size:.8em;letter-spacing:.1rem}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),select,textarea,textarea[type=text]{font-family:inherit;padding:.8rem 1rem;border-radius:4px;border:1px solid var(--color-lightGrey);font-size:1em;transition:all .2s ease;display:block;width:100%}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]):not(:disabled):hover,select:hover,textarea:hover,textarea[type=text]:hover{border-color:var(--color-grey)}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]):focus,select:focus,textarea:focus,textarea[type=text]:focus{outline:none;border-color:var(--color-primary);box-shadow:0 0 1px var(--color-primary)}input.error:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),textarea.error{border-color:var(--color-error)}input.success:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),textarea.success{border-color:var(--color-success)}select{-webkit-appearance:none;background:#f3f3f6 no-repeat 100%;background-size:1ex;background-origin:content-box;background-image:url("data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='60' height='40' fill='%23555'><polygon points='0,0 60,0 30,40'/></svg>")}[type=checkbox],[type=radio]{width:1.6rem;height:1.6rem}.button,[type=button],[type=reset],[type=submit],button{padding:1rem 2.5rem;color:var(--color-darkGrey);background:var(--color-lightGrey);border-radius:4px;border:1px solid transparent;font-size:var(--font-size);line-height:1;text-align:center;transition:opacity .2s ease;text-decoration:none;transform:scale(1);display:inline-block;cursor:pointer}.grouped{display:flex}.grouped>*:not(:last-child){margin-right:16px}.grouped.gapless>*{margin:0 0 0 -1px!important;border-radius:0!important}.grouped.gapless>*:first-child{margin:0!important;border-radius:4px 0 0 4px!important}.grouped.gapless>*:last-child{border-radius:0 4px 4px 0!important}.button+.button{margin-left:1rem}.button:hover,[type=button]:hover,[type=reset]:hover,[type=submit]:hover,button:hover{opacity:.8}.button:active,[type=button]:active,[type=reset]:active,[type=submit]:active,button:active{transform:scale(.98)}input:disabled,button:disabled,input:disabled:hover,button:disabled:hover{opacity:.4;cursor:not-allowed}.button.primary,.button.secondary,.button.dark,.button.error,.button.success,[type=submit]{color:#fff;z-index:1;background-color:#000;background-color:var(--color-primary)}.button.secondary{background-color:var(--color-grey)}.button.dark{background-color:var(--color-darkGrey)}.button.error{background-color:var(--color-error)}.button.success{background-color:var(--color-success)}.button.outline{background-color:transparent;border-color:var(--color-lightGrey)}.button.outline.primary{border-color:var(--color-primary);color:var(--color-primary)}.button.outline.secondary{border-color:var(--color-grey);color:var(--color-grey)}.button.outline.dark{border-color:var(--color-darkGrey);color:var(--color-darkGrey)}.button.clear{background-color:transparent;border-color:transparent;color:var(--color-primary)}.button.icon{display:inline-flex;align-items:center}.button.icon>img{margin-left:2px}.button.icon-only{padding:1rem}::placeholder{color:#bdbfc4}.card{padding:1rem 2rem;border-radius:4px;background:var(--bg-color);box-shadow:0 1px 3px var(--color-grey)}.card p:last-child{margin:0}.card header>*{margin-top:0;margin-bottom:1rem}.tabs{display:flex}.tabs a{text-decoration:none}.tabs>.dropdown>summary,.tabs>a{padding:1rem 2rem;flex:0 1 auto;color:var(--color-darkGrey);border-bottom:2px solid var(--color-lightGrey);text-align:center}.tabs>a.active,.tabs>a:hover{opacity:1;border-bottom:2px solid var(--color-darkGrey)}.tabs>a.active{border-color:var(--color-primary)}.tabs.is-full a{flex:1 1 auto}.bg-primary{background-color:var(--color-primary)!important}.bg-light{background-color:var(--color-lightGrey)!important}.bg-dark{background-color:var(--color-darkGrey)!important}.bg-grey{background-color:var(--color-grey)!important}.bg-error{background-color:var(--color-error)!important}.bg-success{background-color:var(--color-success)!important}.bd-primary{border:1px solid var(--color-primary)!important}.bd-light{border:1px solid var(--color-lightGrey)!important}.bd-dark{border:1px solid var(--color-darkGrey)!important}.bd-grey{border:1px solid var(--color-grey)!important}.bd-error{border:1px solid var(--color-error)!important}.bd-success{border:1px solid var(--color-success)!important}.text-primary{color:var(--color-primary)!important}.text-light{color:var(--color-lightGrey)!important}.text-dark{color:var(--color-darkGrey)!important}.text-grey{color:var(--color-grey)!important}.text-error{color:var(--color-error)!important}.text-success{color:var(--color-success)!important}.text-white{color:#fff!important}.pull-right{float:right!important}.pull-left{float:left!important}.text-center{text-align:center}.text-left{text-align:left}.text-right{text-align:right}.text-justify{text-align:justify}.text-uppercase{text-transform:uppercase}.text-lowercase{text-transform:lowercase}.text-capitalize{text-transform:capitalize}.is-full-screen{width:100%;min-height:100vh}.is-full-width{width:100%!important}.is-vertical-align{display:flex;align-items:center}.is-horizontal-align{display:flex;justify-content:center}.is-center{display:flex;align-items:center;justify-content:center}.is-right{display:flex;align-items:center;justify-content:flex-end}.is-left{display:flex;align-items:center;justify-content:flex-start}.is-fixed{position:fixed;width:100%}.is-paddingless{padding:0!important}.is-marginless{margin:0!important}.is-pointer{cursor:pointer!important}.is-rounded{border-radius:100%}.clearfix{content:"";display:table;clear:both}.is-hidden{display:none!important}@media screen and (max-width: 599px){.hide-xs{display:none!important}}@media screen and (min-width: 600px) and (max-width: 899px){.hide-sm{display:none!important}}@media screen and (min-width: 900px) and (max-width: 1199px){.hide-md{display:none!important}}@media screen and (min-width: 1200px){.hide-lg{display:none!important}}@media print{.hide-pr{display:none!important}}:host,:root{--bg-color: #ffffff;--bg-secondary-color: #f3f3f6;--color-primary: #5783db;--color-lightGrey: #d2d6dd;--color-grey: #3e9a80;--color-darkGrey: #80669d;--color-error: #d43939;--color-success: #33b249;--grid-maxWidth: 150rem;--grid-gutter: 1rem;--font-size: 1.6rem;--font-color: #333333;--font-family-sans: -apple-system, BlinkMacSystemFont, Avenir, "Avenir Next", "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif;--font-family-mono: monaco, "Consolas", "Lucida Console", monospace}:not(:defined){display:none}.cursor-pointer{cursor:pointer}.button-dismiss{border:none;color:inherit;background:inherit;margin-top:-1rem;font-weight:700}.mb-1{margin-bottom:1rem}
`;
var Wt = Object.defineProperty, Jt = Object.getOwnPropertyDescriptor, tt = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? Jt(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && Wt(t, e, o), o;
};
let z = class extends b {
  constructor() {
    super(...arguments), this.isFull = !1, this.activeTab = "", this.tabs = /* @__PURE__ */ new Map();
  }
  async firstUpdated() {
    if (!this.shadowRoot)
      return;
    const r = this.shadowRoot.querySelector("slot");
    if (r) {
      const t = r.assignedNodes();
      this.tabs = new Map(t.filter((e) => e.nodeName === "LIT-TAB").map((e) => e).map((e) => [e.key, e])), this.tabs.forEach((e, i) => {
        e.isActive && !this.activeTab && (this.activeTab = i);
      }), !this.activeTab && this.tabs.size && (this.activeTab = this.tabs.entries().next().value[0]);
    }
  }
  renderInnerContent() {
    if (!this.tabs.size)
      return;
    const r = Array.from(this.tabs.keys());
    return c`
            <nav class="tabs ${this.isFull ? "is-full" : ""}">
                ${r.map((t) => c`
                    <a class="cursor-pointer ${this.activeTab === t ? "active" : ""}" @click="${() => this.activeTab = t}"
                        id="nav-${t}" aria-controls="${t}" role="tab"
                    >
                        ${this.tabs.get(t)}
                    </a>
                `)}
            </nav>
            ${r.map((t) => c`
                <div class="tab-content ${this.activeTab === t ? "" : "is-hidden"}" id="${t}" ?aria-current="${this.activeTab === t}"
                    aria-labelledby="nav-${t}" role="tabpanel"
                >
                    <slot name="${t}"></slot>
                </div>
            `)}
        `;
  }
  render() {
    return c`<slot></slot>${this.renderInnerContent()}`;
  }
};
z.styles = [
  B(Z),
  X`
            .tab-content { padding: 2rem 0; }
        `
];
tt([
  m({ type: Boolean, attribute: "is-full" })
], z.prototype, "isFull", 2);
tt([
  v()
], z.prototype, "activeTab", 2);
tt([
  v()
], z.prototype, "tabs", 2);
z = tt([
  T("lit-nav")
], z);
var ut = /* @__PURE__ */ ((r) => (r.success = "success", r.error = "error", r))(ut || {}), R = /* @__PURE__ */ ((r) => (r.en = "en", r.es = "es", r))(R || {});
const Xt = {
  common: {
    dismiss: "Dismiss"
  },
  modal: {
    confirm: "Confirm",
    cancel: "Cancel"
  },
  table: {
    noData: "No matching data to display.",
    search: "Search",
    first: "First",
    previous: "Previous",
    next: "Next",
    last: "Last",
    perPage: "Per Page",
    add: "Add",
    edit: "Edit",
    delete: "Delete",
    confirmDelete: "Confirm Delete",
    areYouSure: "Are you sure?",
    to: "to",
    of: "of"
  }
}, Yt = {
  common: {
    dismiss: "Despedir"
  },
  modal: {
    confirm: "Confirmar",
    cancel: "Cancelar"
  },
  table: {
    noData: "No hay datos coincidentes para mostrar.",
    search: "Buscar",
    first: "Primero",
    previous: "Previo",
    next: "Próximo",
    last: "Último",
    perPage: "Por página",
    add: "Agregar",
    edit: "Editar",
    delete: "Borrar",
    confirmDelete: "Confirmar eliminación",
    areYouSure: "Estas seguro?",
    to: "a",
    of: "de"
  }
}, Zt = {
  en: Xt,
  es: Yt
};
var te = Object.defineProperty, ee = Object.getOwnPropertyDescriptor, re = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ee(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && te(t, e, o), o;
};
function zt(r, t = "") {
  return Object.keys(r).reduce((e, i) => {
    const o = t.length ? t + "." : "", s = r[i];
    return Array.isArray(s) || Object(s) === s ? Object.assign(e, zt(s, o + i)) : e[o + i] = s, e;
  }, {});
}
const gt = (r) => {
  class t extends r {
    constructor() {
      super(...arguments), this.lang = R.en;
    }
    localize(i) {
      this.i18n || (this.i18n = zt(Zt));
      const o = `${this.lang}.${i}`;
      return Object.prototype.hasOwnProperty.call(this.i18n, o) ? this.i18n[o] : i;
    }
  }
  return re([
    m({ converter: (e) => e && e in R ? R[e] : R.en })
  ], t.prototype, "lang", 2), t;
};
var ie = Object.defineProperty, oe = Object.getOwnPropertyDescriptor, et = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? oe(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && ie(t, e, o), o;
};
let U = class extends gt(b) {
  constructor() {
    super(...arguments), this.type = ut.success, this.noDismiss = !1, this.isDismissed = !1;
  }
  renderInnerContent() {
    return this.noDismiss ? c`<slot></slot>` : c`
            <div class="row">
                <span class="col-11"><slot></slot></span>
                <span class="col-1 text-right">
                    <button class="button icon-only button-dismiss" @click="${() => this.isDismissed = !0}" title="${this.localize("common.dismiss")}"
                        aria-label="${this.localize("common.dismiss")}"
                    >
                        &#10006;
                    </button>
                </span>
            </div>
        `;
  }
  render() {
    if (!(!this.type || this.isDismissed))
      return c`<div class="card bg-${this.type} mb-1 text-white" role="alert">${this.renderInnerContent()}</div>`;
  }
};
U.styles = [
  B(Z),
  X`
            col, [class*=" col-"], [class^="col-"] { margin: 0 calc(var(--grid-gutter) / 2) 0 calc(var(--grid-gutter) / 2); }
        `
];
et([
  m({ converter: (r) => r ? ut[r] : void 0 })
], U.prototype, "type", 2);
et([
  m({ type: Boolean, attribute: "no-dismiss" })
], U.prototype, "noDismiss", 2);
et([
  v()
], U.prototype, "isDismissed", 2);
U = et([
  T("lit-alert")
], U);
var H = /* @__PURE__ */ ((r) => (r.dialog = "dialog", r.confirm = "confirm", r))(H || {}), se = Object.defineProperty, ae = Object.getOwnPropertyDescriptor, F = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ae(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && se(t, e, o), o;
};
const ne = 'button:not(.no-focus), [href], input, select, textarea, [tabindex]:not([tabindex="-1"])';
let C = class extends gt(b) {
  constructor() {
    super(...arguments), this.key = "", this.type = H.dialog, this.href = "", this.isDismissed = !0, this.boundOnKeyDown = this.onKeyDown.bind(this);
  }
  onConfirmClick() {
    var t;
    this.isDismissed = !0;
    const r = Array.from(((t = this.shadowRoot) == null ? void 0 : t.querySelectorAll("form")) ?? []);
    r.length && r[0].action && r[0].submit();
  }
  onOverlayClick(r) {
    (r == null ? void 0 : r.composedPath()[0]).classList.contains("modal-overlay") && (this.isDismissed = !0);
  }
  onCancelClick() {
    this.isDismissed = !0;
  }
  keepFocus(r) {
    var o, s;
    const t = Array.from(((o = this.shadowRoot) == null ? void 0 : o.querySelectorAll(ne)) ?? []).map((a) => a);
    if (!t.length)
      return;
    const e = (s = this.shadowRoot) == null ? void 0 : s.activeElement, i = t.length - 1;
    r.shiftKey ? e === t[0] && (t[i].focus(), r.preventDefault()) : e === t[i] && (t[0].focus(), r.preventDefault());
  }
  onKeyDown(r) {
    this.isDismissed || (r.key ? (r.key === "Escape" && (this.isDismissed = !0), r.key === "Tab" && this.keepFocus(r)) : r.keyCode && (r.keyCode === 27 && (this.isDismissed = !0), r.keyCode === 9 && this.keepFocus(r)));
  }
  async onSubmitClick(r) {
    var t, e;
    r.preventDefault(), this.isDismissed = !1, await this.updateComplete, (e = (t = this.shadowRoot) == null ? void 0 : t.querySelector(".button-dismiss")) == null || e.focus();
  }
  renderDismissButton() {
    const r = this.localize("common.dismiss");
    return c`<button class="button icon-only button-dismiss" @click="${this.onCancelClick}" title="${r}" aria-label="${r}">&#10006;</button>`;
  }
  renderFooterContent() {
    if (this.type !== H.dialog)
      return c`
            <footer class="text-right" role="presentation">
                <button class="button primary" @click="${this.onConfirmClick}">${this.localize("modal.confirm")}</button>
                <button class="button secondary" @click="${this.onCancelClick}">${this.localize("modal.cancel")}</button>
            </footer>
        `;
  }
  renderModal() {
    return c`
            <div id="${this.key}-modal" ?aria-hidden="${this.isDismissed}" tabindex="-1">
                <div class="modal-overlay" tabindex="-1" @click="${this.onOverlayClick}" @keydown="${this.onKeyDown}">
                    <div class="modal-container card bd-grey" role="dialog" aria-modal="true" aria-labelledby="${this.key}-header"
                        aria-describedby="${this.key}-content"
                    >
                        <header class="row" role="presentation">
                            <span class="col-11" id="${this.key}-header">
                                <slot name="modal-header"></slot>
                            </span>
                            <span class="col-1 text-right">
                                ${this.renderDismissButton()}
                            </span>
                        </header>
                        <section id="${this.key}-content" class="mb-1">
                            <slot name="modal-content"></slot>
                        </section>
                        ${this.renderFooterContent()}
                    </div>
                </div>
            </div>
        `;
  }
  renderInnerContent() {
    return c`
            <slot name="button" @click="${this.onSubmitClick}"></slot>
            <slot><button type="submit" class="button primary no-focus" @click="${this.onSubmitClick}">Click</button></slot>
            ${this.isDismissed ? "" : this.renderModal()}
        `;
  }
  render() {
    if (this.type)
      return this.type === H.dialog || !this.href ? c`<div>${this.renderInnerContent()}</div>` : c`<form action="${this.href}">${this.renderInnerContent()}</form>`;
  }
  connectedCallback() {
    super.connectedCallback(), window.addEventListener("keydown", this.boundOnKeyDown);
  }
  disconnectedCallback() {
    window.removeEventListener("keydown", this.boundOnKeyDown), super.disconnectedCallback();
  }
};
C.styles = [
  B(Z),
  X`
            .modal-overlay {
                background: rgba(0,0,0,.6);
                top: 0;
                right: 0;
                bottom: 0;
                left: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                position: fixed;
                z-index: 16777270;
            }
            .modal-container { width: 40%; z-index: 16777271; }
            form { display: inline; }
        `
];
F([
  m()
], C.prototype, "key", 2);
F([
  m({ converter: (r) => r ? H[r] : void 0 })
], C.prototype, "type", 2);
F([
  m()
], C.prototype, "href", 2);
F([
  v()
], C.prototype, "isDismissed", 2);
C = F([
  T("lit-modal")
], C);
var w = /* @__PURE__ */ ((r) => (r.asc = "asc", r.desc = "desc", r))(w || {}), le = Object.defineProperty, ce = Object.getOwnPropertyDescriptor, rt = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ce(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && le(t, e, o), o;
};
let L = class extends b {
  constructor() {
    super(...arguments), this.property = "", this.noSort = !1, this.sortOrder = void 0;
  }
  toggleSort() {
    this.sortOrder === w.asc ? this.sortOrder = w.desc : this.sortOrder === w.desc ? this.sortOrder = void 0 : this.sortOrder = w.asc, this.dispatchEvent(new CustomEvent("litTableSorted", {
      detail: {
        property: this.property,
        sortOrder: this.sortOrder
      },
      bubbles: !0,
      composed: !0
    }));
  }
  renderSort() {
    if (this.sortOrder)
      return this.sortOrder === w.asc ? c`&#129053;` : c`&#129055;`;
  }
  render() {
    return this.noSort ? c`<span style="user-select: none;"><slot></slot></span>` : c`
            <span @click="${this.toggleSort}" style="user-select: none; cursor: pointer;" role="button">
                <slot></slot>
                ${this.renderSort()}
            </span>
        `;
  }
};
rt([
  m()
], L.prototype, "property", 2);
rt([
  m({ type: Boolean, attribute: "no-sort" })
], L.prototype, "noSort", 2);
rt([
  v()
], L.prototype, "sortOrder", 2);
L = rt([
  T("lit-table-header")
], L);
var x = /* @__PURE__ */ ((r) => (r.Page = "page", r.PerPage = "perPage", r.SearchQuery = "searchQuery", r.Sort = "sort", r))(x || {}), de = Object.defineProperty, he = Object.getOwnPropertyDescriptor, f = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? he(t, e) : t, s = r.length - 1, a; s >= 0; s--)
    (a = r[s]) && (o = (i ? a(t, e, o) : a(o)) || o);
  return i && o && de(t, e, o), o;
};
let u = class extends gt(b) {
  constructor() {
    super(...arguments), this.src = "", this.key = "", this.rowKey = "", this.addUrl = "", this.editUrl = "", this.deleteUrl = "", this.data = [], this.filteredRecordTotal = 0, this.filteredData = [], this.sortColumns = [], this.page = 0, this.perPage = 10, this.maxPage = 0, this.searchQuery = "", this.hasActions = !1, this.tableHeaders = /* @__PURE__ */ new Map(), this.actionName = "actions";
  }
  defaultCompare(r, t) {
    return r._index > t._index ? 1 : r._index < t._index ? -1 : 0;
  }
  compare(r, t) {
    let e = 0;
    const i = this.length;
    for (; e < i; e++) {
      const o = this[e], s = r[o.property], a = t[o.property];
      if (s === null)
        return 1;
      if (a === null)
        return -1;
      if (s < a)
        return o.sortOrder === w.asc ? -1 : 1;
      if (s > a)
        return o.sortOrder === w.asc ? 1 : -1;
    }
    return 0;
  }
  filterArray(r) {
    const t = (this || "").split(" ");
    for (const e in r)
      if (e.indexOf("_") < 0 && Object.prototype.hasOwnProperty.call(r, e)) {
        const i = (r[e] + "").toLowerCase();
        if (t.every((o) => i.indexOf(o) > -1))
          return !0;
      }
    return !1;
  }
  onSorted(r) {
    var i;
    if (!((i = r == null ? void 0 : r.detail) != null && i.property))
      return;
    const t = r.detail.property, e = r.detail.sortOrder;
    if (!e)
      this.sortColumns = this.sortColumns.filter((o) => o.property != t);
    else {
      const o = this.sortColumns.findIndex((s) => s.property === t) ?? -1;
      o > -1 ? this.sortColumns[o].sortOrder = e : this.sortColumns.push({ property: t, sortOrder: e });
    }
    this.saveSetting(x.Sort, JSON.stringify(this.sortColumns)), this.filterData();
  }
  replaceInUrl(r, t) {
    if (!r)
      return r;
    let e = r;
    return Object.keys(t).forEach((i) => {
      e = e.replace(`\${${i}}`, encodeURIComponent(t[i]));
    }), e;
  }
  fetchSetting(r) {
    return sessionStorage.getItem(`${this.key}_${r}`);
  }
  saveSetting(r, t) {
    sessionStorage.setItem(`${this.key}_${r}`, t.toString());
  }
  filterData() {
    var t, e;
    const r = this.searchQuery ? (t = this.data) == null ? void 0 : t.filter(this.filterArray.bind(this.searchQuery.toLowerCase())) : [...this.data];
    r.sort((e = this.sortColumns) != null && e.length ? this.compare.bind(this.sortColumns) : this.defaultCompare), this.filteredRecordTotal = r.length, this.maxPage = Math.max(Math.ceil(this.filteredRecordTotal / this.perPage) - 1, 0), this.filteredData = r.slice(this.perPage * this.page, this.perPage * this.page + this.perPage);
  }
  async fetchData() {
    this.src.length && (this.data = (await fetch(this.src, { headers: { "X-Requested-With": "XMLHttpRequest" } }).then((r) => r.json())).map((r, t) => (r._index = t, r)) ?? [], this.filterData());
  }
  async firstUpdated() {
    if (this.perPage = parseInt(this.fetchSetting(x.PerPage) ?? "10", 10), this.page = parseInt(this.fetchSetting(x.Page) ?? "0", 10), this.searchQuery = this.fetchSetting(x.SearchQuery) ?? "", this.sortColumns = JSON.parse(this.fetchSetting(x.Sort) ?? "[]"), this.shadowRoot) {
      const r = this.shadowRoot.querySelector("slot");
      if (r) {
        const t = r.assignedNodes();
        this.tableHeaders = new Map(
          t.filter((e) => e.nodeName === "LIT-TABLE-HEADER").map((e) => e).map((e) => [e.property, e])
        ), this.sortColumns.forEach((e) => {
          const i = this.tableHeaders.get(e.property);
          i && (i.sortOrder = e.sortOrder);
        });
      }
    }
    (this.addUrl || this.editUrl || this.deleteUrl) && (this.hasActions = !0), await this.fetchData();
  }
  onSearchQueryInput(r) {
    this.debounceTimer && clearTimeout(this.debounceTimer), this.debounceTimer = setTimeout(() => {
      this.searchQuery !== r && (this.page = 0, this.saveSetting(x.SearchQuery, r)), this.searchQuery = r, this.filterData();
    }, 250);
  }
  onPerPageInput(r) {
    const t = parseInt(r, 10) ?? 10;
    this.perPage !== t && (this.page = 0, this.saveSetting(x.PerPage, t)), this.perPage = t, this.filterData();
  }
  onFirstPageClick() {
    this.setPage(0);
  }
  onLastPageClick() {
    this.setPage(this.maxPage);
  }
  onPreviousPageClick() {
    this.setPage(Math.max(this.page - 1, 0));
  }
  onNextPageClick() {
    this.setPage(Math.min(this.page + 1, this.maxPage));
  }
  setPage(r) {
    this.page = r, this.saveSetting(x.Page, this.page), this.filterData();
  }
  renderActions(r) {
    const t = this.replaceInUrl(this.editUrl, r), e = this.replaceInUrl(this.deleteUrl, r);
    return c`
            ${t ? c`<a href="${t}" class="button primary button-action icon" title="${this.localize("table.edit")}">&#9998;</a>` : ""}
            ${e ? c`<lit-modal href="${e}" type="confirm" lang="${this.lang}">
                    <span slot="button">
                        <button class="button dark button-action icon" title="${this.localize("table.delete")}">&#10006;</button>
                    </span>
                    <span slot="modal-header"><h4>${this.localize("table.confirmDelete")}</h4></span>
                    <span slot="modal-content">${this.localize("table.areYouSure")}</span>
                </lit-modal>` : ""}
        `;
  }
  renderRows(r) {
    var t;
    return (t = this.filteredData) != null && t.length ? c`
            ${this.filteredData.map((e) => c`
                    <tr>
                        ${r.map((i) => i === this.actionName ? c`<td>${this.renderActions(e)}</td>` : c`<td><slot name="column-data-${i}">${e[i]}</slot></td>`)}
                    </tr>
                `)}
        ` : c`<tr><td colspan="${r.length}" class="text-center">${this.localize("table.noData")}</td></tr>`;
  }
  renderCount() {
    if (this.filteredRecordTotal)
      return c`${this.page * this.perPage + 1} ${this.localize("table.to")} ${Math.min(
        (this.page + 1) * this.perPage,
        this.filteredRecordTotal
      )} ${this.localize("table.of")} ${this.filteredRecordTotal}`;
  }
  renderInnerContent() {
    if (!this.data.length)
      return c`<slot name="loading"><h1 class="text-center"><div class="spinner"></div></h1></slot>`;
    const r = [...this.hasActions ? [this.actionName] : [], ...Array.from(this.tableHeaders.keys())];
    return c`
            <div class="container" id="${this.key}">
                <div class="row">
                    <div class="col col-10-md">
                        <input type="text" name="litTableSearchQuery" placeholder="${this.localize("table.search")}" class="col-6 col-4-md"
                            value="${this.searchQuery}" @input=${(t) => this.onSearchQueryInput(t.target.value)}
                        >
                    </div>
                    <div class="col col-2-md is-right is-vertical-align">
                        ${this.renderCount()}
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <table class="striped">
                            <thead @litTableSorted="${this.onSorted}">
                                <tr>
                                    ${r.map((t) => {
      if (t === this.actionName)
        return c`
                                                <th class="col-min-width">
                                                    ${this.addUrl ? c`<a href="${this.addUrl}" class="button secondary button-action icon"
                                                            title="${this.localize("table.add")}"
                                                        >
                                                            <span class="rotate45">&#10006;</span>
                                                        </a>` : ""}
                                                </th>
                                            `;
      const e = this.tableHeaders.get(t);
      return c`
                                            <th class="col-min-width">${e || t.replace(/\b([a-z])/g, (i, o) => o.toUpperCase())}</th>
                                        `;
    })}
                                </tr>
                            </thead>
                            <tbody>
                                ${this.renderRows(r)}
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-10-md">
                        <button class="button button-action primary flip-horizontal" title="${this.localize("table.first")}" @click="${this.onFirstPageClick}"
                            ?disabled="${this.page === 0}"
                        >
                            <span class="arrow">&#187;</span>
                        </button>
                        <button class="button button-action primary flip-horizontal" title="${this.localize("table.previous")}"
                            @click="${this.onPreviousPageClick}" ?disabled="${this.page === 0}"
                        >
                            <span class="arrow">&#8250;</span>
                        </button>
                        <button class="button button-action primary" title="${this.localize("table.next")}" @click="${this.onNextPageClick}"
                            ?disabled="${this.page === this.maxPage}"
                        >
                            <span class="arrow">&#8250;</span>
                        </button>
                        <button class="button button-action primary" title="${this.localize("table.last")}" @click="${this.onLastPageClick}"
                            ?disabled="${this.page === this.maxPage}"
                        >
                            <span class="arrow">&#187;</span>
                        </button>
                    </div>
                    <div class="col col-2-md">
                        <select name="litTablePerPage" @input=${(t) => this.onPerPageInput(t.target.value)}>
                            <option disabled>${this.localize("table.perPage")}</option>
                            <option value="10" ?selected="${this.perPage === 10}">10</option>
                            <option value="20" ?selected="${this.perPage === 20}">20</option>
                            <option value="50" ?selected="${this.perPage === 50}">50</option>
                            <option value="100" ?selected="${this.perPage === 100}">100</option>
                        </select>
                    </div>
                </div>
            </div>
        `;
  }
  render() {
    return c`<slot></slot>${this.renderInnerContent()}`;
  }
};
u.styles = [
  B(Z),
  X`
            .col-min-width { min-width:100px; }
            .button-action {
                padding: .7rem;
                font-size: 1.8rem;
                font-weight: bold;
                margin-right: .5rem;
                min-width: 34px;
                justify-content: center;
            }
            .button + .button { margin-left: inherit; }
            .flip-horizontal { transform: scaleX(-1); }
            .flip-horizontal:active { transform: scaleX(-.98); }
            .rotate45 { transform: rotate(-45deg); }
            .arrow { font-size: 2.5rem; line-height: .3; }
            .spinner { display: inline-block; width: 80px; height: 80px; }
            .spinner:after {
                content: " ";
                display: block;
                width: 64px;
                height: 64px;
                margin: 8px;
                border-radius: 50%;
                border: 6px solid var(--color-primary);
                border-color: var(--color-primary) transparent var(--color-primary) transparent;
                animation: spinner 1.2s linear infinite;
            }
            @keyframes spinner {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
        `
];
f([
  m()
], u.prototype, "src", 2);
f([
  m()
], u.prototype, "key", 2);
f([
  m({ attribute: "row-key" })
], u.prototype, "rowKey", 2);
f([
  m({ attribute: "add-url" })
], u.prototype, "addUrl", 2);
f([
  m({ attribute: "edit-url" })
], u.prototype, "editUrl", 2);
f([
  m({ attribute: "delete-url" })
], u.prototype, "deleteUrl", 2);
f([
  v()
], u.prototype, "data", 2);
f([
  v()
], u.prototype, "filteredRecordTotal", 2);
f([
  v()
], u.prototype, "filteredData", 2);
f([
  v()
], u.prototype, "sortColumns", 2);
f([
  v()
], u.prototype, "page", 2);
f([
  v()
], u.prototype, "perPage", 2);
f([
  v()
], u.prototype, "maxPage", 2);
f([
  v()
], u.prototype, "searchQuery", 2);
f([
  v()
], u.prototype, "hasActions", 2);
f([
  v()
], u.prototype, "tableHeaders", 2);
u = f([
  T("lit-table")
], u);
