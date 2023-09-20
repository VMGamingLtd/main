using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ResetChildButtonsStateToNormal: MonoBehaviour
    {

        void OnDisable()
        {

            Button[] buttons =  GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                string buttonName = button.gameObject.name;
                foreach (Transform child in button.transform)
                {
                    CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        if (child.name == "Normal")
                        {
                            canvasGroup.alpha = 1;
                        }
                        else if (child.name == "Highlighted")
                        {
                            canvasGroup.alpha = 0;
                        }
                        else if (child.name == "Pressed")
                        {
                            canvasGroup.alpha = 0;
                        }
                    }
                }

            }

        }
    }
}
