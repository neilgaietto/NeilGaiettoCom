

var BadaBing = BadaBing = BadaBing || {};

BadaBing.Boom = new (function () {
    var me = this;
    //public
    this.init = function () {
        bindEvents();

    }

    //private
    function bindEvents() {
        $('#BingItButton').on('click', function (event) {
            $(this).removeClass('btn-success').addClass('btn-danger');
            activate();
        });
    }
    function activate() {
        $.post("/bada/terms", function (data) {
            doSearch(data);
        });


    }
    function doSearch(data) {

        for (var i = 0; i < data.length; i++) {
            //console.log(data[i].Keyword);
            with ({ keyword: data[i].Keyword }) {
                window.setTimeout(function () {
                    openNewTab("http://www.bing.com/search?q=" + keyword);
                }, i * 1000);
            }

        }
    }
    function openNewTab(url) {
        instance = window.open("about:blank");
        instance.document.write("<meta http-equiv=\"refresh\" content=\"0;url=" + url + "\">");
        instance.document.close();
        return false;
    }


});



$(document).ready(function () {
    $.each(BadaBing, function (key, value) {
        if (value.init) {
            value.init();
        }
    });
});