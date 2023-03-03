(function() {
  const t = document.createElement("link").relList;
  if (t && t.supports && t.supports("modulepreload"))
    return;
  for (const o of document.querySelectorAll('link[rel="modulepreload"]'))
    i(o);
  new MutationObserver((o) => {
    for (const s of o)
      if (s.type === "childList")
        for (const n of s.addedNodes)
          n.tagName === "LINK" && n.rel === "modulepreload" && i(n);
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
const Y = window, mt = Y.ShadowRoot && (Y.ShadyCSS === void 0 || Y.ShadyCSS.nativeShadow) && "adoptedStyleSheets" in Document.prototype && "replace" in CSSStyleSheet.prototype, ft = Symbol(), _t = /* @__PURE__ */ new WeakMap();
let Rt = class {
  constructor(t, e, i) {
    if (this._$cssResult$ = !0, i !== ft)
      throw Error("CSSResult is not constructable. Use `unsafeCSS` or `css` instead.");
    this.cssText = t, this.t = e;
  }
  get styleSheet() {
    let t = this.o;
    const e = this.t;
    if (mt && t === void 0) {
      const i = e !== void 0 && e.length === 1;
      i && (t = _t.get(e)), t === void 0 && ((this.o = t = new CSSStyleSheet()).replaceSync(this.cssText), i && _t.set(e, t));
    }
    return t;
  }
  toString() {
    return this.cssText;
  }
};
const q = (r) => new Rt(typeof r == "string" ? r : r + "", void 0, ft), et = (r, ...t) => {
  const e = r.length === 1 ? r[0] : t.reduce((i, o, s) => i + ((n) => {
    if (n._$cssResult$ === !0)
      return n.cssText;
    if (typeof n == "number")
      return n;
    throw Error("Value passed to 'css' function must be a 'css' function result: " + n + ". Use 'unsafeCSS' to pass non-literal values, but take care to ensure page security.");
  })(o) + r[s + 1], r[0]);
  return new Rt(e, r, ft);
}, qt = (r, t) => {
  mt ? r.adoptedStyleSheets = t.map((e) => e instanceof CSSStyleSheet ? e : e.styleSheet) : t.forEach((e) => {
    const i = document.createElement("style"), o = Y.litNonce;
    o !== void 0 && i.setAttribute("nonce", o), i.textContent = e.cssText, r.appendChild(i);
  });
}, wt = mt ? (r) => r : (r) => r instanceof CSSStyleSheet ? ((t) => {
  let e = "";
  for (const i of t.cssRules)
    e += i.cssText;
  return q(e);
})(r) : r;
var at;
const Z = window, At = Z.trustedTypes, Wt = At ? At.emptyScript : "", Ct = Z.reactiveElementPolyfillSupport, gt = { toAttribute(r, t) {
  switch (t) {
    case Boolean:
      r = r ? Wt : null;
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
} }, Ht = (r, t) => t !== r && (t == t || r == r), lt = { attribute: !0, type: String, converter: gt, reflect: !1, hasChanged: Ht };
let z = class extends HTMLElement {
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
  static createProperty(t, e = lt) {
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
    return this.elementProperties.get(t) || lt;
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
        e.unshift(wt(o));
    } else
      t !== void 0 && e.push(wt(t));
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
    return qt(e, this.constructor.elementStyles), e;
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
  _$EO(t, e, i = lt) {
    var o;
    const s = this.constructor._$Ep(t, i);
    if (s !== void 0 && i.reflect === !0) {
      const n = (((o = i.converter) === null || o === void 0 ? void 0 : o.toAttribute) !== void 0 ? i.converter : gt).toAttribute(e, i.type);
      this._$El = t, n == null ? this.removeAttribute(s) : this.setAttribute(s, n), this._$El = null;
    }
  }
  _$AK(t, e) {
    var i;
    const o = this.constructor, s = o._$Ev.get(t);
    if (s !== void 0 && this._$El !== s) {
      const n = o.getPropertyOptions(s), d = typeof n.converter == "function" ? { fromAttribute: n.converter } : ((i = n.converter) === null || i === void 0 ? void 0 : i.fromAttribute) !== void 0 ? n.converter : gt;
      this._$El = s, this[s] = d.fromAttribute(e, n.type), this._$El = null;
    }
  }
  requestUpdate(t, e, i) {
    let o = !0;
    t !== void 0 && (((i = i || this.constructor.getPropertyOptions(t)).hasChanged || Ht)(this[t], e) ? (this._$AL.has(t) || this._$AL.set(t, e), i.reflect === !0 && this._$El !== t && (this._$EC === void 0 && (this._$EC = /* @__PURE__ */ new Map()), this._$EC.set(t, i))) : o = !1), !this.isUpdatePending && o && (this._$E_ = this._$Ej());
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
z.finalized = !0, z.elementProperties = /* @__PURE__ */ new Map(), z.elementStyles = [], z.shadowRootOptions = { mode: "open" }, Ct == null || Ct({ ReactiveElement: z }), ((at = Z.reactiveElementVersions) !== null && at !== void 0 ? at : Z.reactiveElementVersions = []).push("1.5.0");
var ct;
const X = window, U = X.trustedTypes, Pt = U ? U.createPolicy("lit-html", { createHTML: (r) => r }) : void 0, C = `lit$${(Math.random() + "").slice(9)}$`, vt = "?" + C, Jt = `<${vt}>`, N = document, F = (r = "") => N.createComment(r), K = (r) => r === null || typeof r != "object" && typeof r != "function", Mt = Array.isArray, It = (r) => Mt(r) || typeof (r == null ? void 0 : r[Symbol.iterator]) == "function", j = /<(?:(!--|\/[^a-zA-Z])|(\/?[a-zA-Z][^>\s]*)|(\/?$))/g, St = /-->/g, kt = />/g, S = RegExp(`>|[ 	
\f\r](?:([^\\s"'>=/]+)([ 	
\f\r]*=[ 	
\f\r]*(?:[^ 	
\f\r"'\`<>=]|("|')|))|$)`, "g"), Et = /'/g, Ot = /"/g, jt = /^(?:script|style|textarea|title)$/i, Yt = (r) => (t, ...e) => ({ _$litType$: r, strings: t, values: e }), h = Yt(1), E = Symbol.for("lit-noChange"), v = Symbol.for("lit-nothing"), Dt = /* @__PURE__ */ new WeakMap(), T = N.createTreeWalker(N, 129, null, !1), Lt = (r, t) => {
  const e = r.length - 1, i = [];
  let o, s = t === 2 ? "<svg>" : "", n = j;
  for (let l = 0; l < e; l++) {
    const a = r[l];
    let $, p, c = -1, u = 0;
    for (; u < a.length && (n.lastIndex = u, p = n.exec(a), p !== null); )
      u = n.lastIndex, n === j ? p[1] === "!--" ? n = St : p[1] !== void 0 ? n = kt : p[2] !== void 0 ? (jt.test(p[2]) && (o = RegExp("</" + p[2], "g")), n = S) : p[3] !== void 0 && (n = S) : n === S ? p[0] === ">" ? (n = o ?? j, c = -1) : p[1] === void 0 ? c = -2 : (c = n.lastIndex - p[2].length, $ = p[1], n = p[3] === void 0 ? S : p[3] === '"' ? Ot : Et) : n === Ot || n === Et ? n = S : n === St || n === kt ? n = j : (n = S, o = void 0);
    const g = n === S && r[l + 1].startsWith("/>") ? " " : "";
    s += n === j ? a + Jt : c >= 0 ? (i.push($), a.slice(0, c) + "$lit$" + a.slice(c) + C + g) : a + C + (c === -2 ? (i.push(void 0), l) : g);
  }
  const d = s + (r[e] || "<?>") + (t === 2 ? "</svg>" : "");
  if (!Array.isArray(r) || !r.hasOwnProperty("raw"))
    throw Error("invalid template strings array");
  return [Pt !== void 0 ? Pt.createHTML(d) : d, i];
};
class Q {
  constructor({ strings: t, _$litType$: e }, i) {
    let o;
    this.parts = [];
    let s = 0, n = 0;
    const d = t.length - 1, l = this.parts, [a, $] = Lt(t, e);
    if (this.el = Q.createElement(a, i), T.currentNode = this.el.content, e === 2) {
      const p = this.el.content, c = p.firstChild;
      c.remove(), p.append(...c.childNodes);
    }
    for (; (o = T.nextNode()) !== null && l.length < d; ) {
      if (o.nodeType === 1) {
        if (o.hasAttributes()) {
          const p = [];
          for (const c of o.getAttributeNames())
            if (c.endsWith("$lit$") || c.startsWith(C)) {
              const u = $[n++];
              if (p.push(c), u !== void 0) {
                const g = o.getAttribute(u.toLowerCase() + "$lit$").split(C), f = /([.?@])?(.*)/.exec(u);
                l.push({ type: 1, index: s, name: f[2], strings: g, ctor: f[1] === "." ? Gt : f[1] === "?" ? Ft : f[1] === "@" ? Kt : W });
              } else
                l.push({ type: 6, index: s });
            }
          for (const c of p)
            o.removeAttribute(c);
        }
        if (jt.test(o.tagName)) {
          const p = o.textContent.split(C), c = p.length - 1;
          if (c > 0) {
            o.textContent = U ? U.emptyScript : "";
            for (let u = 0; u < c; u++)
              o.append(p[u], F()), T.nextNode(), l.push({ type: 2, index: ++s });
            o.append(p[c], F());
          }
        }
      } else if (o.nodeType === 8)
        if (o.data === vt)
          l.push({ type: 2, index: s });
        else {
          let p = -1;
          for (; (p = o.data.indexOf(C, p + 1)) !== -1; )
            l.push({ type: 7, index: s }), p += C.length - 1;
        }
      s++;
    }
  }
  static createElement(t, e) {
    const i = N.createElement("template");
    return i.innerHTML = t, i;
  }
}
function O(r, t, e = r, i) {
  var o, s, n, d;
  if (t === E)
    return t;
  let l = i !== void 0 ? (o = e._$Co) === null || o === void 0 ? void 0 : o[i] : e._$Cl;
  const a = K(t) ? void 0 : t._$litDirective$;
  return (l == null ? void 0 : l.constructor) !== a && ((s = l == null ? void 0 : l._$AO) === null || s === void 0 || s.call(l, !1), a === void 0 ? l = void 0 : (l = new a(r), l._$AT(r, e, i)), i !== void 0 ? ((n = (d = e)._$Co) !== null && n !== void 0 ? n : d._$Co = [])[i] = l : e._$Cl = l), l !== void 0 && (t = O(r, l._$AS(r, t.values), l, i)), t;
}
class Bt {
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
    const { el: { content: i }, parts: o } = this._$AD, s = ((e = t == null ? void 0 : t.creationScope) !== null && e !== void 0 ? e : N).importNode(i, !0);
    T.currentNode = s;
    let n = T.nextNode(), d = 0, l = 0, a = o[0];
    for (; a !== void 0; ) {
      if (d === a.index) {
        let $;
        a.type === 2 ? $ = new M(n, n.nextSibling, this, t) : a.type === 1 ? $ = new a.ctor(n, a.name, a.strings, this, t) : a.type === 6 && ($ = new Qt(n, this, t)), this.u.push($), a = o[++l];
      }
      d !== (a == null ? void 0 : a.index) && (n = T.nextNode(), d++);
    }
    return s;
  }
  p(t) {
    let e = 0;
    for (const i of this.u)
      i !== void 0 && (i.strings !== void 0 ? (i._$AI(t, i, e), e += i.strings.length - 2) : i._$AI(t[e])), e++;
  }
}
class M {
  constructor(t, e, i, o) {
    var s;
    this.type = 2, this._$AH = v, this._$AN = void 0, this._$AA = t, this._$AB = e, this._$AM = i, this.options = o, this._$Cm = (s = o == null ? void 0 : o.isConnected) === null || s === void 0 || s;
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
    t = O(this, t, e), K(t) ? t === v || t == null || t === "" ? (this._$AH !== v && this._$AR(), this._$AH = v) : t !== this._$AH && t !== E && this.g(t) : t._$litType$ !== void 0 ? this.$(t) : t.nodeType !== void 0 ? this.T(t) : It(t) ? this.k(t) : this.g(t);
  }
  O(t, e = this._$AB) {
    return this._$AA.parentNode.insertBefore(t, e);
  }
  T(t) {
    this._$AH !== t && (this._$AR(), this._$AH = this.O(t));
  }
  g(t) {
    this._$AH !== v && K(this._$AH) ? this._$AA.nextSibling.data = t : this.T(N.createTextNode(t)), this._$AH = t;
  }
  $(t) {
    var e;
    const { values: i, _$litType$: o } = t, s = typeof o == "number" ? this._$AC(t) : (o.el === void 0 && (o.el = Q.createElement(o.h, this.options)), o);
    if (((e = this._$AH) === null || e === void 0 ? void 0 : e._$AD) === s)
      this._$AH.p(i);
    else {
      const n = new Bt(s, this), d = n.v(this.options);
      n.p(i), this.T(d), this._$AH = n;
    }
  }
  _$AC(t) {
    let e = Dt.get(t.strings);
    return e === void 0 && Dt.set(t.strings, e = new Q(t)), e;
  }
  k(t) {
    Mt(this._$AH) || (this._$AH = [], this._$AR());
    const e = this._$AH;
    let i, o = 0;
    for (const s of t)
      o === e.length ? e.push(i = new M(this.O(F()), this.O(F()), this, this.options)) : i = e[o], i._$AI(s), o++;
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
class W {
  constructor(t, e, i, o, s) {
    this.type = 1, this._$AH = v, this._$AN = void 0, this.element = t, this.name = e, this._$AM = o, this.options = s, i.length > 2 || i[0] !== "" || i[1] !== "" ? (this._$AH = Array(i.length - 1).fill(new String()), this.strings = i) : this._$AH = v;
  }
  get tagName() {
    return this.element.tagName;
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  _$AI(t, e = this, i, o) {
    const s = this.strings;
    let n = !1;
    if (s === void 0)
      t = O(this, t, e, 0), n = !K(t) || t !== this._$AH && t !== E, n && (this._$AH = t);
    else {
      const d = t;
      let l, a;
      for (t = s[0], l = 0; l < s.length - 1; l++)
        a = O(this, d[i + l], e, l), a === E && (a = this._$AH[l]), n || (n = !K(a) || a !== this._$AH[l]), a === v ? t = v : t !== v && (t += (a ?? "") + s[l + 1]), this._$AH[l] = a;
    }
    n && !o && this.j(t);
  }
  j(t) {
    t === v ? this.element.removeAttribute(this.name) : this.element.setAttribute(this.name, t ?? "");
  }
}
class Gt extends W {
  constructor() {
    super(...arguments), this.type = 3;
  }
  j(t) {
    this.element[this.name] = t === v ? void 0 : t;
  }
}
const Zt = U ? U.emptyScript : "";
class Ft extends W {
  constructor() {
    super(...arguments), this.type = 4;
  }
  j(t) {
    t && t !== v ? this.element.setAttribute(this.name, Zt) : this.element.removeAttribute(this.name);
  }
}
class Kt extends W {
  constructor(t, e, i, o, s) {
    super(t, e, i, o, s), this.type = 5;
  }
  _$AI(t, e = this) {
    var i;
    if ((t = (i = O(this, t, e, 0)) !== null && i !== void 0 ? i : v) === E)
      return;
    const o = this._$AH, s = t === v && o !== v || t.capture !== o.capture || t.once !== o.once || t.passive !== o.passive, n = t !== v && (o === v || s);
    s && this.element.removeEventListener(this.name, this, o), n && this.element.addEventListener(this.name, this, t), this._$AH = t;
  }
  handleEvent(t) {
    var e, i;
    typeof this._$AH == "function" ? this._$AH.call((i = (e = this.options) === null || e === void 0 ? void 0 : e.host) !== null && i !== void 0 ? i : this.element, t) : this._$AH.handleEvent(t);
  }
}
class Qt {
  constructor(t, e, i) {
    this.element = t, this.type = 6, this._$AN = void 0, this._$AM = e, this.options = i;
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  _$AI(t) {
    O(this, t);
  }
}
const Xt = { P: "$lit$", A: C, M: vt, C: 1, L: Lt, R: Bt, D: It, V: O, I: M, H: W, N: Ft, U: Kt, B: Gt, F: Qt }, zt = X.litHtmlPolyfillSupport;
zt == null || zt(Q, M), ((ct = X.litHtmlVersions) !== null && ct !== void 0 ? ct : X.litHtmlVersions = []).push("2.5.0");
const te = (r, t, e) => {
  var i, o;
  const s = (i = e == null ? void 0 : e.renderBefore) !== null && i !== void 0 ? i : t;
  let n = s._$litPart$;
  if (n === void 0) {
    const d = (o = e == null ? void 0 : e.renderBefore) !== null && o !== void 0 ? o : null;
    s._$litPart$ = n = new M(t.insertBefore(F(), d), d, void 0, e ?? {});
  }
  return n._$AI(r), n;
};
var dt, ht;
let _ = class extends z {
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
    this.hasUpdated || (this.renderOptions.isConnected = this.isConnected), super.update(t), this._$Do = te(e, this.renderRoot, this.renderOptions);
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
    return E;
  }
};
_.finalized = !0, _._$litElement$ = !0, (dt = globalThis.litElementHydrateSupport) === null || dt === void 0 || dt.call(globalThis, { LitElement: _ });
const Tt = globalThis.litElementPolyfillSupport;
Tt == null || Tt({ LitElement: _ });
((ht = globalThis.litElementVersions) !== null && ht !== void 0 ? ht : globalThis.litElementVersions = []).push("3.2.2");
const I = (r) => (t) => typeof t == "function" ? ((e, i) => (customElements.define(e, i), i))(r, t) : ((e, i) => {
  const { kind: o, elements: s } = i;
  return { kind: o, elements: s, finisher(n) {
    customElements.define(e, n);
  } };
})(r, t);
const ee = (r, t) => t.kind === "method" && t.descriptor && !("value" in t.descriptor) ? { ...t, finisher(e) {
  e.createProperty(t.key, r);
} } : { kind: "field", key: Symbol(), placement: "own", descriptor: {}, originalKey: t.key, initializer() {
  typeof t.initializer == "function" && (this[t.key] = t.initializer.call(this));
}, finisher(e) {
  e.createProperty(t.key, r);
} };
function y(r) {
  return (t, e) => e !== void 0 ? ((i, o, s) => {
    o.constructor.createProperty(s, i);
  })(r, t, e) : ee(r, t);
}
function x(r) {
  return y({ ...r, state: !0 });
}
var pt;
((pt = window.HTMLSlotElement) === null || pt === void 0 ? void 0 : pt.prototype.assignedElements) != null;
var re = Object.defineProperty, ie = Object.getOwnPropertyDescriptor, yt = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ie(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && re(t, e, o), o;
};
let tt = class extends _ {
  constructor() {
    super(...arguments), this.key = "", this.isActive = !1;
  }
  render() {
    return h`<slot></slot>`;
  }
};
yt([
  y()
], tt.prototype, "key", 2);
yt([
  y({ type: Boolean, attribute: "is-active" })
], tt.prototype, "isActive", 2);
tt = yt([
  I("lit-tab")
], tt);
const rt = `:root{--bg-color: #ffffff;--bg-secondary-color: #f3f3f6;--color-primary: #14854F;--color-lightGrey: #d2d6dd;--color-grey: #747681;--color-darkGrey: #3f4144;--color-error: #d43939;--color-success: #28bd14;--grid-maxWidth: 120rem;--grid-gutter: 2rem;--font-size: 1.6rem;--font-color: #333333;--font-family-sans: -apple-system, BlinkMacSystemFont, Avenir, "Avenir Next", "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif;--font-family-mono: monaco, "Consolas", "Lucida Console", monospace}html{box-sizing:border-box;font-size:62.5%;line-height:1.15;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}*,*:before,*:after{box-sizing:inherit}body{background-color:var(--bg-color);line-height:1.6;font-size:var(--font-size);color:var(--font-color);font-family:Segoe UI,Helvetica Neue,sans-serif;font-family:var(--font-family-sans);margin:0;padding:0}h1,h2,h3,h4,h5,h6{font-weight:500;margin:.35em 0 .7em}h1{font-size:2em}h2{font-size:1.75em}h3{font-size:1.5em}h4{font-size:1.25em}h5{font-size:1em}h6{font-size:.85em}a{color:var(--color-primary);text-decoration:none}a:hover:not(.button){opacity:.75}button{font-family:inherit}p{margin-top:0}blockquote{background-color:var(--bg-secondary-color);padding:1.5rem 2rem;border-left:3px solid var(--color-lightGrey)}dl dt{font-weight:700}hr{border:none;background-color:var(--color-lightGrey);height:1px;margin:1rem 0}table{width:100%;border:none;border-collapse:collapse;border-spacing:0;text-align:left}table.striped tr:nth-of-type(2n){background-color:var(--bg-secondary-color)}td,th{vertical-align:middle;padding:1.2rem .4rem}thead{border-bottom:2px solid var(--color-lightGrey)}tfoot{border-top:2px solid var(--color-lightGrey)}code,kbd,pre,samp,tt{font-family:var(--font-family-mono)}code,kbd{padding:0 .4rem;font-size:90%;white-space:pre-wrap;border-radius:4px;padding:.2em .4em;background-color:var(--bg-secondary-color);color:var(--color-error)}pre{background-color:var(--bg-secondary-color);font-size:1em;padding:1rem;overflow-x:auto}pre code{background:none;padding:0}abbr[title]{border-bottom:none;text-decoration:underline;text-decoration:underline dotted}img{max-width:100%}fieldset{border:1px solid var(--color-lightGrey)}iframe{border:0}.container{max-width:var(--grid-maxWidth);margin:0 auto;width:96%;padding:0 calc(var(--grid-gutter) / 2)}.row{display:flex;flex-flow:row wrap;justify-content:flex-start;margin-left:calc(var(--grid-gutter) / -2);margin-right:calc(var(--grid-gutter) / -2)}.row.reverse{flex-direction:row-reverse}.col{flex:1}.col,[class*=" col-"],[class^=col-]{margin:0 calc(var(--grid-gutter) / 2) calc(var(--grid-gutter) / 2)}.col-1{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}@media screen and (max-width: 599px){.container{width:100%}.col,[class*=col-],[class^=col-]{flex:0 1 100%;max-width:100%}}@media screen and (min-width: 900px){.col-1-md{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2-md{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3-md{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4-md{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5-md{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6-md{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7-md{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8-md{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9-md{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10-md{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11-md{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12-md{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}}@media screen and (min-width: 1200px){.col-1-lg{flex:0 0 calc((100% / (12/1)) - var(--grid-gutter));max-width:calc((100% / (12/1)) - var(--grid-gutter))}.col-2-lg{flex:0 0 calc((100% / (12/2)) - var(--grid-gutter));max-width:calc((100% / (12/2)) - var(--grid-gutter))}.col-3-lg{flex:0 0 calc((100% / (12/3)) - var(--grid-gutter));max-width:calc((100% / (12/3)) - var(--grid-gutter))}.col-4-lg{flex:0 0 calc((100% / (12/4)) - var(--grid-gutter));max-width:calc((100% / (12/4)) - var(--grid-gutter))}.col-5-lg{flex:0 0 calc((100% / (12/5)) - var(--grid-gutter));max-width:calc((100% / (12/5)) - var(--grid-gutter))}.col-6-lg{flex:0 0 calc((100% / (12/6)) - var(--grid-gutter));max-width:calc((100% / (12/6)) - var(--grid-gutter))}.col-7-lg{flex:0 0 calc((100% / (12/7)) - var(--grid-gutter));max-width:calc((100% / (12/7)) - var(--grid-gutter))}.col-8-lg{flex:0 0 calc((100% / (12/8)) - var(--grid-gutter));max-width:calc((100% / (12/8)) - var(--grid-gutter))}.col-9-lg{flex:0 0 calc((100% / (12/9)) - var(--grid-gutter));max-width:calc((100% / (12/9)) - var(--grid-gutter))}.col-10-lg{flex:0 0 calc((100% / (12/10)) - var(--grid-gutter));max-width:calc((100% / (12/10)) - var(--grid-gutter))}.col-11-lg{flex:0 0 calc((100% / (12/11)) - var(--grid-gutter));max-width:calc((100% / (12/11)) - var(--grid-gutter))}.col-12-lg{flex:0 0 calc((100% / (12/12)) - var(--grid-gutter));max-width:calc((100% / (12/12)) - var(--grid-gutter))}}fieldset{padding:.5rem 2rem}legend{text-transform:uppercase;font-size:.8em;letter-spacing:.1rem}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),select,textarea,textarea[type=text]{font-family:inherit;padding:.8rem 1rem;border-radius:4px;border:1px solid var(--color-lightGrey);font-size:1em;transition:all .2s ease;display:block;width:100%}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]):not(:disabled):hover,select:hover,textarea:hover,textarea[type=text]:hover{border-color:var(--color-grey)}input:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]):focus,select:focus,textarea:focus,textarea[type=text]:focus{outline:none;border-color:var(--color-primary);box-shadow:0 0 1px var(--color-primary)}input.error:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),textarea.error{border-color:var(--color-error)}input.success:not([type="checkbox"]):not([type="radio"]):not([type="submit"]):not([type="color"]):not([type="button"]):not([type="reset"]),textarea.success{border-color:var(--color-success)}select{-webkit-appearance:none;background:#f3f3f6 no-repeat 100%;background-size:1ex;background-origin:content-box;background-image:url("data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='60' height='40' fill='%23555'><polygon points='0,0 60,0 30,40'/></svg>")}[type=checkbox],[type=radio]{width:1.6rem;height:1.6rem}.button,[type=button],[type=reset],[type=submit],button{padding:1rem 2.5rem;color:var(--color-darkGrey);background:var(--color-lightGrey);border-radius:4px;border:1px solid transparent;font-size:var(--font-size);line-height:1;text-align:center;transition:opacity .2s ease;text-decoration:none;transform:scale(1);display:inline-block;cursor:pointer}.grouped{display:flex}.grouped>*:not(:last-child){margin-right:16px}.grouped.gapless>*{margin:0 0 0 -1px!important;border-radius:0!important}.grouped.gapless>*:first-child{margin:0!important;border-radius:4px 0 0 4px!important}.grouped.gapless>*:last-child{border-radius:0 4px 4px 0!important}.button+.button{margin-left:1rem}.button:hover,[type=button]:hover,[type=reset]:hover,[type=submit]:hover,button:hover{opacity:.8}.button:active,[type=button]:active,[type=reset]:active,[type=submit]:active,button:active{transform:scale(.98)}input:disabled,button:disabled,input:disabled:hover,button:disabled:hover{opacity:.4;cursor:not-allowed}.button.primary,.button.secondary,.button.dark,.button.error,.button.success,[type=submit]{color:#fff;z-index:1;background-color:#000;background-color:var(--color-primary)}.button.secondary{background-color:var(--color-grey)}.button.dark{background-color:var(--color-darkGrey)}.button.error{background-color:var(--color-error)}.button.success{background-color:var(--color-success)}.button.outline{background-color:transparent;border-color:var(--color-lightGrey)}.button.outline.primary{border-color:var(--color-primary);color:var(--color-primary)}.button.outline.secondary{border-color:var(--color-grey);color:var(--color-grey)}.button.outline.dark{border-color:var(--color-darkGrey);color:var(--color-darkGrey)}.button.clear{background-color:transparent;border-color:transparent;color:var(--color-primary)}.button.icon{display:inline-flex;align-items:center}.button.icon>img{margin-left:2px}.button.icon-only{padding:1rem}::placeholder{color:#bdbfc4}.nav{display:flex;min-height:5rem;align-items:stretch}.nav img{max-height:3rem}.nav>.container{display:flex}.nav-center,.nav-left,.nav-right{display:flex;flex:1}.nav-left{justify-content:flex-start}.nav-right{justify-content:flex-end}.nav-center{justify-content:center}@media screen and (max-width: 480px){.nav,.nav>.container{flex-direction:column}.nav-center,.nav-left,.nav-right{flex-wrap:wrap;justify-content:center}}.nav a,.nav .brand{text-decoration:none;display:flex;align-items:center;padding:1rem 2rem;color:var(--color-darkGrey)}.nav .active:not(.button){color:#000;color:var(--color-primary)}.nav .brand{font-size:1.75em;padding-top:0;padding-bottom:0}.nav .brand img{padding-right:1rem}.nav .button{margin:auto 1rem}.card{padding:1rem 2rem;border-radius:4px;background:var(--bg-color);box-shadow:0 1px 3px var(--color-grey)}.card p:last-child{margin:0}.card header>*{margin-top:0;margin-bottom:1rem}.tabs{display:flex}.tabs a{text-decoration:none}.tabs>.dropdown>summary,.tabs>a{padding:1rem 2rem;flex:0 1 auto;color:var(--color-darkGrey);border-bottom:2px solid var(--color-lightGrey);text-align:center}.tabs>a.active,.tabs>a:hover{opacity:1;border-bottom:2px solid var(--color-darkGrey)}.tabs>a.active{border-color:var(--color-primary)}.tabs.is-full a{flex:1 1 auto}.tag{display:inline-block;border:1px solid var(--color-lightGrey);text-transform:uppercase;color:var(--color-grey);padding:.5rem;line-height:1;letter-spacing:.5px}.tag.is-small{padding:.4rem;font-size:.75em}.tag.is-large{padding:.7rem;font-size:1.125em}.tag+.tag{margin-left:1rem}details.dropdown{position:relative;display:inline-block}details.dropdown>:last-child{position:absolute;left:0;white-space:nowrap}.bg-primary{background-color:var(--color-primary)!important}.bg-light{background-color:var(--color-lightGrey)!important}.bg-dark{background-color:var(--color-darkGrey)!important}.bg-grey{background-color:var(--color-grey)!important}.bg-error{background-color:var(--color-error)!important}.bg-success{background-color:var(--color-success)!important}.bd-primary{border:1px solid var(--color-primary)!important}.bd-light{border:1px solid var(--color-lightGrey)!important}.bd-dark{border:1px solid var(--color-darkGrey)!important}.bd-grey{border:1px solid var(--color-grey)!important}.bd-error{border:1px solid var(--color-error)!important}.bd-success{border:1px solid var(--color-success)!important}.text-primary{color:var(--color-primary)!important}.text-light{color:var(--color-lightGrey)!important}.text-dark{color:var(--color-darkGrey)!important}.text-grey{color:var(--color-grey)!important}.text-error{color:var(--color-error)!important}.text-success{color:var(--color-success)!important}.text-white{color:#fff!important}.pull-right{float:right!important}.pull-left{float:left!important}.text-center{text-align:center}.text-left{text-align:left}.text-right{text-align:right}.text-justify{text-align:justify}.text-uppercase{text-transform:uppercase}.text-lowercase{text-transform:lowercase}.text-capitalize{text-transform:capitalize}.is-full-screen{width:100%;min-height:100vh}.is-full-width{width:100%!important}.is-vertical-align{display:flex;align-items:center}.is-horizontal-align{display:flex;justify-content:center}.is-center{display:flex;align-items:center;justify-content:center}.is-right{display:flex;align-items:center;justify-content:flex-end}.is-left{display:flex;align-items:center;justify-content:flex-start}.is-fixed{position:fixed;width:100%}.is-paddingless{padding:0!important}.is-marginless{margin:0!important}.is-pointer{cursor:pointer!important}.is-rounded{border-radius:100%}.clearfix{content:"";display:table;clear:both}.is-hidden{display:none!important}@media screen and (max-width: 599px){.hide-xs{display:none!important}}@media screen and (min-width: 600px) and (max-width: 899px){.hide-sm{display:none!important}}@media screen and (min-width: 900px) and (max-width: 1199px){.hide-md{display:none!important}}@media screen and (min-width: 1200px){.hide-lg{display:none!important}}@media print{.hide-pr{display:none!important}}:host,:root{--bg-color: #ffffff;--bg-secondary-color: #f3f3f6;--color-primary: #5783db;--color-lightGrey: #d2d6dd;--color-grey: #3e9a80;--color-darkGrey: #80669d;--color-error: #d43939;--color-success: #33b249;--grid-maxWidth: 150rem;--grid-gutter: 1rem;--font-size: 1.6rem;--font-color: #333333;--font-family-sans: -apple-system, BlinkMacSystemFont, Avenir, "Avenir Next", "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif;--font-family-mono: monaco, "Consolas", "Lucida Console", monospace}:not(:defined){display:none}
`;
var oe = Object.defineProperty, se = Object.getOwnPropertyDescriptor, it = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? se(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && oe(t, e, o), o;
};
let R = class extends _ {
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
    return h`
            <nav class="tabs ${this.isFull ? "is-full" : ""}">
                ${r.map((t) => h`
                    <a class="cursor-pointer ${this.activeTab === t ? "active" : ""}" @click="${() => this.activeTab = t}"
                        id="nav-${t}" aria-controls="${t}" role="tab"
                    >
                        ${this.tabs.get(t)}
                    </a>
                `)}
            </nav>
            ${r.map((t) => h`
                <div class="tab-content ${this.activeTab === t ? "" : "is-hidden"}" id="${t}" ?aria-current="${this.activeTab === t}"
                    aria-labelledby="nav-${t}" role="tabpanel"
                >
                    <slot name="${t}"></slot>
                </div>
            `)}
        `;
  }
  render() {
    return h`<slot></slot>${this.renderInnerContent()}`;
  }
};
R.styles = [
  q(rt),
  et`
            .tab-content { padding: 2rem 0; }
            .cursor-pointer { cursor: pointer; }
        `
];
it([
  y({ type: Boolean, attribute: "is-full" })
], R.prototype, "isFull", 2);
it([
  x()
], R.prototype, "activeTab", 2);
it([
  x()
], R.prototype, "tabs", 2);
R = it([
  I("lit-nav")
], R);
var bt = /* @__PURE__ */ ((r) => (r.success = "success", r.error = "error", r))(bt || {}), B = /* @__PURE__ */ ((r) => (r.en = "en", r.es = "es", r))(B || {});
const ne = {
  alert: {
    dismiss: "Dismiss"
  },
  modal: {
    dismiss: "Dismiss",
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
}, ae = {
  alert: {
    dismiss: "Despedir"
  },
  modal: {
    dismiss: "Despedir",
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
}, le = {
  en: ne,
  es: ae
};
var ce = Object.defineProperty, de = Object.getOwnPropertyDescriptor, he = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? de(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && ce(t, e, o), o;
};
function Vt(r, t = "") {
  return Object.keys(r).reduce((e, i) => {
    const o = t.length ? t + "." : "", s = r[i];
    return Array.isArray(s) || Object(s) === s ? Object.assign(e, Vt(s, o + i)) : e[o + i] = s, e;
  }, {});
}
const $t = (r) => {
  class t extends r {
    constructor() {
      super(...arguments), this.lang = B.en;
    }
    localize(i) {
      this.i18n || (this.i18n = Vt(le));
      const o = `${this.lang}.${i}`;
      return Object.prototype.hasOwnProperty.call(this.i18n, o) ? this.i18n[o] : i;
    }
  }
  return he([
    y({ converter: (e) => e && e in B ? B[e] : B.en })
  ], t.prototype, "lang", 2), t;
};
var pe = Object.defineProperty, ue = Object.getOwnPropertyDescriptor, ot = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ue(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && pe(t, e, o), o;
};
let H = class extends $t(_) {
  constructor() {
    super(...arguments), this.type = bt.success, this.noDismiss = !1, this.isDismissed = !1;
  }
  renderInnerContent() {
    return this.noDismiss ? h`<slot></slot>` : h`
            <div class="row">
                <span class="col-11"><slot></slot></span>
                <span class="col-1 text-right">
                    <button class="button icon-only button-dismiss" @click="${() => this.isDismissed = !0}" title="${this.localize("alert.dismiss")}"
                        aria-label="${this.localize("alert.dismiss")}"
                    >
                        &#10006;
                    </button>
                </span>
            </div>
        `;
  }
  render() {
    if (!(!this.type || this.isDismissed))
      return h`<div class="card bg-${this.type} mb-1 text-white" role="alert">${this.renderInnerContent()}</div>`;
  }
};
H.styles = [
  q(rt),
  et`
            col, [class*=" col-"], [class^="col-"] { margin: 0 calc(var(--grid-gutter) / 2) 0 calc(var(--grid-gutter) / 2); }
            .mb-1 { margin-bottom: 1rem; }
            .button-dismiss { border: none; color: inherit; background: inherit; margin-top: -1rem; font-weight: bold; }
        `
];
ot([
  y({ converter: (r) => r ? bt[r] : void 0 })
], H.prototype, "type", 2);
ot([
  y({ type: Boolean, attribute: "no-dismiss" })
], H.prototype, "noDismiss", 2);
ot([
  x()
], H.prototype, "isDismissed", 2);
H = ot([
  I("lit-alert")
], H);
var G = /* @__PURE__ */ ((r) => (r.dialog = "dialog", r.confirm = "confirm", r))(G || {}), ge = Object.defineProperty, me = Object.getOwnPropertyDescriptor, J = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? me(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && ge(t, e, o), o;
};
const fe = 'button:not(.no-focus), [href], input, select, textarea, [tabindex]:not([tabindex="-1"])';
let D = class extends $t(_) {
  constructor() {
    super(...arguments), this.key = "", this.type = G.dialog, this.href = "", this.isDismissed = !0, this.boundOnKeyDown = this.onKeyDown.bind(this);
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
    const t = Array.from(((o = this.shadowRoot) == null ? void 0 : o.querySelectorAll(fe)) ?? []).map((n) => n);
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
    const r = this.localize("modal.dismiss");
    return h`<button class="button icon-only button-dismiss" @click="${this.onCancelClick}" title="${r}" aria-label="${r}">&#10006;</button>`;
  }
  renderFooterContent() {
    if (this.type !== G.dialog)
      return h`
            <footer class="text-right" role="presentation">
                <button class="button primary" @click="${this.onConfirmClick}">${this.localize("modal.confirm")}</button>
                <button class="button secondary" @click="${this.onCancelClick}">${this.localize("modal.cancel")}</button>
            </footer>
        `;
  }
  renderModal() {
    return h`
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
    return h`
            <slot name="button" @click="${this.onSubmitClick}"></slot>
            <slot><button type="submit" class="button primary no-focus" @click="${this.onSubmitClick}">Click</button></slot>
            ${this.isDismissed ? "" : this.renderModal()}
        `;
  }
  render() {
    if (this.type)
      return this.type === G.dialog || !this.href ? h`<div>${this.renderInnerContent()}</div>` : h`<form action="${this.href}">${this.renderInnerContent()}</form>`;
  }
  connectedCallback() {
    super.connectedCallback(), window.addEventListener("keydown", this.boundOnKeyDown);
  }
  disconnectedCallback() {
    window.removeEventListener("keydown", this.boundOnKeyDown), super.disconnectedCallback();
  }
};
D.styles = [
  q(rt),
  et`
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
            .mb-1 { margin-bottom: 1rem; }
            .button-dismiss { border: none; color: inherit; background: inherit; margin-top: -1rem; font-weight: bold; }
        `
];
J([
  y()
], D.prototype, "key", 2);
J([
  y({ converter: (r) => r ? G[r] : void 0 })
], D.prototype, "type", 2);
J([
  y()
], D.prototype, "href", 2);
J([
  x()
], D.prototype, "isDismissed", 2);
D = J([
  I("lit-modal")
], D);
var P = /* @__PURE__ */ ((r) => (r.asc = "asc", r.desc = "desc", r))(P || {}), ve = Object.defineProperty, ye = Object.getOwnPropertyDescriptor, st = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ye(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && ve(t, e, o), o;
};
let V = class extends _ {
  constructor() {
    super(...arguments), this.property = "", this.noSort = !1, this.sortOrder = void 0;
  }
  toggleSort() {
    this.sortOrder === P.asc ? this.sortOrder = P.desc : this.sortOrder === P.desc ? this.sortOrder = void 0 : this.sortOrder = P.asc, this.dispatchEvent(new CustomEvent("litTableSorted", {
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
      return this.sortOrder === P.asc ? h`&#129053;` : h`&#129055;`;
  }
  render() {
    return this.noSort ? h`<span style="user-select: none;"><slot></slot></span>` : h`
            <span @click="${this.toggleSort}" style="user-select: none; cursor: pointer;" role="button">
                <slot></slot>
                ${this.renderSort()}
            </span>
        `;
  }
};
st([
  y()
], V.prototype, "property", 2);
st([
  y({ type: Boolean, attribute: "no-sort" })
], V.prototype, "noSort", 2);
st([
  x()
], V.prototype, "sortOrder", 2);
V = st([
  I("lit-table-header")
], V);
const be = { ATTRIBUTE: 1, CHILD: 2, PROPERTY: 3, BOOLEAN_ATTRIBUTE: 4, EVENT: 5, ELEMENT: 6 }, $e = (r) => (...t) => ({ _$litDirective$: r, values: t });
class xe {
  constructor(t) {
  }
  get _$AU() {
    return this._$AM._$AU;
  }
  _$AT(t, e, i) {
    this._$Ct = t, this._$AM = e, this._$Ci = i;
  }
  _$AS(t, e) {
    return this.update(t, e);
  }
  update(t, e) {
    return this.render(...e);
  }
}
const { I: _e } = Xt, Ut = () => document.createComment(""), L = (r, t, e) => {
  var i;
  const o = r._$AA.parentNode, s = t === void 0 ? r._$AB : t._$AA;
  if (e === void 0) {
    const n = o.insertBefore(Ut(), s), d = o.insertBefore(Ut(), s);
    e = new _e(n, d, r, r.options);
  } else {
    const n = e._$AB.nextSibling, d = e._$AM, l = d !== r;
    if (l) {
      let a;
      (i = e._$AQ) === null || i === void 0 || i.call(e, r), e._$AM = r, e._$AP !== void 0 && (a = r._$AU) !== d._$AU && e._$AP(a);
    }
    if (n !== s || l) {
      let a = e._$AA;
      for (; a !== n; ) {
        const $ = a.nextSibling;
        o.insertBefore(a, s), a = $;
      }
    }
  }
  return e;
}, k = (r, t, e = r) => (r._$AI(t, e), r), we = {}, Ae = (r, t = we) => r._$AH = t, Ce = (r) => r._$AH, ut = (r) => {
  var t;
  (t = r._$AP) === null || t === void 0 || t.call(r, !1, !0);
  let e = r._$AA;
  const i = r._$AB.nextSibling;
  for (; e !== i; ) {
    const o = e.nextSibling;
    e.remove(), e = o;
  }
};
const Nt = (r, t, e) => {
  const i = /* @__PURE__ */ new Map();
  for (let o = t; o <= e; o++)
    i.set(r[o], o);
  return i;
}, Pe = $e(class extends xe {
  constructor(r) {
    if (super(r), r.type !== be.CHILD)
      throw Error("repeat() can only be used in text expressions");
  }
  ht(r, t, e) {
    let i;
    e === void 0 ? e = t : t !== void 0 && (i = t);
    const o = [], s = [];
    let n = 0;
    for (const d of r)
      o[n] = i ? i(d, n) : n, s[n] = e(d, n), n++;
    return { values: s, keys: o };
  }
  render(r, t, e) {
    return this.ht(r, t, e).values;
  }
  update(r, [t, e, i]) {
    var o;
    const s = Ce(r), { values: n, keys: d } = this.ht(t, e, i);
    if (!Array.isArray(s))
      return this.ut = d, n;
    const l = (o = this.ut) !== null && o !== void 0 ? o : this.ut = [], a = [];
    let $, p, c = 0, u = s.length - 1, g = 0, f = n.length - 1;
    for (; c <= u && g <= f; )
      if (s[c] === null)
        c++;
      else if (s[u] === null)
        u--;
      else if (l[c] === d[g])
        a[g] = k(s[c], n[g]), c++, g++;
      else if (l[u] === d[f])
        a[f] = k(s[u], n[f]), u--, f--;
      else if (l[c] === d[f])
        a[f] = k(s[c], n[f]), L(r, a[f + 1], s[c]), c++, f--;
      else if (l[u] === d[g])
        a[g] = k(s[u], n[g]), L(r, s[c], s[u]), u--, g++;
      else if ($ === void 0 && ($ = Nt(d, g, f), p = Nt(l, c, u)), $.has(l[c]))
        if ($.has(l[u])) {
          const w = p.get(d[g]), nt = w !== void 0 ? s[w] : null;
          if (nt === null) {
            const xt = L(r, s[c]);
            k(xt, n[g]), a[g] = xt;
          } else
            a[g] = k(nt, n[g]), L(r, s[c], nt), s[w] = null;
          g++;
        } else
          ut(s[u]), u--;
      else
        ut(s[c]), c++;
    for (; g <= f; ) {
      const w = L(r, a[f + 1]);
      k(w, n[g]), a[g++] = w;
    }
    for (; c <= u; ) {
      const w = s[c++];
      w !== null && ut(w);
    }
    return this.ut = d, Ae(r, a), E;
  }
});
var A = /* @__PURE__ */ ((r) => (r.Page = "page", r.PerPage = "perPage", r.SearchQuery = "searchQuery", r.Sort = "sort", r))(A || {}), Se = Object.defineProperty, ke = Object.getOwnPropertyDescriptor, b = (r, t, e, i) => {
  for (var o = i > 1 ? void 0 : i ? ke(t, e) : t, s = r.length - 1, n; s >= 0; s--)
    (n = r[s]) && (o = (i ? n(t, e, o) : n(o)) || o);
  return i && o && Se(t, e, o), o;
};
let m = class extends $t(_) {
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
      const o = this[e], s = r[o.property], n = t[o.property];
      if (s === null)
        return 1;
      if (n === null)
        return -1;
      if (s < n)
        return o.sortOrder === P.asc ? -1 : 1;
      if (s > n)
        return o.sortOrder === P.asc ? 1 : -1;
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
    this.saveSetting(A.Sort, JSON.stringify(this.sortColumns)), this.filterData();
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
    this.src.length && (this.data = (await fetch(this.src).then((r) => r.json())).map((r, t) => (r._index = t, r)) ?? [], this.filterData());
  }
  async firstUpdated() {
    if (this.perPage = parseInt(this.fetchSetting(A.PerPage) ?? "10", 10), this.page = parseInt(this.fetchSetting(A.Page) ?? "0", 10), this.searchQuery = this.fetchSetting(A.SearchQuery) ?? "", this.sortColumns = JSON.parse(this.fetchSetting(A.Sort) ?? "[]"), this.shadowRoot) {
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
      this.searchQuery !== r && (this.page = 0, this.saveSetting(A.SearchQuery, r)), this.searchQuery = r, this.filterData();
    }, 250);
  }
  onPerPageInput(r) {
    const t = parseInt(r, 10) ?? 10;
    this.perPage !== t && (this.page = 0, this.saveSetting(A.PerPage, t)), this.perPage = t, this.filterData();
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
    this.page = r, this.saveSetting(A.Page, this.page), this.filterData();
  }
  renderActions(r) {
    const t = this.replaceInUrl(this.editUrl, r), e = this.replaceInUrl(this.deleteUrl, r);
    return h`
            ${t ? h`<a href="${t}" class="button primary button-action icon" title="${this.localize("table.edit")}">&#9998;</a>` : ""}
            ${e ? h`<lit-modal href="${e}" type="confirm" lang="${this.lang}">
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
    return (t = this.filteredData) != null && t.length ? h`
            ${Pe(this.filteredData, (e) => e[this.rowKey], (e) => h`
                    <tr>
                        ${r.map((i) => i === this.actionName ? h`<td>${this.renderActions(e)}</td>` : h`<td><slot name="column-data-${i}">${e[i]}</slot></td>`)}
                    </tr>
                `)}
        ` : h`<tr><td colspan="${r.length}" class="text-center">${this.localize("table.noData")}</td></tr>`;
  }
  renderCount() {
    if (this.filteredRecordTotal)
      return h`${this.page * this.perPage + 1} ${this.localize("table.to")} ${Math.min(
        (this.page + 1) * this.perPage,
        this.filteredRecordTotal
      )} ${this.localize("table.of")} ${this.filteredRecordTotal}`;
  }
  renderInnerContent() {
    if (!this.data.length)
      return h`<slot name="loading"><h1 class="text-center"><div class="spinner"></div></h1></slot>`;
    const r = [...this.hasActions ? [this.actionName] : [], ...Array.from(this.tableHeaders.keys())];
    return h`
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
        return h`
                                                <th class="col-min-width">
                                                    ${this.addUrl ? h`<a href="${this.addUrl}" class="button secondary button-action icon"
                                                            title="${this.localize("table.add")}"
                                                        >
                                                            <span class="rotate45">&#10006;</span>
                                                        </a>` : ""}
                                                </th>
                                            `;
      const e = this.tableHeaders.get(t);
      return h`
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
    return h`<slot></slot>${this.renderInnerContent()}`;
  }
};
m.styles = [
  q(rt),
  et`
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
b([
  y()
], m.prototype, "src", 2);
b([
  y()
], m.prototype, "key", 2);
b([
  y({ attribute: "row-key" })
], m.prototype, "rowKey", 2);
b([
  y({ attribute: "add-url" })
], m.prototype, "addUrl", 2);
b([
  y({ attribute: "edit-url" })
], m.prototype, "editUrl", 2);
b([
  y({ attribute: "delete-url" })
], m.prototype, "deleteUrl", 2);
b([
  x()
], m.prototype, "data", 2);
b([
  x()
], m.prototype, "filteredRecordTotal", 2);
b([
  x()
], m.prototype, "filteredData", 2);
b([
  x()
], m.prototype, "sortColumns", 2);
b([
  x()
], m.prototype, "page", 2);
b([
  x()
], m.prototype, "perPage", 2);
b([
  x()
], m.prototype, "maxPage", 2);
b([
  x()
], m.prototype, "searchQuery", 2);
b([
  x()
], m.prototype, "hasActions", 2);
b([
  x()
], m.prototype, "tableHeaders", 2);
m = b([
  I("lit-table")
], m);
