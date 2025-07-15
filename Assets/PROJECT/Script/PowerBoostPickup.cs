using UnityEngine;

public class PowerBoostPickup : MonoBehaviour
{
    [Header("Power Boost Settings")]
    [SerializeField] private float damageMultiplier = 2f; // Hệ số tăng sức mạnh
    [SerializeField] private float boostDuration = 5f; // Thời gian hiệu lực (giây)
    [SerializeField] private GameObject pickupEffect; // Hiệu ứng khi nhặt
    [SerializeField] private float lifeTime = 15f; // Thời gian tồn tại của vật phẩm

    void Start()
    {
        // Đảm bảo object có Collider2D và được set là Trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning($"PowerBoostPickup {gameObject.name} không có Collider2D!");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning($"PowerBoostPickup {gameObject.name} Collider2D cần được set là Trigger!");
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
                // Áp dụng hiệu ứng tăng sức mạnh
                playerControl.ApplyPowerBoost(damageMultiplier, boostDuration);

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

                Debug.Log($"Player nhặt Power Boost! Sức mạnh x{damageMultiplier} trong {boostDuration} giây");

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
        
        float pulseScale = 1f + 0.2f * Mathf.Sin(Time.time * 6f);
        transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
    }
}