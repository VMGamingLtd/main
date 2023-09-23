using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public class AddAccounPane : MonoBehaviour
{
    public GameObject canvas;
    public GameObject account;

    private GameObject accountClone;


    void OnEnable()
    {
        accountClone =  Instantiate(account, account.transform.position, account.transform.rotation);
        accountClone.transform.SetParent(canvas.transform);

        foreach (Transform child in accountClone.transform)
        {
            var name = child.gameObject.name;

            // Remove all children except:
            if (!(
                name == "Background" ||
                name == "FillBckg" ||
                name == "Level" ||
                name == "Info" ||
                name == "RegisterButton" 
                ))
            {
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            // Change "RegisterButton"
            if (name == "RegisterButton")
            {
                Button button = child.gameObject.GetComponent<Button>();
                button.transition = Selectable.Transition.None;
                Debug.Log($"@@@@@@@@@@@@@@@@@@@@@ cp 300: RemoveAllListeners()");
                // Remove all listeners added by script
                button.onClick.RemoveAllListeners();
                // Try to remove all listeners added by Unity Editor (maximally 5 listener)
                try
                {
                    UnityEventTools.RemovePersistentListener(button.onClick, 0);
                    UnityEventTools.RemovePersistentListener(button.onClick, 0);
                    UnityEventTools.RemovePersistentListener(button.onClick, 0);
                    UnityEventTools.RemovePersistentListener(button.onClick, 0);
                    UnityEventTools.RemovePersistentListener(button.onClick, 0);
                }
                catch (System.Exception)
                {
                    ;
                }
            }
        }   
        
    }

    void OnDisable()
    {
        if (accountClone != null)
        {
            if (accountClone.transform.parent != null)
            {
                accountClone.transform.SetParent(null);
            }       
            Destroy(accountClone);
        }
        
    }
}
