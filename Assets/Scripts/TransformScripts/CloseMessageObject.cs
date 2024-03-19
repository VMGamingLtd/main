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

    public void ConfirmSlotDelettion()
    {
        var parentObj = transform.parent.gameObject;
        var grandParentObj = parentObj.transform.parent.gameObject;

        var slotNumber = grandParentObj.transform.Find("TitleContext/SlotNumber").GetComponent<TextMeshProUGUI>().text;

        if (slotNumber == "1")
        {

        }
        else if (slotNumber == "2")
        {

        }
        else if (slotNumber == "3")
        {

        }
        else if (slotNumber == "4")
        {

        }
    }
}
