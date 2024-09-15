namespace Gaos.Routes.Model.FriendsJson
{

    [System.Serializable]
    public class GetMyFriendsRequest
    {
        public int GroupId;
        public  int MaxCount;
        public bool IsCountOnly;
    }
}
