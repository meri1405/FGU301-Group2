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

      
        if (gameObject.CompareTag("Player"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayHurtSound();
            }
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} health <= 0 → DIE()");
            Die();
        }
    }

    
    public void AddHealth(float amount)
    {
        if (isDead)
        {
            Debug.Log($"{gameObject.name} is dead. Cannot heal.");
            return;
        }

        float oldHealth = currentHealth;
       
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        float actualHealing = currentHealth - oldHealth;

        Debug.Log($"{gameObject.name} healed {actualHealing} HP. Current health: {currentHealth}/{maxHealth}");

       
        if (actualHealing > 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHealSound();
        }

        
        UpdateHealthUI();
    }

    
    private void UpdateHealthUI()
    {
        
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

        
        if (AudioManager.Instance != null)
        {
            if (gameObject.CompareTag("Player"))
            {
                AudioManager.Instance.PlayPlayerDeathSound();
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                AudioManager.Instance.PlayEnemyDeathSound();
            }
        }

       
        if (gameObject.CompareTag("Player"))
        {
            
            PlayerControl playerControl = GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.DestroyWeapon();
            }
            
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopWalkSound();
            }
        }

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
