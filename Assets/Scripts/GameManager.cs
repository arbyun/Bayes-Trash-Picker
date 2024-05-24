using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class that controls and parameterizes the game loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject lusoPrefab;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellGrid;
    [SerializeField] private Transform cellBG;
    [SerializeField] private int numOfRows = 5;
    [SerializeField] private int numOfColumns = 5;
    [SerializeField] private int numOfSteps = 20;
    [SerializeField] private int itemPickSuccessScore;
    [SerializeField] private int itemPickFailureScore;
    [SerializeField] private int wallBumpScore;
    
    [Range(0, 100)] [SerializeField] private int probabilityOfTrash = 40;

    private int _score;
    private int _stepsLeft;
    private List<Cell> _cellList;
    private Cell _currentCell;
    private LusoBehaviour _lb;
    private ControlsManager _cm;
    private MainMenu _mm;
    private ScoreManager _sm;
    private bool _firstGame = true;
    private List<Cell> _availableCells;
    
    /// <summary>
    /// The algorithm used to determine the AI decisions.
    /// </summary>
    public NaiveBayesClassifier NaiveBayesClassifier 
    { 
        get; 
        private set; 
    }

    /// <summary>
    /// The list of cells in the grid.
    /// </summary>
    public List<Cell> CellList
    {
        get => _cellList;
        private set => _cellList = value;
    }

    /// <summary>
    /// Whether we have a human playing, or if it's the AI's turn.
    /// </summary>
    public bool IsPlayerHuman
    {
        get; 
        set;
    }

    /// <summary>
    /// The score so far, based on the reward-related parameters.
    /// </summary>
    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            UpdateUI();
        }
    }

    /// <summary>
    /// How many steps we have left before Game Over.
    /// </summary>
    public int StepsLeft
    {
        get => _stepsLeft;
        private set
        {
            _stepsLeft = value;
            if (_stepsLeft == 0) GameOver();
            UpdateUI();
        }
    }

    private void Start()
    {
        _cm = FindObjectOfType<ControlsManager>();
        _mm = FindObjectOfType<MainMenu>();
        _sm = FindObjectOfType<ScoreManager>();
        NaiveBayesClassifier = new NaiveBayesClassifier();
    }

    /// <summary>
    /// Called when the game starts.
    /// It sets up the board and player.</summary>    
    private void GameStart()
    {
        _score = 0;
        _stepsLeft = numOfSteps;
        
        cellBG.GetComponent<RectTransform>().sizeDelta = new Vector2(numOfColumns * 75, numOfRows * 75);
        
        if (_firstGame)
        {
            _cellList = new List<Cell>(numOfRows + 2 * numOfColumns + 2);
            
            CreateCells();
            GetAvailableCells();
            InstantiatePlayer();
            
            _firstGame = false;
        }
        else
        {
            RandomizeEmptyCellStates();
        }
        
        _lb.UpdateCurrentCell(GetRandomPlayerPosition());
        UpdateUI();
    }

    /// <summary>
    /// Called when the player clicks on the 'Start Game' button.
    /// Calls all the necessary functions for the game to work properly.
    /// </summary>
    public void StartGame()
    {
        GameStart();
        _sm.GameStart();
        _cm.GameStart(IsPlayerHuman);
    }

    /// <summary>
    /// Called when the game ends.
    /// Stops all movement. If the player is human, then <see cref="TrainAI"/> will
    /// be called to train an AI with this game's data.
    /// </summary>  
    private void GameOver()
    {
        _cm.EndGame();
        _sm.TryRegisterNewScore(_score);
        if (IsPlayerHuman) TrainAI();
        _mm.showLeaderboardAfterGame = _sm.LeaderboardUpdated;
        _sm.UpdateFinalScoreText(_score);
        _mm.ShowGameOverMenu();
    }

    /// <summary>
    /// Called upon the end of a human playthrough.
    /// It gets all of the training data that has been collected during the game round,
    /// and then passes this data to NaiveBayesClassifier's Train function.
    /// </summary>
    /// <seealso cref="DataCollector"/>
    private void TrainAI()
    {
        var dataCollector = FindObjectOfType<DataCollector>();
        List<TrainingData> trainingData = dataCollector.GetTrainingData();
        NaiveBayesClassifier.Train(trainingData);
    }

    /// <summary>
    /// Creates a grid of cells, with the number of rows and columns specified by the user.    
    /// It also randomly assigns some cells to have trash in them.
    /// </summary>
    private void CreateCells()
    {
        for(int i = 0; i < numOfRows + 2; i++)
        {
            for(int j = 0; j < numOfColumns + 2; j++)
            {
                Cell newCell = Instantiate(cellPrefab, cellGrid).
                    GetComponent<Cell>();

                newCell.Coordinates = new Vector2(j - 1, i - 1);
                newCell.name = $"({j - 1},{i - 1})";

                if(i == 0 || i == numOfColumns + 1 || j == 0 || 
                    j == numOfColumns + 1)
                {
                    newCell.CellState = Cell.State.Wall;
                }

                else
                { 
                    if (Random.Range(0, 100) <= probabilityOfTrash)
                    {
                        newCell.CellState = Cell.State.HasTrash;
                    }
                }
                
                _cellList.Add(newCell);
                newCell.UpdateCell();
            }
        }
    }

    /// <summary>
    /// Iterates through the _cellList and sets each cell's state to either empty or has trash.    
    /// The probability of a cell having trash is determined by the parameterizable
    /// probabilityOfTrash variable.
    /// </summary>
    private void RandomizeEmptyCellStates()
    {
        foreach (Cell cell in _cellList)
        {
            if (cell.CellState == Cell.State.Wall) continue;
            cell.CellState = Random.Range(0, 100) <= probabilityOfTrash 
                ? Cell.State.HasTrash : Cell.State.Empty;
            cell.UpdateCell();
            cell.ShowFog(true);
        }
    }

    /// <summary>
    /// Instantiates the player character, Luso, and sets his grid dimensions.
    /// </summary>    
    private void InstantiatePlayer()
    {
        _lb = Instantiate(lusoPrefab).GetComponent<LusoBehaviour>();
        _lb.UpdateGridDimensions(new Vector2(numOfRows + 2, numOfColumns + 2));
    }

    /// <summary>
    /// Gets a new cell for the player to move to.
    /// </summary>   
    /// <returns> A random cell from the available cells list.</returns>
    private Cell GetRandomPlayerPosition()
    {
        return _availableCells[Random.Range(0, _availableCells.Count)];
    }

    /// <summary>
    /// Used to populate the _availableCells list with all of the cells in the cellGrid that are not walls.    
    /// </summary>
    private void GetAvailableCells()
    {
        _availableCells = new List<Cell>();

        for(int i = 0; i < cellGrid.childCount; i++)
        {
            Cell currentCell = cellGrid.GetChild(i).GetComponent<Cell>();

            if(currentCell.CellState != Cell.State.Wall)
            {
                _availableCells.Add(currentCell);
            }
        }
    }

    /// <summary>
    /// Updates the number of steps left.
    /// </summary>
    /// <seealso cref="StepsLeft"/>
    public void ActionPlayed() => StepsLeft -= 1;

    /// <summary>
    /// Checks the current cell to see if it has trash.
    /// If so, it removes the trash and adds points to the score.
    /// Otherwise, it subtracts points from the score.
    /// </summary> 
    public void TryPickingItem()
    {
        _currentCell = _cellList[_lb.CurrentCellIndex];
        if (_currentCell.CellState == Cell.State.HasTrash)
        {
            _currentCell.CellState = Cell.State.Empty;
            _currentCell.UpdateCell();
            AddScore(itemPickSuccessScore);
        }
        else
        {
            AddScore(itemPickFailureScore);
        }
    }

    /// <summary>
    /// Called when the player bumps into a wall.    
    /// It adds/takes points to the score.
    /// </summary>
    public void BumpedTheWall()
    {
        AddScore(wallBumpScore);
    }

    /// <summary>
    /// Adds the parameter to the Score variable.
    /// </summary>    
    /// <param name="scoreToAdd"> The score to add.</param>
    private void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    /// <summary>
    /// Updates the UI with the current score and steps left.
    /// </summary>    
    private void UpdateUI()
    {
        uiManager.UpdateUI(_score, _stepsLeft);
    }
}
