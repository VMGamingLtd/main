using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class UIElementSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [Header("Settings")]
        public bool enableHoverSound = true;
        public bool enableClickSound = true;
        public bool checkForInteraction = true;

        private Button sourceButton;
        private AudioManager audioManager;

        void OnEnable()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            if (checkForInteraction == true) { sourceButton = gameObject.GetComponent<Button>(); }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableHoverSound == true)
            {
                audioManager.PlayHoverSound();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableClickSound == true)
            {
                audioManager.PlayPressedSound();
            }
        }
    }
}