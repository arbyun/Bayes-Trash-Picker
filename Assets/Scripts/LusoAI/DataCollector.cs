using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    private readonly List<TrainingData> _trainingDataList = new();

    public void RecordAction(int currentCellIndex, Cell.State[] neighboringCells, string action)
    {
        _trainingDataList.Add(new TrainingData(currentCellIndex, neighboringCells, action));
    }

    public List<TrainingData> GetTrainingData()
    {
        return _trainingDataList;
    }
}