
function GAO_WebSocketCreate(GAOS_WS) {
  if (!window.GAO_WEB_SOCKETS) {
    window.GAO_WEB_SOCKETS = [];
  }
  var ws = new WebSocket(Pointer_stringify(GAOS_WS));
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3000: GAO_WebSocketCreate: ' + GAOS_WS);
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3010: GAO_WebSocketCreate: ' + ws);
  window.GAO_WEB_SOCKETS.push(ws);
  return window.GAO_WEB_SOCKETS.length - 1;
}

function GAO_WebSocketOnOpen(ws, fnName) {
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3020: GAO_WebSocketOnOpen: ' + ws);
  window.GAO_WEB_SOCKETS[ws].addEventListener('open', function (event) {
    console.info('websocket opend');
    window.unityInstance.SendMessage('WebSocketClientJs', fnName);
  })
}

function GAO_WebSocketOnClose(ws, fnName) {
  window.GAO_WEB_SOCKETS[ws].addEventListener('close', function (event) {
    console.info('websocket closed');
    window.unityInstance.SendMessage('WebSocketClientJs', fnName);
  })
}

function GAO_WebSocketOnError(ws, fnName, errorStr) {
  window.GAO_WEB_SOCKETS[ws].addEventListener('error', function (event) {
    console.error('websocket error:', err);
    window.unityInstance.SendMessage('WebSocketClientJs', fnName, event.data.toString());
  })
}

function GAO_WebSocketOnMessage(ws, fnName) {
  window.GAO_WEB_SOCKETS[ws].addEventListener('message', function (event) {
    if (typeof event.data === 'string') {
      window.unityInstance.SendMessage('WebSocketClientJs', fnname, event.data);
    } else if (event.data instanceof ArrayBuffer || event.data instanceof Blob) {
      console.error('websocket - received binary data, binary data not supported');
    } else {
      console.error('websocket - received data of unknown format');
    }
  })
}

function GAO_WebSocketSend(ws, data) {
  if (typeof data !== 'string') {
    console.error('websocket - only string data supported');
    throw new Error('only string data supported')
  }
  var str = Pointer_stringify(data)
  window.GAO_WEB_SOCKETS[ws].send(str);
}

mergeInto(LibraryManager.library, {

  WebSocketCreate: GAO_WebSocketCreate,

  WebSocketOnOpen: GAO_WebSocketOnOpen,

  WebSocketOnClose: GAO_WebSocketOnClose,

  WebSocketOnError: GAO_WebSocketOnError,

  WebSocketOnMessage: GAO_WebSocketOnMessage,

  WebSockeSend: GAO_WebSockeSend,

});
