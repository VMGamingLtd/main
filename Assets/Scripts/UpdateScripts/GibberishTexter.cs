using System.Collections;
using TMPro;
using UnityEngine;

public class GibberishTexter : MonoBehaviour
{
    private TextMeshProUGUI TextComponent;
    private readonly float TypingSpeed = 0.01f;

    private readonly string FullText = "Dungeon found ... Initiating search protocol ... Error ... Location may contain hostile lifeforms ... Awaiting additional approach plan ... Resources needed for fully exploring dungeon...unkown... Please initiate recalculation protocols to calculate necessary resources for full dungeon exploration.";


    private void OnEnable()
    {
        TextComponent = GetComponent<TextMeshProUGUI>();
        TextComponent.text = "";
        _ = StartCoroutine(StartTextAnimation());
    }

    private protected IEnumerator StartTextAnimation()
    {
        foreach (char letter in FullText.ToCharArray())
        {
            TextComponent.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    private void OnDisable()
    {
        TextComponent.text = "";
    }
}
