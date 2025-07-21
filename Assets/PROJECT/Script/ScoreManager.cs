using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public ScoreData scoreData; // Kéo ScriptableObject vào đây trong Inspector
    private const string scoreKey = "PlayerScore";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore(); // Khôi phục khi khởi động
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
