using TMPro;
using UnityEngine;

//public class ScoreUI : MonoBehaviour
//{
//    public int score = 0;
//    public TextMeshProUGUI scoreText;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    public void AddScore (int enemyScore)
//    {
//        score += enemyScore;
//        UpdateScore();
//    }
//    void UpdateScore()
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = "" + score;
//        }
//    }
//}


public class ScoreUI : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        //score = PlayerPrefs.GetInt("PlayerCoins", 0);
        UpdateScore();
    }
    public void AddScore(int enemyScore)
    {
        score += enemyScore;
        //PlayerPrefs.SetInt("PlayerCoins", score); 
        //PlayerPrefs.Save(); 
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
