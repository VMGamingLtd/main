using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public GameObject[] PlayerStats;
    public GameObject[] PlayerSkills;
    public GameObject[] LevelUpButtons;
    public GameObject[] ResetButtons;


    public void ShowLevelUpButtons(bool IsEnabled)
    {
        foreach (var button in LevelUpButtons)
        {
            button.SetActive(IsEnabled);
        }
    }

    public void EvaluateResetButtons()
    {
        if (Player.StatPointsInStrength > 0)
        {
            ResetButtons[0].SetActive(true);
        }
        else { ResetButtons[0].SetActive(false); }

        if (Player.StatPointsInPerception > 0)
        {
            ResetButtons[1].SetActive(true);
        }
        else { ResetButtons[1].SetActive(false); }

        if (Player.StatPointsInIntelligence > 0)
        {
            ResetButtons[2].SetActive(true);
        }
        else { ResetButtons[2].SetActive(false); }

        if (Player.StatPointsInAgility > 0)
        {
            ResetButtons[3].SetActive(true);
        }
        else { ResetButtons[3].SetActive(false); }

        if (Player.StatPointsInCharisma > 0)
        {
            ResetButtons[4].SetActive(true);
        }
        else { ResetButtons[4].SetActive(false); }

        if (Player.StatPointsInWillpower > 0)
        {
            ResetButtons[5].SetActive(true);
        }
        else { ResetButtons[5].SetActive(false); }
    }

    public void RefreshStats()
    {
        PlayerStats[0].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.StatPoints.ToString();
        PlayerStats[1].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Strength.ToString();
        PlayerStats[2].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Perception.ToString();
        PlayerStats[3].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Intelligence.ToString();
        PlayerStats[4].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Agility.ToString();
        PlayerStats[5].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Charisma.ToString();
        PlayerStats[6].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Willpower.ToString();

        PlayerSkills[0].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.SkillPoints.ToString();
        PlayerSkills[1].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Biology.ToString();
        PlayerSkills[2].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Engineering.ToString();
        PlayerSkills[3].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Chemistry.ToString();
        PlayerSkills[4].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Physics.ToString();
        PlayerSkills[5].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Computing.ToString();
        PlayerSkills[6].transform.Find("Content/Value").GetComponent<TextMeshProUGUI>().text = Player.Psychology.ToString();
    }
}
