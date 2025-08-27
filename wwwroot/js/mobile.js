const connection=new signalR.HubConnectionBuilder().withUrl("/chathub").withAutomaticReconnect().build();
const messagesList=document.getElementById('messagesList');
const sendButton=document.getElementById('sendButton');
const userInput=document.getElementById('userInput');
const messageInput=document.getElementById('messageInput');
connection.on("ReceiveMessage",(user,message,time)=>{const div=document.createElement('div');div.className='message';div.textContent=`[${time}] ${user}: ${message}`;if(user===userInput.value){div.classList.add('self');}else{div.classList.add('other');}messagesList.appendChild(div);messagesList.scrollTop=messagesList.scrollHeight;});
sendButton.addEventListener('click',async()=>{const user=userInput.value||'Anon';const msg=messageInput.value;if(!msg)return;messageInput.value='';try{await connection.invoke("SendMessage",user,msg);}catch(err){console.error(err);}});
async function start(){try{await connection.start();console.log('Connected to SignalR hub');}catch(err){console.error('Connection failed, retrying...',err);setTimeout(start,2000);}}start();
