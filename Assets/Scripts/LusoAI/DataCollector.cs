using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data-storing class for a given play-through.
/// It stores <see cref="TrainingData"/>, to be sent to the <see cref="NaiveBayesClassifier"/> upon
/// the end of a given game run.
/// </summary>
public class DataCollector : MonoBehaviour
{
    private readonly List<TrainingData> _trainingDataList = new();

    /// <summary>
    /// Records the current state of the game and what action was taken. 
    /// </summary>
    /// <param name="currentCellIndex"> The index of the current cell in the grid.</param>
    /// <param name="neighboringCells"> The states of the neighboring cells.</param>
    /// <param name="action"> The action that the player took.</param>
    public void RecordAction(int currentCellIndex, Cell.State[] neighboringCells, Action action)
    {
        _trainingDataList.Add(new TrainingData(currentCellIndex, neighboringCells, action));
    }

    /// <summary>
    /// Gets the data that was gathered and trained so far.
    /// </summary>
    /// <returns> A list of training data.</returns>
    public List<TrainingData> GetTrainingData()
    {
        return _trainingDataList;
    }
}