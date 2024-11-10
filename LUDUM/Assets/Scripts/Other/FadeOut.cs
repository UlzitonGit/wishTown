using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string nextSceneName;
    public float fadeDuration = 2f;
    public Image darkPanelImage;

    private bool isFadingOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFadingOut)
        {
            isFadingOut = true;
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            darkPanelImage.color = new Color(darkPanelImage.color.r, darkPanelImage.color.g, darkPanelImage.color.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}