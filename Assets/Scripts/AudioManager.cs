using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sound;
    [SerializeField] public AudioClip[] musicClips;
    [SerializeField] public AudioClip[] hitClips;
    [SerializeField] public AudioClip columnPowerUp;
    [SerializeField] public AudioClip rowPowerUp;
    [SerializeField] public AudioClip powerUse;
    [SerializeField] public AudioClip countdownSound;
    [SerializeField] public AudioClip expGainedSound;
    [SerializeField] public AudioClip levelUpSound;

    public bool isMuted = false;
    public bool isPlayingSound = false;

    public static AudioManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic(0);
    }

    public void PlayMusic(int i)
    {
        switch (i)
        {
            case 0:
                music.volume = .6f;
                music.loop = false;
                break;
            case 1:
                music.volume = .3f;
                music.loop = true;
                break;
            default:
                break;
        }
        if (!isMuted)
        {
            music.clip = musicClips[i];
            music.Play();
        }
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void PauseMusic()
    {
        music.Pause();
    }

    public void Play()
    {
        if (!isMuted)
        {
            music.Play();
        }
    }

    public void PlayHitSound()
    {
        sound.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)]);
    }

    public void PlayColumnBombSound()
    {
        sound.PlayOneShot(columnPowerUp);
    }

    public void PlayRowBombSound()
    {
        sound.PlayOneShot(rowPowerUp);
    }

    public void PlayPowerUse()
    {
        sound.PlayOneShot(powerUse);
    }

    public void PlayCountdownSound()
    {
        sound.PlayOneShot(countdownSound);
    }

    public IEnumerator PlayExperienceGainedSound()
    {
        isPlayingSound = true;
        sound.clip = expGainedSound;
        sound.Play();
        yield return new WaitForSeconds(.15f);
        isPlayingSound = false;
    }

    public void StopExperienceGainedSound()
    {
        sound.loop = false;
    }

    public void PlayLevelUpSound()
    {
        sound.PlayOneShot(levelUpSound);
    }

}
