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

    /// <inheritdoc cref="GameManager.StartGame"/>
    public void GameStart()
    {
        LeaderboardUpdated = false;
    }

    /// <summary>
    /// Checks if new score is in the top 6 and if so, places it on its place
    /// in the top and updates the leaderboard ui.
    /// </summary>
    /// <param name="newScore">Score to add to the leaderboard</param>
    public void TryRegisterNewScore(int newScore)
    {
        if(GetNewScoreInTop6(newScore) > top6Scores.Capacity - 1) return;

        AddNewScoreToList(newScore);
        UpdateLeaderboard();
        LeaderboardUpdated = true;
    }

    /// <summary>
    /// Adds the received score to the leaderboard and sorts the list
    /// descendingly.
    /// </summary>
    /// <param name="newScore"></param>
    private void AddNewScoreToList(int newScore)
    {
        top6Scores.Add(newScore);
        
        //Sort comparison from Anthony Pegram at https://stackoverflow.com/questions/3062513/how-can-i-sort-generic-list-desc-and-asc
        top6Scores.Sort((a, b) => b.CompareTo(a)); 
    }

    /// <summary>
    /// Gets the score's potential position on the leaderboard.
    /// </summary>
    /// <param name="newScore">score to add to the leaderboard</param>
    /// <returns>The score position on the leaderboard</returns>
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

    /// <summary>
    /// Updates the leaderboard text.
    /// </summary>
    private void UpdateLeaderboard()
    {
        LeaderboardText.text = $"1st: {GetTop6Scores(1)}\n" +
                               $"2nd: {GetTop6Scores(2)}\n" +
                               $"3rd: {GetTop6Scores(3)}\n" +
                               $"4th: {GetTop6Scores(4)}\n" +
                               $"5th: {GetTop6Scores(5)}\n" +
                               $"6th: {GetTop6Scores(6)}";
    }

    /// <summary>
    /// Gets the score at the provided position in the leaderboard.
    /// </summary>
    /// <param name="topXth">Desired score's position on the leaderboard</param>
    /// <returns></returns>
    private string GetTop6Scores(int topXth)
    {
        return topXth > top6Scores.Count ? "Empty" : top6Scores[topXth - 1].ToString();
    }

    /// <summary>
    /// Updates the Game Over message with the achieved score.
    /// </summary>
    /// <param name="finalScore">Score achieved this game</param>
    public void UpdateFinalScoreText(int finalScore)
    {
        FinalScoreText.text = "Final Score: " + finalScore;
    }
}
