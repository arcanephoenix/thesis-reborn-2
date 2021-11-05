using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class MyLuaFunctions : MonoBehaviour
{
    public GameObject nameEntryForm;
    public GameObject allLightsParent;
    public Color lightColor;
    public DialogueDatabase database;

    private void Start()
    {
        foreach (Conversation convo in database.conversations)
        {
            Debug.Log(convo.id);
            foreach (DialogueEntry dialogueEntry in convo.dialogueEntries)
            {
                Debug.Log(database.GetActor(dialogueEntry.ActorID).Name + dialogueEntry.currentDialogueText);
            }
        }
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("ShowForm", this, typeof(MyLuaFunctions).GetMethod("ShowForm"));
        Lua.RegisterFunction("TurnLightsOff", this, typeof(MyLuaFunctions).GetMethod("TurnLightsOff"));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("ShowForm");
        Lua.UnregisterFunction("TurnLightsOff");
    }
    public void ShowForm()
    {
        Debug.Log("running lua function showform");
        nameEntryForm.SetActive(true);
    }
    public void TurnLightsOff()
    {
        
        //Debug.Log("turning lights off boo");
        Light[] lightList = allLightsParent.GetComponentsInChildren<Light>();
        foreach(Light light in lightList)
        {
            light.enabled = true;
            light.color = lightColor;
        }
    }
}
