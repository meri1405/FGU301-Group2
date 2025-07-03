using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject startPanel;
    public GameObject EndPanel;
    public GameObject healthBar;
    public GameObject killTextTMP;
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
        Time.timeScale = 0; // Pause the game at the start
    }

    public void startGame()
    {
        startPanel.SetActive(false);
        if (healthBar != null) healthBar.SetActive(true);
        if (killTextTMP != null) killTextTMP.SetActive(true);
        Time.timeScale = 1f; // Resume the game
    }
    public void endGame()
    {
        EndPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game when ending
    }
    public void restartGame()
    {
        Time.timeScale = 1f; // Reset lại time scale trước khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void quitGame()
    {
        Application.Quit(); // Quit the application
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
