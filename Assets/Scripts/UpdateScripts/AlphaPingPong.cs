using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaPingPong : MonoBehaviour
{
    private Image myImage;
    public float InitialValue;
    public float EndValue;
    public float TotalDuration;
    private float currentTime;
    private Color myColor;

    void Start()
    {
        myImage = GetComponent<Image>();
        currentTime = 0f;
        myColor = myImage.color;
        myColor.a = InitialValue;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(currentTime / TotalDuration);
        myColor.a = Mathf.Lerp(InitialValue, EndValue, normalizedTime);
        myImage.color = myColor;

        if (normalizedTime >= 1f)
        {
            currentTime = 0f;
            float tempValue = InitialValue;
            InitialValue = EndValue;
            EndValue = tempValue;
        }
    }
}
