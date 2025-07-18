﻿using UnityEngine;
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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

        Time.timeScale = 1f; // Reset lại time scale trước khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }

#if UNITY_EDITOR
        // Nếu đang chạy trong Unity Editor → dừng Play Mode
        EditorApplication.isPlaying = false;
#else
        // Nếu đã build ra file → thoát game
        Application.Quit();
#endif
    }

    void Update()
    {

    }
}
