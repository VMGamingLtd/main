using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnapCurrentTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    void Awake()
    {
        System.DateTime currentTime = System.DateTime.Now;
        string timeString = currentTime.ToString("HH:mm:ss");
        timeText.text = timeString;
    }
}
