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
    
    private GameManager gm;
    public bool showLeaderboardAfterGame;

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
        gm.StartGame(true);
    }

    public void StartGameAI()
    {
        GameplayElements.SetActive(true);
        ControlTypeButtons.SetActive(false);
        MainMenuElements.SetActive(false);
        gm.StartGame(false);
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
        ReturnToMainMenu();
        if (showLeaderboardAfterGame)
        {
            ShowLeaderboard();
        }
    }
}
