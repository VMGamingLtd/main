using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public interface IFilter<T>
{
    bool Filter(T item);
}



public interface IDataSource<T>
{
    T[] GetData();
    void SetFilter(IFilter<T> filter);
}

public class FriendModel
{
    public string UserName { get; set; }
    public string Status { get; set; }
}

public class FriendNameFilter : IFilter<FriendModel>
{
    private string SearchSubstring { get; set; }
    public bool Filter(FriendModel item)
    {
        if (SearchSubstring == null || SearchSubstring.Length == 0)
        {
            return true;
        }
        else
        {
            return item.UserName.Contains(SearchSubstring);
        }
    }

    public void SetSearchSubstring(string substring)
    {
        this.SearchSubstring = substring;
    }   
}


class SimpleFriendDataSource : IDataSource<FriendModel>
{
    private FriendModel[] Friends { get; set; }
    private IFilter<FriendModel> Filter = null;


    public SimpleFriendDataSource(IFilter<FriendModel> filter = null)
    {
        this.Friends = new FriendModel[8]
        {
            new FriendModel { UserName = "Anderson", Status = "Online" },
            new FriendModel { UserName = "Bennett", Status = "Offline" },
            new FriendModel { UserName = "Carter", Status = "Offline" },
            new FriendModel { UserName = "Davis", Status = "Online" },
            new FriendModel { UserName = "Evans", Status = "Offline" },
            new FriendModel { UserName = "Foster", Status = "Offline" },
            new FriendModel { UserName = "Garcia", Status = "Online" },
            new FriendModel { UserName = "Harrison", Status = "Online" },
            /*
            new FriendModel { UserName = "Johnson", Status = "Online" },
            new FriendModel { UserName = "Mitchell_1", Status = "Online" },
            new FriendModel { UserName = "Mitchell_2", Status = "Online" },
            new FriendModel { UserName = "Mitchell_3", Status = "Online" },
            new FriendModel { UserName = "Mitchell_4", Status = "Online" },
            */
        };
        this.Filter = filter;
    }

    public FriendModel[] GetData()
    {
        List<FriendModel> friends = new List<FriendModel>();
        for (int i = 0; i < Friends.Length; i++)
        {
            if (Filter == null)
            {
                friends.Add(Friends[i]);
            }
            else
            {
                if (Filter.Filter(Friends[i]))
                {
                    friends.Add(Friends[i]);
                }
            }
        }
        return friends.ToArray();
    }

    public void SetFilter(IFilter<FriendModel> filter)
    {
        Filter = filter;
    }
}





public class FriendsManager : MonoBehaviour
{
    private IDataSource<FriendModel> DataSource = null;
    private FriendNameFilter Filter = null;

    public GameObject FriendsButton;
    public Transform List;

    //public GameObject SearchTextBox;
    public TMP_InputField SearchTextBox;


    FriendsManager()
    {
        Filter = new FriendNameFilter();
        DataSource = new SimpleFriendDataSource(Filter);

    }

    public void AddFriend(FriendModel friend)
    {
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1100: AddFriend() - {friend.UserName}");
        Instantiate(FriendsButton, List);
        Transform childObject_friendUsername = FriendsButton.transform.Find("Info/FriendUsername");
        TextMeshProUGUI friendUsername = childObject_friendUsername.GetComponent<TextMeshProUGUI>();
        friendUsername.text = friend.UserName;
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1200: AddFriend() - friendUsername.text: {friendUsername.text}");

        Transform childObject_friendStatus = FriendsButton.transform.Find("Info/Status");
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

    public void DisplayFriends()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1000: DisplayFriends() - start");
        RemoveAllFriends();

        FriendModel[] friends = DataSource.GetData();
        foreach (FriendModel friend in friends)
        {
            AddFriend(friend);
        }
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1500: DisplayFriends() - end");
    }

    public void Search(string search)
    {
        //Filter.SetSearchSubstring(search);
        //DisplayFriends();
    }

    public void OnEnable()
    {
        //Filter.SetSearchSubstring(null);
        DisplayFriends();

        //SearchTextBox.onValueChanged.AddListener(OnSearchTextBoxChange);
    }

    public void OnSearchTextBoxChange(string text)
    { 
        //Filter.SetSearchSubstring(text);
        //DisplayFriends();
    }




}
