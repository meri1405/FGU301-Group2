using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healthAmount = 20f;
    [SerializeField] private GameObject pickupEffect;
    public float lifeTime = 10f;
    void Start()
    {
        // Đảm bảo object có Collider2D và được set là Trigger
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
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (collision.CompareTag("Player"))
        {
            // Tìm thành phần Health trên player
            Health playerHealth = collision.GetComponent<Health>();

            // Nếu người chơi có component health
            if (playerHealth != null)
            {
                // Kiểm tra xem player có cần hồi máu không (không hồi khi đã full máu)
                if (playerHealth.GetCurrentHealth() >= playerHealth.GetMaxHealth())
                {
                    Debug.Log("Player đã có máu đầy, không cần hồi máu!");
                    return; // Không nhặt item nếu máu đã đầy
                }

                // Tăng máu cho người chơi
                playerHealth.AddHealth(healthAmount);

                // Tạo hiệu ứng hạt (nếu có)
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                // Hủy vật phẩm sau khi nhặt
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
        // Hiệu ứng nhấp nháy nhẹ để thu hút sự chú ý
        float pulseScale = 1f + 0.1f * Mathf.Sin(Time.time * 4f);
        transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
    }
}
