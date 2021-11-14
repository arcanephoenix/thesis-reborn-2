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

    private string playerIPAString;
    private string playerString;

    private List<string> customDialogue;

    private void Start()
    {
        customDialogue = new List<string>();
        playerString = PlayerPrefs.GetString("playerName");
        playerIPAString = PlayerPrefs.GetString("playerIPA");
        string fakeName = DialogueLua.GetVariable("fakeName").asString;
        DialogueLua.SetVariable("playerName", playerString);
        foreach (Conversation convo in database.conversations)
        {
            foreach (DialogueEntry dialogueEntry in convo.dialogueEntries)
            {
                if (dialogueEntry.currentDialogueText.Contains("[var=playerName]"))
                {
                    //Debug.Log(dialogueEntry.id + " " + dialogueEntry.currentDialogueText);
                    customDialogue.Add(dialogueEntry.currentDialogueText);
                }
                
            }
        }

        for(int i=0;i<customDialogue.Count;i++)
        {
            customDialogue[i] = customDialogue[i].Replace("[var=[playerName]", playerString);

        }


    }

    string ssmlGenerator(string text)
    {
        string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
            "<voice name = 'en-US-JacobNeural'>" +
            text +
            "</voice>" +
            "</speak>";
        return ssmlMessage;
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
