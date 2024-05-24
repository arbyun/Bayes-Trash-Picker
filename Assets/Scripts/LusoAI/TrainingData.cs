using System;

public struct TrainingData
{
    public int CellIndex { get; set; }
    public Cell.State[] NeighboringCells { get; set; }
    public Action Action { get; set; }

    public TrainingData(int cellIndex, Cell.State[] neighboringCells, Action action)
    {
        CellIndex = cellIndex;
        NeighboringCells = neighboringCells;
        Action = action;
    }
}