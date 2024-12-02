#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GroupJson
{
    [System.Serializable]
    public class GetUsersListRequest
    {
       public  string FilterUserName;
       public  int MaxCount;
    }
}
