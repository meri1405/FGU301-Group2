using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_VOLUME_KEY = "SoundVolume";

    void Start()
    {
        LoadAudioSettings();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAudioSettings();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveAudioSettings();
        }
    }

    void OnDestroy()
    {
        SaveAudioSettings();
    }

    public void SaveAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, AudioManager.Instance.musicVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, AudioManager.Instance.soundVolume);
            PlayerPrefs.Save();
            
            Debug.Log($"Audio settings saved - Music: {AudioManager.Instance.musicVolume}, Sound: {AudioManager.Instance.soundVolume}");
        }
    }

    public void LoadAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
            float soundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1.0f);

            AudioManager.Instance.SetMusicVolume(musicVolume);
            AudioManager.Instance.SetSoundVolume(soundVolume);
            
            Debug.Log($"Audio settings loaded - Music: {musicVolume}, Sound: {soundVolume}");
        }
    }

    public void ResetAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(0.5f);
            AudioManager.Instance.SetSoundVolume(1.0f);
            SaveAudioSettings();
            
            Debug.Log("Audio settings reset to defaults");
        }
    }
}