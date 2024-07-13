function UnityBrowserChannel_BaseMessages_receiveString(str) {
    window.unityInstance.SendMessage('WebSocketClientJs', 'UnityBrowserChannel_BaseMessages_receiveString', str);
}

function UnityBrowserChannel_init() {
  if (!window.GAO_UnityBrowserChannel) {
    window.GAO_UnityBrowserChannel = {};
  }
  if (!window.GAO_UnityBrowserChannel.BaseMessages) {
    window.GAO_UnityBrowserChannel.BaseMessages = {}
  }
  if (!window.GAO_UnityBrowserChannel.BaseMessages.sendString) {
      window.GAO_UnityBrowserChannel.BaseMessages.sendString = UnityBrowserChannel_BaseMessages_receiveString;
  }
}

