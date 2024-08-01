using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventIconData : MonoBehaviour, IPointerClickHandler
{
    public EventIconType Type;
    public EventSize Size;
    public string Name;
    public float CurrentQuantity;
    public float MinQuantityRange;
    public float MaxQuantityRange;
    public int EventLevel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick.transform.TryGetComponent<EventIconData>(out var eventIconData))
        {
            FightManager fightManager = GameObject.Find("FIGHTMANAGER").GetComponent<FightManager>();
            fightManager.EventSize = Size;

            if (eventIconData.Type == EventIconType.VolcanicCave)
            {
                InteractionManager interactionManager = GameObject.Find("INTERACTIONMANAGER").GetComponent<InteractionManager>();
                interactionManager.DungeonUI.SetActive(true);
                Transform DungeonName = interactionManager.DungeonUI.transform.Find("DungeonName").transform;
                TranslationManager translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
                interactionManager.DungeonUI.transform.Find("Info/Level").GetComponent<TextMeshProUGUI>().text = $"T{EventLevel}";
                interactionManager.DungeonUI.transform.Find("Info/Size").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Size.ToString());
                DungeonName.GetChild(0).name = Name;
                DungeonName.GetChild(0).GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
            }
        }
    }
}
