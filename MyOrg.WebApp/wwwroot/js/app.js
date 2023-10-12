"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/mainHub").build();

document.getElementById("processButton").disabled = true;

connection.on("ReceiveMainMessage", function (message) {
    putIntoChannel(message, "messagesListMain");
});

connection.on("ReceiveDebugMessage", function (message) {
    putIntoChannel(message, "messagesListDebug");
});

function putIntoChannel(message, listId) {
    var li = document.createElement("li");
    var pre = document.createElement("pre");
    li.appendChild(pre);
    var o = JSON.parse(`${message}`);
    document.getElementById(listId).prepend(li);
    pre.textContent = JSON.stringify(o, Object.keys(o).sort(), 2);
}


connection.start().then(function () {
    document.getElementById("processButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("changeUserButton").addEventListener("click", function (event) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "/Home/UserName");
    var input = document.createElement("input");
    input.setAttribute("type", "text");
    input.setAttribute("name", "userName");
    input.setAttribute("value", document.getElementById("userName").value);
    form.appendChild(input);
    document.body.appendChild(form);
    form.submit();
});

document.getElementById("processButton").addEventListener("click", function (event) {
    var side1 = document.getElementById("side1").value;
    var side2 = document.getElementById("side2").value;

    const response = fetch('/Pythagoras/', {
        method: 'POST',
        headers: {
            Accept: 'application.json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ "side1": parseInt(side1), "side2": parseInt(side2) }),
        cache: 'default'
    });
    response.catch(error => document.getElementById("resultDiv").innerHTML = error);
    response.then(response => response.text()).then(text => document.getElementById("resultDiv").innerHTML = text);
    event.preventDefault();
});

