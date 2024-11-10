using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public DialogueM dialogueManager;

    private bool isPuzzle1Solved = false;
    private bool isPuzzle2Solved = false;

    public void Puzzle1Solved()
    {
        isPuzzle1Solved = true;
        CheckAllPuzzlesSolved();
    }

    public void Puzzle2Solved()
    {
        isPuzzle2Solved = true;
        CheckAllPuzzlesSolved();
    }

    private void CheckAllPuzzlesSolved()
    {
        if (isPuzzle1Solved && isPuzzle2Solved)
        {
            dialogueManager.StartPostPuzzleDialogue();
        }
    }
}