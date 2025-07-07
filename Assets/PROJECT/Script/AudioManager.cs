using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource soundEffectAudioSource;
    public AudioSource walkAudioSource; // AudioSource riêng cho walk sound
    
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
    
    [Header("Volume Controls")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float soundVolume = 1f;
    
    // Singleton để có thể truy cập từ bất kỳ script nào
    public static AudioManager Instance;
    
    void Awake()
    {
        // Tạo Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị xóa khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Phát nhạc nền khi game bắt đầu
        PlayBackgroundMusic();
    }
    
    // Phát nhạc nền
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicAudioSource != null)
        {
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.loop = true; // Lặp lại
            musicAudioSource.Play();
        }
    }
    
    // Dừng nhạc nền
    public void StopBackgroundMusic()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }
    
    // Phát hiệu ứng âm thanh bắn súng
    public void PlayShootSound()
    {
        PlaySoundEffect(shootSound);
    }
    
    // Bắt đầu phát âm thanh bước chân (liên tục)
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
    
    // Dừng âm thanh bước chân
    public void StopWalkSound()
    {
        if (walkAudioSource != null && walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }
    
    // Phát âm thanh bước chân (method cũ - giữ để tương thích)
    public void PlayWalkSound()
    {
        StartWalkSound();
    }
    
    // Phát âm thanh bị thương
    public void PlayHurtSound()
    {
        PlaySoundEffect(hurtSound);
    }
    
    // Phát âm thanh hồi máu
    public void PlayHealSound()
    {
        PlaySoundEffect(healSound);
    }
    
    // Phát âm thanh nhấn nút
    public void PlayButtonSound()
    {
        PlaySoundEffect(buttonClickSound);
    }
    
    // Phát âm thanh enemy chết
    public void PlayEnemyDeathSound()
    {
        PlaySoundEffect(enemyDeathSound);
    }
    
    // Phát âm thanh player chết
    public void PlayPlayerDeathSound()
    {
        PlaySoundEffect(playerDeathSound);
    }
    
    // Hàm chung để phát hiệu ứng âm thanh
    private void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && soundEffectAudioSource != null)
        {
            soundEffectAudioSource.volume = soundVolume;
            soundEffectAudioSource.PlayOneShot(clip);
        }
    }
    
    // Thay đổi âm lượng nhạc
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }
    
    // Thay đổi âm lượng hiệu ứng
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