using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text FinalScoreText;
    [SerializeField] private TMP_Text LeaderboardText;
    private List<int> top6Scores;
    
    public bool LeaderboardUpdated { get; private set; }

    private void Start()
    {
        top6Scores = new List<int>(7);
    }

    public void GameStart()
    {
        LeaderboardUpdated = false;
    }

    public void TryRegisterNewScore(int newScore)
    {
        if(GetNewScoreInTop6(newScore) > top6Scores.Capacity - 1) return;

        AddNewScoreToList(newScore);
        UpdateLeaderboard();
        LeaderboardUpdated = true;
    }

    private void AddNewScoreToList(int newScore)
    {
        top6Scores.Add(newScore);
        top6Scores.Sort((a, b) => b.CompareTo(a)); //Sort comparison from Anthony Pegram at https://stackoverflow.com/questions/3062513/how-can-i-sort-generic-list-desc-and-asc
    }

    private int GetNewScoreInTop6(int newScore)
    {
        int scoreTopPos = 1;
        
        foreach (int score in top6Scores)
        {
            if (newScore <= score)
            {
                scoreTopPos++;
                continue;
            }
            
            break;
        }
        return scoreTopPos;
    }

    private void UpdateLeaderboard()
    {
        LeaderboardText.text = $"1st: {GetTop6Scores(1)}\n" +
                               $"2nd: {GetTop6Scores(2)}\n" +
                               $"3rd: {GetTop6Scores(3)}\n" +
                               $"4th: {GetTop6Scores(4)}\n" +
                               $"5th: {GetTop6Scores(5)}\n" +
                               $"6th: {GetTop6Scores(6)}";
    }

    private string GetTop6Scores(int topXth)
    {
        return topXth > top6Scores.Count ? "Empty" : top6Scores[topXth - 1].ToString();
    }

    public void UpdateFinalScoreText(int finalScore)
    {
        FinalScoreText.text = "Final Score: " + finalScore;
    }
}
