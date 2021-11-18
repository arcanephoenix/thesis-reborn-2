using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public TMP_Text popupText;
    public bool canInteract = false;
    public EndingSetter endingSetter;

    private void Start()
    {
        popupText.enabled = false;
    }

    public void EndGameLeave()
    {
        endingSetter.endingCanvas.SetActive(true);
        endingSetter.finText.text = $"Thank you {PlayerPrefs.GetString("playerName")}";
        endingSetter.endingText.text = "You left the factory. You did not trust AIKA, and it is unclear what the AI's motives are. In the back of your mind, you wonder - what was so important for the AI to tell me? \n You do not accept payment. You receive no further communication. This job is over.";
    }


}
