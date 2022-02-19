function showNotification(messageModel) {
    const notificationToastTemplate = $("#received-message-notifications-area > div").last().clone();
    notificationToastTemplate.find("div.toast-header strong").eq(0).text(messageModel.receivedDate);
    notificationToastTemplate.find("div.toast-body").eq(0).text("You have a new message from " + messageModel.sender);

    $("#received-message-notifications-area").prepend(notificationToastTemplate);
    notificationToastTemplate.removeClass("hide");
    notificationToastTemplate.addClass("show");

    setTimeout(function () {
        notificationToastTemplate.remove();
    }, 5000);
}

$(document).ready(function () {
    const messageNotifierHubConnection = new signalR.HubConnectionBuilder().withUrl("/message-notifier").build();

    messageNotifierHubConnection.on("ShowNotification", function (messageModel) {
        $.when($.ajax({
            type: "GET",
            url: "/",
            async: true,
            success: function (receivedMessagesTable) {
                $("#received-messages-table-wrapper").empty();
                $("#received-messages-table-wrapper").append(receivedMessagesTable);
            }
        })).then(function () {
            showNotification(messageModel);
        });
    });

    messageNotifierHubConnection.start();
});
