using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Enumerations;

public class WalletManager : MonoBehaviour
{
    [SerializeField] GameObject CurrencyTemplate;
    [SerializeField] Transform CurrencyList;
    [SerializeField] GameObject DialogueObject;
    private TranslationManager translationManager;
    private string recipient = null;
    private int currencyQuantity = 0;
    private CurrencyType currencyType;

    private void Awake()
    {
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
    }
    private void OnEnable()
    {
        if (Player.Currencies != null && Player.Currencies.Count > 0)
        {
            foreach (var currency in Player.Currencies)
            {
                GameObject newCurrency = Instantiate(CurrencyTemplate, CurrencyList);
                newCurrency.transform.Find("Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignMiscSpriteToSlot(currency.FullName);
                newCurrency.transform.Find("ShortName").GetComponent<TextMeshProUGUI>().text = currency.ShortName;
                newCurrency.name = currency.ShortName;
                newCurrency.transform.Find("FullName").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(currency.FullName);
                newCurrency.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = currency.Quantity.ToString("F2");
                newCurrency.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void RequestCurrencyMode()
    {
        DialogueObject.SetActive(true);
        DialogueObject.name = Constants.CurrencyRequest;
        DialogueObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Constants.CurrencyRequest);
        UpdateCurrencyType(EventSystem.current.currentSelectedGameObject);

    }

    public void SendCurrencyMode()
    {
        DialogueObject.SetActive(true);
        DialogueObject.name = Constants.CurrencySend;
        DialogueObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Constants.CurrencySend);
        UpdateCurrencyType(EventSystem.current.currentSelectedGameObject);
    }

    public void UpdateRecipient(string name)
    {
        recipient = name;
    }

    public void UpdateQuantity()
    {
        var inputTextObject = EventSystem.current.currentSelectedGameObject;
        if (inputTextObject != null)
        {
            if (int.TryParse(inputTextObject.GetComponent<TMP_InputField>().text, out int quantity))
            {
                currencyQuantity = quantity;
            }
        }     
    }

    public void ProcessMethod()
    {
        if (DialogueObject.name == Constants.CurrencyRequest)
        {
            ProcessRequest();
        }
        else if (DialogueObject.name == Constants.CurrencySend)
        {
            ProcessSend();
        }
    }
    private void ProcessRequest()
    {
        Debug.Log(recipient);
        Debug.Log(currencyQuantity);
        Debug.Log(currencyType);
    }

    private void ProcessSend()
    {

    }

    private void UpdateCurrencyType(GameObject clickedButton)
    {
        if (clickedButton != null)
        {
            string parentName = clickedButton.transform.parent.gameObject.name;

            try
            {
                currencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), parentName, true);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Invalid currency type: " + parentName);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in CurrencyList)
        {
            Destroy(child.gameObject);
        }
    }
}
