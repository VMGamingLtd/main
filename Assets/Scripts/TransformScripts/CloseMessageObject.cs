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

    private void OnDeleteSlotComplete(DeleteSlotResponse response)
    {
        if (response != null)
        {
            var parentObj = transform.parent.gameObject;
            var grandParentObj = parentObj.transform.parent.gameObject;
            grandParentObj.SetActive(false);
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@ cp 2340: slot was deleted");
        } else {
            Debug.LogError("Failed to delete slot");
        }
    }

    public void ConfirmSlotDelettion()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@ cp 2000: ConfirmSlotDelettion()");
        var parentObj = transform.parent.gameObject;
        var grandParentObj = parentObj.transform.parent.gameObject;

        var slotNumber = grandParentObj.transform.Find("TitleContext/SlotNumber").GetComponent<TextMeshProUGUI>().text;

        int slotNumberInt = int.Parse(slotNumber);
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@ cp 2100");
        StartCoroutine(Gaos.GameData.DeleteSlot.DeleteSlotCall(slotNumberInt, OnDeleteSlotComplete));
    }
}
