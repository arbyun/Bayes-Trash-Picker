using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stepsText;

    public void UpdateUI(int score, int steps)
    {
        scoreText.text = $"Score: {score}";
        stepsText.text = $"Steps Remaining: {steps}";
    }
}
