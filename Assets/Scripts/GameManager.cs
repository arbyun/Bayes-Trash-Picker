using System;
using System.Collections.Generic;
using LusoAI;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    [Range(0, 100)]
    [SerializeField] private int probabilityOfTrash = 40;

    private int score;
    private int stepsLeft;
    private List<Cell> cellList;
    private Cell currentCell;
    private LusoBehaviour lb;
    private ControlsManager cm;
    private MainMenu mm;
    private bool firstGame = true;
    private List<Cell> availableCells;
    
    public NaiveBayesClassifier NaiveBayesClassifier { get; private set; }

    public List<Cell> CellList
    {
        get => cellList;
        private set => cellList = value;
    }
    
    public bool IsPlayerHuman { get; set; }

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            UpdateUI();
        }
    }

    public int StepsLeft
    {
        get => stepsLeft;
        private set
        {
            stepsLeft = value;
            if (stepsLeft == 0) GameOver();
            UpdateUI();
        }
    }

    private void Start()
    {
        cm = FindObjectOfType<ControlsManager>();
        mm = FindObjectOfType<MainMenu>();
        NaiveBayesClassifier = new NaiveBayesClassifier();
    }

    private void GameStart()
    {
        score = 0;
        stepsLeft = numOfSteps;
        
        cellBG.GetComponent<RectTransform>().sizeDelta = new Vector2(numOfColumns * 75, numOfRows * 75);
        
        if (firstGame)
        {
            cellList = new List<Cell>(numOfRows + 2 * numOfColumns + 2);
            
            CreateCells();
            GetAvailableCells();
            InstantiatePlayer();
            
            firstGame = false;
        }
        else
        {
            RandomizeEmptyCellStates();
        }
        
        lb.UpdateCurrentCell(GetRandomPlayerPosition());
        UpdateUI();
    }

    public void StartGame()
    {
        GameStart();
        cm.GameStart(IsPlayerHuman);
    }

    private void GameOver()
    {
        cm.EndGame();
        if (IsPlayerHuman) TrainAI();
        mm.ShowGameOverMenu();
    }

    private void TrainAI()
    {
        var dataCollector = FindObjectOfType<DataCollector>();
        List<TrainingData> trainingData = dataCollector.GetTrainingData();
        NaiveBayesClassifier.Train(trainingData);
    }

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
                
                cellList.Add(newCell);
                newCell.UpdateCell();
            }
        }
    }

    private void RandomizeEmptyCellStates()
    {
        print(cellList.Count);
        foreach (Cell cell in cellList)
        {
            if (cell.CellState == Cell.State.Wall) continue;
            cell.CellState = Random.Range(0, 100) <= probabilityOfTrash 
                ? Cell.State.HasTrash : Cell.State.Empty;
            cell.UpdateCell();
        }
    }

    private void InstantiatePlayer()
    {
        lb = Instantiate(lusoPrefab).GetComponent<LusoBehaviour>();
        lb.UpdateGridDimensions(new Vector2(numOfRows + 2, numOfColumns + 2));
    }

    private Cell GetRandomPlayerPosition()
    {
        return availableCells[Random.Range(0, availableCells.Count)];
    }

    private void GetAvailableCells()
    {
        availableCells = new List<Cell>();

        for(int i = 0; i < cellGrid.childCount; i++)
        {
            Cell currentCell = cellGrid.GetChild(i).GetComponent<Cell>();

            if(currentCell.CellState != Cell.State.Wall)
            {
                availableCells.Add(currentCell);
            }
        }
    }

    public void ActionPlayed() => StepsLeft -= 1;

    public void TryPickingItem()
    {
        currentCell = cellList[lb.CurrentCellIndex];
        if (currentCell.CellState == Cell.State.HasTrash)
        {
            currentCell.CellState = Cell.State.Empty;
            currentCell.UpdateCell();
            AddScore(itemPickSuccessScore);
        }
        else
        {
            AddScore(itemPickFailureScore);
        }
    }

    public void BumpedTheWall()
    {
        AddScore(wallBumpScore);
    }

    private void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    private void UpdateUI()
    {
        uiManager.UpdateUI(score, stepsLeft);
    }
}
