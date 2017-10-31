// Declare a proxy to reference the hub.
var chat = $.connection.chatHub;
var names = [];

// Create a function that the hub can call to broadcast messages.
chat.client.broadcastMessage = function (name, message, id) {
    var li = document.createElement("li");
    var nameDiv = document.createElement("div");
    var contentDiv = document.createElement("div");

    nameDiv.className = "active accnt";
    nameDiv.innerText = name;
    contentDiv.innerText = message;

    li.appendChild(nameDiv);
    li.appendChild(contentDiv)

    // Add the message to the page.
    $('#discussion').append(li);

    if (names[id] == null)
        names[id] = [];
    names[id][names[id].length] = nameDiv;
};

chat.client.deactivateUser = function (id) {
    if (names[id] != null)
    {
        for (var nameDiv of names[id])
            nameDiv.className = "inactive accnt"
    }
}

// Set initial focus to name input box.
$('#displayname').focus();

// Start the connection.
$.connection.hub.start().done(function () {
    $('#sendmessage').click(function () {
        // Call the Send method on the hub.
        chat.server.send($('#message').val());
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    });

    $('#setname').click(function () {
        // Call the Send method on the hub.
        chat.server.setName($('#displayname').val());
        // Clear text box and reset focus for next comment.
        $('#displayname').val('').focus();
    });

    chat.server.init($('#displayname').val());
});

