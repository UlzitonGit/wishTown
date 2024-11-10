using System.Collections;
using UnityEngine;

public class TriHan : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject switchObject;
    public float activationDuration = 1f;
    public AudioClip triggerAudioClip;
    public AudioSource audioSource;
    public DiManager dialogueManager;

    private bool hasTriggered = false;

    private void Start()
    {
        switchObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ActivateObjects());
        }
    }

    private IEnumerator ActivateObjects()
    {
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        audioSource.clip = triggerAudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(activationDuration);

        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        dialogueManager.StartDialogue();
    }
}