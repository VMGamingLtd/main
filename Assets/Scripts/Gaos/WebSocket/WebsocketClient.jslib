
function GAO_WebSocketCreate(GAOS_WS, fnNameOnOpen, fnNameOnClose, fnNameOnError, fnNameOnMessage) {
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
    console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2000: ${ws}, readyState ${websocket.readyState}`);
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnOpenStr);
  })

  websocket.addEventListener('close', function (event) {
    console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2100: ${ws} readyState ${websocket.readyState}`);
    console.info('websocket closed');
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnCloseStr);
    delete window.GAO_WEB_SOCKETS.websockets[ws];
  })


  websocket.addEventListener('error', function (event) {
    console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2110: ${ws} readyState ${websocket.readyState}`);
    console.error('websocket error:', err);
    window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnErrorStr, "error");
  })

  websocket.addEventListener('message', function (event) {
    console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2115: ${ws} readyState ${websocket.readyState}`);
    if (typeof event.data === 'string') {
      console.error('websocket - received text data, text data not supported');
      window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnMessageStr, event.data);
    } else if (event.data instanceof ArrayBuffer) {
      var binaryData = new Uint8Array(event.data);

      // Allocate memory on emcscripten heap (Emcscripten heap is just javascript ArrayBuffer allocated and passed to module on startup,
      // Module.HEAPU32 is just Uint32Array view on that ArrayBuffer - 'Module.HEAPU32 = new Uint32Array(arrayBuffer)'). 
      // We are allocating memory on emcscripten for the binary data and the length of the binary data.
      // _malloc() always returns memory alligned to the biggest word for the architecture which should be 8 bytes word for int64 which
      // guarantees that memory will be always properly alligned for any smaller words like int32, in16, int8).
      // We are using the first 4 bystes for passing the length of the binary data which follows after.
      var bufferPtr = Module._malloc(Uint32Array.BYTES_PER_ELEMENT + binaryData.length * binaryData.BYTES_PER_ELEMENT);
      // write the length of the binary data to the buffer
      Module.HEAPU32.set([binaryData.length], bufferPtr);
      // write the binary data to the buffer
      Module.HEAPU8.set(binaryData, bufferPtr + Uint32Array.BYTES_PER_ELEMENT);

      window.unityInstance.SendMessage('WebSocketClientJs', fnNameOnMessageStr, bufferPtr);

      Module._free(bufferPtr);
    } else {
      console.error('websocket - received data of unknown format');
    }
  })

  console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3000: opened ws ${ws}`);
  return ws;
}

function GAO_WebSocketSend(ws, data, length) {
  var websocket = window.GAO_WEB_SOCKETS.websockets[ws];
  console.log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2250: send");
  if (websocket) {
    console.log(`@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2120: ${ws} readyState ${websocket.readyState}`);
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

mergeInto(LibraryManager.library, {

  WebSocketCreate: GAO_WebSocketCreate,

  WebSocketSend: GAO_WebSocketSend,

  WebSocketReadyState: GAO_WebSocketReadyState,

});
