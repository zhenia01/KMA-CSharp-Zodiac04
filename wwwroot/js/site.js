"use strict";

$(() => {
    const $editModalPlaceholder = $("#edit-person-modal-placeholder");
    const $personListPlaceholder = $("#person-list-placeholder");

    $personListPlaceholder.on("click",
        "button[data-toggle='ajax-modal']",
        function(e) {
            const url = $(this).data("url");
            $.get(url).done((data) => {
                $editModalPlaceholder.empty();
                $editModalPlaceholder.append(data);
                $editModalPlaceholder.find(".modal").first().modal("show");
                const index = `${$(this).parents("tr").find("#person-index").text()}`;
                $editModalPlaceholder.find("#person-index").attr("value", index);
            });
        });

    $personListPlaceholder.on("click",
        "button[data-toggle='ajax-delete']",
        function(e) {
            const url = $(this).data("url");
            const index = parseInt(`${$(this).parents("tr").find("#person-index").text()}`);

            $.get(url, { "index": index-1 }).done(() => {
                $.get("/?handler=PersonList").done((list) => {
                    $personListPlaceholder.empty();
                    $personListPlaceholder.append(list);
                });
            });
        });

    $editModalPlaceholder.on("click",
        "[data-save='modal']",
        function(e) {
            e.preventDefault();

            const $form = $(this).parents(".modal").find("#birthDateForm");
            const data = $form.serialize();
            const actionUrl = $form.attr("action");
            $.post(actionUrl, data).done((edited) => {
                const newBody = $(".modal-body", edited);
                $editModalPlaceholder.find(".modal-body").replaceWith(newBody);

                const isValid = newBody.find('[name="IsValid"]').val() === "True";
                if (isValid) {
                    $editModalPlaceholder.find(".modal").first().modal("hide");
                }

                $.get("/?handler=PersonList").done((list) => {
                    $personListPlaceholder.empty();
                    $personListPlaceholder.append(list);
                });
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