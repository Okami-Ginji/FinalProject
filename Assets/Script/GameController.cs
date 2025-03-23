using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseMenu;
    public GameOver gameOver;
    public CinemachineTargetGroup targetGroup;

    private ScoreUI scoreUI;
    public static bool GamePause = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        PlayerSpawn();
        scoreUI = FindObjectOfType<ScoreUI>();

    }

    private void PlayerSpawn()
    {
        GameObject characterPrefab = SelectionController.instance.selectedPlayer.prefab;
        GameObject characterTmp = Instantiate(characterPrefab, SelectionController.instance.selectedPlayer.selectionLocation, Quaternion.identity);
        PlayerControl playerScript = FindObjectOfType<PlayerControl>();
        if (playerScript != null)
        {
            playerScript.enabled = true;
            playerScript.healthBar = FindObjectOfType<HealthBar>();

            GameObject playerUI = GameObject.Find("PlayerUI");
            playerScript.ammo = GameObject.FindWithTag("NumberAmo");

            CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
            {
                target = characterTmp.transform,
                weight = 1f,
                radius = 4f
            };

            List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>(targetGroup.m_Targets);
            targets.Add(newTarget);
            targetGroup.m_Targets = targets.ToArray();
        }
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerControl>() == null)
        {
            GameOver();
        }
        //else
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        if (GamePause)
        //        {
        //            Resume();
        //        }
        //        else
        //        {
        //            Pause();
        //        }
        //    }
        //}

    }

    public void GameFinish()
    {
        Time.timeScale = 0f;
        int scoreStock = PlayerPrefs.GetInt("PlayerCoins", 0);
        PlayerPrefs.SetInt("PlayerCoins", scoreUI.score + scoreStock);
        PlayerPrefs.Save();
        //SceneManager.LoadScene("Menu");
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
