using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaos.WebSocket;
using UnityEngine;

namespace Gaos.Messages.GaosServer
{
    public class GroupCreditsChange
    {
        private static string CLASS_NAME = typeof(GroupCreditsChange).Name;

        public static void receive(IWebSocketClient ws, GaoProtobuf.MessageHeader header,  byte[] message)
        {
            const string METHOD_NAME = "receive()";
            GaoProtobuf.gaos.GroupCreditsChange groupCreditsChange;
            try
            {
                groupCreditsChange = GaoProtobuf.gaos.GroupCreditsChange.Parser.ParseFrom(message);
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: groupCreditsChange: group: {header.GroupId}, user: {header.FromId}, credits: {groupCreditsChange.Credits}");
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }
        }
    }
}
