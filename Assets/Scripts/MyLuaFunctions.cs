using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class MyLuaFunctions : MonoBehaviour
{
    public GameObject nameEntryForm;
    private void OnEnable()
    {
        Lua.RegisterFunction("ShowForm", this, typeof(MyLuaFunctions).GetMethod("ShowForm"));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("ShowForm");
    }
    public void ShowForm()
    {
        Debug.Log("running lua function showform");
        nameEntryForm.SetActive(true);
    }
}
