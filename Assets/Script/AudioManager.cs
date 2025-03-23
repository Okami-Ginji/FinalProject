using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    //audio clip
    public AudioClip musicMenuScreen;
    public AudioClip musicPlayScreen;
    public AudioClip clickSound;
    public AudioClip healthClip;
    public AudioClip expClip;

    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        musicAudioSource.clip = musicMenuScreen;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    void Update()
    {     
        if (SceneManager.GetActiveScene().name == "Meadow" || SceneManager.GetActiveScene().name == "Desert" || SceneManager.GetActiveScene().name == "Rock")
        {
            if (musicAudioSource.clip != musicPlayScreen)
            {
                musicAudioSource.clip = musicPlayScreen;
                //musicAudioSource.loop = true;
                musicAudioSource.Play();
            }
           
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaySFX(clickSound);
            }
            if (musicAudioSource.clip != musicMenuScreen)
            {
                musicAudioSource.clip = musicMenuScreen;
                //musicAudioSource.loop = true;
                musicAudioSource.Play();
            }

        }
    }


    public void PlaySFX(AudioClip sfxclip)
    {
        vfxAudioSource.clip = sfxclip;
        vfxAudioSource.PlayOneShot(sfxclip);
    }
}