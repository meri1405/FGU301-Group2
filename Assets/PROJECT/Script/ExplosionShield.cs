using UnityEngine;

public class ExplosionShield : MonoBehaviour
{
    public float shieldDuration = 5f;
    public float shieldHealth = 30f;
    public float explosionRadius = 3f;
    public float explosionDamage = 100f;
    public GameObject explosionEffect;
    public LayerMask enemyLayer;

    private float currentHealth;

    private void Start()
    {
        currentHealth = shieldHealth;
        Invoke(nameof(Explode), shieldDuration); // tự nổ sau thời gian
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Gây sát thương
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Health enemyHealth = hit.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(explosionDamage);
                    Debug.Log($"💥 Shield exploded and hit {hit.name} for {explosionDamage}");
                }
            }
        }

        // Hiệu ứng nổ
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // huỷ khiên sau khi nổ
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Giảm máu khi bị va chạm với enemy
            TakeDamage(10f); // mỗi va chạm trừ 10 máu (tùy chỉnh)
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
