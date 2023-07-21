
using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

namespace Gaos.ChatRoom.ChatRoom
{
    public class ExistsChatRoom
    {
        public readonly static string CLASS_NAME = typeof(ExistsChatRoom).Name;
        public static async UniTask<Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomResponse> CallAsync(string chatRoomName)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomRequest request = new Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomRequest();
                request.ChatRoomName = chatRoomName;
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/chatRoom/existsChatRoom", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error calling existsChatRoom");
                    throw new System.Exception("error calling existsChatRoom");
                }
                else
                {
                    Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomResponse>(apiCall.ResponseJsonStr);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }

    public class CreateChatRoom
    {
        public readonly static string CLASS_NAME = typeof(CreateChatRoom).Name;

        public static async UniTask<Gaos.Routes.Model.ChatRoomJson.CreateChatRoomResponse> CallAsync(string chatRoomName)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.ChatRoomJson.CreateChatRoomRequest request = new Gaos.Routes.Model.ChatRoomJson.CreateChatRoomRequest();
                request.ChatRoomName = chatRoomName;
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/chatRoom/createChatRoom", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error calling createChatRoom");
                    throw new System.Exception("error calling createChatRoom");
                }
                else
                {
                    Gaos.Routes.Model.ChatRoomJson.CreateChatRoomResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.ChatRoomJson.CreateChatRoomResponse>(apiCall.ResponseJsonStr);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class EnsureChatRoomExists
    {
        public readonly static string CLASS_NAME = typeof(EnsureChatRoomExists).Name;

        // Returns the chat room id if chat romm exists, otherwise returns -1
        public static async UniTask<int> CallAsync(string chatRoomName)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {
                Gaos.Routes.Model.ChatRoomJson.ExistsChatRoomResponse existsChatRoomResponse = await ExistsChatRoom.CallAsync(chatRoomName);

                if (existsChatRoomResponse == null)
                {
                    // Create new chat room
                    Gaos.Routes.Model.ChatRoomJson.CreateChatRoomResponse createChatRoomResponse = await CreateChatRoom.CallAsync(chatRoomName);
                    return createChatRoomResponse.ChatRoomId;
                }
                else
                {
                    return existsChatRoomResponse.ChatRoomId;
                }

            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }


    }
    public class ReadMessages
    {
        public readonly static string CLASS_NAME = typeof(ReadMessages).Name;
        public static async UniTask<Gaos.Routes.Model.ChatRoomJson.ReadMessagesResponse> CallAsync(int chatRoomId, int lastMessageId, int count)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.ChatRoomJson.ReadMessagesRequest request = new Gaos.Routes.Model.ChatRoomJson.ReadMessagesRequest();
                request.ChatRoomId = chatRoomId;
                request.LastMessageId = lastMessageId;
                request.Count = count;
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/chatRoom/readMessages", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error calling readMessages");
                    throw new System.Exception("error calling readMessages");
                }
                else
                {
                    Gaos.Routes.Model.ChatRoomJson.ReadMessagesResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.ChatRoomJson.ReadMessagesResponse>(apiCall.ResponseJsonStr);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }

    public class ReadMessagesBackwards
    {
        public readonly static string CLASS_NAME = typeof(ReadMessagesBackwards).Name;
        public static async UniTask<Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsResponse> CallAsync(int chatRoomId, int lastMessageId, int count)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsRequest request = new Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsRequest();
                request.ChatRoomId = chatRoomId;
                request.LastMessageId = lastMessageId;
                request.Count = count;
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/chatRoom/readMessagesBackwards", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error calling readMessagesBackwards");
                    throw new System.Exception("error calling readMessagesBackwards");
                }
                else
                {
                    Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsResponse>(apiCall.ResponseJsonStr);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }

    public class WriteMessage
    {
        public readonly static string CLASS_NAME = typeof(WriteMessage).Name;
        public static async UniTask<Gaos.Routes.Model.ChatRoomJson.WriteMessageResponse> CallAsync(int chatRoomId, string message)
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.ChatRoomJson.WriteMessageRequest request = new Gaos.Routes.Model.ChatRoomJson.WriteMessageRequest();
                request.ChatRoomId = chatRoomId;
                request.Message = message;
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/chatRoom/writeMessages", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error calling writeMessages");
                    throw new System.Exception("error calling writeMessages");
                }
                else
                {
                    Gaos.Routes.Model.ChatRoomJson.WriteMessageResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.ChatRoomJson.WriteMessageResponse>(apiCall.ResponseJsonStr);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }
}
