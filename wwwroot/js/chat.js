const connection = new signalR.HubConnectionBuilder()
  .withUrl("/chathub")
  .withAutomaticReconnect()
  .build();

const messagesList = document.getElementById('messagesList');
const sendButton = document.getElementById('sendButton');
const userInput = document.getElementById('userInput');
const messageInput = document.getElementById('messageInput');

connection.on("ReceiveMessage", (user, message, time) => {
  const li = document.createElement('li');
  li.textContent = `[${time}] ${user}: ${message}`;
  messagesList.appendChild(li);
});

sendButton.addEventListener('click', async () => {
  const user = userInput.value || 'Anon';
  const msg = messageInput.value || '';
  if(!msg) return;
  messageInput.value = '';
  try {
    await connection.invoke("SendMessage", user, msg);
  } catch (e) {
    console.error(e);
  }
});

async function start(){
  try{
    await connection.start();
    console.log('connected');
  }catch(err){
    console.error('retry in 2s', err);
    setTimeout(start,2000);
  }
}
start();
