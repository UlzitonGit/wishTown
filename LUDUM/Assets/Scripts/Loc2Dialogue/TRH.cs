using UnityEngine;

public class TriggerHandle : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            DialogueM dialogueManager = FindObjectOfType<DialogueM>();
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue();
            }
        }
    }
}