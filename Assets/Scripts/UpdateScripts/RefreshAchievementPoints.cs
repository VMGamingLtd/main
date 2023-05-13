using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshAchievementPoints : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = AchievementPoints.achievementPoints.ToString();
    }

}
