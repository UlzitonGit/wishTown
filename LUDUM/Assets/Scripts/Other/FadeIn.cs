using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInPanel : MonoBehaviour
{
    public Image darkPanelImage;
    public float fadeDuration = 2f;

    private void Start()
    {
        darkPanelImage.color = new Color(darkPanelImage.color.r, darkPanelImage.color.g, darkPanelImage.color.b, 1f);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            darkPanelImage.color = new Color(darkPanelImage.color.r, darkPanelImage.color.g, darkPanelImage.color.b, alpha);
            yield return null;
        }
    }
}