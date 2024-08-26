using UnityEngine;
using UnityEngine.EventSystems;

public class FlagNameUpdater : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        GameObject.Find("Canvas/CountryMenu").SetActive(false);
    }
}
