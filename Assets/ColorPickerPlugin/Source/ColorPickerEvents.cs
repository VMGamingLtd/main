using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker
{
    /// <summary>
    /// Predefined color picker events.
    /// Assign the script to the object with Color Picker VR or PC (depending on the target platform).
    /// </summary>
    [AddComponentMenu("Matej Vanco/Color Picker/Color Picker Predefined Events")]
    public sealed class ColorPickerEvents : MonoBehaviour
    {
        [Tooltip("Is Color Picker focused on VR or PC/Mobile?")]
        [SerializeField] private bool isVR = false;

        private ColorPicker cpPC;
        private ColorPickerVR cpVR;

        private void Awake()
        {
            if (isVR)
            {
                if (!GetComponent<ColorPickerVR>())
                    Debug.LogError("Color Picker Events: The events are set to VR and the script needs to be attached to object with ColorPickerVR!");
                cpVR = GetComponent<ColorPickerVR>();
            }
            else
            {
                if (!GetComponent<ColorPicker>())
                    Debug.LogError("Color Picker Events: The events are set to NON-VR and the script needs to be attached to object with ColorPicker!");
                cpPC = GetComponent<ColorPicker>();
            }
        }

        #region Predefined simple events

        public void PUBLIC_SetColor(Image @Image)
        {
            if (isVR)
            {
                @Image.color = cpVR.ColorPickerResult;
            }
            else
            {
                @Image.color = cpPC.ColorPickerResult;

            }
        }

        public void UpdatePrimaryColor()
        {
            UIManagerReference uIManagerReference = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();
            uIManagerReference.SetPrimaryColor(cpPC.ColorPickerResult);
        }

        public void UpdateSecondaryColor()
        {
            UIManagerReference uIManagerReference = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();
            uIManagerReference.SetSecondaryColor(cpPC.ColorPickerResult);
        }

        public void UpdateBackgroundColor()
        {
            UIManagerReference uIManagerReference = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();
            uIManagerReference.SetBackgroundColor(cpPC.ColorPickerResult);
        }

        public void UpdateSecondaryBackgroundColor()
        {
            UIManagerReference uIManagerReference = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();
            uIManagerReference.SetSecondaryBackgroundColor(cpPC.ColorPickerResult);
        }

        public void UpdateNegativeColor()
        {
            UIManagerReference uIManagerReference = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();
            uIManagerReference.SetNegativeColor(cpPC.ColorPickerResult);
        }

        public void PUBLIC_SetColor(Text @Text)
        {
            if (isVR) @Text.color = cpVR.ColorPickerResult;
            else @Text.color = cpPC.ColorPickerResult;
        }

        public void PUBLIC_SetColor(MeshRenderer @MeshRenderer)
        {
            if (isVR) @MeshRenderer.material.color = cpVR.ColorPickerResult;
            else @MeshRenderer.material.color = cpPC.ColorPickerResult;
        }

        public void PUBLIC_SetColor(Material @Material)
        {
            if (isVR) Material.color = cpVR.ColorPickerResult;
            else Material.color = cpPC.ColorPickerResult;
        }

        public void PUBLIC_SetColor(TextMesh @TextMesh)
        {
            if (isVR) TextMesh.color = cpVR.ColorPickerResult;
            else TextMesh.color = cpPC.ColorPickerResult;
        }

        public void PUBLIC_SetColor_FindObjectsByTag(string Tag)
        {
            foreach (GameObject gm in GameObject.FindGameObjectsWithTag(Tag))
            {
                if (gm.GetComponent<Renderer>())
                {
                    if (isVR) gm.GetComponent<Renderer>().material.color = cpVR.ColorPickerResult;
                    else gm.GetComponent<Renderer>().material.color = cpPC.ColorPickerResult;
                }
            }
        }

        private string targetVar;

        /// <summary>
        /// MONOBEHAVIOUR CONNECTOR: Set Color in internal variable
        /// </summary>
        public void PUBLIC_SetColor_Mono(string VariableName)
        {
            targetVar = VariableName;
        }

        /// <summary>
        /// MONOBEHAVIOUR CONNECTOR: Set Color in internal variable
        /// </summary>
        public void PUBLIC_SetColor_Mono(MonoBehaviour @MonoBehaviour)
        {
            if (MonoBehaviour.GetType().GetField(targetVar) != null && MonoBehaviour.GetType().GetField(targetVar).GetValue(MonoBehaviour).GetType() == typeof(Color))
                MonoBehaviour.GetType().GetField(targetVar).SetValue(MonoBehaviour, ((isVR) ? cpVR.ColorPickerResult : cpPC.ColorPickerResult));
        }

        /// <summary>
        /// Set Color in internal Monobehaviour variable
        /// </summary>
        public void PUBLIC_SetColor_Mono(MonoBehaviour @MonoBehaviour, string Variable)
        {
            if (MonoBehaviour.GetType().GetField(Variable) != null && MonoBehaviour.GetType().GetField(Variable).GetValue(MonoBehaviour).GetType() == typeof(Color))
                MonoBehaviour.GetType().GetField(Variable).SetValue(MonoBehaviour, ((isVR) ? cpVR.ColorPickerResult : cpPC.ColorPickerResult));
        }

        #endregion
    }
}