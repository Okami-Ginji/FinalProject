using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameOver gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(FindObjectOfType<PlayerControl>() == null)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        gameOver.Setup();
        if (gameOverScreen.activeInHierarchy)
        {
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        

    }
}
