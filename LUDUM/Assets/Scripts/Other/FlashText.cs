using UnityEngine;
using TMPro;

public class TextFlasher : MonoBehaviour
{
    public TMP_Text textComponent;
    public float flashSpeed = 1.0f;

    private Color originalColor;

    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TMP_Text>();
        }

        if (textComponent != null)
        {
            originalColor = textComponent.color;
        }
        else
        {
            Debug.LogError("TMP_Text component not found!");
        }
    }

    void Update()
    {
        if (textComponent != null)
        {
            float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1.0f) / 2.0f;
            Color newColor = originalColor;
            newColor.a = alpha;

            textComponent.color = newColor;
        }
    }
}