using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuElements;
    [SerializeField] private GameObject MainMenuButtons;
    [SerializeField] private GameObject LeaderboardElements;
    [SerializeField] private GameObject ControlTypeButtons;
    [SerializeField] private GameObject GameplayElements;
    [SerializeField] private GameObject GameOverElements;
    [SerializeField] private GameObject WarningElement;
    
    private GameManager gm;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void ShowLeaderboard()
    {
        LeaderboardElements.SetActive(true);
        MainMenuButtons.SetActive(false);
    }

    public void HideLeaderboard()
    {
        LeaderboardElements.SetActive(false);
        MainMenuButtons.SetActive(true);
    }

    public void ShowControlTypes()
    {
        ControlTypeButtons.SetActive(true);
        MainMenuButtons.SetActive(false);
    }

    public void HideControlTypes()
    {
        ControlTypeButtons.SetActive(false);
        MainMenuButtons.SetActive(true);
    }

    public void StartGameHuman()
    {
        GameplayElements.SetActive(true);
        ControlTypeButtons.SetActive(false);
        MainMenuElements.SetActive(false);
        gm.IsPlayerHuman = true;
        gm.StartGame();
    }

    public void StartGameAI()
    {
        if (!gm.NaiveBayesClassifier.HasTrainingData())
        {
            WarningElement.SetActive(true);
            return;
        }
        
        GameplayElements.SetActive(true);
        ControlTypeButtons.SetActive(false);
        MainMenuElements.SetActive(false);
        gm.IsPlayerHuman = false;
        gm.StartGame();
    }

    public void ReturnToMainMenu()
    {
        GameplayElements.SetActive(false);
        MainMenuElements.SetActive(true);
        MainMenuButtons.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        GameOverElements.SetActive(true);
    }

    public void HideGameOverMenu()
    {
        GameOverElements.SetActive(false);
    }
}
