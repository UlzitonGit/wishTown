using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToBlackAndChangeScene : MonoBehaviour
{
    public string sceneToLoad;
    public float fadeDuration = 1.5f;

    private Image image;
    private bool isFading = false;
    private float fadeTimer = 0f;

    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component is missing on PanelDark.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            isFading = true;
        }

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            Color color = image.color;
            color.a = alpha;
            image.color = color;

            if (fadeTimer >= fadeDuration)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}