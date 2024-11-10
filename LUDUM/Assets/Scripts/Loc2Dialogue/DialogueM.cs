using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueM : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text taskText;
    public GameObject gnome;
    public GameObject wall;
    public Animator clownAnimator;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public GameObject Exit;

    private string[] dialogues = {
        "Slavik: My name is Slavik and I'm the head hatter of this place",
        "Misha: H-Hi, my name is...",
        "Clown: Yes yes yes, I'm a clown, my little friend is Misha",
        "Slavik: What brings you to our small town? Maybe you want to help?",
        "Misha: We don't mind helping",
        "Slavik: Our pipes and main computer broke and the little sheep ran out of the pen. Your help would be appreciated and we will give you a box of chocolates as a gift later!"
    };

    private float[] dialogueDurations = { 3f, 3f, 3f, 3f, 3f, 8f };

    private string[] postPuzzleDialogues = {
        "Voice: They are deceiving you, don't believe them",
        "Misha: Who's that?",
        "Clown: What are you talking about? It's just me and you. Let's go help with the sheep!"
    };

    private float[] postPuzzleDurations = { 3f, 3f, 4f };

    private int currentDialogueIndex = 0;
    private bool isPostPuzzle = false;

    private void Start()
    {
        taskText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        currentDialogueIndex = 0;
        isPostPuzzle = false;
        dialogueText.gameObject.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    public void StartPostPuzzleDialogue()
    {
        currentDialogueIndex = 0;
        isPostPuzzle = true;
        dialogueText.gameObject.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        string[] currentDialogues = isPostPuzzle ? postPuzzleDialogues : dialogues;
        float[] currentDurations = isPostPuzzle ? postPuzzleDurations : dialogueDurations;

        while (currentDialogueIndex < currentDialogues.Length)
        {
            string message = currentDialogues[currentDialogueIndex];
            dialogueText.text = message;
            audioSource.clip = audioClips[currentDialogueIndex];
            audioSource.Play();

            if (message.Contains("Clown"))
            {
                clownAnimator.SetBool("isTalking", true);
            }

            if (message == "Voice: They are deceiving you, don't believe them")
            {
                taskText.gameObject.SetActive(false);
            }

            float waitTime = Mathf.Max(currentDurations[currentDialogueIndex], audioSource.clip.length);
            yield return new WaitForSeconds(waitTime);

            if (message.Contains("Clown"))
            {
                clownAnimator.SetBool("isTalking", false);
            }

            currentDialogueIndex++;
        }

        dialogueText.gameObject.SetActive(false);

        if (!isPostPuzzle)
        {
            if (currentDialogues[currentDialogueIndex - 1].Contains("chocolates as a gift later!"))
            {
                StartCoroutine(ScaleDownGnome());
                wall.SetActive(false);
            }

            taskText.text = "Task: Repair pipes and main computer";
            taskText.gameObject.SetActive(true);
        }
        else
        {
            taskText.text = "Task: Go to the woods";
            taskText.gameObject.SetActive(true);
            Exit.gameObject.SetActive(true);
        }
    }

    private IEnumerator ScaleDownGnome()
    {
        Vector3 originalScale = gnome.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            gnome.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gnome.transform.localScale = targetScale;
    }
}