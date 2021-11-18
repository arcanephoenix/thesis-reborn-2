using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NameEntryControlScript : MonoBehaviour
{
    private int stringCounter = 0;
    public string[] introDialogue;
    public TMP_Text dialogText;
    public TMP_Text finalButton;
    private void Start()
    {
        dialogText.text = introDialogue[0];
    }

    public void NextButtonPressed()
    {

        if (stringCounter < introDialogue.Length - 1)
        {
            stringCounter++;
            dialogText.text = introDialogue[stringCounter];
            if (stringCounter == introDialogue.Length - 1) finalButton.text = "Acknowledge";
        }
        else
        {
            PlayerPrefs.SetString("playerName", "Alex");
            // go to game scene, prepare
            SceneManager.LoadScene("SampleScene");
        }
    } 
        
}
