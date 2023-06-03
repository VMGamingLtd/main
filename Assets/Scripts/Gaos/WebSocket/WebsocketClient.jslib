let webSockets = []

mergeInto(LibraryManager.library, {

  WebSocketCreate: function (GAOS_WS) {

    var ws = new WebSocket(Pointer_stringify(GAOS_WS));
    webSockets.push(ws);
    return ws.length;
  }

  WebSocketOnOpen: function (ws, fnName) {
    webSockets[ws].addEventListener('open' (event) => {
      console.info(`websocket ${ws} - opend`);
      unityInstance.SendMessage('WebSocketClientJs',fnName);
    })
  }

  WebSocketOnClose: function (ws, fnName) {
    webSockets[ws].addEventListener('close' (event) => {
      console.info(`websocket ${ws} - closed`);
      unityInstance.SendMessage('WebSocketClientJs',fnName);
    })
  }

  WebSocketOnError: function (ws, fnName, errorStr) {
    webSockets[ws].addEventListener('error' (event) => {
      console.error(`websocket ${ws} - error:`, err);
      unityInstance.SendMessage('WebSocketClientJs',fnName, event.data.toString());
    })
  }

  WebSocketOnMessage: function (ws, fnName) {
    webSockets[ws].addEventListener('message' (event) => {
      if (typeof event.data === 'string') {
        unityInstance.SendMessage('WebSocketClientJs', fnname, event.data);
      } else if (event.data instanceof ArrayBuffer || event.data instanceof Blob) {
        console.error(`websocket ${ws} - received binary data, binary data not supported`);
      } else {
        console.error(`websocket ${ws} - received data of unknown format`);
      }
    })
  }

  WebSockeSend: function (ws, data) {
    if (typeof data !== 'string') {
      console.error(`websocket ${ws} - only string data supported`);
      throw new Error('only string data supported')
    }
    let str = Pointer_stringify(data)
    webSockets[ws].send(str);
  }

});
