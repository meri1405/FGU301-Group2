using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource soundEffectAudioSource;
    public AudioSource walkAudioSource; 
    
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    
    [Header("Sound Effects")]
    public AudioClip shootSound;
    public AudioClip walkSound;
    public AudioClip hurtSound;
    public AudioClip healSound;
    public AudioClip buttonClickSound;
    public AudioClip enemyDeathSound;
    public AudioClip playerDeathSound;
    public AudioClip pickupPowerupsSound; 

    [Header("Volume Controls")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float soundVolume = 1f;
    
    
    public static AudioManager Instance;
    
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
        PlayBackgroundMusic();
    }
    
   
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicAudioSource != null)
        {
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.loop = true; 
            musicAudioSource.Play();
        }
    }
    
    
    public void StopBackgroundMusic()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }
    
   
    public void PlayShootSound()
    {
        PlaySoundEffect(shootSound);
    }
    
    
    public void StartWalkSound()
    {
        if (walkSound != null && walkAudioSource != null && !walkAudioSource.isPlaying)
        {
            walkAudioSource.clip = walkSound;
            walkAudioSource.volume = soundVolume;
            walkAudioSource.loop = true;
            walkAudioSource.Play();
        }
    }
    
    
    public void StopWalkSound()
    {
        if (walkAudioSource != null && walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }
    
    
    public void PlayWalkSound()
    {
        StartWalkSound();
    }
    
   
    public void PlayHurtSound()
    {
        PlaySoundEffect(hurtSound);
    }
    
    
    public void PlayHealSound()
    {
        PlaySoundEffect(healSound);
    }
    
    
    public void PlayButtonSound()
    {
        PlaySoundEffect(buttonClickSound);
    }
    
   
    public void PlayEnemyDeathSound()
    {
        PlaySoundEffect(enemyDeathSound);
    }

    public void PlayPickupSound()
    {
        PlaySoundEffect(pickupPowerupsSound);
    }

    
    public void PlayPlayerDeathSound()
    {
        PlaySoundEffect(playerDeathSound);
    }
    
    
    private void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && soundEffectAudioSource != null)
        {
            soundEffectAudioSource.volume = soundVolume;
            soundEffectAudioSource.PlayOneShot(clip);
        }
    }
    
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }
    
    
    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        if (soundEffectAudioSource != null)
        {
            soundEffectAudioSource.volume = soundVolume;
        }
        if (walkAudioSource != null)
        {
            walkAudioSource.volume = soundVolume;
        }
    }
}