using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;

namespace Friends
{
    public class FriendRequestModel
    {
        public int GroupId { get; set; }
        public int GroupOwnerId { get; set; }
        public string GroupOwnerName { get; set; }
    }

    public class FriendsTabFriendsRequestsManager : MonoBehaviour
    {
        private static string CLASS_NAME = typeof(FriendsTabFriendsRequestsManager).Name;

        //private static int MAX_SCROLL_LIST_LINES_CPUNT = 100;
        private static int MAX_SCROLL_LIST_LINES_COUNT = 10;

        public GameObject FriendRequestButton;
        public Transform List; // scroll list containing friend reuest buttons



        //public GameObject SearchTextBox;
        public TMP_InputField SearchTextBox;

        private FriendRequestModel[] AllRequests = new FriendRequestModel[MAX_SCROLL_LIST_LINES_COUNT]; 
        private int LastIndexAllRequests = -1;

        private int[] FilteredRequessts = new int[MAX_SCROLL_LIST_LINES_COUNT];
        private int LastIndexFilteredUsers = -1;

        private GameObject[] AllRequestsButtons = new GameObject[MAX_SCROLL_LIST_LINES_COUNT]; // all friend request buttons in the scroll list
        private int LastIndexAllRequestsButtons = -1;

        FriendsTabFriendsRequestsManager()
        {
        }

        private async UniTaskVoid Init()
        {
            bool isOnSearchTextBoxChange = false;

            RemoveAllFriendRequestButtons();
            await ReadAllRequests();
            AllocateFriendRequestButtons();
            FilterRequests("");
            DisplayFilteredRequests();

            if (!isOnSearchTextBoxChange)
            {
                SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);
            }

        }

        private async UniTask ReadAllRequests(string frienNameSubstring = null)
        {
            Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse response = await Gaos.Friends.Friends.GetFriendRequests.CallAsync(MAX_SCROLL_LIST_LINES_COUNT, frienNameSubstring);
            if (response == null)
            {
                // error occured
                return; 
            }
            LastIndexAllRequests = -1;
            if (response == null)
            {
                return; 
            }

            for (int i = 0; i < response.FriendRequests.Count; i++)
            {

                // fill in friends array
                AllRequests[++LastIndexAllRequests] = new FriendRequestModel {
                    GroupId = response.FriendRequests[i].GroupId,
                    GroupOwnerId = response.FriendRequests[i].GroupOwnerId,
                    GroupOwnerName = response.FriendRequests[i].GroupOwnerName,
                };

                // linit maximal number of users to be desplayed
                if (i + 1 == MAX_SCROLL_LIST_LINES_COUNT)
                {
                    // TODO: maake backend return no more than MAX_SCROLL_LIST_LINES_CPUNT users
                    break;
                }
            }

        }

        private void AllocateFriendRequestButtons()
        {
            int n = (LastIndexAllRequests + 1) - (LastIndexAllRequestsButtons + 1);
            while (n > 0)
            {
                GameObject friendsButton = Instantiate(FriendRequestButton, List);
                friendsButton.transform.position = Vector3.zero;
                friendsButton.SetActive(false);
                AllRequestsButtons[++LastIndexAllRequestsButtons] = friendsButton;
                --n;

            }
        }
        
        private void FilterRequests(string substring)
        {
            LastIndexFilteredUsers = -1;

            for (int i = 0; i <= LastIndexAllRequests; i++)
            {
                if (substring == "" || substring == null || AllRequests[i].GroupOwnerName.Contains(substring))
                {
                    FilteredRequessts[++LastIndexFilteredUsers] = i;
                }
            }
        }


        private void DisplayFilteredRequests()
        {
            
            for (int i = 0; i <= LastIndexFilteredUsers; i++)
            {
                int index = FilteredRequessts[i];
                FriendRequestModel friendRequest = AllRequests[index];

                AllRequestsButtons[i].SetActive(true);

                Transform childObject_friendUsername = AllRequestsButtons[i].transform.Find("Info/FriendUsername");
                TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
                friendUsername.text = friendRequest.GroupOwnerName;

                Transform childObject_friendStatus = AllRequestsButtons[i].transform.Find("Info/Status");
                TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
                friendStatus.text = "status_foo";
            }

            for (int i = LastIndexFilteredUsers + 1; i <= LastIndexAllRequestsButtons; i++)
            {
                AllRequestsButtons[i].SetActive(false);
            }
        }


        public void RemoveAllFriendRequestButtons()
        {
            foreach (Transform child in List)
            {
                GameObject.Destroy(child.gameObject);
            }
            LastIndexAllRequestsButtons = -1;
        }


        public void OnSearchTextBoxChange(string text)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 455: OnSearchTextBoxChange(): {text}");
            FilterRequests(text);
            DisplayFilteredRequests();
        }

        public void OnEnable()
        {
            Init().Forget();
        }

        public void OnDisable()
        {
            SearchTextBox.onValueChanged.RemoveListener(OnSearchTextBoxChange);

        }

        private async UniTask  GuiReadAllUsersList(string userNamePattern)
        {
            RemoveAllFriendRequestButtons();
            await ReadAllRequests(userNamePattern);
            AllocateFriendRequestButtons();
            FilterRequests("");
            DisplayFilteredRequests();
        }

        private async UniTaskVoid OnSearchIconClickAsync(string userNamePattern)
        {
            await GuiReadAllUsersList(userNamePattern);
        }


        public void OnSearchIconClick()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 400: OnSearchIconClick(): {SearchTextBox.text}");
            OnSearchIconClickAsync(SearchTextBox.text).Forget();
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







