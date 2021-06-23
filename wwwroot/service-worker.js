// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
console.log("service-worker");
self.addEventListener('fetch', () => { });


importScripts('https://www.gstatic.com/firebasejs/7.10.0/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/7.10.0/firebase-messaging.js');

firebase.initializeApp({
  apiKey: "AIzaSyD24oaukrvdsx9lRqarKcg2SC7hV886vac",
  projectId: "medieval-warfare",
  messagingSenderId: "754418426163",
  appId: "1:754418426163:web:3f222422be5ee60f5d9e46"
});

const messaging = firebase.messaging();
messaging.usePublicVapidKey('BJlaPdeE3mUReTWUgbr6iLkaKfcQ4UsyXup0xVXKm9n1xBCEHb5G5rSyufNCmtaZiyiZq-26ZKWeQ92vD2rTzec');