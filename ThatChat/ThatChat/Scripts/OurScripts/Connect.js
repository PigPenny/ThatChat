// Declare a proxy to reference the hub.
var chat = $.connection.chatHub;
// Arrray used to store list of user names
var names = [];
// Array used to store list of chat room names
var chats = [];

// Create a function that the hub can call to broadcast messages.
// Chandu Dissanayake/Andrew Busto
// October 17, 2017
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

// Create a function that the hub can call to add a chat room to the hub
// Paul McCarlie
// November 7, 2017
chat.client.addChat = function (name, id) {
    var li = document.createElement("li");

    var a = document.createElement('a');
    var linkText = document.createTextNode(name);
    a.appendChild(linkText);
    a.title = name;
    a.href = "#";
    a.className = "btn";
    document.body.appendChild(a);

    li.appendChild(a);
    a.onclick = function () {
        $("#discussion").empty();
        chat.server.selectChatRoom(id);
        chat.server.init();
        closeSide();
    };

    // Add the message to the page.
    $('#chatRooms').append(li);

    var nameObject = { "name": name, "element": li };
    chats.push(nameObject);
};

// Create a function that the hub can call to deactivate a user no longer in use
// Andrew Busto
// October 31, 2017
chat.client.deactivateUser = function (id) {
    if (names[id] != null) {
        for (var nameDiv of names[id])
            nameDiv.className = "inactive accnt";
    }
};

// Set initial focus to name input box
$('#displayname').focus();

// Start the connection.
$.connection.hub.start().done(function () {

    // Calls the sendmessage function when the user presses enter in the message textbox 
    // Paul McCarlie
    // November 9, 2017
    $('#message').keypress(function (e) {
        if (e.which == 13) {
            $('#sendmessage').click();
        }
    });

    // Sends a message when the user clicks send
    // Paul McCarlie
    // October 28, 2017
    $('#sendmessage').click(function () {
        // Call the Send method on the hub.
        chat.server.send($('#message').val());
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    });

    // Calls the setname function when the user presses enter in the setname textbox
    // Paul McCarlie
    // November 9, 2017
    $('#displayname').keypress(function (e) {
        if (e.which == 13) {
            $('#setname').click();
        }
    });

    // Sets/resets the users name when the user clicks setname
    // Andrew Busto
    // November 7, 2017
    $('#setname').click(function () {
        // Call the Send method on the hub.
        chat.server.setName($('#displayname').val());
        // Clear text box and reset focus for next comment.
        $('#displayname').val('');
        $('#message').focus();
    });

    // Adds a chat when the user clicks the button
    // Connor Goudie/Chandu Dissanayake
    // November 6, 2017
    $('#ButtonChatAdd').click(function () {
        chat.server.addChat($('#TextBoxChatAdd').val());
        $('#TextBoxChatAdd').val('');
        $('#message').focus();
        $('.closebtn').click();
    });

    //Puts all the chats currently existing in the chat list
    chat.server.populateChats();
    //Adds the new user specified in the displayname textbox
    chat.server.addUser($('#displayname').val());
    //Puts the user in the first chat room on the list
    chat.server.selectChatRoom(0);
    chat.server.init();

    //options for our fuzzy search, only threshold should need to be changed
    var options = {

        //Will sort the results based on how close the search is to the real answer
        shouldSort: true,

        //This will determine how close the search has match a conversation, 0 means it must match perfectly, 
        //1 means it does not have to match at all
        threshold: 0.0,

        //Where to start matching, 0 is at the beginning of the string
        location: 0,

        //How close the the match must be to the location
        distance: 0,

        //Maximum length of the string to be matched
        maxPatternLength: 64,

        //Match strings of length at least this
        minMatchCharLength: 1,

        //Which properties in the object to be matched
        keys: [
            "name"
        ]
    };

    // Searches the list of existing chatrooms using the string specified in the chat search box
    // Paul McCarlie
    // November 16, 2017
    $('#searchButton').click(function () {
        var fuse = new Fuse(chats, options); // "list" is the item array

        var chat = $('#searchBox').val();

        var result = fuse.search(chat);

        $('#chatRooms').empty();
        for (var i = 0; i < result.length; i++) {
            $('#chatRooms').append(result[i]["element"]);
        }

    });
});

