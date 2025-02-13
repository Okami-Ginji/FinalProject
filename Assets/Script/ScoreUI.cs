using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore (int enemyScore)
    {
        score += enemyScore;
        UpdateScore();
    }
    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }
}
