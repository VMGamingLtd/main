using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Friends
{
    public class FriendModel
    {
        public string UserName { get; set; }
        public string Status { get; set; }
    }

    public class FriendsManager : MonoBehaviour
    {
        //private static int MAX_SCROLL_LIST_LINES_CPUNT = 100;
        private static int MAX_SCROLL_LIST_LINES_CPUNT = 10;

        public GameObject FriendsButton;
        public Transform List; // scroll list containing friends buttons

        //public GameObject SearchTextBox;
        public TMP_InputField SearchTextBox;

        private FriendModel[] AllUsers = new FriendModel[MAX_SCROLL_LIST_LINES_CPUNT]; 
        private int LastIndexAllUsers = -1;

        private int[] FilteredUsers = new int[MAX_SCROLL_LIST_LINES_CPUNT];
        private int LastIndexFilteredUsers = -1;

        private GameObject[] AllFriendsButtons = new GameObject[MAX_SCROLL_LIST_LINES_CPUNT]; // all friends buttons in the scroll list
        private int LastIndexAllFriendsButtons = -1;

        FriendsManager()
        {
        }

        private async UniTaskVoid Init()
        {
            RemoveAllFriendsButtons();
            await ReadAllUsers();
            AllocateFriendsButtons();
            FilterUsers("");
            DisplayFilteredUsers();

            SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);

        }

        private async UniTask ReadAllUsers()
        {
            Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = await Gaos.Friends.Friends.GetUsersList.CallAsync();
            LastIndexAllUsers = -1;
            if (response == null)
            {
                return; 
            }

            for (int i = 0; i < response.Users.Length; i++)
            {

                // fill in friends array
                AllUsers[++LastIndexAllUsers] = new FriendModel {
                    UserName = response.Users[i].Name,
                    // TODO: get status
                    Status = "online",
                };

                // linit maximal number of users to be desplayed
                if (i + 1 == MAX_SCROLL_LIST_LINES_CPUNT)
                {
                    // TODO: maake backend return no more than MAX_SCROLL_LIST_LINES_CPUNT users
                    break;
                }
            }

        }

        private void AllocateFriendsButtons()
        {
            int n = (LastIndexAllUsers + 1) - (LastIndexAllFriendsButtons + 1);
            while (n > 0)
            {
                GameObject friendsButton = Instantiate(FriendsButton, List);
                friendsButton.SetActive(false);
                AllFriendsButtons[++LastIndexAllFriendsButtons] = friendsButton;
                --n;

            }
        }
        
        private void FilterUsers(string substring)
        {
            LastIndexFilteredUsers = -1;

            for (int i = 0; i <= LastIndexAllUsers; i++)
            {
                if (substring == "" || substring == null || AllUsers[i].UserName.Contains(substring))
                {
                    FilteredUsers[++LastIndexFilteredUsers] = i;
                }
            }
        }

        private void DisplayFilteredUsers()
        {
            for (int i = 0; i <= LastIndexFilteredUsers; i++)
            {
                int index = FilteredUsers[i];

                AllFriendsButtons[i].SetActive(true);

                Transform childObject_friendUsername = AllFriendsButtons[i].transform.Find("Info/FriendUsername");
                TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
                friendUsername.text = AllUsers[index].UserName;

                Transform childObject_friendStatus = AllFriendsButtons[i].transform.Find("Info/Status");
                TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
                friendStatus.text = AllUsers[index].Status;
            }

            for (int i = LastIndexFilteredUsers + 1; i <= LastIndexAllFriendsButtons; i++)
            {
                AllFriendsButtons[i].SetActive(false);
            }
        }


        public void RemoveAllFriendsButtons()
        {
            foreach (Transform child in List)
            {
                GameObject.Destroy(child.gameObject);
            }
            LastIndexAllFriendsButtons = -1;
        }

        public void OnSearchTextBoxChange(string text)
        {
            FilterUsers(text);
            DisplayFilteredUsers();
        }

        public void OnEnable()
        {
            Init().Forget();
        }

        public void OnDisable()
        {
            SearchTextBox.onValueChanged.RemoveListener(OnSearchTextBoxChange);

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







