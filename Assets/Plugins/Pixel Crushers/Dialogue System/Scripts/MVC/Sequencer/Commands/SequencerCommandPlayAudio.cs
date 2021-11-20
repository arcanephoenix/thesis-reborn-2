using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Microsoft.CognitiveServices.Speech;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandPlayAudio : SequencerCommand
    { // Rename to SequencerCommand<YourCommand>
        public SpeechConfig speechConfig;
        public SpeechSynthesizer synthesizer;

        private float stopTime = 0;
        private AudioSource audioSource = null;
        private int nextClipIndex = 2;
        private AudioClip currentClip = null;
        private AudioClip originalClip = null;
        private bool restoreOriginalClip = false; // Don't restore; could stop next entry's AudioWait that runs same frame.

        private string playerName;
        private string ipaName;

        public IEnumerator Start()
        {
            string subscriptionKey = "f68d0b98d82f4ef1895f3f541548cd47";
            string regionTTS = "centralindia";
            speechConfig = SpeechConfig.FromSubscription(subscriptionKey, regionTTS);
            speechConfig.SetProfanity(ProfanityOption.Raw);
            speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);
            //speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
            synthesizer = new SpeechSynthesizer(speechConfig, null);
            playerName = DialogueLua.GetVariable("playerName").asString;
            string fakeName = DialogueLua.GetVariable("fakeName").asString;
            ipaName = PlayerPrefs.GetString("playerIPA");
            string dialogueString;

            if (DialogueManager.currentConversationState != null)
            {
                dialogueString = DialogueManager.currentConversationState.subtitle.formattedText.text;
            }
            else
            {
                dialogueString = "invalid string";
                Debug.LogError("convo is not active");
            }
            
            //string dialogueString = GetParameter(0);
            //Debug.Log("dialogue string: "+dialogueString);
            //dialogueString = dialogueString.Replace("[playerName]", playerName);
            //dialogueString = dialogueString.Replace("[fakeName]", fakeName);
            Transform subject = GetSubject(1);
            nextClipIndex = 2;
            if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait({1})", new System.Object[] { DialogueDebug.Prefix, GetParameters() }));
            audioSource = SequencerTools.GetAudioSource(subject);
            if (audioSource == null)
            {
                if (DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait() command: can't find or add AudioSource to {1}.", new System.Object[] { DialogueDebug.Prefix, subject.name }));
                Stop();
            }
            else
            {
                originalClip = audioSource.clip;
                stopTime = DialogueTime.time + 2; // Give time for yield return null.
                yield return null;
                originalClip = audioSource.clip;
                TryAudioClip(dialogueString);
            }
        }

        string ssmlGenerator(string text, string playerName, string ipaName)
        {
            text = text.Replace(playerName, $"<phoneme alphabet='ipa' ph='{ipaName}'>{playerName}</phoneme>");
            string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
                "<voice name = 'en-US-AriaNeural'>" +
                text +
                "</voice>" +
                "</speak>";

            return ssmlMessage;
        }

        string ssmlGenerator(string text, string playerName)
        {
            string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
                "<voice name = 'en-US-AriaNeural'>" +
                text +
                "</voice>" +
                "</speak>";
            return ssmlMessage;
        }

        private void TryAudioClip(string dialogueText)
        {
            try
            {
                if (string.IsNullOrEmpty(dialogueText))
                {
                    if (DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: PlayAudio() command: No text provided.", new System.Object[] { DialogueDebug.Prefix }));
                    stopTime = 0;
                }
                else
                {
                    string ssmlDialogueText = ssmlGenerator(dialogueText, playerName, ipaName); //for tts version
                    //string ssmlDialogueText = ssmlGenerator(dialogueText, playerName); //for control version
                    var result = synthesizer.SpeakSsmlAsync(ssmlDialogueText).Result;

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
                        if (IsAudioMuted())
                        {
                            if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): waiting but not playing '{1}'; audio is muted.", new System.Object[] { DialogueDebug.Prefix, dialogueText }));
                        }
                        else
                        {
                            if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): playing '{1}'.", new System.Object[] { DialogueDebug.Prefix, dialogueText }));
                            currentClip = audioclip;
                            audioSource.clip = audioclip;
                            audioSource.Play();
                            stopTime = DialogueTime.time + audioclip.length;
                        }

                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Debug.Log(cancellation.Reason + "  " + cancellation.ErrorDetails);
                        Stop();
                    }
                    
                    /*
                    DialogueManager.LoadAsset(audioClipName, typeof(AudioClip),
                        (asset) =>
                        {
                            var audioClip = asset as AudioClip;
                            if (audioClip == null)
                            {
                                if (DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait() command: Clip '{1}' wasn't found.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                                stopTime = 0;
                            }
                            else
                            {
                                if (IsAudioMuted())
                                {
                                    if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): waiting but not playing '{1}'; audio is muted.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                                }
                                else
                                {
                                    if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): playing '{1}'.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                                    currentClip = audioClip;
                                    audioSource.clip = audioClip;
                                    audioSource.Play();
                                }
                                stopTime = DialogueTime.time + audioClip.length;
                            }
                        });
                    */
                }

            }
            catch (System.Exception)
            {
                stopTime = 0;
            }
        }

        public void Update()
        {
            if (DialogueTime.time >= stopTime)
            {
                DialogueManager.UnloadAsset(currentClip);
                if (nextClipIndex < parameters.Length)
                {
                    TryAudioClip(GetParameter(nextClipIndex));
                    nextClipIndex++;
                }
                else
                {
                    currentClip = null;
                    Stop();
                }
            }
        }

        public void OnDialogueSystemPause()
        {
            if (audioSource == null) return;
            audioSource.Pause();
        }

        public void OnDialogueSystemUnpause()
        {
            if (audioSource == null) return;
            audioSource.Play();
        }

        public void OnDestroy()
        {
            if (audioSource != null)
            {
                DialogueManager.UnloadAsset(currentClip);
                if (audioSource.isPlaying && (audioSource.clip == currentClip))
                {
                    audioSource.Stop();
                }
                if (restoreOriginalClip) audioSource.clip = originalClip;
            }
        }

    }

}