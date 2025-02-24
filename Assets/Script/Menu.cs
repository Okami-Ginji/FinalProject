using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public static bool GamePause = false;

    public GameObject pauseMenu;
    public GameObject dieMenu;
    private PlayerControl playerControl;
    private AudioManager audioManager;
    private void Awake()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    //Escmenu
    void Update()
    {        
        if (playerControl.currentHealth > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (playerControl.currentHealth <= 0)
        {
            ShowDieMenu();
        }
    }
    public void LoadGame()
    {        
        SceneManager.LoadScene("Map");
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    //esc menu
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
        audioManager.musicAudioSource.UnPause();
        playerControl.footstepSound.UnPause();
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
        audioManager.musicAudioSource.Pause();
        playerControl.footstepSound.Pause();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void ShowDieMenu()
    {
        dieMenu.SetActive(true);       
    }

   
}
