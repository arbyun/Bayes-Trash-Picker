using System;
using UnityEngine;

public class LusoBehaviour : MonoBehaviour
{
    private Cell[] CellArray;
    private int currentCellIndex;
    private Vector2 gridDimensions;
    
    public int CurrentCell
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

    private int upMoveCellIndexDelta;
    private int downMoveCellIndexDelta;
    private int leftMoveCellIndexDelta;
    private int rightMoveCellIndexDelta;
    
    private void Start()
    {
        try
        {
            CellArray = FindObjectOfType<GameManager>().CellArray;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log(
                "Couldn't find cellArray, either it wasn't created " +
                      "or there is no object with a GameManager in the scene");
        }
    }

    public void UpdateCurrentCell(int newCellIndex)
    {
        CurrentCell = newCellIndex;
    }

    public void UpdateGridDimensions(Vector2 newGridDimensions)
    {
        GridDimensions = newGridDimensions;
    }
    
    private void UpdatePositionInGrid(int newPositionCellIndex)
    {
        CurrentCell = newPositionCellIndex;
    }

    private void OnCurrentCellChange()
    {
        transform.position = CellArray[CurrentCell].transform.position;
    }
    
    private void CalculateMoveIndexDeltas(int numOfCollumns)
    {
        upMoveCellIndexDelta = -numOfCollumns;
        downMoveCellIndexDelta = numOfCollumns;
        rightMoveCellIndexDelta = 1;
        leftMoveCellIndexDelta = -1;
    }
}