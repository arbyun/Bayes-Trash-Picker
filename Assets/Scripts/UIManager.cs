using TMPro;
using UnityEngine;

/// <summary>
/// Class made to keep track of score and steps left.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stepsText;

    /// <summary>
    /// Updates the 'score' and 'steps remaining' text on the screen.
    /// </summary>    
    /// <param name="score"> Score of the player </param>
    /// <param name="steps"> Number of steps remaining in the current level.</param>
    public void UpdateUI(int score, int steps)
    {
        scoreText.text = $"Score: {score}";
        stepsText.text = $"Steps Remaining: {steps}";
    }
}
