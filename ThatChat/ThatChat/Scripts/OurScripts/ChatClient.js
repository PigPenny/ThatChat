var username = "";
function embedFile(regex, content, type)
{
    if (regex.exec(content.innerText) != null) {
        var media = document.createElement(type);
        media.onerror = function () {
            this.remove();
        };

        console.log(type);
        media.src = content.innerText;
        media.style.maxWidth = '80%';
        media.style.maxHeight = '80%';
        content.appendChild(document.createElement('br'));
        content.appendChild(media);
        media.controls = true;
    }
}

function addMedia(content) {
    embedFile(/\.(jpeg|jpg|gif|png)$/, content, 'img');
    embedFile(/\.(mp4|webm)$/, content, 'video');
}
$(this).attr("placeholder", "Type your answer here");
chat.client.displayName = function (name) {
    $('#displayname').attr("placeholder", name);
};

// Create a function that the hub can call to broadcast messages.
// Chandu Dissanayake/Andrew Busto
// October 17, 2017
chat.client.broadcastMessage = function (name, message, id, active) {
    var li          = document.createElement("li");
    var nameDiv     = document.createElement("div");
    var contentDiv  = document.createElement("div");

    //Add names for changing the css class later
    if (names[id] == null)
        names[id] = [];

    Math.seedrandom(id);
    var r = 255;
    var g = Math.floor(Math.random() * 234) + 16;
    var b = Math.floor(Math.random() * 140) + 16
    nameDiv.style.color = "#" + r.toString(16) + g.toString(16) + b.toString(16);

    if (active) {
        nameDiv.className = "active accnt";
    }
    else {
        nameDiv.className = "inactive accnt";
    }

    nameDiv.innerText = name;
    contentDiv.innerText = message;
    addMedia(contentDiv);

    if (prevId != id)
        li.appendChild(nameDiv);

    li.appendChild(contentDiv);
    prevId = id;

    // Add the message to the page.
    $('#discussion').append(li);

    //scroll to bottom of chat div
    $("#discussionScrollDiv").scrollTop($("#discussionScrollDiv")[0].scrollHeight);
    names[id][names[id].length] = nameDiv;
};

// Create a function that the hub can call to add a chat room to the hub
// Paul McCarlie
// November 7, 2017
chat.client.addChat = function (name, id, count) {
    var a = document.createElement('a');
    console.log(name);
    var linkText = document.createTextNode(name);
    console.log(linkText);
    a.appendChild(linkText);
    a.title = name;
    a.innerText = name + " " + count;
    a.href = "#";
    a.className = "form-control";
    a.id = id;
    document.body.appendChild(a);

    a.onclick = function () {
        prevId = -1;
        $("#discussion").empty();
        chat.server.selectChatRoom(id);
        chat.server.init();
        closeSide();
    };

    // Add the message to the page.
    $('#chatRooms').append(a);

    var nameObject = { "id": id, "name": name, "element": a };
    chats.push(nameObject);
};

chat.client.removeChat = function (id) {
    for (var i = chats.length - 1; i >= 0; i--) {
        if (chats[i].id === id) {
            chats[i].element.remove();
            chats.splice(i, 1);
        }
    }
}

chat.client.updateChatUserCount = function (id, name, count) {
    chats.forEach(function (item) {
        if (item.id == id) {
            item.element.innerText = name + " " + count;
        }
    });
}

// Create a function that the hub can call to deactivate a user no longer in use
// Andrew Busto
// October 31, 2017
chat.client.deactivateUser = function (id) {
    if (names[id] != null) {
        for (var nameDiv of names[id])
            nameDiv.className = "inactive accnt";
    }
};

chat.client.ping = function () {
    chat.server.respond();
    //console.log("ping");
};

//function setName() {
//    var name = document.getElementById("displayname").val;
//}
