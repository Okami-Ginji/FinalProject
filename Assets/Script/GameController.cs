using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject pauseMenu;
    public TimeUI timeUI;
    public AudioClip DiedSound;
    public AudioClip WinSound;

    public CinemachineTargetGroup targetGroup;
    private AudioManager_PlayScreen audioManager;
    private ScoreUI scoreUI;
    
    public static bool GamePause = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AudioManager_MenuScreen.instance != null)
        {
            Destroy(AudioManager_MenuScreen.instance.gameObject);
        }
        audioManager = FindObjectOfType<AudioManager_PlayScreen>();
        audioManager.musicAudioSource.UnPause();
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
        if(timeUI.timeRemaining <= 0)
        {
            GameFinish();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameFinish();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
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
        audioManager.musicAudioSource.Pause();
        audioManager.PlaySFX(WinSound);
        gameWinScreen.SetActive(true);        
        if (gameWinScreen.activeInHierarchy)
        {
            Time.timeScale = 0f;
            int scoreStock = PlayerPrefs.GetInt("PlayerCoins", 0);
            PlayerPrefs.SetInt("PlayerCoins", scoreUI.score + scoreStock);
            PlayerPrefs.Save();
            int countMap = MapSellectionController.instance.mapSprites.Count;
            int unlockMap = MapSellectionController.instance.selectedMap + 1;
            if (countMap > unlockMap)
            {
                PlayerPrefs.SetInt("Map_" + unlockMap + "_Unlocked", 1);
                PlayerPrefs.Save();

            }
        }
        //SceneManager.LoadScene("Menu");
    }

    public void goNextMap()
    {
        Destroy(gameObject);
        MapSellectionController.instance.mapName = MapSellectionController.instance.mapSprites[MapSellectionController.instance.selectedMap + 1].name;
        SceneManager.LoadScene(MapSellectionController.instance.mapName);
    }

    public void backHome()
    {
        PlayerPrefs.Save();
        Destroy(MapSellectionController.instance.gameObject);
        Destroy(SelectionController.instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }

    public void LoadGame()
    {
        string mapName = MapSellectionController.instance.mapName;
        SceneManager.LoadScene(mapName);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        audioManager.musicAudioSource.Pause();
        audioManager.PlaySFX(DiedSound);
        gameOverScreen.SetActive(true);  
        
        if (gameOverScreen.activeInHierarchy)
        {
            Time.timeScale = 0f;
        }
    }

}
