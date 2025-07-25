using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject speedBuffIcon;
    [SerializeField] private GameObject powerBuffIcon;
    [SerializeField] private TextMeshProUGUI speedBuffTimer;
    [SerializeField] private TextMeshProUGUI powerBuffTimer;

    private PlayerControl playerControl;

    void Start()
    {
        
        playerControl = FindObjectOfType<PlayerControl>();
        
        
        if (speedBuffIcon != null) speedBuffIcon.SetActive(false);
        if (powerBuffIcon != null) powerBuffIcon.SetActive(false);
    }

    void Update()
    {
        if (playerControl == null) return;

       
        UpdateSpeedBuffUI();
        
        
        UpdatePowerBuffUI();
    }

    private void UpdateSpeedBuffUI()
    {
        bool hasSpeedBuff = playerControl.GetCurrentSpeedMultiplier() > 1f;
        
        if (speedBuffIcon != null)
        {
            speedBuffIcon.SetActive(hasSpeedBuff);
        }
        
        if (speedBuffTimer != null)
        {
            if (hasSpeedBuff)
            {
                
                speedBuffTimer.text = $"x{playerControl.GetCurrentSpeedMultiplier():F1}";
            }
            speedBuffTimer.gameObject.SetActive(hasSpeedBuff);
        }
    }

    private void UpdatePowerBuffUI()
    {
        bool hasPowerBuff = playerControl.GetCurrentDamageMultiplier() > 1f;
        
        if (powerBuffIcon != null)
        {
            powerBuffIcon.SetActive(hasPowerBuff);
        }
        
        if (powerBuffTimer != null)
        {
            if (hasPowerBuff)
            {
                
                powerBuffTimer.text = $"x{playerControl.GetCurrentDamageMultiplier():F1}";
            }
            powerBuffTimer.gameObject.SetActive(hasPowerBuff);
        }
    }
}