using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that contains the logic for the player and for the overall prefab.
/// </summary>
public class LusoBehaviour : MonoBehaviour
{
    private List<Cell> _cellList;
    private int _currentCellIndex;
    private Vector2 _gridDimensions;
    private RectTransform _rectTransform;
    private List<Action> _moveMethods;
    private System.Random _rnd;
    private GameManager _gm;
    private int _upMoveCellIndexDelta;
    private int _downMoveCellIndexDelta;
    private int _leftMoveCellIndexDelta;
    private int _rightMoveCellIndexDelta;
    
    /// <summary>
    /// The cell where the player/Luso is standing.
    /// </summary>
    public int CurrentCellIndex
    {
        get => _currentCellIndex;
        private set
        {
            _currentCellIndex = value;
            OnCurrentCellChange();
        }
    }

    /// <summary>
    /// The dimensions of the grid.
    /// </summary>
    public Vector2 GridDimensions
    {
        get => _gridDimensions;
        private set
        {
            _gridDimensions = value;
            CalculateMoveIndexDeltas((int)value.y);
        }
    }

    private void Awake()
    {
        _cellList = FindObjectOfType<GameManager>().CellList;
        _rectTransform = transform as RectTransform;
        if (_cellList is null)
            Debug.Log(
                "Couldn't find cellArray, either it wasn't created " +
                "or there is no object with a GameManager in the scene");
    }

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _moveMethods = new List<Action>{MoveUp, MoveDown, MoveLeft, MoveRight};
        _rnd = new System.Random();
    }

    /// <summary>
    /// Updates the current cell index to the new cell's index in the list of cells.
    /// </summary>
    /// <param name="newCell"> The index of the new cell in the list. </param>
    public void UpdateCurrentCell(Cell newCell)
    {
        CurrentCellIndex = _cellList.IndexOf(newCell);
    }

    /// <summary>
    /// Updates the grid dimensions of the game board.
    /// </summary> 
    /// <param name="newGridDimensions"> The new dimensions of the grid.</param>
    public void UpdateGridDimensions(Vector2 newGridDimensions)
    {
        GridDimensions = newGridDimensions;
    }
    
    /// <summary>
    /// Updates the current cell index of the player to a new position.    
    /// </summary>
    /// <param name="newPositionCellIndex"> The new cell index to move the player to.</param>
    private void UpdatePositionInGrid(int newPositionCellIndex)
    {
        CurrentCellIndex = newPositionCellIndex;
    }

    /// <summary>
    /// Called when the CurrentCellIndex changes.    
    /// It sets the parent of the Luso gameobject to be the current cell,
    /// and then moves it to that cell's position.
    /// </summary>
    private void OnCurrentCellChange()
    {
        _rectTransform.SetParent(_cellList[CurrentCellIndex].transform as RectTransform);
        _rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    
    /// <summary>
    /// Calculates the number of cells to move in each direction.
    /// </summary> 
    /// <remarks>The function sets four private variables: _upMoveCellIndexDelta, _downMoveCellIndexDelta, 
    /// _rightMoveCellIndexDelta and _leftMoveCellIndexDelta.</remarks>
    /// <param name="numOfColumns"> The number of columns in the grid</param>
    private void CalculateMoveIndexDeltas(int numOfColumns)
    {
        _upMoveCellIndexDelta = -numOfColumns;
        _downMoveCellIndexDelta = numOfColumns;
        _rightMoveCellIndexDelta = 1;
        _leftMoveCellIndexDelta = -1;
    }

    /// <summary>
    /// Checks if the player can move to a new cell.    
    /// If so, it updates the player's position in the grid.
    /// </summary>
    /// <param name="moveCellIndexDelta"> The cell index that the player wants to be moved to.</param>
    private void TryMove(int moveCellIndexDelta)
    {
        if(!CheckValidMove(moveCellIndexDelta)) return;

        UpdatePositionInGrid(CurrentCellIndex + moveCellIndexDelta);
    }
    
    /// <summary>
    /// Checks if the player can move to a cell in the direction of their input. 
    /// </summary>
    /// <param name="moveCellIndexDelta"> Used to determine the direction of movement. 
    /// If it's positive, then we are moving right, if it's negative - left.</param>
    /// <returns> True if the move is valid, and false if it isn't.</returns>
    private bool CheckValidMove(int moveCellIndexDelta)
    {
        if (moveCellIndexDelta == 0) return false;
            
        Cell targetCell = _cellList[CurrentCellIndex + moveCellIndexDelta];
        
        if(targetCell.CellState == Cell.State.Wall) _gm.BumpedTheWall();
        
        return targetCell.CellState != Cell.State.Wall;
    }

    /// <summary>
    /// Moves the player up one cell in the grid.
    /// </summary>    
    public void MoveUp()
    {
        TryMove(_upMoveCellIndexDelta);
    }

    /// <summary>
    /// Moves the player down one cell in the grid.
    /// </summary>    
    public void MoveDown()
    {
        TryMove(_downMoveCellIndexDelta);
    }

    /// <summary>
    /// Moves the player to the left.
    /// </summary>    
    public void MoveLeft()
    {
        TryMove(_leftMoveCellIndexDelta);
    }

    /// <summary>
    /// Moves the player to the right.
    /// </summary>    
    public void MoveRight()
    {
        TryMove(_rightMoveCellIndexDelta);
    }

    /// <summary>
    /// Chooses a random move method from the list of available methods and calls it.
    /// </summary>    
    public void MoveRandom()
    {
        _moveMethods[_rnd.Next(0, _moveMethods.Count)]();
    }
    
    /// <summary>
    /// Calculates the nearest neighboring cells on the grid.
    /// </summary>
    /// <returns> An array of cell.State objects, each representing the state of a neighboring cell.</returns>
    public Cell.State[] GetNeighboringCells()
    {
        Cell.State[] neighboringCells = new Cell.State[5];
        neighboringCells[0] = _cellList[CurrentCellIndex].CellState;
        neighboringCells[1] = _cellList[CurrentCellIndex + _upMoveCellIndexDelta].CellState;
        neighboringCells[2] = _cellList[CurrentCellIndex + _downMoveCellIndexDelta].CellState;
        neighboringCells[3] = _cellList[CurrentCellIndex + _leftMoveCellIndexDelta].CellState;
        neighboringCells[4] = _cellList[CurrentCellIndex + _rightMoveCellIndexDelta].CellState;
        return neighboringCells;
    }
}