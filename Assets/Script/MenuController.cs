using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void NewGameButton()
    {        
        SceneManager.LoadScene("ChooseMap");
    }

    public void ExitGameButton()
    {
        SceneManager.LoadScene("ChooseMap");
    }

}
