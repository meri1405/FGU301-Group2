using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class RoundManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public EnemySpawner spawner;
    public float roundDuration = 90f;
    public int zombiesPerRound = 30;
    public float timeBetweenRounds = 3f;
    public int maxRounds = 5;

    [Header("UI Elements")]
    public TMP_Text roundText;
    public TMP_Text timerText;
    public TMP_Text roundTransitionText;
    public GameObject winScreen; // Panel hiện ra khi thắng

    [Header("Transition Settings")]
    public float transitionDisplayTime = 2f;

    private float timeLeft;
    private int currentRound = 0;

    void Start()
    {
        StartCoroutine(HandleRounds());
    }

    IEnumerator HandleRounds()
    {
        while (currentRound < maxRounds)
        {
            currentRound++;
            roundText.text = "Round " + currentRound;
            timeLeft = roundDuration;

            spawner.ClearAllZombies();

            // 🎯 Hiệu ứng chuyển round
            yield return StartCoroutine(ShowRoundTransition(currentRound));

            // Spawn zombie cho round
            spawner.SpawnZombiesForRound(currentRound, zombiesPerRound);

            // Đếm ngược thời gian
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = FormatTime(timeLeft);
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenRounds);
        }

        WinGame();
    }

    IEnumerator ShowRoundTransition(int round)
    {
        roundTransitionText.text = "ROUND " + round;
        roundTransitionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(transitionDisplayTime);
        roundTransitionText.gameObject.SetActive(false);
    }

    void WinGame()
    {
        roundText.text = "YOU WIN!";
        timerText.text = "";
        winScreen.SetActive(true);
        spawner.ClearAllZombies();
        Time.timeScale = 0f; // Dừng game (tuỳ chọn)
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
