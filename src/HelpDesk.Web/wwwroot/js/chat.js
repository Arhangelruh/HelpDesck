let userName = document.getElementById("userName").value;
let userGroup = document.getElementById("userGroup").value;

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// отправка сообщения в группу
document.getElementById("sendButton").addEventListener("click", () => {

    const message = document.getElementById("message").value;
    hubConnection.invoke("Send", message, userName, userGroup)
        .catch(error => console.error(error));
});

// получение сообщения для определенной группы
hubConnection.on("Receive", (message, user) => {

    // создаем элемент <b> для имени пользователя
    const userNameElem = document.createElement("b");
    userNameElem.textContent = `${user}: `;

    // создает элемент <p> для сообщения пользователя
    const elem = document.createElement("p");
    elem.appendChild(userNameElem);
    elem.appendChild(document.createTextNode(message));

    const firstElem = document.getElementById("chatroom").firstChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);
});

// получение общего уведомления
hubConnection.on("Notify", message => {

    const elem = document.createElement("p");
    elem.textContent = message;

    const firstElem = document.getElementById("chatroom").firstChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);
});

hubConnection.start()
    .then(() => {
        document.getElementById("sendButton").disabled = false;
        hubConnection.invoke("Enter", userName, userGroup);
    })
    .catch ((err) => console.error(err));
