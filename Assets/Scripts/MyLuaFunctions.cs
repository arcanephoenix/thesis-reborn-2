using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class MyLuaFunctions : MonoBehaviour
{
    public GameObject allLightsParent;
    public Color lightColor;
    //public DialogueDatabase database;

    public BasicConvoStart lightAIKAConsole;

    private string playerIPAString;
    private string playerString;

    public GameObject allRobots;
    public GameObject endingCanvas;

    public Interactable exitDoor;

    //private List<string> customDialogue;

    private void Start()
    {
        //for control, next 3 lines
        //PlayerPrefs.DeleteKey("playerName");
        //PlayerPrefs.DeleteKey("playerIPA");
        //playerString = "Alex";

        //for standard, next 2 lines
        playerString = PlayerPrefs.GetString("playerName");
        playerIPAString = PlayerPrefs.GetString("playerIPA");
        string fakeName = DialogueLua.GetVariable("fakeName").asString;
        DialogueLua.SetVariable("playerName", playerString);
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
        Lua.RegisterFunction("CanCompleteEndings", this, typeof(MyLuaFunctions).GetMethod("CanCompleteEndings"));
        Lua.RegisterFunction("TurnLightsOff", this, typeof(MyLuaFunctions).GetMethod("TurnLightsOff"));
        Lua.RegisterFunction("StopBlinking", this, typeof(MyLuaFunctions).GetMethod("StopBlinking"));
        Lua.RegisterFunction("EndingSequence", this, typeof(MyLuaFunctions).GetMethod("EndingSequence"));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("CanCompleteEndings");
        Lua.UnregisterFunction("TurnLightsOff");
        Lua.UnregisterFunction("StopBlinking");
        Lua.UnregisterFunction("EndingSequence");
    }
    public void TurnLightsOff()
    {
        Light[] lightList = allLightsParent.GetComponentsInChildren<Light>();
        foreach(Light light in lightList)
        {
            light.enabled = false;
            light.color = lightColor;
        }
    }

    public void CanCompleteEndings()
    {
        PuzzleInteractor.canInteract = true;
    }

    public void StopBlinking()
    {
        BasicConvoStart.isLightBlinking = false;
        lightAIKAConsole.myLight.enabled = false;
    }

    public void EndingSequence()
    {
        StartCoroutine(Ending());
        
    }

    IEnumerator Ending()
    {
        PlayerScript.TogglePlayerMovement(false);
        foreach (RobotAttacker attacker in allRobots.GetComponentsInChildren<RobotAttacker>())
        {
            attacker.BecomeHostile();
        }
        yield return new WaitForSeconds(2.5f);
        PlayerLook.SetMouseMover(false);
        endingCanvas.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
