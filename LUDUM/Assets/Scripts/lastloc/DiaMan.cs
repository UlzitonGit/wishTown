using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DiaMan : MonoBehaviour
{
    
    public Image panelDark;
    public GameObject womanMan;
    public TMP_Text interactionText;
    public TMP_Text dialogueText;
    public TMP_Text taskText;
    public TMP_Text rayC;
    public AudioClip[] audioClips;
    public string[] dialogueMessages;
    public float[] messageDurations;
    public string nextSceneName;
    public float interactionDistance = 5f;
    
    private AudioSource audioSource;
    private int currentMessageIndex = 0;
    private bool isLookingAtWomanMan = false;
    private bool isDialogueActive = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeInPanel());
        taskText.text = "Task: Talk to strange entities";
        taskText.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(false);
        rayC.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckPlayerLookingAtWomanMan();

        if (isLookingAtWomanMan && !isDialogueActive)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, womanMan.transform.position);
            if (distance <= interactionDistance)
            {
                rayC.text = "Press \"E\" to talk";
                rayC.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(StartDialogue());
                }
            }
            else
            {
                rayC.gameObject.SetActive(false);
            }
        }
        else
        {
            rayC.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeInPanel()
    {
        float duration = 2f;
        float elapsed = 0f;
        Color panelColor = panelDark.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelColor.a = Mathf.Lerp(1, 0, elapsed / duration);
            panelDark.color = panelColor;
            yield return null;
        }
    }

    private void CheckPlayerLookingAtWomanMan()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            isLookingAtWomanMan = hit.collider.gameObject == womanMan;
        }
        else
        {
            isLookingAtWomanMan = false;
        }
    }

    private IEnumerator StartDialogue()
    {
        isDialogueActive = true;
        taskText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        rayC.gameObject.SetActive(false);

        for (int i = 0; i < dialogueMessages.Length; i++)
        {
            dialogueText.text = dialogueMessages[i];
            audioSource.clip = audioClips[i];
            audioSource.Play();

            yield return new WaitForSeconds(messageDurations[i]);
        }

        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        StartCoroutine(FadeOutPanel());
    }

    private IEnumerator FadeOutPanel()
    {
        float duration = 2f;
        float elapsed = 0f;
        Color panelColor = panelDark.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelColor.a = Mathf.Lerp(0, 1, elapsed / duration);
            panelDark.color = panelColor;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}