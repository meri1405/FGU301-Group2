using UnityEngine;

public class ZombieChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float damage;
    public float health = 30.0f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;
    [SerializeField] private float deathAnimationDuration = 2f; // Thời gian animation chết

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Nếu player chưa được gán, tìm tự động
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        // Không di chuyển nếu đã chết hoặc không tìm thấy player
        if (isDead || player == null) return;

        // Di chuyển về phía player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Quay mặt zombie theo vị trí player
        if (player.position.x < transform.position.x)
        {
            // Player bên trái → zombie nhìn trái
            spriteRenderer.flipX = true;
        }
        else
        {
            // Player bên phải → zombie nhìn phải
            spriteRenderer.flipX = false;
        }
    }

    // Xử lý khi zombie va chạm với player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Gây sát thương cho player
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // Xử lý khi zombie bị bắn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Bullet"))
        {
            // Lấy thông tin sát thương từ đạn
            var bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }
    }

    // Phương thức để zombie nhận sát thương
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        Debug.Log($"Zombie took damage: {damageAmount}, Health remaining: {health}");

        // Hiệu ứng nháy đỏ khi bị đánh
        StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            Debug.Log("Zombie health <= 0, triggering Die");
            Die();
        }
    }

    // Hiệu ứng nháy màu khi bị đánh
    private System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // Xử lý khi zombie chết
    private void Die()
    {
        Debug.Log("Die method called. Setting isDead = true");
        isDead = true;

        // Phát âm thanh enemy chết
        PlayEnemyDeathSound();

        // Vô hiệu hóa collision để không va chạm với player
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Dừng di chuyển nếu có Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Thay thế velocity bằng linearVelocity
            rb.bodyType = RigidbodyType2D.Kinematic; // Không chịu tác động của lực vật lý
        }

        // Kích hoạt animation chết
        if (animator != null)
        {
            Debug.Log("Setting Die trigger on animator");
            animator.SetTrigger("Die");
        }

        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy died → +1 kill");
            KillCounterTMP.Instance.AddKill();
        }

        // Hủy game object sau khi animation kết thúc
        Debug.Log($"Scheduled destruction in {deathAnimationDuration} seconds");
        Destroy(gameObject, deathAnimationDuration);
    }

    // Phát âm thanh khi enemy chết
    private void PlayEnemyDeathSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeathSound();
            Debug.Log("🔊 Playing enemy death sound");
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null - cannot play enemy death sound!");
        }
    }

    // Phương thức lấy lượng sát thương của zombie
    public float GetDamage()
    {
        return damage;
    }
}