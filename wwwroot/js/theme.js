window.theme = {
 get: function () {
 try {
 return localStorage.getItem('koleos-theme') || 'light';
 } catch (e) { return 'light'; }
 },
 set: function (value) {
 try {
 localStorage.setItem('koleos-theme', value);
 if (value === 'dark') document.documentElement.classList.add('theme-dark'); else document.documentElement.classList.remove('theme-dark');
 return value;
 } catch (e) { return null; }
 },
 toggle: function () {
 var current = this.get();
 var next = current === 'dark' ? 'light' : 'dark';
 this.set(next);
 return next;
 },
 init: function () {
 var t = this.get();
 this.set(t);
 }
};
// init immediately
window.addEventListener('DOMContentLoaded', function () { if (window.theme) window.theme.init(); });