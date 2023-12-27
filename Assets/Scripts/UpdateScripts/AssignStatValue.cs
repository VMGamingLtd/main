using TMPro;
using UnityEngine;

public class AssignStatValue : MonoBehaviour
{
    public StatsManager StatsManager;
    public void ResetStatValue()
    {
        if (gameObject.name == "ResetStrengthButton" && Player.StatPointsInStrength > 0)
        {
            int RefundPoints = Player.Strength - 10;
            Player.Strength -= RefundPoints;
            Player.StatPointsInStrength = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Strength.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        else if (gameObject.name == "ResetPerceptionButton" && Player.StatPointsInPerception > 0)
        {
            int RefundPoints = Player.Perception - 10;
            Player.Perception -= RefundPoints;
            Player.StatPointsInPerception = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Perception.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        else if (gameObject.name == "ResetIntelligenceButton" && Player.StatPointsInIntelligence > 0)
        {
            int RefundPoints = Player.Intelligence - 10;
            Player.Intelligence -= RefundPoints;
            Player.StatPointsInIntelligence = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Intelligence.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        else if (gameObject.name == "ResetAgilityButton" && Player.StatPointsInAgility > 0)
        {
            int RefundPoints = Player.Agility - 10;
            Player.Agility -= RefundPoints;
            Player.StatPointsInAgility = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Agility.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        else if (gameObject.name == "ResetCharismaButton" && Player.StatPointsInCharisma > 0)
        {
            int RefundPoints = Player.Charisma - 10;
            Player.Charisma -= RefundPoints;
            Player.StatPointsInCharisma = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Charisma.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        else if (gameObject.name == "ResetWillpowerButton" && Player.StatPointsInWillpower > 0)
        {
            int RefundPoints = Player.Willpower - 10;
            Player.Willpower -= RefundPoints;
            Player.StatPointsInWillpower = 0;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Willpower.ToString();
            StatsManager.ShowLevelUpButtons(true);
        }
        StatsManager.EvaluateResetButtons();
    }
    public void ChooseStatValue()
    {
        if (gameObject.name == "StrengthButton")
        {
            Player.Strength++;
            Player.StatPointsInStrength++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Strength.ToString();
        }
        else if (gameObject.name == "PerceptionButton")
        {
            Player.Perception++;
            Player.StatPointsInPerception++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Perception.ToString();
        }
        else if (gameObject.name == "IntelligenceButton")
        {
            Player.Intelligence++;
            Player.StatPointsInIntelligence++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Intelligence.ToString();
        }
        else if (gameObject.name == "AgilityButton")
        {
            Player.Agility++;
            Player.StatPointsInAgility++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Agility.ToString();
        }
        else if (gameObject.name == "CharismaButton")
        {
            Player.Charisma++;
            Player.StatPointsInCharisma++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Charisma.ToString();
        }
        else if (gameObject.name == "WillpowerButton")
        {
            Player.Willpower++;
            Player.StatPointsInWillpower++;
            gameObject.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = Player.Willpower.ToString();
        }
        StatsManager.EvaluateResetButtons();
    }
}
