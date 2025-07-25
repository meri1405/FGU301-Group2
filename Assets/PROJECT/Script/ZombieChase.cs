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
    [SerializeField] private float deathAnimationDuration = 2f; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        
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
        
        if (isDead || player == null) return;

       
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        
        if (player.position.x < transform.position.x)
        {
           
            spriteRenderer.flipX = true;
        }
        else
        {
           
            spriteRenderer.flipX = false;
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Bullet"))
        {
            
            var bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }
    }

    
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        Debug.Log($"Zombie took damage: {damageAmount}, Health remaining: {health}");

        
        StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            Debug.Log("Zombie health <= 0, triggering Die");
            Die();
        }
    }

    
    private System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

   
    private void Die()
    {
        Debug.Log("Die method called. Setting isDead = true");
        isDead = true;

        
        PlayEnemyDeathSound();

        
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
            Debug.Log("Setting Die trigger on animator");
            animator.SetTrigger("Die");
        }

        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy died → +1 kill");
            KillCounterTMP.Instance?.AddKill();
            FindObjectOfType<PlayerDash>()?.OnEnemyKilled();
        }


       
        Debug.Log($"Scheduled destruction in {deathAnimationDuration} seconds");
        Destroy(gameObject, deathAnimationDuration);
    }

    
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

    
    public float GetDamage()
    {
        return damage;
    }
}