using System;
using System.Collections.Generic;
using UnityEngine;

public class LusoBehaviour : MonoBehaviour
{
    private List<Cell> cellList;
    private int currentCellIndex;
    private Vector2 gridDimensions;
    private RectTransform rectTransform;
    private List<Action> moveMethods;
    private System.Random rnd;
    private GameManager gm;
    private int upMoveCellIndexDelta;
    private int downMoveCellIndexDelta;
    private int leftMoveCellIndexDelta;
    private int rightMoveCellIndexDelta;
    
    public int CurrentCellIndex
    {
        get => currentCellIndex;
        private set
        {
            currentCellIndex = value;
            OnCurrentCellChange();
        }
    }

    public Vector2 GridDimensions
    {
        get => gridDimensions;
        private set
        {
            gridDimensions = value;
            CalculateMoveIndexDeltas((int)value.y);
        }
    }

    private void Awake()
    {
        cellList = FindObjectOfType<GameManager>().CellList;
        rectTransform = transform as RectTransform;
        if (cellList is null)
            Debug.Log(
                "Couldn't find cellArray, either it wasn't created " +
                "or there is no object with a GameManager in the scene");
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        moveMethods = new List<Action>{MoveUp, MoveDown, MoveLeft, MoveRight};
        rnd = new System.Random();
    }

    public void UpdateCurrentCell(Cell newCell)
    {
        CurrentCellIndex = cellList.IndexOf(newCell);
    }

    public void UpdateGridDimensions(Vector2 newGridDimensions)
    {
        GridDimensions = newGridDimensions;
    }
    
    private void UpdatePositionInGrid(int newPositionCellIndex)
    {
        CurrentCellIndex = newPositionCellIndex;
    }

    private void OnCurrentCellChange()
    {
        rectTransform.SetParent(cellList[CurrentCellIndex].transform as RectTransform);
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    
    private void CalculateMoveIndexDeltas(int numOfColumns)
    {
        upMoveCellIndexDelta = -numOfColumns;
        downMoveCellIndexDelta = numOfColumns;
        rightMoveCellIndexDelta = 1;
        leftMoveCellIndexDelta = -1;
    }

    private void TryMove(int moveCellIndexDelta)
    {
        if(!CheckValidMove(moveCellIndexDelta)) return;

        UpdatePositionInGrid(CurrentCellIndex + moveCellIndexDelta);
    }
    
    private bool CheckValidMove(int moveCellIndexDelta)
    {
        if (moveCellIndexDelta == 0) return false;
            
        Cell targetCell = cellList[CurrentCellIndex + moveCellIndexDelta];
        
        if(targetCell.CellState == Cell.State.Wall) gm.BumpedTheWall();
        
        return targetCell.CellState != Cell.State.Wall;
    }

    public void MoveUp()
    {
        TryMove(upMoveCellIndexDelta);
    }

    public void MoveDown()
    {
        TryMove(downMoveCellIndexDelta);
    }

    public void MoveLeft()
    {
        TryMove(leftMoveCellIndexDelta);
    }

    public void MoveRight()
    {
        TryMove(rightMoveCellIndexDelta);
    }

    public void MoveRandom()
    {
        moveMethods[rnd.Next(0, moveMethods.Count)]();
    }
}