using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float deathAnimationDuration = 2f;

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
        if (isDead)
        {
            Debug.Log($"{gameObject.name} already dead. Ignored.");
            return;
        }

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage, remaining: {currentHealth}");

        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} health <= 0 → DIE()");
            Die();
        }
    }

    // Phương thức tăng máu (được gọi từ HealthPickup)
    public void AddHealth(float amount)
    {
        if (isDead)
        {
            Debug.Log($"{gameObject.name} is dead. Cannot heal.");
            return;
        }

        float oldHealth = currentHealth;
        // Tăng máu nhưng không vượt quá giới hạn
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        float actualHealing = currentHealth - oldHealth;
        
        Debug.Log($"{gameObject.name} healed {actualHealing} HP. Current health: {currentHealth}/{maxHealth}");

        // Cập nhật UI nếu có (có thể thêm sau)
        UpdateHealthUI();
    }

    // Phương thức cập nhật UI (có thể mở rộng sau)
    private void UpdateHealthUI()
    {
        // Tìm và cập nhật UI health bar nếu có
        // Có thể gọi event hoặc tìm UIManager để cập nhật
        // Tạm thời để trống, có thể mở rộng sau
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} DIED");
        isDead = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(GameOverAfterAnimation());
        }
        else
        {
            //  Thêm xử lý tăng kill
            if (gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy died. Kill +1");
                KillCounterTMP.Instance.AddKill();
            }

            Destroy(gameObject, deathAnimationDuration);
        }
    }

    private IEnumerator GameOverAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        Debug.Log("Game Over!");

        // Gọi UIManager để hiện EndPanel
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
        {
            ui.endGame();
        }
        else
        {
            Debug.LogError("UIManager not found in scene!");
        }
    }

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
