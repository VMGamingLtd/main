function GAO_WebSocketCreate(GAOS_WS, fnNameOnOpen, fnNameOnClose, fnNameOnError, fnNameOnMessage) {
  UnityBrowserChannel_init();

  var fnNameOnOpenStr = UTF8ToString(fnNameOnOpen); 
  var fnNameOnCloseStr = UTF8ToString(fnNameOnClose);
  var fnNameOnErrorStr = UTF8ToString(fnNameOnError);
  var fnNameOnMessageStr = UTF8ToString(fnNameOnMessage);

  if (!window.GAO_WEB_SOCKETS) {
    window.GAO_WEB_SOCKETS = {
      lastN: 0,
      websockets: {}
    }
  }

  var websocket = new WebSocket(UTF8ToString(GAOS_WS));
  websocket.binaryType = "arraybuffer";
  window.GAO_WEB_SOCKETS.lastN += 1;
  var ws = window.GAO_WEB_SOCKETS.lastN;
  window.GAO_WEB_SOCKETS.websockets[ws] = websocket;

  websocket.addEventListener('open', function (event) {
    console.info('websocket opened');
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnOpenStr);
  })

  websocket.addEventListener('close', function (event) {
    console.info('websocket closed');
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnCloseStr);
    delete window.GAO_WEB_SOCKETS.websockets[ws];
  })


  websocket.addEventListener('error', function (event) {
    console.error('websocket error:', err);
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnErrorStr, "error");
  })

  websocket.addEventListener('message', function (event) {
    if (typeof event.data === 'string') {
      console.error('websocket - received text data, text data not supported');
      window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnMessageStr, event.data);
    } else if (event.data instanceof ArrayBuffer) {
      var binaryData = new Uint8Array(event.data);

      // base64 encode the binary data
      let binaryString = '';
      const len = binaryData.byteLength;
      for (let i = 0; i < len; i++) {
          binaryString += String.fromCharCode(binaryData[i]);
      }
      let binaryString64 = window.btoa(binaryString);
      window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnMessageStr, binaryString64);
    }
  })

  return ws;
}

function GAO_WebSocketSend(ws, data, length) {
  var websocket = window.GAO_WEB_SOCKETS.websockets[ws];
  if (websocket) {
    var binaryData = new Uint8Array(Module.HEAPU8.buffer, data, length);
    websocket.send(binaryData);
  }
}

function GAO_WebSocketReadyState(ws) {
  var websocket = window.GAO_WEB_SOCKETS.websockets[ws];
  if (websocket) {
    return websocket.readyState;
  }
  else
  {
    return 3; // CLOSED
  }
}

function GAO_WebSocketClose(ws) {
  var websocket = window.GAO_WEB_SOCKETS.websockets[ws];
  if (websocket) {
    websocket.close();
  }
}

function GAO_UnityBrowserChannel_BaseMessages_sendString(_str) {
  var str = UTF8ToString(_str); 
  if (window.GAO_UnityBrowserChannel) {
    if (window.GAO_UnityBrowserChannel.BaseMessages) {
      if (window.GAO_UnityBrowserChannel.BaseMessages.receiveString) {
        window.GAO_UnityBrowserChannel.BaseMessages.receiveString(str);
      } else {
        console.error('WebsocketClientJs:GAO_UnityBrowserChannel_BaseMessages_sendString():  GAO_UnityBrowserChannel.BaseMessages.receiveString not found');
      }
    } else {
      console.error('WebsocketClientJs:GAO_UnityBrowserChannel_BaseMessages_sendString():  GAO_UnityBrowserChannel.BaseMessages not found');
    }
  } else {
    console.error('WebsocketClientJs:GAO_UnityBrowserChannel_BaseMessages_sendString():  GAO_UnityBrowserChannel not found');
  }
}

mergeInto(LibraryManager.library, {

  WebSocketCreate: GAO_WebSocketCreate,
  WebSocketSend: GAO_WebSocketSend,
  WebSocketReadyState: GAO_WebSocketReadyState,
  WebSocketClose: GAO_WebSocketClose,

  UnityBrowserChannel_BaseMessages_sendString: GAO_UnityBrowserChannel_BaseMessages_sendString,
});
