using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float boostDuration = 2f; 
    [SerializeField] private GameObject pickupEffect; 
    [SerializeField] private float lifeTime = 15f;

    void Start()
    {
        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning($"SpeedBoostPickup {gameObject.name} không có Collider2D!");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning($"SpeedBoostPickup {gameObject.name} Collider2D cần được set là Trigger!");
            collider.isTrigger = true;
        }

       
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            
            PlayerControl playerControl = collision.GetComponent<PlayerControl>();

            if (playerControl != null)
            {
               
                playerControl.ApplySpeedBoost(speedMultiplier, boostDuration);

                
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPickupSound();
                }

                Debug.Log($"Player nhặt Speed Boost! Tốc độ x{speedMultiplier} trong {boostDuration} giây");

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player không có component PlayerControl!");
            }
        }
    }

    void Update()
    {
        
        
        float pulseScale = 1f + 0.15f * Mathf.Sin(Time.time * 5f);
        transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
    }
}