using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class VeryBasicScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(DialogueLua.GetVariable("playerName").AsString);

    }
}
