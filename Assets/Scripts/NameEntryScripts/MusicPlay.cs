using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using PixelCrushers.DialogueSystem;

public class MusicPlay : MonoBehaviour
{
    private string subscriptionKey;
    private string regionTTS;

    public DialogueDatabase database;

    private object threadlocker = new object();
    private bool waitingForSpeak;
    //public string message;
    private string ssmlMessage;
    //public AudioSource audioPlayer;

    SpeechConfig speechConfig;
    SpeechSynthesizer synthesizer;

    int i = 0;
    

    string ssmlGenerator(string name, string nameIPA)
    {
        string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
            "<voice name = 'en-US-AriaNeural'>" +
            "<phoneme alphabet='" +
            nameIPA +
            "'>" +
            name +
            "</voice>" +
            "</speak>";
        return ssmlMessage;
    }

    string ssmlGenerator(string text)
    {
        string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
            "<voice name = 'en-US-AriaNeural'>" +
            text +
            "</voice>" +
            "</speak>";
        return ssmlMessage;
    }

    private void Start()
    {
        subscriptionKey = "f68d0b98d82f4ef1895f3f541548cd47";
        regionTTS = "centralindia";
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, regionTTS);
        speechConfig.SetProfanity(ProfanityOption.Raw);
        //speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);
        //foreach (Conversation convo in database.conversations)
        //{
            foreach(DialogueEntry dialogueEntry in database.conversations[0].dialogueEntries)
            {
            if (dialogueEntry.currentDialogueText.Contains("[var="))
            {
                dialogueEntry.currentDialogueText = dialogueEntry.currentDialogueText.Replace("[var=fakeName]", DialogueLua.GetVariable("fakeName").asString);
                dialogueEntry.currentDialogueText = dialogueEntry.currentDialogueText.Replace("[var=playerName]", DialogueLua.GetVariable("playerName").asString);
            }
            var audioConfig = AudioConfig.FromWavFileOutput("E:\\generatedFiles\\ai1" + i + ".wav");
            i++;
            //speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
            synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
            synthesizer.SpeakSsmlAsync(ssmlGenerator(dialogueEntry.currentDialogueText));
            //Debug.Log(dialogueEntry.currentDialogueText);
                    
                                
            }
        //}
        //var audioConfig = AudioConfig.FromWavFileOutput("E:\\hello.wav");
        //speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
        //synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
        //synthesizer.SpeakSsmlAsync(ssmlGenerator("Welcome to this factory facility, guest."));


    }

    private void OnDestroy()
    {
        synthesizer.Dispose();
    }
}
