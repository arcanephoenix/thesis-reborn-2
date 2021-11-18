using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingSetter : MonoBehaviour
{
    public GameObject endingCanvas;
    public TMP_Text endingText;
    public TMP_Text finText;

    public void OnExit()
    {
        Application.Quit();
    }
}
