using UnityEngine;
using UnityEngine.EventSystems;

public class FlagNameUpdater : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CoroutineManager coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        Gaos.Context.Authentication.SetCountry(gameObject.name);
        coroutineManager.CallCountryUpdate(gameObject.name);
        GameObject.Find("Canvas/CountryMenu").SetActive(false);
    }
}
