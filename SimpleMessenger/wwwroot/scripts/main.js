function showValidationErrors(form) {
    const formValidator = form.validate();

    if (!form.valid()) {
        const validationErrorsArea = $("#form-validation-errors-area");
        var validationErrorsAlert = validationErrorsArea.find("div.alert");
        var validationErrors = "";

        for (var i = 0; i < formValidator.errorList.length; i++) {
            validationErrors += formValidator.errorList[i].message;
            if (i < formValidator.errorList.length - 1)
                validationErrors += "</br>";
        }

        console.log(validationErrors);

        if (validationErrorsAlert.length == 0)
            validationErrorsArea.append("<div class=\"alert alert-danger alert-dismissible fade show\"></div>");
        validationErrorsAlert = validationErrorsArea.find("div.alert");

        validationErrorsAlert.eq(0).empty();
        validationErrorsAlert.eq(0).append(validationErrors);
        validationErrorsAlert.eq(0).append("<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>");
    }
}

$(document).ready(function () {
    var selectedMessageSubject = "";
    var selectedMessageBody = "";
    var selectedMessageSender = "";

    $("#login-button").click(function () {
        showValidationErrors($("#login-form"));
    });

    $("#register-button").click(function () {
        showValidationErrors($("#registration-form"));
    });

    $("#message-send-button").click(function () {
        showValidationErrors($("#message-send-form"));
    });

    $("#message-reply-button").click(function () {
        $("#message-view-modal-window").modal("hide");

        $("#new-message-recipients-input").val(selectedMessageSender + ";");
        $("#new-message-subject-input").val(selectedMessageSubject + ": Re");

        $("#message-send-modal-window").modal("show");
    });

    $("#close-message-send-modal-window").click(function () {
        $("#form-validation-errors-area").remove();
        $("#new-message-recipients-input").val("");
        $("#new-message-subject-input").val("");
        $("#new-message-body-input").trumbowyg("empty");
    });

    $("#message-create-button").click(function () {
        $("#message-send-modal-window").modal("show");
    });

    $(document).on("click", "#received-messages-table tbody tr", function () {
        const selectedRowHiddenInputs = $(this).find("input:hidden");
        selectedMessageSubject = selectedRowHiddenInputs.eq(0).val();
        selectedMessageBody = selectedRowHiddenInputs.eq(1).val();
        selectedMessageSender = selectedRowHiddenInputs.eq(2).val();

        $("#selected-message-sender-area").text("The message received from " + selectedMessageSender);
        $("#selected-message-body-area").empty();
        $("#selected-message-body-area").append(selectedMessageBody);

        $("#message-view-modal-window").modal("show");
    });

    $("#new-message-body-input").trumbowyg({
        btns: [
            ["undo", "redo"],
            ["removeformat"],
            ["formatting"],
            ["strong", "em", "del"],
            ["superscript", "subscript"],
            ["unorderedList", "orderedList"],
            ["justifyLeft", "justifyCenter", "justifyRight", "justifyFull"]
        ]
    });
});
