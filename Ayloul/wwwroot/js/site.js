// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function sendMessage() {

    let message =
        document.getElementById("messageInput").value;


    connection.invoke(
        "SendMessage",
        @Model.ChatRoom_Id,
        "@User.Identity.Name",
        message
    );


    document.getElementById("messageInput").value = "";

}
id = "qj1l6s"
function scrollBottom() {
    let box = document.getElementById("messages");

    box.scrollTop = box.scrollHeight;
}


scrollBottom();