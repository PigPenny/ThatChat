// Declare a proxy to reference the hub.
var chat = $.connection.chatHub;
var names = [];
var chatNames = [];

// Create a function that the hub can call to broadcast messages.
chat.client.broadcastMessage = function (name, message, id, active) {
    var li = document.createElement("li");
    var nameDiv = document.createElement("div");
    var contentDiv = document.createElement("div");

    if (active == true) {
        nameDiv.className = "active accnt";
    }
    else {
        nameDiv.className = "inactive accnt";
    }

    nameDiv.innerText = name;
    contentDiv.innerText = message;

    li.appendChild(nameDiv);
    li.appendChild(contentDiv);
    li.className = "remove";

    // Add the message to the page.
    $('#discussion').append(li);

    if (names[id] == null)
        names[id] = [];
    names[id][names[id].length] = nameDiv;
};

chat.client.addChat = function (name, id) {
    var li = document.createElement("li");
    var nameDiv = document.createElement("div");

    nameDiv.innerText = name;

    li.appendChild(nameDiv);
    li.onclick = function () {
        $("#discussion").empty();

        chat.server.selectChatRoom(id);
        chat.server.init();
    }

    // Add the message to the page.
    $('#chatRooms').append(li);

    var nameObject = { "name" : name };
    chatNames.push(nameObject);
}

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

    $('#message').keypress(function (e) {
        if (e.which == 13) {
            $('#sendmessage').click();
        }
    });

    $('#sendmessage').click(function () {
        // Call the Send method on the hub.
        chat.server.send($('#message').val());
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    });

    $('#displayname').keypress(function (e) {
        if (e.which == 13) {
            $('#setname').click();
        }
    });

    $('#setname').click(function () {
        // Call the Send method on the hub.
        chat.server.setName($('#displayname').val());
        // Clear text box and reset focus for next comment.
        $('#displayname').val('').focus();
    });

    chat.server.populateChats();
    chat.server.addUser($('#displayname').val());
    chat.server.selectChatRoom(0);
    chat.server.init();

    var options = {
        shouldSort: true,
        threshold: 0.0,
        location: 0,
        distance: 100,
        maxPatternLength: 32,
        minMatchCharLength: 1,
        keys: [
            "name"
        ]
    };

    $('#searchButton').click(function () {
        console.log(chatNames);
        var fuse = new Fuse(chatNames, options); // "list" is the item array

        var chat = $('#searchBox').val();
        console.log(chat);
        var result = fuse.search(chat);
        console.log(result);
        $('#chatRooms').empty();
        for (var i = 0; i < result.length; i++) {
            var li = document.createElement("li");
            var nameDiv = document.createElement("div");
            //console.log(result[i]);
            nameDiv.innerText = result[i].name;

            li.appendChild(nameDiv)
            // Add the message to the page.
            $('#chatRooms').append(li);
        }

    })
});

