//Site namespace
var NeilSite = NeilSite = NeilSite || {};

//global js object
NeilSite.Global = new (function () {
    var me = this;
    this.init = function () {
        me.BindEvents();
    }

    this.BindEvents = function () {

        //bind form submit links
        $('.FormSubmitButton').click(function (e) {
            e.preventDefault();
            $(this).closest('form').submit();
        });
    }

});

$(document).ready(function () {
    NeilSite.Global.init();
});