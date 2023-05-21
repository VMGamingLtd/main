using UnityEngine;
using UnityEngine.UI;

public class ResetAnimation : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }
}
