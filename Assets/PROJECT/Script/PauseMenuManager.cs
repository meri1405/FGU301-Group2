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
    [SerializeField] private Button backToMainButton; // N�t quay l?i t? settings panel
    
    private bool isPaused = false;
    private bool wasGamePausedBefore = false; // ?? ki?m tra game ?� b? pause tr??c ?� ch?a
    private UIManager uiManager;
    private AudioSettings audioSettings;

    void Start()
    {
        // T�m UIManager v� AudioSettings
        uiManager = FindObjectOfType<UIManager>();
        audioSettings = FindObjectOfType<AudioSettings>();

        // ??m b?o pause menu b? ?n khi b?t ??u
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Setup sliders v?i gi� tr? hi?n t?i t? AudioManager
        SetupVolumeSliders();
        
        // Setup button listeners
        SetupButtonListeners();
    }

    void Update()
    {
        // Ki?m tra n?u ng??i ch?i ?n ESC v� c� th? pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ki?m tra xem c� th? pause kh�ng
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
            // Set gi� tr? ban ??u cho sliders
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
        // L?u tr?ng th�i time scale tr??c ?�
        wasGamePausedBefore = (Time.timeScale == 0f);
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
            
        // ??m b?o settings panel b? ?n
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Hi?n th? cursor
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
        
        // Ch? resume n?u game kh�ng b? pause t? tr??c
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

        // C?p nh?t l?i slider values khi m? settings
        UpdateSliderValues();
    }

    public void BackToMainPauseMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        // L?u settings khi quay l?i
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

        // L?u settings tr??c khi quit
        if (audioSettings != null)
        {
            audioSettings.SaveAudioSettings();
        }

        Time.timeScale = 1f; // Reset time scale tr??c khi quit

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

    // Callback functions cho sliders
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

    // Public getter ?? c�c script kh�c c� th? ki?m tra tr?ng th�i pause
    public bool IsPaused()
    {
        return isPaused;
    }

    // Method ?? force pause t? script kh�c
    public void ForcePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
    }

    // Method ?? force resume t? script kh�c
    public void ForceResume()
    {
        if (isPaused)
        {
            ResumeGame();
        }
    }
}