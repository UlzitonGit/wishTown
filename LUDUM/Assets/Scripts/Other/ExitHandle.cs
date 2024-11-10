using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class ExitTrigger : MonoBehaviour
{
    public Image darkPanel;
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float duration = 2f;
        float elapsedTime = 0f;

        Color initialColor = darkPanel.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);

            darkPanel.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}