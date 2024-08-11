using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;

namespace Friends
{
    public class FriendModel
    {
        public string UserName { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public bool? IsFriend { get; set; }
    }

    public class FriendsTabForGroupOwnerManager : MonoBehaviour
    {
        private static string CLASS_NAME = typeof(FriendsTabForGroupOwnerManager).Name;

        //private static int MAX_SCROLL_LIST_LINES_CPUNT = 100;
        private static int MAX_SCROLL_LIST_LINES_COUNT = 10;

        public GameObject FriendsButton;
        public Transform List; // scroll list containing friends buttons

        public GameObject Title;
        public GameObject Message;
        public TMP_Text MessageText;

        public GameObject AddFriendDialog;
        public TMP_Text AddFriendDialogText;

        public GameObject RemoveFriendDialog;
        public TMP_Text RemoveFriendDialogText;


        //public GameObject SearchTextBox;
        public TMP_InputField SearchTextBox;

        private FriendModel[] AllUsers = new FriendModel[MAX_SCROLL_LIST_LINES_COUNT]; 
        private int LastIndexAllUsers = -1;

        private int[] FilteredUsers = new int[MAX_SCROLL_LIST_LINES_COUNT];
        private int LastIndexFilteredUsers = -1;

        private GameObject[] AllFriendsButtons = new GameObject[MAX_SCROLL_LIST_LINES_COUNT]; // all friends buttons in the scroll list
        private int LastIndexAllFriendsButtons = -1;

        FriendsTabForGroupOwnerManager()
        {
        }

        private async UniTaskVoid Init()
        {
            bool isOnSearchTextBoxChange = false;

            RemoveAllFriendsButtons();
            await ReadAllUsers();
            AllocateFriendsButtons();
            FilterUsers("");
            DisplayFilteredUsers();
            DisplayTitle();

            if (!isOnSearchTextBoxChange)
            {
                SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);
            }

        }

        private async UniTask ReadAllUsers(string frienNameSubstring = null)
        {
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp200 ReadAllUsers()");
            Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = await Gaos.Friends.Friends.GetUsersList.CallAsync(frienNameSubstring, MAX_SCROLL_LIST_LINES_COUNT);
            if (response == null)
            {
                // error occured
                return; 
            }
            LastIndexAllUsers = -1;
            if (response == null)
            {
                return; 
            }

            for (int i = 0; i < response.Users.Length; i++)
            {

                // fill in friends array
                AllUsers[++LastIndexAllUsers] = new FriendModel {
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

        private void AllocateFriendsButtons()
        {
            int n = (LastIndexAllUsers + 1) - (LastIndexAllFriendsButtons + 1);
            while (n > 0)
            {
                GameObject friendsButton = Instantiate(FriendsButton, List);
                friendsButton.transform.position = Vector3.zero;
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
                int index = FilteredUsers[i];
                FriendModel user = AllUsers[index];

                AllFriendsButtons[i].SetActive(true);

                Transform childObject_friendUsername = AllFriendsButtons[i].transform.Find("Info/FriendUsername");
                TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
                if (user.IsFriend == true)
                {
                    friendUsername.text = $"{user.UserName} ({friendIndicator})";
                }
                else
                {
                    friendUsername.text = user.UserName;
                }

                Transform childObject_friendStatus = AllFriendsButtons[i].transform.Find("Info/Status");
                TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
                friendStatus.text = user.Status;
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

        public void SetTitlePaneDisplay()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 300: SetTitlePaneDisplay(): {LastIndexFilteredUsers + 1}");
            if (LastIndexFilteredUsers == 0)
            {
                FriendModel user = AllUsers[FilteredUsers[0]];   
                if (user.IsFriend == false)
                {
                    if (user.UserId != Gaos.Context.Authentication.GetUserId())
                    {
                        DisplayAddFriendDialog(user);
                    }
                    else
                    {
                        // logged in user cannt add himself as a friend
                        DisplayTitle();
                    }
                    DisplayAddFriendDialog(user);
                }
                else
                {
                    DisplayRemoveFriendDialog(user);
                }
            }
            else
            {
                DisplayTitle();
            }
        }

        public void OnSearchTextBoxChange(string text)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 455: OnSearchTextBoxChange(): {text}");
            FilterUsers(text);
            DisplayFilteredUsers();
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
            RemoveAllFriendsButtons();
            await ReadAllUsers(userNamePattern);
            AllocateFriendsButtons();
            FilterUsers("");
            DisplayFilteredUsers();
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


        public void DisplayTitle()
        {
            Title.SetActive(true);
            Message.SetActive(false);
            AddFriendDialog.SetActive(false);
            RemoveFriendDialog.SetActive(false);
        }

        public void DisplayMessage(string message)
        {
            Title.SetActive(false);
            Message.SetActive(true);
            MessageText.text = message;
            AddFriendDialog.SetActive(false);
            RemoveFriendDialog.SetActive(false);
        }

        public void DisplayAddFriendDialog(FriendModel user)
        {
            string message;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    message = $"Add {user.UserName} to friends?";
                    break;
                case SystemLanguage.Russian:
                    message = $"Добавить {user.UserName} в друзья?";
                    break;
                case SystemLanguage.Chinese:
                    message = $"将 {user.UserName} 添加到好友?";
                    break;
                case SystemLanguage.Slovak:
                    message = $"Pridať {user.UserName} do priateľov?";
                    break;
                default:
                    message = $"Add {user.UserName} to friends?";
                    break;
            }
            AddFriendDialogText.text = message;

            Title.SetActive(false);
            Message.SetActive(false);
            AddFriendDialog.SetActive(true);
            RemoveFriendDialog.SetActive(false);
        }

        public void DisplayRemoveFriendDialog(FriendModel user)
        {
            string message;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    message = $"Remove {user.UserName} from friends?";
                    break;
                case SystemLanguage.Russian:
                    message = $"Удалить {user.UserName} из друзей?";
                    break;
                case SystemLanguage.Chinese:
                    message = $"从好友中删除 {user.UserName}？";
                    break;
                case SystemLanguage.Slovak:
                    message = $"Odstrániť {user.UserName} z priateľov?";
                    break;
                default:
                    message = $"Remove {user.UserName} from friends?";
                    break;
            }
            RemoveFriendDialogText.text = message;

            Title.SetActive(false);
            Message.SetActive(false);
            AddFriendDialog.SetActive(false);
            RemoveFriendDialog.SetActive(true);
        }


        public async UniTaskVoid OnAddFriendDialogButtonYesAsync()
        {
            var response = await Gaos.Friends.Friends.AddFriend.CallAsync(AllUsers[FilteredUsers[0]].UserId);
            DisplayTitle();
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
            DisplayTitle();
            SearchTextBox.text = "";
            await GuiReadAllUsersList("");
        }

        public void OnAddFriendDialogButtonNo()
        {
            OnAddFriendDialogButtonNoAsync().Forget();
        }

        public async UniTaskVoid OnRemoveFriendDialogButtonYesAsync()
        {
            var response = await Gaos.Friends.Friends.RemoveFriend.CallAsync(AllUsers[FilteredUsers[0]].UserId);
            DisplayTitle();
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
            DisplayTitle();
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
            DisplayMessage("Hello world!");
        }


        public void OnMessageExitButtonClick()
        {
            DisplayTitle();
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







