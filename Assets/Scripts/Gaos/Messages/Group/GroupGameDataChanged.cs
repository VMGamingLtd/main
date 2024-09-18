using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gaos.Messages.Group
{
    public class GroupGameDataChanged
    {
        public static string CLASS_NAME = typeof(GroupGameDataChanged).Name;

        public static void receive(Gaos.WebSocket.IWebSocketClient ws, byte[] message)
        {
            const string METHOD_NAME = "receive()";
            GaoProtobuf.GroupDataChanged groupDataChanged;
            try
            {
                groupDataChanged = GaoProtobuf.GroupDataChanged.Parser.ParseFrom(message);
                OnGroupDataChanged?.Invoke(groupDataChanged); // invoke all listeners
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }
        }

        public delegate void GroupDataChangedHandler(GaoProtobuf.GroupDataChanged message);
        private static event GroupDataChangedHandler OnGroupDataChanged;

        public static void RegisterListener(GroupDataChangedHandler listener)
        {
            OnGroupDataChanged += listener;
        }

        public static void DeregisterListener(GroupDataChangedHandler listener)
        {
            OnGroupDataChanged -= listener;
        }
    }
}
