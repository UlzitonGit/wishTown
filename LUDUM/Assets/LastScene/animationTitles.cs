using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Image panelDark;
    public Animator cameraAnimator;
    public RectTransform panelHigh;
    public RectTransform panelDown;
    public TMP_Text titlesText;
    public float fadeDuration = 1.5f;
    public float moveSpeed = 100f;
    public float moveDuration = 2.0f;
    public float titlesMoveSpeed = 50f;
    public AudioSource audioSource;
    public AudioSource titlesAudioSource;
    public float titlesDelay = 2.0f;
    public string nextSceneName;

    private bool isAnimationPlaying = false;
    private bool isPanelsMoving = false;
    private bool isTitlesMoving = false;

    void Start()
    {
        StartCoroutine(FadeOutPanelDark());
    }

    IEnumerator FadeOutPanelDark()
    {
        float elapsedTime = 0f;
        Color panelColor = panelDark.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            panelDark.color = panelColor;
            yield return null;
        }

        StartCoroutine(PlayTitlesWithDelay());

        cameraAnimator.SetBool("CameraAnim", true);
        isAnimationPlaying = true;
    }

    IEnumerator PlayTitlesWithDelay()
    {
        yield return new WaitForSeconds(titlesDelay);
        titlesAudioSource.Play();
    }

    void Update()
    {
        if (isAnimationPlaying && cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !cameraAnimator.IsInTransition(0))
        {
            isAnimationPlaying = false;
            isPanelsMoving = true;
            StartCoroutine(MovePanels());
        }
    }

    IEnumerator MovePanels()
    {
        float elapsedTime = 0f;
        float initialVolume = audioSource.volume;

        while (elapsedTime < moveDuration)
        {
            float step = moveSpeed * Time.deltaTime;
            panelHigh.anchoredPosition = new Vector2(panelHigh.anchoredPosition.x, panelHigh.anchoredPosition.y - step);
            panelDown.anchoredPosition = new Vector2(panelDown.anchoredPosition.x, panelDown.anchoredPosition.y + step);

            audioSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        isPanelsMoving = false;

        StartCoroutine(MoveTitles());
    }

    IEnumerator MoveTitles()
    {
        isTitlesMoving = true;
        while (titlesAudioSource.isPlaying)
        {
            float step = titlesMoveSpeed * Time.deltaTime;
            titlesText.rectTransform.anchoredPosition = new Vector2(titlesText.rectTransform.anchoredPosition.x, titlesText.rectTransform.anchoredPosition.y - step);
            yield return null;
        }
        isTitlesMoving = false;
        ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}