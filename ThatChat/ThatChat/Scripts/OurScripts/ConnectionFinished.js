// The message displayed to new users
var startMessage = "Hello, welcome to ThatChat! " +
    "You're not connected to any chats right now, " +
    "you'll have to select one from the list to the " +
    "left before you can start talking to anyone. " +
    "You can customize your name with the box to the " +
    "top of the page, and you can send messages with the box to the bottom!";

var background = true;

// Start the connection.
$.connection.hub.start().done(function () {
    $('#counter').text(0);

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

    $('#TextBoxChatAdd').keypress(function (e) {
        if (e.which == 13) {
            $('#ButtonChatAdd').click();
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
        closeSide();
    });

    // Adds a chat when the user clicks the button
    // Connor Goudie/Chandu Dissanayake
    // November 6, 2017
    $('#ButtonChatAdd').click(function () {
        let name = $('#TextBoxChatAdd').val();
        if (valid(name)) {
            $('#TextBoxChatAdd').val('');
            $('#message').focus();
            chat.server.addChat(name);
            $('#error').css("display", "none");
        } else {
            $('#error').css("display", "block");
        }
    });

    //Puts all the chats currently existing in the chat list
    chat.server.populateChats();
    //Adds the new user specified in the displayname textbox
    chat.server.addUser($('#displayname').val());
    chat.client.broadcastMessage("GOD", startMessage, 0, true);

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
    $('#searchBox').keyup(function () {
        var fuse = new Fuse(chats, options); // "list" is the item array

        var chat = $('#searchBox').val();
        if (chat != "") {
            var result = fuse.search(chat);
            $('#chatRooms').empty();
            for (var i = 0; i < result.length; i++) {
                $('#chatRooms').append(result[i]["element"]);
            }
            //chat.server.populateChats();
        } else {
            $('#chatRooms').empty();
            for (var cr of chats) {
                $('#chatRooms').append(cr.element);
            }
        }
    });

    $('#message').keyup(function () {
        var length = $('#message').val().length;
        if (length < 120) {
            $('#counter').css("color", "white");
        } else {
            $('#counter').css("color", "red");
        }
        $('#counter').text(length);
    });

    $('#OnOff').click(function () {
        background = !background;
        console.log(background);
        if (!background) {
            $('#OnOff').attr('value', "Background: Static");
            $('body').removeClass("grad");
            $('body').addClass("stat");
        } else if (background) {
            $('#OnOff').attr('value', "Background: Dynamic");
            $('body').removeClass("stat");
            $('body').addClass("grad");
        }
    })
});

function valid(name) {
    var regex = /^[\w\-\s]+$/;
    return name.length > 0 && regex.exec(name) != null && name.length <= 64;
}

// Set initial focus to name input box
$('#displayname').focus();