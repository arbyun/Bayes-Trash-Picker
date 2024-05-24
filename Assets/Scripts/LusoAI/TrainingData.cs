using System;

/// <summary>
/// A container class for a given action taken by the player.
/// </summary>
/// <seealso cref="DataCollector"/>
public struct TrainingData
{
    /// <summary>
    /// The index the player was on, on the moment the data was recorded.
    /// </summary>
    public int CellIndex
    {
        get; 
        set;
    }

    /// <summary>
    /// The state of the cells neighbouring ours, on the moment the data was recorded.
    /// </summary>
    public Cell.State[] NeighboringCells
    {
        get; 
        set;
    }

    /// <summary>
    /// The action the player chose to take.
    /// </summary>
    public Action Action
    {
        get; 
        set;
    }

    public TrainingData(int cellIndex, Cell.State[] neighboringCells, Action action)
    {
        CellIndex = cellIndex;
        NeighboringCells = neighboringCells;
        Action = action;
    }
}