using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Gao.Friends
{
    public class FriendModel
    {
        public string UserName { get; set; }
        public string Status { get; set; }
    }

    public class FriendsManager : MonoBehaviour
    {
        private IDataSource<FriendModel> DataSource = null;
        private FriendNameFilter Filter = null;

        public GameObject FriendsButton;
        public Transform List;

        //public GameObject SearchTextBox;
        public TMP_InputField SearchTextBox;

        private FriendModel[] AllUsers = new FriendModel[0];


        FriendsManager()
        {
        }

        private async UniTask<FriendModel[]> ReadAllUsers()
        {
            Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = await Gaos.Friends.Friends.GetUsersList.CallAsync();
            if (response == null)
            {
                return new FriendModel[] { };
            }

            FriendModel[] friends = new FriendModel[response.Users.Length];
            for (int i = 0; i < response.Users.Length; i++)
            {
                // fill in friends array
                friends[i] = new FriendModel {
                    UserName = response.Users[i].Name,
                    // TODO: get status from response
                    Status = "online",
                };
            }

            return friends;
        }



        public void AddFriend(FriendModel friend)
        {
            GameObject clonedButton = Instantiate(FriendsButton, List);
            Transform childObject_friendUsername = clonedButton.transform.Find("Info/FriendUsername");
            TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
            friendUsername.text = friend.UserName;

            Transform childObject_friendStatus = clonedButton.transform.Find("Info/Status");
            TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
            friendStatus.text = friend.Status;
        }

        public void RemoveAllFriends()
        {
            foreach (Transform child in List)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        public void OnEnable()
        {
            SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);
        }

        public void OnSearchTextBoxChange(string text)
        { 
        }




    }

}



/*
        this.Friends = new FriendModel[13]
        {
            new FriendModel { UserName = "Anderson", Status = "Online" },
            new FriendModel { UserName = "Bennett", Status = "Offline" },
            new FriendModel { UserName = "Carter", Status = "Offline" },
            new FriendModel { UserName = "Davis", Status = "Online" },
            new FriendModel { UserName = "Evans", Status = "Offline" },
            new FriendModel { UserName = "Foster", Status = "Offline" },
            new FriendModel { UserName = "Garcia", Status = "Online" },
            new FriendModel { UserName = "Harrison", Status = "Online" },
            new FriendModel { UserName = "Johnson", Status = "Online" },
            new FriendModel { UserName = "Mitchell_1", Status = "Online" },
            new FriendModel { UserName = "Mitchell_2", Status = "Online" },
            new FriendModel { UserName = "Mitchell_3", Status = "Online" },
            new FriendModel { UserName = "Mitchell_4", Status = "Online" },
        };
*/







