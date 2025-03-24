using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image messageBox;
    public void ShowMessage()
    {
        messageBox.gameObject.SetActive(true);
    }
    public void OffMessage()
    {
        messageBox.gameObject.SetActive(false);
    }
    public void NewGameButton()
    {       
        ResetGameProgress();
        PlayerPrefs.SetInt("Player_AlreadyPlay", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ChooseMap");
    }

    public void LoadGameButton()
    {
        bool isPlayed = PlayerPrefs.GetInt("Player_AlreadyPlay", 0) == 1;
        if (isPlayed)
        {
            SceneManager.LoadScene("ChooseMap");
        }
        else
        {
            LoadIntro();
        }            
    }

    public void LoadIntro()
    {      
        SceneManager.LoadScene("Intro");
    }
    public void ExitGameButton()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    private void ResetGameProgress()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();
    }

}
