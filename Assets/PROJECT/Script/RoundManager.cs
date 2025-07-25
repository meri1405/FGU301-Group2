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

    [Header("Win UI")]
    public GameObject endPanel;

    [Header("UI Elements")]
    public TMP_Text roundText;
    public TMP_Text timerText;
    public TMP_Text roundTransitionText;
    public GameObject winScreen; 

    [Header("Transition Settings")]
    public float transitionDisplayTime = 2f;

    private float timeLeft;
    private int currentRound = 0;

    void Start()
    {
        endPanel.SetActive(false);
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

            
            yield return StartCoroutine(ShowRoundTransition(currentRound));

            
            spawner.SpawnZombiesForRound(currentRound, zombiesPerRound);

            
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = FormatTime(timeLeft);
                yield return null;
            }

           
            if (currentRound == 3)
            {
                yield return new WaitForSeconds(1f); 
                WinGame();
                yield break;
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
        roundText.text = "";
        timerText.text = "";

        roundTransitionText.gameObject.SetActive(false);

        spawner.ClearAllZombies();
        endPanel.SetActive(true); 
        Time.timeScale = 0f;
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
}
