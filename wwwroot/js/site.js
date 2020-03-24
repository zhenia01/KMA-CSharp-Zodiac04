"use strict";

$(() => {
    const $editModalPlaceholder = $("#edit-person-modal-placeholder");

    $('button[data-toggle="ajax-modal"]').click(function(event) {
        const url = $(this).data("url");
        $.get(url).done((data) => {
            $editModalPlaceholder.empty();
            $editModalPlaceholder.append(data);
            $editModalPlaceholder.find(".modal").first().modal("show");
        });
    });

    $editModalPlaceholder.on("click",
        "[data-save='modal']",
        function(e) {
            e.preventDefault();

            const $form = $(this).parents(".modal").find("#birthDateForm");
            var data = $form.serialize();
            //data += append(`&index=${$(this).parents("#person-index").text()}`);
            const actionUrl = $form.attr("action");

            $.post(actionUrl, data).done((data) => {
                var newBody = $(".modal-body", data);
                $editModalPlaceholder.find(".modal-body").replaceWith(newBody);
            });
        });

    $editModalPlaceholder.on("focusin",
        "input.datepicker",
        (e) => {
            $(".datepicker").first().datepicker({
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
        });
});