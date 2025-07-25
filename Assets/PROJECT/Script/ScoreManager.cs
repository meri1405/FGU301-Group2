using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public ScoreData scoreData; 
    private const string scoreKey = "PlayerScore";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        scoreData.currentScore += amount;
        SaveScore();
    }

    public void ResetScore()
    {
        scoreData.currentScore = 0;
        SaveScore();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(scoreKey, scoreData.currentScore);
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        scoreData.currentScore = PlayerPrefs.GetInt(scoreKey, 0);
    }
}
