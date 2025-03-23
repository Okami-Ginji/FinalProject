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
        SceneManager.LoadScene("ChooseMap");
    }

    public void LoadGameButton()
    {
        SceneManager.LoadScene("ChooseMap");
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
