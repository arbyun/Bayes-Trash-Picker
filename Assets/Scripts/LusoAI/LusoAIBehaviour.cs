using System;
using System.Collections;
using LusoAI;
using UnityEngine;

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

    private IEnumerator AIPlay()
    {
        var neighboringCells = _luso.GetNeighboringCells();
        Action action = _classifier.Predict(neighboringCells);
        action?.Invoke();
        _gm.ActionPlayed();

        yield return new WaitForSeconds(0.5f);
        _play = null;
    }

    public void Play()
    {
        _play ??= StartCoroutine(AIPlay());
    }
}