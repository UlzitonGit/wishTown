using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TriggerType triggerType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (triggerType)
            {
                case TriggerType.T1:
                    dialogueManager.HandleTriggerT1();
                    break;
                case TriggerType.Room:
                    dialogueManager.HandleTriggerRoom();
                    break;
                case TriggerType.Nscene:
                    dialogueManager.HandleTriggerNscene();
                    break;
            }
        }
    }
}

public enum TriggerType
{
    T1,
    Room,
    Nscene
}