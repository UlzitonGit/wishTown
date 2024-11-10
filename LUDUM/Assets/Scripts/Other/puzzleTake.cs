using UnityEngine;
using System.Collections;
using TMPro;

public class PuzzleInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 3f;

    public GameObject[] puzzlePieces;
    public GameObject[] heldPuzzlePieces;
    public GameObject[] insertionPoints;
    public GameObject[] completedPuzzlePieces;

    public GameObject smileObject;

    public AudioClip pickUpClip;
    public AudioClip puzzleInsertClip;
    public AudioClip winMusicClip;

    public AudioSource playerAudioSource;
    public AudioSource boardAudioSource;
    public TMP_Text messageText;
    public GameObject cursor;

    public PuzzleManager puzzleManager;

    private GameObject currentHeldPiece = null;
    private int currentHeldIndex = -1;
    private int insertedPuzzleCount = 0;
    private bool[] insertionUsed;

    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        insertionUsed = new bool[insertionPoints.Length];
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (currentHeldPiece == null)
        {
            CheckForPuzzlePiece();
        }
        else
        {
            CheckForInsertionPoint();
        }

        if (messageText != null && !messageText.gameObject.activeSelf && cursor != null)
        {
            cursor.SetActive(true);
        }
    }

    void CheckForPuzzlePiece()
    {
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            for (int i = 0; i < puzzlePieces.Length; i++)
            {
                if (hitObject == puzzlePieces[i])
                {
                    ShowMessage("Press 'E' to take");

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        TakePuzzlePiece(i);
                    }
                    return;
                }
            }
        }

        HideMessage();
    }

    void CheckForInsertionPoint()
    {
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            for (int i = 0; i < insertionPoints.Length; i++)
            {
                if (hitObject == insertionPoints[i] && IsCorrectInsertionPoint(i))
                {
                    ShowMessage("Press 'E' to insert");

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        InsertPuzzlePiece(i);
                    }
                    return;
                }
            }
        }

        HideMessage();
    }

    void TakePuzzlePiece(int index)
    {
        puzzlePieces[index].SetActive(false);
        heldPuzzlePieces[index].SetActive(true);
        currentHeldPiece = heldPuzzlePieces[index];
        currentHeldIndex = index;

        PlayAudioClip(playerAudioSource, pickUpClip);
    }

    void InsertPuzzlePiece(int index)
    {
        currentHeldPiece.SetActive(false);
        completedPuzzlePieces[index].SetActive(true);
        currentHeldPiece = null;
        currentHeldIndex = -1;
        insertedPuzzleCount++;
        insertionUsed[index] = true;

        PlayAudioClip(playerAudioSource, puzzleInsertClip);

        if (insertedPuzzleCount == puzzlePieces.Length)
        {
            puzzleManager.Puzzle1Solved();
            StartCoroutine(ShowSmileAfterDelay());
        }
    }

    IEnumerator ShowSmileAfterDelay()
    {
        PlayAudioClip(boardAudioSource, winMusicClip);

        yield return new WaitForSeconds(3f);

        foreach (var piece in completedPuzzlePieces)
        {
            piece.SetActive(false);
        }

        if (smileObject != null)
        {
            smileObject.SetActive(true);
        }
    }

    bool IsCorrectInsertionPoint(int index)
    {
        return currentHeldIndex == index && !insertionUsed[index];
    }

    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
        }

        if (cursor != null)
        {
            cursor.SetActive(false);
        }
    }

    void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void PlayAudioClip(AudioSource audioSource, AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}