"use strict";

$(() => {
    const $editPersonModalPlaceholder = $("#edit-person-modal-placeholder");
    const $personListPlaceholder = $("#person-list-placeholder");
    const $addPersonModalPlaceholder = $("#add-person-modal-placeholder");

    $personListPlaceholder.on("click", "button[data-toggle='sort-person-list']",
        function() {
            const index = parseInt($(this).parents(".filter-sort").data("action-index"));
            const url = $(this).data("url");

            $.get(url, { "index": index }).done((list) => {
                $personListPlaceholder.empty();
                $personListPlaceholder.append(list);
            });
        });    

    $personListPlaceholder.on("click", "a[data-toggle='apply-options-filter-person-list']",
        function() {
            const index = parseInt($(this).parents(".filter-sort").data("action-index"));
            const url = $(this).parent(".dropdown-menu").data("url");
            const value = $(this).text();

            $.get(url, {"index":index, "value":value}).done((list) => {
                $personListPlaceholder.empty();
                $personListPlaceholder.append(list);
            });
        });   
    
    $personListPlaceholder.on("click", "button[data-toggle='apply-text-filter-person-list']",
        function() {
            const index = parseInt($(this).parents(".filter-sort").data("action-index"));
            const url = $(this).parents(".dropdown-menu").data("url");
            const value = $(this).parent().siblings("input[type='text']").val();

            $.get(url, {"index":index, "value":value}).done((list) => {
                $personListPlaceholder.empty();
                $personListPlaceholder.append(list);
            });
        });

    $personListPlaceholder.on("click", "button[data-toggle='remove-filter-person-list']",
        function() {
            const index = parseInt($(this).parents(".filter-sort").data("action-index"));
            const url = $(this).data("url");

            $.get(url, {"index":index}).done((list) => {
                $personListPlaceholder.empty();
                $personListPlaceholder.append(list);
            });
        });    
    
    $personListPlaceholder.on("click", "button[data-toggle='remove-all-filters-person-list']",
        function() {
            const url = $(this).data("url");

            $.get(url).done((list) => {
                $personListPlaceholder.empty();
                $personListPlaceholder.append(list);
            });
        });

    $personListPlaceholder.on("click",
        "button[data-toggle='edit-person-modal']",
        function(e) {
            const url = $(this).data("url");
            const index = parseInt(`${$(this).parents("tr").find("#person-index").text()}`);

            $.get(url, { "index": index - 1 }).done((data) => {
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

    $("body").on("focusin",
        "input.datepicker",
        function () {
            $(this).datepicker({
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