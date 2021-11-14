using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
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

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;
    public AudioSource audioPlayer;

    string[] consonants, vowels, condesc, vowdesc;

    public string[] introDialogue1;
    public string[] introDialogue2;

    public void PlayAudio()
    {
        string newMessage = string.Empty;

        var result = synthesizer.SpeakSsmlAsync(GenerateSSMLText(nameInput.text, nameIPA.text)).Result;

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            var samplecount = result.AudioData.Length / 2;
            var audiodata = new float[samplecount];
            for (int i = 0; i < samplecount; i++)
            {
                audiodata[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0f;
            }
            var audioclip = AudioClip.Create("SynthAudio", samplecount, 1, 24000, false);
            audioclip.SetData(audiodata, 0);
            audioPlayer.clip = audioclip;
            audioPlayer.Play();
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Debug.Log(cancellation.Reason + "  " + cancellation.ErrorDetails);
        }

    }

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


        // audio subscription setup
        string subscriptionKey = "f68d0b98d82f4ef1895f3f541548cd47";
        string regionTTS = "centralindia";
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, regionTTS);
        speechConfig.SetProfanity(ProfanityOption.Raw);
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);
        //speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
        synthesizer = new SpeechSynthesizer(speechConfig, null);

    }

    private string GenerateSSMLText(string name, string nameIPA)
    {
        string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
            "<voice name = 'en-US-AriaNeural'>" +
            "<phoneme alphabet='ipa' ph='" + nameIPA +"'>" +
            name +
            "</phoneme>" +
            "</voice>" +
            "</speak>";
        return ssmlMessage;
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
