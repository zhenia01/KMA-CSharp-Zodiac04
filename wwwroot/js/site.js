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

    const $form = $("#birthDateForm");
    const $editModal = $("#editPersonModal");

    $form.submit((e) => {
        e.preventDefault();
        const dataToSend = $form.serialize();
        const actionUrl = $form.attr("action");

        $.post(actionUrl, dataToSend).done((data) => {
            var $newBody = $(".modal-body", data);
            $editModal.find(".modal-body").replaceWith($newBody);
        });
    });

});