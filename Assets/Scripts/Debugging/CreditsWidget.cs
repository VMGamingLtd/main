using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Debuggging
{
    public class CreditsWidget : MonoBehaviour
    {
        private bool showConsole = false;

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

            GUI.Box(new Rect(10,10,200,190), $"Credits {Credits.credits}");

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

    }
}
