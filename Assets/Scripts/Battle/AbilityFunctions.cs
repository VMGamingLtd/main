using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private FightManager fightManager;
    void Awake()
    {
        fightManager = GameObject.Find("FIGHTMANAGER").GetComponent<FightManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!fightManager.IsPerformingAbility)
        {
            fightManager.TargetService.HighlightPossibleTargets(fightManager.ActiveCombatant, gameObject.name);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!fightManager.IsPerformingAbility)
        {
            fightManager.TargetService.ClearAllHighlights();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fightManager.IsPerformingAbility)
        {
            fightManager.TargetService.BlinkAvailableTargets(eventData.pointerClick.transform.gameObject.name);
        }
    }
}
