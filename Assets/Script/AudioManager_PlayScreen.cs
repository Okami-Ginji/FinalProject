using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager_PlayScreen : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    //audio clip   
    public AudioClip musicPlayScreen;
    public AudioClip musicBossFight;
    public AudioClip clickSound;
    public AudioClip healthClip;
    public AudioClip expClip;

    private bool isBossMusicPlaying = false;
    private bool isGameMusicPlaying = false;

    //private static AudioManager_PlayScreen instance;

    //void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    private void Start()
    {
        musicAudioSource.clip = musicPlayScreen;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Meadow" ||
         SceneManager.GetActiveScene().name == "Desert" ||
         SceneManager.GetActiveScene().name == "Rock")
        {
            BossAI bossScript = FindObjectOfType<BossAI>();

            if (bossScript != null)
            {
               
                if (!isBossMusicPlaying)
                {
                    musicAudioSource.clip = musicBossFight;
                    musicAudioSource.Play();
                    isBossMusicPlaying = true;
                    isGameMusicPlaying = false; 
                }
            }
            else
            {
                
                if (!isGameMusicPlaying)
                {
                    musicAudioSource.clip = musicPlayScreen;
                    musicAudioSource.Play();
                    isGameMusicPlaying = true;
                    isBossMusicPlaying = false; 
                }
            }
        }       
    }


    public void PlaySFX(AudioClip sfxclip)
    {
        vfxAudioSource.clip = sfxclip;
        vfxAudioSource.PlayOneShot(sfxclip);
    }
}