using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Settings UI")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button backToMainButton; 
    
    private bool isPaused = false;
    private bool wasGamePausedBefore = false; 
    private UIManager uiManager;
    private AudioSettings audioSettings;

    void Start()
    {
        
        uiManager = FindObjectOfType<UIManager>();
        audioSettings = FindObjectOfType<AudioSettings>();

        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        
        SetupVolumeSliders();
        
        
        SetupButtonListeners();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            bool canPause = true;
            if (uiManager != null)
            {
                canPause = uiManager.CanPause();
            }

            if (canPause)
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    private void SetupVolumeSliders()
    {
        if (AudioManager.Instance != null)
        {
            
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = AudioManager.Instance.musicVolume;
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            if (soundVolumeSlider != null)
            {
                soundVolumeSlider.value = AudioManager.Instance.soundVolume;
                soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
            }
        }
    }

    private void SetupButtonListeners()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (quitGameButton != null)
            quitGameButton.onClick.AddListener(QuitGame);

        if (backToMainButton != null)
            backToMainButton.onClick.AddListener(BackToMainPauseMenu);
    }

    public void PauseGame()
    {
        
        wasGamePausedBefore = (Time.timeScale == 0f);
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
            
       
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        isPaused = false;
        
      
        if (!wasGamePausedBefore)
        {
            Time.timeScale = 1f;
        }
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
            
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Debug.Log("Game Resumed");
    }

    public void OpenSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
            
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

       
        UpdateSliderValues();
    }

    public void BackToMainPauseMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        
        if (audioSettings != null)
        {
            audioSettings.SaveAudioSettings();
        }

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
            
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

       
        if (audioSettings != null)
        {
            audioSettings.SaveAudioSettings();
        }

        Time.timeScale = 1f; 

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateSliderValues()
    {
        if (AudioManager.Instance != null)
        {
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = AudioManager.Instance.musicVolume;

            if (soundVolumeSlider != null)
                soundVolumeSlider.value = AudioManager.Instance.soundVolume;
        }
    }

    
    private void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    private void OnSoundVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSoundVolume(value);
        }
    }

    
    public bool IsPaused()
    {
        return isPaused;
    }

   
    public void ForcePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
    }

    
    public void ForceResume()
    {
        if (isPaused)
        {
            ResumeGame();
        }
    }
}