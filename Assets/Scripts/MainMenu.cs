using UnityEngine;

/// <summary>
/// Class taking the role of managing UI objects in the scene.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuElements;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject leaderboardElements;
    [SerializeField] private GameObject controlTypeButtons;
    [SerializeField] private GameObject gameplayElements;
    [SerializeField] private GameObject gameOverElements;
    [SerializeField] private GameObject warningElement;
    
    private GameManager _gm;

    public void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Called when the player clicks on the Leaderboard button in the main menu.    
    /// It deactivates all of the buttons in MainMenuButtons and activates all of the
    /// elements in Leaderboard Elements.
    /// </summary>
    public void ShowLeaderboard()
    {
        leaderboardElements.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    /// <summary>
    /// Hides the leaderboard elements and shows the main menu buttons.
    /// </summary> 
    public void HideLeaderboard()
    {
        leaderboardElements.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    /// <summary>
    /// Called when the player clicks on the 'Controls' button in the main menu.    
    /// It deactivates all of the buttons in MainMenuButtons and activates all of those in ControlTypeButtons.
    /// </summary>
    public void ShowControlTypes()
    {
        controlTypeButtons.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    /// <summary>
    /// Hides the ControlTypeButtons game object and shows the MainMenuButtons game object.
    /// </summary>    
    public void HideControlTypes()
    {
        controlTypeButtons.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    /// <summary>
    /// Called when the player clicks on the 'Human Play' button in the Control Type menu.    
    /// It sets up all of the necessary game objects and variables to start a game with an human player.
    /// </summary>
    public void StartGameHuman()
    {
        gameplayElements.SetActive(true);
        controlTypeButtons.SetActive(false);
        mainMenuElements.SetActive(false);
        _gm.IsPlayerHuman = true;
        _gm.StartGame();
    }

    /// <summary>
    /// Called when the user clicks on the 'AI Play' button in the main menu.    
    /// It sets up all of the necessary game objects and variables to start a game with an AI player.
    /// </summary>
    /// <remarks> It also throws an error if the training data isn't enough for the AI to play. </remarks>
    public void StartGameAI()
    {
        if (!_gm.NaiveBayesClassifier.HasTrainingData())
        {
            warningElement.SetActive(true);
            return;
        }
        
        gameplayElements.SetActive(true);
        controlTypeButtons.SetActive(false);
        mainMenuElements.SetActive(false);
        _gm.IsPlayerHuman = false;
        _gm.StartGame();
    }

    /// <summary>
    /// Called when the player clicks on the 'Return to Main Menu' button.    
    /// It deactivates all of the Gameplay elements and activates all of the Main Menu elements.
    /// </summary>
    public void ReturnToMainMenu()
    {
        gameplayElements.SetActive(false);
        mainMenuElements.SetActive(true);
        mainMenuButtons.SetActive(true);
    }

    /// <summary>
    /// Called when the player dies, aka, when the remaining steps are 0.
    /// It activates the GameOver menu elements.
    /// </summary>
    public void ShowGameOverMenu()
    {
        gameOverElements.SetActive(true);
    }

    /// <summary>
    /// Hides the GameOver menu.
    /// </summary>
    public void HideGameOverMenu()
    {
        gameOverElements.SetActive(false);
    }
}
