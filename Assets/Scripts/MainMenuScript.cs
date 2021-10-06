using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject nameEntryPanel;
    public TMP_Text titleCard;
    public GameObject nameEntryTextBox;
    private TMP_InputField nameEntryInput;
    public string gameTitle = "factory reset";
    private int i = 0;


    private void Start()
    {
        StartCoroutine(FirstStart());
        nameEntryInput = nameEntryTextBox.GetComponent<TMP_InputField>();
    }

    public void NameEntry()
    {
        string playerName = nameEntryInput.text;
        PlayerPrefs.SetString("playerName", playerName);
        SceneManager.LoadScene("Location");
    }

    private IEnumerator FirstStart()
    {
        while (i < gameTitle.Length)
        {
            i++;
            titleCard.text = gameTitle.Substring(0, i);
            yield return new WaitForSeconds(0.8f);
        }
        mainMenuPanel.SetActive(true);
        while (mainMenuPanel.GetComponent<CanvasGroup>().alpha < 1)
        {
            mainMenuPanel.GetComponent<CanvasGroup>().alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    } 

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        nameEntryPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToControls()
    {
        StartCoroutine(ShowControlPanel());
    }
    public void GoToMainMenu()
    {
        StartCoroutine(ShowMainMenuPanel());
    }
    
    private IEnumerator ShowControlPanel()
    {
        while (mainMenuPanel.GetComponent<CanvasGroup>().alpha > 0)
        {
            mainMenuPanel.GetComponent<CanvasGroup>().alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
        while (controlsPanel.GetComponent<CanvasGroup>().alpha < 1)
        {
            controlsPanel.GetComponent<CanvasGroup>().alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

    }

    private IEnumerator ShowMainMenuPanel()
    {
        while (controlsPanel.GetComponent<CanvasGroup>().alpha > 0)
        {
            controlsPanel.GetComponent<CanvasGroup>().alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        while (mainMenuPanel.GetComponent<CanvasGroup>().alpha < 1)
        {
            mainMenuPanel.GetComponent<CanvasGroup>().alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
