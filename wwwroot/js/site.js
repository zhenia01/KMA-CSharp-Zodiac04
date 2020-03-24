"use strict";

$(() => {
    const $editPersonModalPlaceholder = $("#edit-person-modal-placeholder");
    const $personListPlaceholder = $("#person-list-placeholder");
    const $addPersonModalPlaceholder = $("#add-person-modal-placeholder");

    $personListPlaceholder.on("click",
        "button[data-toggle='edit-person-modal']",
        function(e) {
            const url = $(this).data("url");

            const index = parseInt(`${$(this).parents("tr").find("#person-index").text()}`);

            $.get(url, {"index": index-1}).done((data) => {
                $editPersonModalPlaceholder.empty();
                $editPersonModalPlaceholder.append(data);
                $editPersonModalPlaceholder.find(".modal").first().modal("show");
            });
        });

    $personListPlaceholder.on("click",
        "button[data-toggle='delete-person-modal']",
        function(e) {

            const url = $(this).data("url");
            const index = parseInt(`${$(this).parents("tr").find("#person-index").text()}`);

            $.get(url, { "index": index - 1 }).done(() => {
                $.get("/?handler=PersonList").done((list) => {
                    $personListPlaceholder.empty();
                    $personListPlaceholder.append(list);
                });
            });
        });

    $personListPlaceholder.on("click",
        "button[data-toggle='add-person-modal']",
        function(e) {
            const url = $(this).data("url");
            $.get(url).done((data) => {
                $addPersonModalPlaceholder.empty();
                $addPersonModalPlaceholder.prepend(data);
                $addPersonModalPlaceholder.find(".modal").first().modal("show");
            });
        });

    $editPersonModalPlaceholder.on("click",
        "button[data-save='modal']",
        function(e) {
            e.preventDefault();

            const $form = $(this).parents(".modal").find("#editPersonForm");
            const data = $form.serialize();
            const actionUrl = $form.attr("action");

            $.post(actionUrl, data).done((edited) => {
                const newBody = $(".modal-body", edited);
                $editPersonModalPlaceholder.find(".modal-body").replaceWith(newBody);

                const isValid = newBody.find('[name="IsValid"]').val() === "True";
                if (isValid) {
                    $.get("/?handler=PersonList").done((list) => {
                        $editPersonModalPlaceholder.find(".modal").first().modal("hide");
                        $personListPlaceholder.empty();
                        $personListPlaceholder.append(list);
                    });
                }
            });
        });

    $addPersonModalPlaceholder.on("click",
        "button[data-save='modal']",
        function(e) {
            e.preventDefault();

            const $form = $(this).parents(".modal").find("#addPersonForm");
            const data = $form.serialize();
            const actionUrl = $form.attr("action");

            $.post(actionUrl, data).done((edited) => {
                const newBody = $(".modal-body", edited);
                $addPersonModalPlaceholder.find(".modal-body").replaceWith(newBody);

                const isValid = newBody.find('[name="IsValid"]').val() === "True";
                if (isValid) {
                    $.get("/?handler=PersonList").done((list) => {
                        $addPersonModalPlaceholder.find(".modal").first().modal("hide");
                        $personListPlaceholder.empty();
                        $personListPlaceholder.append(list);
                    });
                }
            });
        });

    $("[id$='modal-placeholder']").on("focusin",
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