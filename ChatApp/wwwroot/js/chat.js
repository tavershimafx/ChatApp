"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub")
    .withAutomaticReconnect([0, 1000, 3000, 5000, 7000, 10000, 12000, 15000, 20000, 30000 ])
    .build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ChatMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    
    li.textContent = `${user} says ${message}`;
});

connection.on("LoadMessages", function (username, messages) {
    console.log("previous messages")
    console.log(messages)
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("user").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("connectBtn").addEventListener("click", function (event) {
    start()
    event.preventDefault();
});


async function start() {
    try {
        await connection.start();
        document.getElementById("sendButton").disabled = false;
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

// prevent the tab from sleeping and disconnecting a connection
//var lockResolver;
//if (navigator && navigator.locks && navigator.locks.request) {
//    const promise = new Promise((res) => {
//        lockResolver = res;
//    });

//    navigator.locks.request('unique_lock_name', { mode: "shared" }, () => {
//        return promise;
//    });
//}