let userName = document.getElementById("userName").value;
let userGroup = document.getElementById("userGroup").value;

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// отправка сообщения в группу
document.getElementById("message").addEventListener("keydown", event => {
    if( event.code === 'Enter' ){
    const message = document.getElementById("message").value;
    hubConnection.invoke("Send", message, userName, userGroup)
        .catch(error => console.error(error));
        document.getElementById("message").value = "";
    }
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
    userNameElem.setAttribute("class","chat_string")
    if(user == userName){
        userNameElem.setAttribute("style","color:blue;")
    }
    else{
        userNameElem.setAttribute("style","color:red;")
    }

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
        hubConnection.invoke("Enter", userName, userGroup);
    })
    .catch ((err) => console.error(err));
