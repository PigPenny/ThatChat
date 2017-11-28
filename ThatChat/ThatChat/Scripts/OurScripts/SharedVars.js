// Declare a proxy to reference the hub.
var chat = $.connection.chatHub;
// Arrray used to store list of user names
var names = [];
// Array used to store list of chat room names
var chats = [];
// Stores the name currently being used
var currentName;