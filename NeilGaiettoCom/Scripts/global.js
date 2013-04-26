//Site namespace
var NeilSite = NeilSite = NeilSite || {};

//global js object
NeilSite.Global = new (function () {
    var me = this;
    //public
    this.init = function () {
        bindEvents();
        setEmailLinks();
    }

    //private
    function bindEvents() {
    }
    function setEmailLinks() {//sets email on all email links
        var em = $.map("aNbecidle.fGgahijektltmon@ogpmqarislt.ucvowmx".split(''), function (n, i) { return i % 2 ? n : null; }).join("");
        $('a.emailAddress').html(em).attr('href','mailto:'+em);
    }

});

//init all objects added to namespace at initial load
$(document).ready(function () {
    $.each(NeilSite, function (key, value) {
        if (value.init) {
            value.init();
        }
    });
});