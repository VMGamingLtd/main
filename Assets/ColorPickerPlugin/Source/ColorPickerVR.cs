using UnityEngine;
using UnityEngine.Events;

namespace ColorPicker
{
    /// <summary>
    /// Color picker for VR.
    /// Assign the script to the target VR Controller. The ray's direction is set to local-forward.
    /// </summary>
    [AddComponentMenu("Matej Vanco/Color Picker/Color Picker VR")]
    public sealed class ColorPickerVR : MonoBehaviour
    {
        [Space]
        public bool colorPickerIsActive = true;

        [Header("* Pointer Settings")]
        [Tooltip("Pointer object that will visualize the picked color")]
        [SerializeField] private MeshRenderer Pointer;
        [Tooltip("Additional Y offset of the picked color pointer")]
        [SerializeField] private float pointerYOffset = 0.15f;
        [Tooltip("Line between ray origin and ray hit")]
        [SerializeField] private LineRenderer PointerLine; 
        [Space]
        [SerializeField] private LayerMask allowedLayers = ~0;

        [Tooltip("If enabled, the picked color will consist of R G B A. Otherwise only R G B")]
        [SerializeField] private bool includeAlphaChannel = true;
        [Header("* Picking Input")]
        [Tooltip("Mostly used for external source - enable/disable the variable for picking the color")] 
        public bool pickerInput = false;

        [Header("Picking Color Event")]
        [SerializeField] private UnityEvent Event;

        private bool pointEntered = false;

        /// <summary>
        /// Main picking result - final picking result (readonly)
        /// </summary>
        public Color ColorPickerResult { private set; get; }
        public event System.Action<Color> OnColorPick;

        private void OnPickingColor(Renderer hitObjRender, Vector3 hitpointCoords)
        {
            if (hitObjRender.material.mainTexture == null)
                return;
            if (!hitObjRender.material.mainTexture.isReadable)
            {
                Debug.LogError("Color Picker VR: main texture on the hit object is not set to 'Readable'! Select the texture and enable the 'Read/Write Enabled' property.");
                return;
            }

            Texture2D tex = ((Texture2D)hitObjRender.material.mainTexture);
            var mPos = hitpointCoords;
            mPos.x *= tex.width;
            mPos.y *= tex.height;

            ColorPickerResult = tex.GetPixel((int)mPos.x, (int)mPos.y);

            if (!includeAlphaChannel)
            {
                Color c = ColorPickerResult;
                c.a = 1.0f;
                ColorPickerResult = c;
            }

            Pointer.material.color = ColorPickerResult;
            PointerLine.startColor = ColorPickerResult;
            PointerLine.endColor = ColorPickerResult;

            OnColorPick?.Invoke(ColorPickerResult);
            Event?.Invoke();
        }

        private void Update()
        {
            if (!colorPickerIsActive)
                return;

            Ray r = new Ray(transform.position, transform.forward);
            bool physics = Physics.Raycast(r, out RaycastHit h, Mathf.Infinity, allowedLayers);

            Renderer rend = null;

            pointEntered = physics && h.collider && h.collider.GetComponent<MeshCollider>() && h.collider.TryGetComponent(out rend) && rend.material && rend.material.mainTexture != null;

            if (!pointEntered || rend == null)
            {
                if (Pointer.enabled)
                    Pointer.enabled = false;
                if (PointerLine.enabled)
                    PointerLine.enabled = false;
                return;
            }

            if (!PointerLine.enabled)
                PointerLine.enabled = true;

            PointerLine.SetPosition(0, r.origin);
            PointerLine.SetPosition(1, h.point);

            if (!pickerInput)
            {
                if (Pointer.enabled)
                    Pointer.enabled = false;
                return;
            }

            if (!Pointer.enabled)
                Pointer.enabled = true;
            Pointer.transform.position = h.point + (Vector3.up * pointerYOffset);

            OnPickingColor(rend, h.textureCoord);
        }

        /// <summary>
        /// Call this method to enable/ disable picking input
        /// </summary>
        public void PUBLIC_VRInputRequest(bool InputDown)
        {
            pickerInput = InputDown;
        }

        /// <summary>
        /// Active/ Disable color picker system by its activation
        /// </summary>
        public void PUBLIC_ActiveDisable()
        {
            if (colorPickerIsActive)
            {
                if (Pointer.enabled)
                    Pointer.enabled = false;
                if (PointerLine.enabled)
                    PointerLine.enabled = false;
                colorPickerIsActive = false;
            }
            else
                colorPickerIsActive = true;
        }

        /// <summary>
        /// Active/ Disable color picker system by boolean
        /// </summary>
        public void PUBLIC_ActiveDisable(bool Active)
        {
            colorPickerIsActive = Active;
            if (colorPickerIsActive)
                return;
            if (Pointer.enabled)
                Pointer.enabled = false;
            if (PointerLine.enabled)
                PointerLine.enabled = false;
        }
    }
}