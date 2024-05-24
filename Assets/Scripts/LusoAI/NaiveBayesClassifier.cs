using System.Collections.Generic;
using System.Linq;

public class NaiveBayesClassifier
{
    private readonly Dictionary<string, int> _actionCounts;
    private readonly Dictionary<string, Dictionary<string, int>> _featureActionCounts;
    private readonly Dictionary<string, int> _featureCounts;

    public NaiveBayesClassifier()
    {
        _actionCounts = new Dictionary<string, int>();
        _featureActionCounts = new Dictionary<string, Dictionary<string, int>>();
        _featureCounts = new Dictionary<string, int>();
    }

    public void Train(List<TrainingData> trainingData)
    {
        foreach (var data in trainingData)
        {
            string action = data.Action;
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

    public string Predict(Cell.State[] neighboringCells)
    {
        Dictionary<string, double> actionProbabilities = new Dictionary<string, double>();
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
}
