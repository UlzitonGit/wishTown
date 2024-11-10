using System.Collections;
using UnityEngine;
using TMPro;

public class DiManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text taskText;
    public AudioClip[] dialogueAudioClips;
    public AudioSource audioSource;
    public Animator clownAnimator;
    public GameObject clown;
    public GameObject[] gnomes;
    public GameObject exitStop;

    public AudioClip specialAudioClip;

    private string[] dialogues = {
        "Misha: What just happened?",
        "Clown: What? We're just taking a walk, by the way, turn off the flashlight or you'll run out of batteries",
        "Misha: Ah, yeah okay, okay, I guess...",
        "Voice: They're lying..."
    };

    private string[] newDialogues = {
        "Voice: Go away quickly Misha, if you don't do it now, they'll get you!",
        "Clown: So that's who you were secretly talking to, I thought we were friends Misha...",
        "Misha: No, I didn't do anything!",
        "Voice: THROW THE CLOWN FAST AND KILL THE GNOMES BY USING FLASHLIGHT!!!"
    };

    private float[] dialogueDurations = { 3f, 5f, 4f, 3f };
    private float[] newDialogueDurations = { 3f, 5f, 2f, 3f };
    private int currentDialogueIndex = 0;

    private void Start()
    {
        taskText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        exitStop.SetActive(true);
    }

    public void StartDialogue()
    {
        StartCoroutine(PlayDialogue(dialogues, dialogueDurations));
    }

    public void StartNewDialogueSequence()
    {
        StartCoroutine(PlayDialogue(newDialogues, newDialogueDurations, true));
    }

    private IEnumerator PlayDialogue(string[] dialogues, float[] durations, bool isFinalSequence = false)
    {
        dialogueText.gameObject.SetActive(true);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueText.text = dialogues[i];

            if (dialogues[i] == "Voice: Go away quickly Misha, if you don't do it now, they'll get you!")
            {
                audioSource.clip = specialAudioClip;
            }
            else
            {
                audioSource.clip = dialogueAudioClips[i];
            }

            if (audioSource.clip == null)
            {
                Debug.LogError("Audio clip is missing for dialogue: " + dialogues[i]);
            }
            else
            {
                Debug.Log("Playing audio clip for dialogue: " + dialogues[i]);
                audioSource.Play();
            }

            if (dialogues[i].Contains("Clown"))
            {
                clownAnimator.SetBool("isTalking", true);
            }

            if (dialogues[i] == "Voice: They're lying...")
            {
                taskText.text = "Task: Get the sheep into their pens";
                taskText.gameObject.SetActive(true);
            }
            else if (dialogues[i] == "Voice: Go away quickly Misha, if you don't do it now, they'll get you!")
            {
                taskText.gameObject.SetActive(false);
            }
            else if (dialogues[i] == "Voice: THROW THE CLOWN FAST AND KILL THE GNOMES BY USING FLASHLIGHT!!!")
            {
                taskText.text = "Task: Destroy the gnomes with a flashlight";
                taskText.gameObject.SetActive(true);
            }

            if (dialogues[i] == "Clown: So that's who you were secretly talking to, I thought we were friends Misha...")
            {
                foreach (GameObject gnome in gnomes)
                {
                    gnome.SetActive(true);
                }
            }

            yield return new WaitForSeconds(durations[i]);

            if (dialogues[i].Contains("Clown"))
            {
                clownAnimator.SetBool("isTalking", false);
            }
        }

        dialogueText.gameObject.SetActive(false);

        if (isFinalSequence)
        {
            clown.SetActive(false);
            StartCoroutine(CheckGnomesAndShowExitTask());
        }
    }

    private IEnumerator CheckGnomesAndShowExitTask()
    {
        while (true)
        {
            bool allGnomesInactive = true;
            foreach (GameObject gnome in gnomes)
            {
                if (gnome.activeSelf)
                {
                    allGnomesInactive = false;
                    break;
                }
            }

            if (allGnomesInactive)
            {
                exitStop.SetActive(false);
                taskText.text = "Task: Leave the woods";
                taskText.gameObject.SetActive(true);
                break;
            }
            else
            {
                exitStop.SetActive(true);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}