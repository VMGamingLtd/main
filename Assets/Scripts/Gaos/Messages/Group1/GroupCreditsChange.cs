using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaos.WebSocket;
using UnityEngine;

namespace Gaos.Messages.Group1
{
    public class GroupCreditsChange
    {
        private static string CLASS_NAME = typeof(GroupCreditsChange).Name;

        // Define a delegate for the event
        public delegate void GroupCreditsChangeHandler(int groupId, int userId, float credits);

        // Define the event
        public static event GroupCreditsChangeHandler OnGroupCreditsChange;

        public static void receive(IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "receive()";
            GaoProtobuf.gaos.GroupCreditsChange groupCreditsChange;
            try
            {
                groupCreditsChange = GaoProtobuf.gaos.GroupCreditsChange.Parser.ParseFrom(message);
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: groupCreditsChange: group: {header.GroupId}, user: {header.FromId}, credits: {groupCreditsChange.Credits}");
                Credits.credits = groupCreditsChange.Credits;

                // Trigger the event
                OnGroupCreditsChange?.Invoke(header.GroupId, header.FromId, groupCreditsChange.Credits);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }
        }
    }
}
