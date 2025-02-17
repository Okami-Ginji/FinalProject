using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    //audio clip
    public AudioClip musicClip;
    public AudioClip healthClip;

    private void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip sfxclip)
    {
        vfxAudioSource.clip = sfxclip;
        vfxAudioSource.PlayOneShot(sfxclip);
    }
}