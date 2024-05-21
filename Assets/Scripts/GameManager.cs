using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellGrid;
    [SerializeField] private Transform cellBG;
    [SerializeField] private int numOfRows = 5;
    [SerializeField] private int numOfCollumns = 5;
    [SerializeField] private int numOfSteps = 20;
    [SerializeField] private GameObject LusoPrefab;
    
    [Range(0, 100)]
    [SerializeField] private int probabilityOfTrash = 40;

    private int score;
    private int stepsLeft;
    private List<Cell> cellList;
    public List<Cell> CellList
    {
        get => cellList;
        private set => cellList = value;
    }

    private void Start()
    {
        score = 0;
        stepsLeft = numOfSteps;
        cellBG.GetComponent<RectTransform>().sizeDelta = new Vector2(numOfCollumns * 75, numOfRows * 75);

        cellList = new List<Cell>(numOfRows * numOfSteps);

        CreateCells();
        InstantiatePlayer();
        uiManager.UpdateUI(score, stepsLeft);
    }

    private void CreateCells()
    {
        for(int i = 0; i < numOfRows + 2; i++)
        {
            for(int j = 0; j < numOfCollumns + 2; j++)
            {
                Cell newCell = Instantiate(cellPrefab, cellGrid).
                    GetComponent<Cell>();

                newCell.Coordinates = new Vector2(j - 1, i - 1);
                newCell.name = $"({j - 1},{i - 1})";

                if(i == 0 || i == numOfCollumns + 1 || j == 0 || 
                    j == numOfCollumns + 1)
                {
                    newCell.CellState = Cell.State.Wall;
                }

                else if (Random.Range(0, 100) <= probabilityOfTrash)
                {
                    newCell.CellState = Cell.State.HasTrash;
                    cellList.Add(newCell);
                }

                newCell.UpdateCell();
            }
        }
    }

    private void InstantiatePlayer()
    {
        List<Cell> availableCells = new List<Cell>();

        for(int i = 0; i < cellGrid.childCount; i++)
        {
            Cell currentCell = cellGrid.GetChild(i).GetComponent<Cell>();

            if(currentCell.CellState != Cell.State.Wall)
            {
                availableCells.Add(currentCell);
            }
        }

        int startingCell = Random.Range(0, availableCells.Count);
        LusoBehaviour lusoBehaviour = Instantiate(LusoPrefab).GetComponent<LusoBehaviour>();
        lusoBehaviour.UpdateCurrentCell(startingCell);
        lusoBehaviour.UpdateGridDimensions(new Vector2(numOfRows, numOfCollumns));
        

    }
}
