using UnityEngine;
using TMPro;

public class PipePuzzle : MonoBehaviour
{
    public Transform[] pipesX;
    public Transform[] pipesY;
    public Transform[] pipesZ;
    public Transform[] pipesZCheck;
    public float interactionDistance = 5f;
    public LayerMask pipeLayer;
    public Vector3[] correctRotationsX;
    public Vector3[] correctRotationsY;
    public Vector3[] correctRotationsZ;
    public Vector3[] correctRotationsZCheck1;
    public Vector3[] correctRotationsZCheck2;
    public AudioSource rotateSound;
    public AudioSource puzzleSolvedSound;

    public TextMeshProUGUI rotationMessageTMP;
    public GameObject cursorPanel;

    public PuzzleManager puzzleManager;

    private Camera mainCamera;
    private bool puzzleSolved = false;

    void Start()
    {
        mainCamera = Camera.main;
        rotationMessageTMP.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!puzzleSolved)
        {
            CheckForPipeInteraction();

            if (AreAllPipesCorrectlyRotated())
            {
                puzzleSolved = true;
                puzzleSolvedSound.Play();
                rotationMessageTMP.gameObject.SetActive(false);
                puzzleManager.Puzzle2Solved();
            }
        }
    }

    void CheckForPipeInteraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, pipeLayer))
        {
            Transform pipe = hit.transform;

            if (System.Array.IndexOf(pipesX, pipe) != -1 || System.Array.IndexOf(pipesY, pipe) != -1 || System.Array.IndexOf(pipesZ, pipe) != -1 || System.Array.IndexOf(pipesZCheck, pipe) != -1)
            {
                HandlePipeRotation(pipe);
                rotationMessageTMP.gameObject.SetActive(true);
                cursorPanel.SetActive(false);
            }
        }
        else
        {
            rotationMessageTMP.gameObject.SetActive(false);
            cursorPanel.SetActive(true);
        }
    }

    void HandlePipeRotation(Transform pipe)
    {
        rotationMessageTMP.text = "Press 'E' to rotate";

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 axis = Vector3.zero;
            if (System.Array.IndexOf(pipesX, pipe) != -1)
                axis = Vector3.right;
            else if (System.Array.IndexOf(pipesY, pipe) != -1)
                axis = Vector3.up;
            else if (System.Array.IndexOf(pipesZ, pipe) != -1 || System.Array.IndexOf(pipesZCheck, pipe) != -1)
                axis = Vector3.forward;

            RotatePipe(pipe, axis);
            rotateSound.Play();
        }
    }

    void RotatePipe(Transform pipe, Vector3 axis)
    {
        Quaternion rotation = Quaternion.Euler(axis * 90f);
        pipe.localRotation = pipe.localRotation * rotation;

        Vector3 currentRotation = pipe.localEulerAngles;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360f);
        currentRotation.y = Mathf.Repeat(currentRotation.y, 360f);
        currentRotation.z = Mathf.Repeat(currentRotation.z, 360f);

        pipe.localEulerAngles = currentRotation;
    }

    bool IsPipeCorrectlyRotated(Transform pipe, Vector3 axis)
    {
        Vector3 currentRotation = pipe.localEulerAngles;
        int pipeIndex = GetPipeIndex(pipe);

        if (System.Array.IndexOf(pipesX, pipe) != -1)
        {
            return Mathf.Approximately(currentRotation.x, correctRotationsX[pipeIndex].x);
        }
        else if (System.Array.IndexOf(pipesY, pipe) != -1)
        {
            return Mathf.Approximately(currentRotation.y, correctRotationsY[pipeIndex].y);
        }
        else if (System.Array.IndexOf(pipesZ, pipe) != -1)
        {
            return Mathf.Approximately(currentRotation.z, correctRotationsZ[pipeIndex].z);
        }
        else if (System.Array.IndexOf(pipesZCheck, pipe) != -1)
        {
            return (Mathf.Approximately(currentRotation.z, correctRotationsZCheck1[pipeIndex].z) ||
                    Mathf.Approximately(currentRotation.z, correctRotationsZCheck2[pipeIndex].z));
        }

        return false;
    }

    int GetPipeIndex(Transform pipe)
    {
        int index = System.Array.IndexOf(pipesX, pipe);
        if (index != -1) return index;

        index = System.Array.IndexOf(pipesY, pipe);
        if (index != -1) return index;

        index = System.Array.IndexOf(pipesZ, pipe);
        if (index != -1) return index;

        index = System.Array.IndexOf(pipesZCheck, pipe);
        return index;
    }

    bool AreAllPipesCorrectlyRotated()
    {
        for (int i = 0; i < pipesX.Length; i++)
        {
            if (!IsPipeCorrectlyRotated(pipesX[i], Vector3.right))
            {
                return false;
            }
        }
        for (int i = 0; i < pipesY.Length; i++)
        {
            if (!IsPipeCorrectlyRotated(pipesY[i], Vector3.up))
            {
                return false;
            }
        }
        for (int i = 0; i < pipesZ.Length; i++)
        {
            if (!IsPipeCorrectlyRotated(pipesZ[i], Vector3.forward))
            {
                return false;
            }
        }
        for (int i = 0; i < pipesZCheck.Length; i++)
        {
            if (!IsPipeCorrectlyRotated(pipesZCheck[i], Vector3.forward))
            {
                return false;
            }
        }
        return true;
    }
}