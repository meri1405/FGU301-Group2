using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float deathAnimationDuration = 2f; // Thời gian animation chết

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        UnityEngine.Debug.Log($"{gameObject.name} took damage: {damageAmount}, Health remaining: {currentHealth}");

        // Hiệu ứng nháy đỏ khi bị đánh
        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            UnityEngine.Debug.Log($"{gameObject.name} health <= 0, triggering Die");
            Die();
        }
    }

    // Hiệu ứng nháy màu khi bị đánh
    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // Xử lý khi chết
    private void Die()
    {
        UnityEngine.Debug.Log("Die method called. Setting isDead = true");
        isDead = true;

        // Vô hiệu hóa collision
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Dừng di chuyển nếu có Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Kích hoạt animation chết
        if (animator != null)
        {
            UnityEngine.Debug.Log("Setting Die trigger on animator");
            animator.SetTrigger("Die");
        }

        // Xử lý đặc biệt nếu là player
        if (gameObject.CompareTag("Player"))
        {
            // Không hủy player, chỉ vô hiệu hóa
            // Có thể thêm logic hiển thị màn hình Game Over sau animation
            StartCoroutine(GameOverAfterAnimation());
        }
        else
        {
            // Đối với các đối tượng không phải player (như Zombie)
            Destroy(gameObject, deathAnimationDuration);
        }
    }

    // Hiển thị Game Over sau khi animation chết kết thúc
    private IEnumerator GameOverAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);

        // Tại đây có thể thêm logic hiển thị màn hình Game Over
        UnityEngine.Debug.Log("Game Over!");

        // Nếu có GameManager, gọi phương thức GameOver
        // GameManager.Instance.GameOver();
    }

    // Các phương thức truy vấn
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}