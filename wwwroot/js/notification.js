navigator.serviceWorker.addEventListener('message', event => {
    console.log(event);
    if (event.data === 'newData') {
      showData();
    }
  });

  
(async ()=>{ 
   await initNotification();
})();
  
async function initNotification() {
  await navigator.serviceWorker.ready;  
  initialiseFirebaseApp();
  const messaging = await setUpFirebaseMessagingService();
  await requestPermission(messaging);
  await sendToken(messaging);
}

function initialiseFirebaseApp() {
  firebase.initializeApp({
    apiKey: "AIzaSyD24oaukrvdsx9lRqarKcg2SC7hV886vac",
    projectId: "medieval-warfare",
    messagingSenderId: "754418426163",
    appId: "1:754418426163:web:3f222422be5ee60f5d9e46"
  });
}

async function setUpFirebaseMessagingService() {
  const registration = await navigator.serviceWorker.register('/service-worker.js');
  const messaging = firebase.messaging();
  messaging.usePublicVapidKey('BJlaPdeE3mUReTWUgbr6iLkaKfcQ4UsyXup0xVXKm9n1xBCEHb5G5rSyufNCmtaZiyiZq-26ZKWeQ92vD2rTzec');
  messaging.useServiceWorker(registration);
  return messaging;
}

async function requestPermission(messaging){
  try {
    await messaging.requestPermission();
  } catch (e) {
    console.log('Unable to get permission', e);
    return;
  }
}

async function sendToken(messaging) {
  const currentToken = await messaging.getToken();
  console.log('currentToken : ' + currentToken);
  fetch('https://medieval-warfare.herokuapp.com/api/register', { method: 'post', body: currentToken });
  showData();
  
  messaging.onTokenRefresh(async () => {
    console.log('token refreshed');
    const newToken = await messaging.getToken();
    fetch('https://medieval-warfare.herokuapp.com/api/register', { method: 'post', body: currentToken });
  });
}