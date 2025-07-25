using UnityEngine;
using TMPro; 

public class KillCounterTMP : MonoBehaviour
{
    public TMP_Text killText;  
    private int killCount = 0;

    public static KillCounterTMP Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddKill()
    {
        killCount++;
        UpdateKillText();
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(1);
        }
    }

    private void UpdateKillText()
    {
        if (killText != null)
        {
            killText.text = "Kills: " + killCount;
        }
    }
}
