yepnope({
    test: Modernizr.inputtypes.date,
    nope: ["/Scripts/jquery-ui.js", "/Content/themes/base/jquery-ui.css"],
    callback: function () {
        $("#TickDate").datepicker({ dateFormat: "yy-mm-dd" });
        $("#TickDate").attr("placeholder", "Дата");
    }
});