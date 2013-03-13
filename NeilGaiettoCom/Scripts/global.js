//Site namespace
var NeilSite = NeilSite = NeilSite || {};

//global js object
NeilSite.Global = new (function () {
    var me = this;
    //public
    this.init = function () {
        bindEvents();
    }

    //private
    function bindEvents() {
    }

});

//init all objects added to namespace
$(document).ready(function () {
    $.each(NeilSite, function (key, value) {
        if (value.init) {
            value.init();
        }
    });
});