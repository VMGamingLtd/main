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

        public void OnEnable()
        {
            LoadGroupUntill().Forget();
            // Register the listener
            Gaos.Messages.Group1.GroupCreditsChange.OnGroupCreditsChange += HandleGroupCreditsChange;
        }

        public void OnDisable()
        {
            // Unregister the listener
            Gaos.Messages.Group1.GroupCreditsChange.OnGroupCreditsChange -= HandleGroupCreditsChange;
        }

        private void HandleGroupCreditsChange(int groupId, int userId, float credits)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@: HandleGroupCreditsChange: {groupId} {userId} {credits}");
            if (groupId != mGroupId) return;

            UpdateCreditsForUser(userId, credits);

        }

        private void UpdateCreditsForUser(int userId, float credits)
        {
            if (userId == groupOwner.UserId)
            {
                groupOwner.Credits = credits;
            }
            else
            {
                var member = groupMembers.Find(m => m.UserId == userId);
                if (member != null)
                {
                    member.Credits = credits;
                }
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@@ BackQuote"); //@@@@@@@@@@@@@@@@@ 
                showConsole = !showConsole;
            }
        }

        private string textField_value;

        public void OnGUI()
        {
            if (!showConsole) return;

            GUI.Box(new Rect(10,10,200,400), $"Credits {Credits.credits}");

            GUI.Label (new Rect (25, 50, 100, 30), "Value"); textField_value = GUI.TextField (new Rect (65, 50, 100, 30), textField_value);
            if(GUI.Button(new Rect(25, 130,60,30), "Add"))
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Add"); 
                AddCredits();
            }
            if(GUI.Button(new Rect(95, 130,60,30), "Reset")) 
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Reset"); 
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
            if (result == null)
            {
                Debug.LogError($"Error adding credits");
            }
            else
            {
                Debug.Log($"Credits added");
                Credits.credits = result.Credits;

                UpdateCreditsForUser(Gaos.Context.Authentication.GetUserId(), result.Credits);

                // Update the credits for the group owner

            }
        }


        async void ResetCredits()
        {
            var result = await Gaos.GroupData1.ResetMyCredits.CallAsync(float.Parse(textField_value));
            if (result == null)
            {
                Debug.LogError($"Error resetting credits: {result.ErrorMessage}");
            }
            else
            {
                Debug.Log($"Credits reset: {result.Credits}");
                Credits.credits = result.Credits;
                UpdateCreditsForUser(Gaos.Context.Authentication.GetUserId() , result.Credits);
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

            var getMyGroupResponse = await Gaos.Groups.Groups.GetMyGroup.CallAsync();
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

            var getMyGroupMembersResponse = await Gaos.Groups.Groups.GetMyGroupMembers.CallAsync(getMyGroupResponse.GroupId, 100);
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

            // Keep loading initial credits for each user
            List<UniTask> tasks = new List<UniTask>();
            foreach (var member in groupMembers)
            {
                tasks.Add(LoadCreditsForUser(member.UserId));
            }
            await UniTask.WhenAll(tasks);

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
