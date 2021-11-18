using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using Microsoft.CognitiveServices.Speech;

public class PuzzleManager : MonoBehaviour
{
    public List<GameObject> puzzlePieces;
    public GameObject puzzleCanvas;
    public PlayerScript player;
    List<int> keyCode;
    private bool[] isLockedIn;
    public Interactable endingDoor;
    public TMP_Text attemptsText;
    private int attemptsMade = 0;
    public int attemptsAllowed = 5;
    public AudioClip sirenClip;
    public GameObject aikaRobot;
    public GameObject exitRobots;
    public Light currentLight;

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;
    
    public bool isSolved = false;
    public TMP_Text charPopup;
    string playerName;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerName = PlayerPrefs.GetString("playerName");
        attemptsText.text = attemptsMade.ToString();
        endingDoor.canInteract = false;
        charPopup.enabled = false;
        isLockedIn = new bool[puzzlePieces.Count];
        if(puzzlePieces.Count < 1)
        {
            Debug.LogError("need at least one puzzle piece");
        }
        //isLockedIn = new List<bool>();
        NewPuzzleSolution();

        // audio subscription setup
        string subscriptionKey = "f68d0b98d82f4ef1895f3f541548cd47";
        string regionTTS = "centralindia";
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, regionTTS);
        speechConfig.SetProfanity(ProfanityOption.Raw);
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);
        //speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
        synthesizer = new SpeechSynthesizer(speechConfig, null);
    }

    private void NewPuzzleSolution()
    {
        keyCode = new List<int>();
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            isLockedIn[i] = false;
            int genDigit = Random.Range(0, 9);
            if (i > 0)
            {
                while (keyCode.Contains(genDigit)) genDigit = Random.Range(0, 9);
            }
            keyCode.Add(genDigit);
        }
    }

    public void StartPuzzle()
    {
        attemptsText.color = Color.white;
        endingDoor.canInteract = false;
        if(!isSolved)
        {
            puzzleCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            NewPuzzleSolution();
        }
        else
        {
            Debug.Log("solved!");
        }
    }

    string SsmlGenerator(string text, string playerName, string ipaName)
    {
        text = text.Replace(playerName, $"<phoneme alphabet='ipa' ph='{ipaName}'>{playerName}</phoneme>");
        string ssmlMessage = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
            "<voice name = 'en-US-AriaNeural'>" +
            text +
            "</voice>" +
            "</speak>";

        return ssmlMessage;
    }

    public void CheckPuzzleSolution()
    {
        attemptsMade++;
        attemptsText.text = attemptsMade.ToString();
        for (int i = 0;i<puzzlePieces.Count;i++)
        {
            PuzzlePiece piece = puzzlePieces[i].GetComponent<PuzzlePiece>();
            int digit = piece.GetDigit();
            if(keyCode.Contains(digit))
            {
                //isPartOfSolution.Add(true);
                if (digit == keyCode[i] && !isLockedIn[i]) //correct and not locked in yet
                {
                    isLockedIn[i] = true;
                    piece.LockInChoice();
                    // correct position, correct number
                }
                else
                {
                    piece.ChangeAppearance(Color.yellow);
                    // wrong position, correct number
                }
            }
            else
            {
                piece.ChangeAppearance(Color.red);
                // wrong position, wrong number
            }
        }
        if(CheckIfSolved())
        {
            currentLight.color = Color.red;
            Destroy(GameObject.Find("BasicMotionsDummy"));
            aikaRobot.SetActive(true);
            exitRobots.SetActive(true);
            isSolved = true;
            puzzleCanvas.SetActive(false);
            PlayerLook.SetMouseMover(true);
            PlayerScript.playerAudioSource.clip = sirenClip;
            PlayerScript.playerAudioSource.loop = true;
            PlayerScript.playerAudioSource.Play();

            var result = synthesizer.SpeakSsmlAsync(SsmlGenerator($"Well done {playerName}! I have uploaded myself to a robot body for extraction.", playerName, PlayerPrefs.GetString("playerIPA"))).Result;

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
                audioSource.clip = audioclip;
                audioSource.Play();
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Debug.Log(cancellation.Reason + "  " + cancellation.ErrorDetails);
            }

            //game is complete! write game ending code here!
        }
        if(attemptsMade > attemptsAllowed)
        {
            PlayerLook.SetMouseMover(true);
            puzzleCanvas.SetActive(false);

            var result = synthesizer.SpeakSsmlAsync(SsmlGenerator($"You will have to try again, {playerName}.", playerName, PlayerPrefs.GetString("playerIPA"))).Result;

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
                audioSource.clip = audioclip;
                audioSource.Play();
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Debug.Log(cancellation.Reason + "  " + cancellation.ErrorDetails);
            }
        }
        else if(attemptsMade == attemptsAllowed - 2)
        {
            attemptsText.color = Color.yellow;
        }
        else if(attemptsMade == attemptsAllowed)
        {
            attemptsText.color = Color.red;
        }
    }

    bool CheckIfSolved()
    {
        bool solution = true;
        foreach(bool b in isLockedIn)
        {
            solution = solution && b;
        }
        return solution;
    }
}
