using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    public List<GameObject> puzzlePieces;
    public GameObject puzzleCanvas;
    public PlayerScript player;
    List<int> keyCode;
    private bool[] isLockedIn;
    public bool isSolved = false;
    public TMP_Text charPopup;
    private void Start()
    {
        charPopup.enabled = false;
        isLockedIn = new bool[4];
        if(puzzlePieces.Count < 1)
        {
            Debug.LogError("need at least one puzzle piece");
        }
        //isLockedIn = new List<bool>();
        NewPuzzleSolution();
    }

    private void NewPuzzleSolution()
    {
        keyCode = new List<int>();
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            isLockedIn[i] = false;
            int genDigit = Random.Range(0, 9);
            if (i > 0)
            {
                while (keyCode.Contains(genDigit)) genDigit = Random.Range(0, 9);
            }
            keyCode.Add(genDigit);
        }
    }

    public void StartPuzzle()
    {
        if(!isSolved)
        {
            puzzleCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            NewPuzzleSolution();
        }
        else
        {
            Debug.Log("solved!");
        }
    }

    public void CheckPuzzleSolution()
    {
        for(int i = 0;i<puzzlePieces.Count;i++)
        {
            PuzzlePiece piece = puzzlePieces[i].GetComponent<PuzzlePiece>();
            int digit = piece.GetDigit();
            if(keyCode.Contains(digit))
            {
                //isPartOfSolution.Add(true);
                if (digit == keyCode[i] && !isLockedIn[i]) //correct and not locked in yet
                {
                    isLockedIn[i] = true;
                    piece.LockInChoice();
                    // correct position, correct number
                }
                else
                {
                    piece.ChangeAppearance(Color.yellow);
                    // wrong position, correct number
                }
            }
            else
            {
                piece.ChangeAppearance(Color.red);
                // wrong position, wrong number
            }
        }
        if(CheckIfSolved())
        {
            isSolved = true;
            puzzleCanvas.SetActive(false);
            //game is complete! write game ending code here!
        }
    }

    bool CheckIfSolved()
    {
        bool solution = true;
        foreach(bool b in isLockedIn)
        {
            solution = solution && b;
        }
        return solution;
    }
}
