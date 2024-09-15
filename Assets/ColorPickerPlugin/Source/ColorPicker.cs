using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace ColorPicker
{
    /// <summary>
    /// Color picker for PC & Mobile.
    /// Written by Matej Vanco in 2019, Updated in 2024
    /// </summary>
    [AddComponentMenu("Matej Vanco/Color Picker/Color Picker")]
    [RequireComponent(typeof(Image))]
    public sealed class ColorPicker : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        [Header("* Texture Source")]
        [SerializeField] private Image targetPaletteImage;
        [Header("* Image Pointer")]
        [SerializeField] private Image pickingPointer;
        [Space]
        [Tooltip("If enabled, the picked color will consist of R G B A. Otherwise only R G B")]
        [SerializeField] private bool includeAlphaChannel = true;
        [Tooltip("Hide the picking pointer graphics on drop? Also show pointer on drag?")]
        [SerializeField] private bool hidePointerOnDrop = true;

        [Header("Picking Color Event")]
        [SerializeField] private UnityEvent colorPickerPublicEvent;

        /// <summary>
        /// Main picking result - final picking result (readonly)
        /// </summary>
        public Color ColorPickerResult { private set; get; }
        public event System.Action<Color> OnColorPick;

        private Texture2D paletteImgSrc;
        private bool cursorEnter = false;

        #region Pointer Events

        public void OnPointerEnter(PointerEventData e)
            => cursorEnter = true;

        public void OnPointerExit(PointerEventData e)
            => cursorEnter = false;

        public void OnPointerClick(PointerEventData e)
            => OnPickingColor();

        public void OnDrag(PointerEventData e)
        {
            OnPickingColor();
            if (pickingPointer && hidePointerOnDrop)
                pickingPointer.enabled = true;
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (pickingPointer && hidePointerOnDrop)
                pickingPointer.enabled = false;
        }

        private void OnPickingColor()
        {
            if (!cursorEnter)
                return;

            Vector2 mPos = Input.mousePosition;
            pickingPointer.transform.position = mPos;

            mPos = targetPaletteImage.rectTransform.InverseTransformPoint(mPos);

            mPos.x += targetPaletteImage.rectTransform.rect.width * targetPaletteImage.rectTransform.pivot.x;
            mPos.y += targetPaletteImage.rectTransform.rect.height * targetPaletteImage.rectTransform.pivot.y;

            mPos *= new Vector2(
                paletteImgSrc.width / targetPaletteImage.rectTransform.rect.width,
                paletteImgSrc.height / targetPaletteImage.rectTransform.rect.height);

            if (mPos.x <= paletteImgSrc.width && mPos.x >= 0 && mPos.y <= paletteImgSrc.height && mPos.y >= 0)
                ColorPickerResult = paletteImgSrc.GetPixel((int)mPos.x, (int)mPos.y);

            if (!includeAlphaChannel)
            {
                Color c = ColorPickerResult;
                c.a = 1.0f;
                ColorPickerResult = c;
            }

            OnColorPick?.Invoke(ColorPickerResult);
            colorPickerPublicEvent?.Invoke();
        }

        #endregion

        private void Awake()
        {
            if (targetPaletteImage == null)
            {
                Debug.LogError("Color Picker: Target Palette Image is missing!");
                return;
            }
            if (targetPaletteImage.sprite == null)
            {
                Debug.LogError("Color Picker: Target Palette Image does not contain any sprite!");
                return;
            }

            paletteImgSrc = targetPaletteImage.sprite.texture;
            if (paletteImgSrc.isReadable == false)
            {
                Debug.LogError("Color Picker: Target Palette Image sprite is not set to 'Readable'! Select the sprite source texture and enable the 'Read/Write Enabled' property.");
                return;
            }
        }
    }
}