using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager_MenuScreen : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    //audio clip
    public AudioClip musicMenuScreen;
    public AudioClip clickSound;

    private bool isBossMusicPlaying = false;
    private bool isGameMusicPlaying = false;

    public static AudioManager_MenuScreen instance;

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


    public void PlaySFX(AudioClip sfxclip)
    {
        vfxAudioSource.clip = sfxclip;
        vfxAudioSource.PlayOneShot(sfxclip);
    }
}