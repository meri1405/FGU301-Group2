using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject EndPanel;
    public GameObject healthBar;
    public GameObject killTextTMP;

    private PauseMenuManager pauseMenuManager;

    void Start()
    {
        if (startPanel == null)
        {
            Debug.LogError("startPanel is not assigned!");
        }
        if (EndPanel == null)
        {
            Debug.LogError("EndPanel is not assigned!");
        }

        startPanel.SetActive(true);
        EndPanel.SetActive(false);

        if (healthBar != null) healthBar.SetActive(false);
        if (killTextTMP != null) killTextTMP.SetActive(false);
        else
        {
            Debug.LogError("healthBar or killTextTMP is not assigned!");
        }

        
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();

        Time.timeScale = 0;
    }

    public void startGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        startPanel.SetActive(false);
        if (healthBar != null) healthBar.SetActive(true);
        if (killTextTMP != null) killTextTMP.SetActive(true);
        Time.timeScale = 1f;
    }

    public void endGame()
    {
        
        if (pauseMenuManager != null)
        {
            pauseMenuManager.ForceResume();
        }

        EndPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void restartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        Time.timeScale = 1f;

#if UNITY_EDITOR
        // Nếu đang chạy trong Unity Editor → dừng Play Mode
        EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }

    void Update()
    {
        
        bool canPause = !startPanel.activeSelf && !EndPanel.activeSelf;
        
        if (canPause && pauseMenuManager != null && Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    
    public bool CanPause()
    {
        return !startPanel.activeSelf && !EndPanel.activeSelf;
    }
}
