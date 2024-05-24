public struct TrainingData
{
    public int CellIndex { get; set; }
    public Cell.State[] NeighboringCells { get; set; }
    public string Action { get; set; }

    public TrainingData(int cellIndex, Cell.State[] neighboringCells, string action)
    {
        CellIndex = cellIndex;
        NeighboringCells = neighboringCells;
        Action = action;
    }
}