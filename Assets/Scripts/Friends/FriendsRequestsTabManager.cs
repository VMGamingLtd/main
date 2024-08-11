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
        public string UserName { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public bool? IsFriend { get; set; }
    }

    public class FriendsRequestsTabManager : MonoBehaviour
    {
        private static string CLASS_NAME = typeof(FriendsRequestsTabManager).Name;

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

        FriendsRequestsTabManager()
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
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp200 ReadAllUsers()");
            Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = await Gaos.Friends.Friends.GetUsersList.CallAsync(frienNameSubstring, MAX_SCROLL_LIST_LINES_COUNT);
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

            for (int i = 0; i < response.Users.Length; i++)
            {

                // fill in friends array
                AllRequests[++LastIndexAllRequests] = new FriendRequestModel {
                    UserId = response.Users[i].Id,
                    UserName = response.Users[i].Name,
                    IsFriend = response.Users[i].IsFriend,
                    // TODO: get status
                    Status = "online",
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
                if (substring == "" || substring == null || AllRequests[i].UserName.Contains(substring))
                {
                    FilteredRequessts[++LastIndexFilteredUsers] = i;
                }
            }
        }


        private void DisplayFilteredRequests()
        {
            string friendIndicator;
            // translate word "friend" to the current language
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    friendIndicator = "friend";
                    break;
                case SystemLanguage.Russian:
                    friendIndicator = "друг";
                    break;
                case SystemLanguage.Chinese:
                    friendIndicator = "朋友";
                    break;
                case SystemLanguage.Slovak:
                    friendIndicator = "priateľ";
                    break;
                default:
                    friendIndicator = "friend";
                    break;
            }
            
            for (int i = 0; i <= LastIndexFilteredUsers; i++)
            {
                int index = FilteredRequessts[i];
                FriendRequestModel user = AllRequests[index];

                AllRequestsButtons[i].SetActive(true);

                Transform childObject_friendUsername = AllRequestsButtons[i].transform.Find("Info/FriendUsername");
                TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
                if (user.IsFriend == true)
                {
                    friendUsername.text = $"{user.UserName} ({friendIndicator})";
                }
                else
                {
                    friendUsername.text = user.UserName;
                }

                Transform childObject_friendStatus = AllRequestsButtons[i].transform.Find("Info/Status");
                TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
                friendStatus.text = user.Status;
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

        public void SetTitlePaneDisplay()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 300: SetTitlePaneDisplay(): {LastIndexFilteredUsers + 1}");
            if (LastIndexFilteredUsers == 0)
            {
                FriendRequestModel user = AllRequests[FilteredRequessts[0]];   
                if (user.IsFriend == false)
                {
                    if (user.UserId != Gaos.Context.Authentication.GetUserId())
                    {
                    }
                    else
                    {
                        // logged in user cannt add himself as a friend
                    }
                }
                else
                {
                }
            }
            else
            {
            }
        }

        public void OnSearchTextBoxChange(string text)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 455: OnSearchTextBoxChange(): {text}");
            FilterRequests(text);
            DisplayFilteredRequests();
            SetTitlePaneDisplay();
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
            /*
            RemoveAllFriendsButtons();
            await ReadAllUsers(userNamePattern);
            AllocateFriendsButtons();
            FilterUsers("");
            DisplayFilteredUsers();
            */

            await GuiReadAllUsersList(userNamePattern);

            SetTitlePaneDisplay();

        }


        public async UniTaskVoid OnAddFriendDialogButtonYesAsync()
        {
            var response = await Gaos.Friends.Friends.AddFriend.CallAsync(AllRequests[FilteredRequessts[0]].UserId);
            SearchTextBox.text = "";
            await GuiReadAllUsersList("");
        }

        public void OnAddFriendDialogButtonYes()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 900: OnAddFriendDialogButtonYes()");
            OnAddFriendDialogButtonYesAsync().Forget();
        }

        public async UniTaskVoid OnAddFriendDialogButtonNoAsync()
        {
            SearchTextBox.text = "";
            await GuiReadAllUsersList("");
        }

        public void OnAddFriendDialogButtonNo()
        {
            OnAddFriendDialogButtonNoAsync().Forget();
        }

        public async UniTaskVoid OnRemoveFriendDialogButtonYesAsync()
        {
            var response = await Gaos.Friends.Friends.RemoveFriend.CallAsync(AllRequests[FilteredRequessts[0]].UserId);
            SearchTextBox.text = "";
            await GuiReadAllUsersList("");
        }

        public void OnRemoveFriendDialogButtonYes()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 910: OnRemoveFriendDialogButtonYes()");
            OnRemoveFriendDialogButtonYesAsync().Forget();
        }

        public async UniTaskVoid OnRemoveFriendDialogButtonNoAsync()
        {
            SearchTextBox.text = "";
            await GuiReadAllUsersList("");
        }

        public void OnRemoveFriendDialogButtonNo()
        {
            OnRemoveFriendDialogButtonNoAsync().Forget(); 
        }


        public void OnSearchIconClick()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 400: OnSearchIconClick(): {SearchTextBox.text}");
            OnSearchIconClickAsync(SearchTextBox.text).Forget();
        }

        public void OnAddFriendButtonClick()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 500: OnAddFriendButtonClick()");
        }


        public void OnMessageExitButtonClick()
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







