using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The logic for the Naive Bayes algorithm used on our AI.
/// </summary>
public class NaiveBayesClassifier
{
    private readonly Dictionary<Action, int> _actionCounts;
    private readonly Dictionary<Action, Dictionary<string, int>> _featureActionCounts;
    private readonly Dictionary<string, int> _featureCounts;

    public NaiveBayesClassifier()
    {
        _actionCounts = new Dictionary<Action, int>();
        _featureActionCounts = new Dictionary<Action, Dictionary<string, int>>();
        _featureCounts = new Dictionary<string, int>();
    }

    /// <summary>
    /// Trains the Naive Bayes classifier using the provided training data. 
    /// </summary>
    /// <remarks> The training process involves counting the occurrences of each action and the 
    /// co-occurrences of features (representing the state of neighboring cells) with 
    /// each action. These counts are used to calculate probabilities that form the 
    /// basis of the Naive Bayes classifier. During training, the classifier learns 
    /// the likelihood of each action given the observed features.</remarks>
    /// <param name="trainingData">The training data consisting of neighboring cell 
    /// states and corresponding actions. See <see cref="TrainingData"/>.</param>
    public void Train(List<TrainingData> trainingData)
    {
        foreach (var data in trainingData)
        {
            Action action = data.Action;
            if (!_actionCounts.ContainsKey(action))
            {
                _actionCounts[action] = 0;
                _featureActionCounts[action] = new Dictionary<string, int>();
            }
            _actionCounts[action]++;

            for (int i = 0; i < data.NeighboringCells.Length; i++)
            {
                string feature = $"{i}_{data.NeighboringCells[i]}";
                if (!_featureCounts.ContainsKey(feature))
                    _featureCounts[feature] = 0;
                _featureCounts[feature]++;

                if (!_featureActionCounts[action].ContainsKey(feature))
                    _featureActionCounts[action][feature] = 0;
                _featureActionCounts[action][feature]++;
            }
        }
    }

    /// <summary>
    /// Predicts the best action based on the current state of neighboring cells using 
    /// the trained Naive Bayes classifier.
    /// </summary>
    /// <remarks> The method calculates the posterior probability of each action
    /// given the observed features (neighboring cell states), utilizing the counts
    /// obtained during training. It then selects the action with the highest 
    /// posterior probability as the predicted action. This prediction is based on the 
    /// assumption of independence between features, as per the Naive Bayes classifier.
    /// </remarks>
    /// <param name="neighboringCells"> The states of neighboring cells.</param>
    /// <returns> The predicted action.</returns>
    public Action Predict(Cell.State[] neighboringCells)
    {
        Dictionary<Action, double> actionProbabilities = new Dictionary<Action, double>();
        foreach (var action in _actionCounts.Keys)
        {
            double actionProbability = (double)_actionCounts[action] / _actionCounts.Values.Sum();
            double conditionalProbability = 1.0;

            for (int i = 0; i < neighboringCells.Length; i++)
            {
                string feature = $"{i}_{neighboringCells[i]}";
                double featureProbability = 1.0;
                if (_featureCounts.ContainsKey(feature))
                {
                    featureProbability = (double)_featureActionCounts[action].GetValueOrDefault(feature, 0) 
                                         / _actionCounts[action];
                }
                conditionalProbability *= featureProbability;
            }
            actionProbabilities[action] = actionProbability * conditionalProbability;
        }

        return actionProbabilities.OrderByDescending(kv => kv.Value).First().Key;
    }

    /// <summary>
    /// Checks to see if the dictionary has any entries.
    /// </summary> 
    /// <returns> True if the dictionary has any elements; otherwise, it returns false.</returns>
    public bool HasTrainingData()
    {
        return _actionCounts.Count != 0;
    }
}

