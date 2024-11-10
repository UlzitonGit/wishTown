using UnityEngine;
using TMPro;

public class RamManager : MonoBehaviour
{
    public GameObject[] rams;
    public GameObject[] heldRams;
    public GameObject[] insertPoints;
    public GameObject[] placedRams;
    public GameObject switchObject;

    public AudioClip pickupSound;
    public AudioClip placeSound;
    public AudioSource audioSource;
    public DiManager dialogueManager;
    public TMP_Text messageText;

    private int currentHeldRamIndex = -1;
    private int placedRamCount = 0;

    void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentHeldRamIndex == -1)
        {
            CheckForRamPickup();
        }
        else
        {
            CheckForRamPlacement();
        }
    }

    void CheckForRamPickup()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5f))
        {
            bool lookingAtRam = false;
            for (int i = 0; i < rams.Length; i++)
            {
                if (hit.collider.gameObject == rams[i])
                {
                    messageText.text = "Press 'E' to pick up";
                    messageText.gameObject.SetActive(true);
                    lookingAtRam = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        rams[i].SetActive(false);
                        heldRams[i].SetActive(true);
                        currentHeldRamIndex = i;

                        if (audioSource != null && pickupSound != null)
                        {
                            audioSource.PlayOneShot(pickupSound);
                        }
                    }
                }
            }
            if (!lookingAtRam)
            {
                messageText.gameObject.SetActive(false);
            }
        }
        else
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void CheckForRamPlacement()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject == insertPoints[currentHeldRamIndex])
            {
                messageText.text = "Press 'E' to put a ram";
                messageText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldRams[currentHeldRamIndex].SetActive(false);
                    placedRams[currentHeldRamIndex].SetActive(true);
                    currentHeldRamIndex = -1;
                    placedRamCount++;

                    if (audioSource != null && placeSound != null)
                    {
                        audioSource.PlayOneShot(placeSound);
                    }

                    if (placedRamCount == rams.Length)
                    {
                        switchObject.SetActive(false);
                        dialogueManager.StartNewDialogueSequence();
                    }
                }
            }
            else
            {
                messageText.gameObject.SetActive(false);
            }
        }
        else
        {
            messageText.gameObject.SetActive(false);
        }
    }
}