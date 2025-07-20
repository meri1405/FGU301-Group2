using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 2f; // Hệ số tăng tốc độ
    [SerializeField] private float boostDuration = 2f; // Thời gian hiệu lực (giây)
    [SerializeField] private GameObject pickupEffect; // Hiệu ứng khi nhặt
    [SerializeField] private float lifeTime = 15f; // Thời gian tồn tại của vật phẩm

    void Start()
    {
        // Đảm bảo object có Collider2D và được set là Trigger
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

        // Hủy vật phẩm sau thời gian lifeTime
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (collision.CompareTag("Player"))
        {
            // Tìm thành phần PlayerControl trên player
            PlayerControl playerControl = collision.GetComponent<PlayerControl>();

            if (playerControl != null)
            {
                // Áp dụng hiệu ứng tăng tốc
                playerControl.ApplySpeedBoost(speedMultiplier, boostDuration);

                // Tạo hiệu ứng hạt (nếu có)
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                // Phát âm thanh pickup nếu có
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPickupSound();
                }

                Debug.Log($"Player nhặt Speed Boost! Tốc độ x{speedMultiplier} trong {boostDuration} giây");

                // Hủy vật phẩm sau khi nhặt
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
        // Hiệu ứng nhấp nháy để thu hút sự chú ý
        
        float pulseScale = 1f + 0.15f * Mathf.Sin(Time.time * 5f);
        transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
    }
}