// The WebUI data lists (data/*.json: units, stages...) change when the game gets new units, but
// browsers keep their old copy (no Cache-Control header). Every data/*.json request revalidates with
// the server (ETag -> 304 when unchanged), and the main lists are refreshed when the page starts.
(function () {
    try {
        var origFetch = window.fetch;
        var isData = function (u) { return /(^|\/)data\/[^?#]*\.json([?#]|$)/.test(u); };
        window.fetch = function (input, init) {
            var url = typeof input === 'string' ? input : (input && input.url) || '';
            if (isData(url) && !(init && init.cache)) {
                init = Object.assign({}, init || {}, { cache: 'no-cache' });
            }
            return origFetch.call(this, input, init);
        };
        ['data/MobileSuits.json', 'data/Stages.json', 'data/Navigators.json', 'data/Bgms.json'].forEach(function (u) {
            origFetch(u, { cache: 'no-cache' }).catch(function () { });
        });
    } catch (e) { }
})();
