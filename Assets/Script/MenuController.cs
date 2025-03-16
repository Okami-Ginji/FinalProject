using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

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
        SceneManager.LoadScene("ChooseMap");
    }
    private void ResetGameProgress()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();
    }
}
