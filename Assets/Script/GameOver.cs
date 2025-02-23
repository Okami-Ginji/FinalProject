using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
        Transform child = transform.Find("DiedImage"); 
        if (child != null)
        {
            Animator anim = child.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("play");
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      
    }
}
