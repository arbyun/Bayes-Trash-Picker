using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the logic for when Luso is being controlled by the AI.
/// </summary>
public class LusoAIBehaviour : MonoBehaviour
{
    private GameManager _gm;
    private NaiveBayesClassifier _classifier;
    private LusoBehaviour _luso;
    private Coroutine _play;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _luso = FindObjectOfType<LusoBehaviour>();
        _classifier = _gm.NaiveBayesClassifier;
    }

    /// <summary>
    /// Simulates a play action on Luso by the AI.    
    /// It uses the <see cref="NaiveBayesClassifier"/> to predict what action it should take
    /// based on its neighboring cells' state.</summary>
    private IEnumerator AIPlay()
    {
        var neighboringCells = _luso.GetNeighboringCells();
        Action action = _classifier.Predict(neighboringCells);
        action?.Invoke();
        _gm.ActionPlayed();

        yield return new WaitForSeconds(0.5f);
        _play = null;
    }

    /// <summary>
    /// Runs the AI's turn.
    /// </summary>
    public void Play()
    {
        _play ??= StartCoroutine(AIPlay());
    }
}