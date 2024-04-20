using Gaos.GameData;
using Gaos.Routes.Model.GameDataJson;
using TMPro;
using UnityEngine;

public class CloseMessageObject : MonoBehaviour
{
    public void CloseMessage()
    {
        var parentObj = transform.parent.gameObject;
        var grandParentObj = parentObj.transform.parent.gameObject;
        grandParentObj.SetActive(false);
    }

    private void CloseMessageWindow()
    {
        var parentObj = transform.parent.gameObject;
        var grandParentObj = parentObj.transform.parent.gameObject;
        grandParentObj.SetActive(false);
    }


    private void SetSlotToDefaultState(int slotNumber)
    {
        // set the save slot to defualt state 

        // serach for //Canvas/SaveSlots/SaveSlot1 using transform.Find()
        var saveSlot = GameObject.Find($"/Canvas/SaveSlots/SaveSlot{slotNumber}");
        var name = saveSlot.transform.Find("Info/Name").GetComponent<TextMeshProUGUI>();
        var status = saveSlot.transform.Find("Info/Status").GetComponent<TextMeshProUGUI>();
        var slotDeleteButton = saveSlot.transform.Find($"/Canvas/SaveSlots/DeleteSlot{slotNumber}").gameObject;


        var translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        var nameDefault = translationManager.Translate($"EmptySlot{slotNumber}");
        var statusDefault = translationManager.Translate($"StartNewGame");

        // set the name and status of the save slot to the default values
        name.text = nameDefault;
        status.text = statusDefault;
        // hide the delete button
        slotDeleteButton.SetActive(false);

    }

    private void OnDeleteSlotComplete(DeleteSlotResponse response, int slotNumber)
    {
        if (response != null)
        {
            CloseMessageWindow();
            SetSlotToDefaultState(slotNumber);
            Gaos.Context.Authentication.RemoveUserSlot(slotNumber);

        } else {
            Debug.LogError("Failed to delete slot");
        }
    }

    public Gaos.GameData.DeleteSlot.OnDeleteSlotComplete MakeOnDeleteSlotCompleteFn(int slotNumber)
    {
        return (response) =>
        {
            OnDeleteSlotComplete(response, slotNumber);
        };
    }


    public void ConfirmSlotDelettion()
    {
        var parentObj = transform.parent.gameObject;
        var grandParentObj = parentObj.transform.parent.gameObject;

        var slotNumber = grandParentObj.transform.Find("TitleContext/SlotNumber").GetComponent<TextMeshProUGUI>().text;

        int slotNumberInt = int.Parse(slotNumber);


        StartCoroutine(Gaos.GameData.DeleteSlot.DeleteSlotCall(slotNumberInt, MakeOnDeleteSlotCompleteFn(slotNumberInt)));
    }
}
