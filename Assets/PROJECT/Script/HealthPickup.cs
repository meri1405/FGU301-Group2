using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healthAmount = 20f;
    [SerializeField] private GameObject pickupEffect;
    public float lifeTime = 10f;
    void Start()
    {
       
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning($"HealthPickup {gameObject.name} không có Collider2D!");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning($"HealthPickup {gameObject.name} Collider2D cần được set là Trigger!");
            collider.isTrigger = true;
        }

         Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            
            Health playerHealth = collision.GetComponent<Health>();

            
            if (playerHealth != null)
            {
                
                if (playerHealth.GetCurrentHealth() >= playerHealth.GetMaxHealth())
                {
                    Debug.Log("Player đã có máu đầy, không cần hồi máu!");
                    return; 
                }

               
                playerHealth.AddHealth(healthAmount);

                
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player không có component Health!");
            }
        }
    }

    void Update()
    {
        
        float pulseScale = 1f + 0.1f * Mathf.Sin(Time.time * 4f);
        transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
    }
}
