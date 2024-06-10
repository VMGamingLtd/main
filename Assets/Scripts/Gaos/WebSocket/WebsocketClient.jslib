
function GAO_WebSocketCreate(GAOS_WS) {
  if (!window.GAO_WEB_SOCKETS) {
    window.GAO_WEB_SOCKETS = [];
  }
  var ws = new WebSocket(UTF8ToString(GAOS_WS));
  ws.binaryType = "arraybuffer";
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3000: GAO_WebSocketCreate: ' + GAOS_WS);
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3010: GAO_WebSocketCreate: ' + ws);
  window.GAO_WEB_SOCKETS.push(ws);
  return window.GAO_WEB_SOCKETS.length - 1;
}

function GAO_WebSocketOnOpen(ws, fnName) {
  fnName = UTF8ToString(fnName);
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3020: GAO_WebSocketOnOpen: ' + ws);
  window.GAO_WEB_SOCKETS[ws].addEventListener('open', function (event) {
    console.info('websocket opend');
  console.log('@@@@@@@@@@@@@@@@@@@@@ cp 3030: ' + fnName);
    window.unityInstance.SendMessage('WebSocketClientJs', fnName);
  })
}

function GAO_WebSocketOnClose(ws, fnName) {
  fnName = UTF8ToString(fnName);
  window.GAO_WEB_SOCKETS[ws].addEventListener('close', function (event) {
    console.info('websocket closed');
    window.unityInstance.SendMessage('WebSocketClientJs', fnName);
  })
}

function GAO_WebSocketOnError(ws, fnName, errorStr) {
  fnName = UTF8ToString(fnName);
  window.GAO_WEB_SOCKETS[ws].addEventListener('error', function (event) {
    console.error('websocket error:', err);
    window.unityInstance.SendMessage('WebSocketClientJs', fnName, event.data.toString());
  })
}

function GAO_WebSocketOnMessage(ws, fnName) {
  fnName = UTF8ToString(fnName);
  window.GAO_WEB_SOCKETS[ws].addEventListener('message', function (event) {
    if (typeof event.data === 'string') {
      console.error('websocket - received text data, text data not supported');
      window.unityInstance.SendMessage('WebSocketClientJs', fnName, event.data);
    } else if (event.data instanceof ArrayBuffer) {
      var binaryData = new Uint8Array(event.data);

      // Allocate memory on emcscripten heap (Emcscripten heap is just javascript ArrayBuffer allocated and passed to module on startup,
      // Module.HEAPU32 is just Uint32Array view on that ArrayBuffer - 'Module.HEAPU32 = new Uint32Array(arrayBuffer)'). 
      // We are allocating memory on emcscripten for the binary data and the length of the binary data.
      // _malloc() always returns memory alligned to the biggest word for the architecture which should be 8 bytes word for int64 which
      // guarantees that memory will be always properly alligned for any smaller words line int32, in16, int8).
      // We are using the first 4 bystes for passing the length of the binary data which follows after.
      var bufferPtr = Module._malloc(Uint32Array.BYTES_PER_ELEMENT +  binaryData.length * binaryData.BYTES_PER_ELEMENT);
      // write the length of the binary data to the buffer
      Module.HEAPU32.set([binaryData.length], bufferPtr);
      // write the binary data to the buffer
      Module.HEAPU8.set(binaryData, bufferPtr + Uint32Array.BYTES_PER_ELEMENT);

      window.unityInstance.SendMessage('WebSocketClientJs', fnName, bufferPtr);

      Module._free(bufferPtr);
    } else {
      console.error('websocket - received data of unknown format');
    }
  })
}

function GAO_WebSocketSend(ws, data, length) {
  var binaryData = new Uint8Array(Module.HEAPU8.buffer, data, length);
  window.GAO_WEB_SOCKETS[ws].send(binaryData);
}

mergeInto(LibraryManager.library, {

  WebSocketCreate: GAO_WebSocketCreate,

  WebSocketOnOpen: GAO_WebSocketOnOpen,

  WebSocketOnClose: GAO_WebSocketOnClose,

  WebSocketOnError: GAO_WebSocketOnError,

  WebSocketOnMessage: GAO_WebSocketOnMessage,

  WebSocketSend: GAO_WebSocketSend,

});
