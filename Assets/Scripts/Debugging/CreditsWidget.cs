using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Debuggging
{
    public class CreditsWidget : MonoBehaviour
    {
        public static string CLASS_NAME = typeof(CreditsWidget).Name;

        private bool showConsole = false;

        private int mGroupId = -1;
        private class GroupMember
        {
            public string UserName = "";
            public int UserId = -1;
            public float Credits = 0.0f;
        }

        private GroupMember groupOwner = new GroupMember();
        private List<GroupMember> groupMembers = new List<GroupMember>();

        void OnEnable()
        {
            LoadGroupUntill().Forget();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                showConsole = !showConsole;
            }
        }

        private string textField_value;
        private string textField_diff;

        void OnGUI()
        {
            if (!showConsole) return;

            GUI.Box(new Rect(10,10,200,400), $"Credits {Credits.credits}");

            GUI.Label (new Rect (25, 50, 100, 30), "Value"); textField_value = GUI.TextField (new Rect (65, 50, 100, 30), textField_diff);
            GUI.Label (new Rect (25, 90, 100, 30), "Diff"); textField_diff = GUI.TextField (new Rect (65, 90, 100, 30), textField_diff);
            if(GUI.Button(new Rect(25, 130,60,30), "Plus"))
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Increse"); 
                AddCredits();
            }
            if(GUI.Button(new Rect(95, 130,60,30), "Minus")) 
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Decrease"); 
            }

            GUI.Label(new Rect(25, 180, 200, 20), $"[ {groupOwner.UserName} {groupOwner.Credits} ]");
            int i = 0;
            foreach (var member in groupMembers)
            {
                GUI.Label(new Rect(25, 200 + (i * 20), 200, 20), $"- {member.UserName} {member.Credits}");
                i++;
            }
        }

        async void AddCredits()
        {
            var result = await Gaos.GroupData1.AddMyCredits.CallAsync(float.Parse(textField_value));
            if (result.IsError == true)
            {
                Debug.LogError($"Error adding credits: {result.ErrorMessage}");
            }
            else
            {
                Debug.Log($"Credits added: {result.Credits}");
                Credits.credits = result.Credits;
            }
        }

        async void ResetCredits()
        {
            var result = await Gaos.GroupData1.ResetMyCredits.CallAsync(float.Parse(textField_value));
            if (result.IsError == true)
            {
                Debug.LogError($"Error resetting credits: {result.ErrorMessage}");
            }
            else
            {
                Debug.Log($"Credits reset: {result.Credits}");
                Credits.credits = result.Credits;
            }
        }

        async UniTask LoadCreditsForUser(int userId)
        {
            var result = await Gaos.GroupData1.GetCredits.CallAsync(mGroupId, userId);
            if (result == null)
            {
                return;
            }

            foreach (var member in groupMembers)
            {
                if (member.UserId == userId)
                {
                    member.Credits = result.Credits;
                    break;
                }
            }
        }

        async UniTask<bool> LoadGroup()
        {
            const string METHOD_NAME = "LoadGroup()";

            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: Loading group");

            var getMyGroupResponse = await Gaos.Friends.Friends.GetMyGroup.CallAsync();
            if (getMyGroupResponse == null)
            {
                return false;
            }

            if (!(getMyGroupResponse.IsGroupMember || getMyGroupResponse.IsGroupOwner))
            {
                return false;
            }

            mGroupId = getMyGroupResponse.GroupId;
            groupOwner.UserName = getMyGroupResponse.GroupOwnerName;
            groupOwner.UserId = getMyGroupResponse.GroupOwnerId;

            var getMyGroupMembersResponse = await Gaos.Friends.Friends.GetMyGroupMembers.CallAsync(getMyGroupResponse.GroupId, 100);
            if (getMyGroupMembersResponse == null)
            {
                return false;
            }

            groupMembers.Clear();
            foreach (var member in getMyGroupMembersResponse.Users)
            {
                GroupMember groupMember = new GroupMember();
                groupMember.UserName = member.UserName;
                groupMember.UserId = member.UserId;
                groupMember.Credits = 0.0f;
                groupMembers.Add(groupMember);
            }
            return true;
        }

        async UniTaskVoid LoadGroupUntill()
        {
            const string METHOD_NAME = "LoadGroupUntill()";
            while (await LoadGroup() == false)
            {
                await UniTask.Delay(3000);
            }
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: Group loaded");
        }

    }
}
