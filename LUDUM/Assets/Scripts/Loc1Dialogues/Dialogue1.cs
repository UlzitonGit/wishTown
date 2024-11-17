using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image planeDarkImage;
    public TMP_Text dialogueText;
    public TMP_Text taskText;
    public TMP_Text raycastText;
    public AudioSource audioSource;
    public AudioClip[] dialogueClips;
    public AudioSource grohotSound;
    public AudioSource openSound;
    public AudioSource krikiSound;
    public GameObject key;
    public GameObject door;
    public GameObject secondDoor;
    public Animator clownAnimator;
    public string nextSceneName;
    public GameObject DialogueCor;
    public GameObject ExitDialogue;
    [SerializeField] Transform lookTransform;
    private bool hasKey = false;
    private bool canPickUpKey = false;
    private bool triggerT1Activated = false;
    private bool triggerRoomActivated = false;
    private bool triggerNsceneActivated = false;
    private bool dialogueWindowVisible = true;

    void Start()
    {
        taskText.gameObject.SetActive(false);
        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        float fadeDuration = 1.5f;
        Color initialColor = planeDarkImage.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            planeDarkImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1 - normalizedTime);
            yield return null;
        }
        planeDarkImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        yield return ShowDialogue("Clown: At this pantry is cozy", 3f, 0);
        yield return ShowDialogue("Misha: ...", 3f, 1);

        grohotSound.Play();
        dialogueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        dialogueText.gameObject.SetActive(true);

        yield return ShowDialogue("Clown: It seems bad, doesn't it? Let's go for a walk", 3f, 2);
        yield return ShowDialogue("Misha: Come on, but the door's locked", 3f, 3);
        yield return ShowDialogue("Clown: we should find the key", 3f, 4);

        dialogueText.gameObject.SetActive(false);

        taskText.text = "Task: Find the key";
        taskText.gameObject.SetActive(true);
        canPickUpKey = true;
    }

    IEnumerator ShowDialogue(string message, float duration, int clipIndex)
    {
        Debug.Log("Showing dialogue: " + message);
        dialogueText.text = message;

        if (clipIndex >= 0 && clipIndex < dialogueClips.Length)
        {
            Debug.Log("Playing audio clip: " + clipIndex);
            audioSource.clip = dialogueClips[clipIndex];
            audioSource.Play();
        }
       

        if (message.Contains("Clown"))
        {
            clownAnimator.SetBool("isTalking", true);
        }

        yield return new WaitForSeconds(duration);

        if (message.Contains("Clown"))
        {
            clownAnimator.SetBool("isTalking", false);
        }


        if (message == "Clown: Okay, what am I saying? Let's go to our room")
        {
            DialogueCor.SetActive(false);
        }
    }

    void Update()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == key && !hasKey && canPickUpKey)
            {
                raycastText.text = "Press \"E\" to take a key";
                raycastText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hasKey = true;
                    key.SetActive(false);
                    raycastText.gameObject.SetActive(false);
                    taskText.text = "Task: Open the door";
                }
            }
            else if (hit.collider.gameObject == door && hasKey)
            {
                raycastText.text = "Press \"E\" to open the door";
                raycastText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.SetActive(false);
                    secondDoor.SetActive(true);
                    openSound.Play();
                    raycastText.gameObject.SetActive(false);
                    taskText.gameObject.SetActive(false);
                }
            }
            else
            {
                raycastText.gameObject.SetActive(false);
            }
        }
        else
        {
            raycastText.gameObject.SetActive(false);
        }
    }

    public void HandleTriggerT1()
    {
        if (!triggerT1Activated)
        {
            triggerT1Activated = true;
            StartCoroutine(TriggerT1Sequence());
        }
    }

    public void HandleTriggerRoom()
    {
        if (!triggerRoomActivated)
        {
            triggerRoomActivated = true;
            StartCoroutine(TriggerRoomSequence());
        }
    }

    public void HandleTriggerNscene()
    {
        if (!triggerNsceneActivated)
        {
            triggerNsceneActivated = true;
            StartCoroutine(TriggerNsceneSequence());
        }
    }

    IEnumerator TriggerT1Sequence()
    {
        krikiSound.Play();
        yield return new WaitForSeconds(1.5f);
        dialogueText.gameObject.SetActive(true);

        yield return ShowDialogue("Misha: Is it because of me? Why do they fight all the time?", 3f, 5);
        yield return ShowDialogue("Clown: It's not true, it's just that sometimes it happens and yeeeaaaah, nobody gets away from it.", 5f, 6);
        yield return ShowDialogue("Misha: Away from it?", 3f, 7);
        yield return ShowDialogue("Clown: Okay, what am I saying? Let's go to our room", 3f, 8);

        dialogueWindowVisible = false;
        dialogueText.gameObject.SetActive(false);

        taskText.text = "Task: Go to your room";
        taskText.gameObject.SetActive(true);
    }

    IEnumerator TriggerRoomSequence()
    {
        if (!dialogueWindowVisible)
        {
            dialogueText.gameObject.SetActive(true);
            yield return ShowDialogue("Clown: Come on, I'll show you my secret, secret room that holds secrets - your closet", 2f, 9);
            dialogueWindowVisible = true;
            StartCoroutine(HideDialogueAfterSeconds(2f));
        }

        taskText.text = "Task: Enter the closet";
        taskText.gameObject.SetActive(true);
        ExitDialogue.gameObject.SetActive(false);
    }

    IEnumerator HideDialogueAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dialogueText.gameObject.SetActive(false);
    }

    IEnumerator TriggerNsceneSequence()
    {
        taskText.gameObject.SetActive(false);

        float fadeDuration = 1.5f;
        Color initialColor = planeDarkImage.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            planeDarkImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, normalizedTime);
            yield return null;
        }
        planeDarkImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}