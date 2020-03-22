"use strict";

$(() => {
    $(".datepicker").datepicker({
        clearBtn: true,
        format: "dd-mm-yyyy",
        endDate: "0d",
        startDate: "-135y",
        weekStart: 1,
        onClose: true,
        immediateUpdates: true,
        keyboardNavigation: false,
        todayBtn: "linked"
    });

    //$("#birthDateForm").submit((e) => {
    //    e.preventDefault();
    //});

});