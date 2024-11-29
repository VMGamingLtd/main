﻿using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Friends
{

    public class FriendsTabAddFriendManager : MonoBehaviour
    {
        public class FriendAddModel
        {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public bool? IsFriend { get; set; }
            public bool? IsFriendRequest { get; set; }
        }

        private static string CLASS_NAME = typeof(FriendsTabAddFriendManager).Name;

        private static int MAX_SCROLL_LIST_LINES_COUNT = 10;

        private FriendAddModel[] AllUsers = new FriendAddModel[MAX_SCROLL_LIST_LINES_COUNT]; 
        private int LastIndexAllUsers = -1;

        private int[] FilteredUsers = new int[MAX_SCROLL_LIST_LINES_COUNT];
        private int LastIndexFilteredUsers = -1;

        public GameObject FriendsButton;
        public Transform List; // scroll list containing friends buttons

        public GameObject Title;

        //public GameObject SearchTextBox;
        public TMP_InputField SearchTextBox;

        private GameObject[] AllFriendsButtons = new GameObject[MAX_SCROLL_LIST_LINES_COUNT]; // all friends buttons in the scroll list
        private int LastIndexAllFriendsButtons = -1;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private async UniTaskVoid Init()
        {
            RemoveAllFriendsButtons();
            await ReadAllUsers();

            AllocateFriendsButtons();
            FilterUsers("");
            DisplayFilteredUsers();

            DisplayTitle();
            SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);
        }

        private void DisplayFilteredUsers()
        {

            for (int i = 0; i <= LastIndexFilteredUsers; i++)
            {
                int index_allusers = FilteredUsers[i];
                FriendAddModel user = AllUsers[index_allusers];

                AllFriendsButtons[i].SetActive(true);

                DisplayFriendButton(i);
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
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 455: OnSearchTextBoxChange(): {text}");
            FilterUsers(text);
            DisplayFilteredUsers();
            DisplayTitle();
        }

        public void OnEnable()
        {
            RemoveAllFriendsButtons();
            SearchTextBox.text = "";
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

        private async UniTask ReadAllUsers(string friendNameSubstring = null)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 500: ReadAllUsers({friendNameSubstring})");
            Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchResponse response = await Gaos.Friends.GetUsersForFriendsSearch.CallAsync(friendNameSubstring, MAX_SCROLL_LIST_LINES_COUNT);
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
                AllUsers[++LastIndexAllUsers] = new FriendAddModel {
                    UserId = response.Users[i].UserId,
                    UserName = response.Users[i].UserName,
                    IsFriend = response.Users[i].IsFriend,
                    IsFriendRequest = response.Users[i].IsFriendRequest,
                };

                if (i + 1 == MAX_SCROLL_LIST_LINES_COUNT)
                {
                    break;
                }
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

        private void DisplayFriendButton(int index)
        {
            int index_allusers = FilteredUsers[index];
            FriendAddModel user = AllUsers[index_allusers];

            // user name
            Transform childObject_friendUsername = AllFriendsButtons[index].transform.Find("Info/FriendUsername");
            TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
            friendUsername.text = user.UserName;

            // user status
            Transform childObject_friendStatus = AllFriendsButtons[index].transform.Find("Info/Status");
            TextMeshProUGUI friendStatus = childObject_friendStatus.GetComponent<TextMeshProUGUI>();
            friendStatus.text = "";
            if (user.IsFriendRequest == true)
            {
                string message;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        message = "Friend request";
                        break;
                    case SystemLanguage.Russian:
                        message = "Запрос в друзья";
                        break;
                    case SystemLanguage.Chinese:
                        message = "好友请求";
                        break;
                    case SystemLanguage.Slovak:
                        message = "Žiadosť o priateľstvo";
                        break;
                    default:
                        message = "Friend request";
                        break;
                }
                friendStatus.text = message;
            }
            if (user.IsFriend == true)
            {
                string message;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        message = "Friend";
                        break;
                    case SystemLanguage.Russian:
                        message = "Друг";
                        break;
                    case SystemLanguage.Chinese:
                        message = "朋友";
                        break;
                    case SystemLanguage.Slovak:
                        message = "Priateľ";
                        break;
                    default:
                        message = "Friend";
                        break;
                }
                friendStatus.text = message;
            }

            // disable / enable buttons
            if (user.IsFriend == true)
            {
                AllFriendsButtons[index].transform.Find("ButtonAdd").gameObject.SetActive(false);
                AllFriendsButtons[index].transform.Find("ButtonRevoke").gameObject.SetActive(true);
            }
            else
            {
                if (user.IsFriendRequest == true)
                {
                    AllFriendsButtons[index].transform.Find("ButtonAdd").gameObject.SetActive(false);
                    AllFriendsButtons[index].transform.Find("ButtonRevoke").gameObject.SetActive(true);
                }
                else if (user.IsFriend == false)
                {
                    AllFriendsButtons[index].transform.Find("ButtonAdd").gameObject.SetActive(true);
                    AllFriendsButtons[index].transform.Find("ButtonRevoke").gameObject.SetActive(false);
                }
                else
                {
                    AllFriendsButtons[index].transform.Find("ButtonAdd").gameObject.SetActive(false);
                    AllFriendsButtons[index].transform.Find("ButtonRevoke").gameObject.SetActive(false);
                }
            }
        }

        private void OnButtonAddClicked(int index)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 300: OnAddFriendButtonClicked({index})");
            int index_allusers = FilteredUsers[index];
            FriendAddModel user = AllUsers[index_allusers];

            if (user.IsFriend == true) {
                return;
            }

            Gaos.Friends.RequestFriend.CallAsync(user.UserId).Forget();
            user.IsFriendRequest = true;
            DisplayFriendButton(index);
        }

        private UnityAction  MakeOnButtonAddClicked(int index)
        {
            return () =>
            {
                OnButtonAddClicked(index);
            };
        }

        private void OnButtonRevokeClicked(int index)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 310: OnRevokeFriendButtonClicked({index})");
            int index_allusers = FilteredUsers[index];
            FriendAddModel user = AllUsers[index_allusers];

            Gaos.Friends.RemoveFriend.CallAsync(user.UserId).Forget();
            user.IsFriend = false;
            user.IsFriendRequest = false;
            DisplayFriendButton(index);
        }

        private UnityAction  MakeOnButtonRevokeClicked(int index)
        {
            return () =>
            {
                OnButtonRevokeClicked(index);
            };
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

                // Add onClick handler to `ButtonAdd`
                Transform transformButtonAdd = friendsButton.transform.Find("ButtonAdd");
                if (transformButtonAdd == null)
                {
                    Debug.LogWarning($"ButtonAdd not found");
                }
                else
                {
                    Button buttonAdd = transformButtonAdd.GetComponent<Button>();
                    if (buttonAdd == null)
                    {
                        Debug.LogWarning($"ButtonAdd not found");
                    }
                    else
                    {
                        buttonAdd.onClick.AddListener(MakeOnButtonAddClicked(LastIndexAllFriendsButtons));
                    }
                }

                // Add onClick handler to `ButtonRevoke`
                Transform transformButtonRevoke = friendsButton.transform.Find("ButtonRevoke");
                if (transformButtonRevoke == null)
                {
                    Debug.LogWarning($"ButtonRevoke not found");
                }
                else
                {
                    Button buttonRevoke = transformButtonRevoke.GetComponent<Button>();
                    if (buttonRevoke == null)
                    {
                        Debug.LogWarning($"ButtonRevoke not found");
                    }
                    else
                    {
                        buttonRevoke.onClick.AddListener(MakeOnButtonRevokeClicked(LastIndexAllFriendsButtons));
                    }
                }

            }
        }

        private async UniTaskVoid OnSearchIconClickAsync(string userNamePattern)
        {
            await GuiReadAllUsersList(userNamePattern);
            DisplayTitle();
        }

        public void DisplayTitle()
        {
            Title.SetActive(true);
        }

        public void OnSearchIconClick()
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 400: OnSearchIconClick(): {SearchTextBox.text}");
            OnSearchIconClickAsync(SearchTextBox.text).Forget();
        }
    }
}
