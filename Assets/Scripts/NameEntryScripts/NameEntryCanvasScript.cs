using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class NameEntryCanvasScript : MonoBehaviour
{
    private string nameText;
    private string nameIPAText;
    public GameObject nameEntryForm;
    public GameObject dialogForm;
    public GameObject vowelObject, consonantObject;
    public TMP_InputField nameInput;
    public TMP_Text nameIPA;
    public TMP_Text dialogText;
    private int stringCounter = 0;
    //public GameObject[] vowelObjectSet, consonantObjectSet;
    bool isNameEntered = false; // if false, use introDialogue1, if true use introDialogue2

    string[] consonants, vowels, condesc, vowdesc;

    public string[] introDialogue1;
    public string[] introDialogue2;

    private void Start()
    {
        dialogText.text = introDialogue1[0];
        nameIPA.text = "";
        consonants = new string[] { "b", "d", "d͡ʒ", "ð", "f", "g", "h", "j", "k", "l", "m", "n", "ŋ", "p", "ɹ", "s", "ʃ", "t", "t͡ʃ", "θ", "v", "w", "z", "ʒ" };
        vowels = new string[] { "ə", "ɚ", "æ", "aɪ", "aʊ", "ɑ", "eɪ", "ɝ", "ɛ", "i", "ɪ", "oʊ", "ɔ", "ɔɪ", "u", "ʊ", "ʌ" };

        condesc = new string[] { "{b}ed", "{d}ig", "{j}ump", "{th}en", "{f}ive", "{g}ame", "{h}ouse", "{y}es", "{c}at", "{l}ay", "{m}ouse", "{n}ap", "thi{ng}", "s{p}eak", "{r}ed", "{s}eem", "{sh}ip", "{t}rap", "{ch}art", "{th}in", "{v}est", "{w}est", "{z}ero", "vi{s}ion" };
        vowdesc = new string[] { "{a}ren{a}", "read{er}", "tr{a}p", "pr{i}ce", "m{ou}th", "f{a}ther", "f{a}ce", "n{ur}se", "dr{e}ss", "fl{ee}ce", "k{i}t", "g{oa}t", "th{ou}ght", "ch{oi}ce", "g{oo}se", "f{oo}t", "str{u}t" };

        for (int i = 0; i < vowels.Length; i++)
        {
            Transform buttonSet = vowelObject.transform.GetChild(i);
            string s = vowels[i];
            buttonSet.GetChild(0).GetComponentInChildren<TMP_Text>().text = s;
            buttonSet.GetChild(0).GetComponent<Button>().onClick.AddListener(() => AddToString(s));

            buttonSet.GetChild(1).GetComponent<TMP_Text>().text = vowdesc[i];
        }
        
        for (int i = 0;i<consonants.Length;i++)
        {
            Transform buttonSet = consonantObject.transform.GetChild(i);

            string con = consonants[i];
            buttonSet.GetChild(0).GetComponentInChildren<TMP_Text>().text = con;
            buttonSet.GetChild(0).GetComponent<Button>().onClick.AddListener(() => AddToString(con));

            buttonSet.GetChild(1).GetComponent<TMP_Text>().text = condesc[i];
        }
        
    }

    public void Erase()
    {
        if(nameIPA.text != "")
        {
            nameIPA.text = nameIPA.text.Substring(0, nameIPA.text.Length - 1);
        }
    }

    public void AddToString(string str)
    {
        nameIPA.text += str;
    }

    /*
    private void OnEnable()
    {
        Lua.RegisterFunction("ButtonClicked", this, typeof(NameEntryCanvasScript).GetMethod("ButtonClicked"));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("ButtonClicked");
    }
    */
    public void ButtonClicked()
    {
        nameText = nameInput.text;
        nameIPAText = nameIPA.text;
        PlayerPrefs.SetString("playerName", nameText);
        PlayerPrefs.SetString("playerIPA", nameIPAText);
        nameEntryForm.SetActive(false);
    }

    public void NextButtonPressed()
    {
        if(isNameEntered) //introDialogue2
        {
            if(stringCounter < introDialogue2.Length)
            {
                dialogText.text = introDialogue2[stringCounter];
                stringCounter++;
            }
            else
            {
                // go to game scene, prepare
                SceneManager.LoadScene("SampleScene");
            }
        }
        else //introdialogue1
        {
            if(stringCounter < introDialogue1.Length)
            {
                dialogText.text = introDialogue1[stringCounter];
                stringCounter++;
            }
            else //introDialogue1 completed
            {
                stringCounter = 0;
                nameEntryForm.SetActive(true);
                dialogForm.SetActive(false);
            }
        }
    }

    

    public void NameEntryButtonPressed()
    {
        string playerName = nameInput.text;
        string playerIPA = nameIPA.text;
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetString("playerIPA", playerIPA);
        isNameEntered = true;
        dialogForm.SetActive(true);
        nameEntryForm.SetActive(false);
    }

}
