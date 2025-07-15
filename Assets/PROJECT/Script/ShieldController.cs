using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float maxShieldHealth = 50f;
    public float explosionRadius = 3f;
    public float explosionDamage = 30f;
    public GameObject explosionEffect;

    private float currentShieldHealth;
    public bool isActive = false;
    private PlayerControl player;

    void Start()
    {
        currentShieldHealth = maxShieldHealth;
        player = GetComponentInParent<PlayerControl>();
        gameObject.SetActive(false); // Tắt khiên ban đầu
    }

    public void ActivateShield()
    {
        if (!isActive)
        {
            currentShieldHealth = maxShieldHealth;
            gameObject.SetActive(true);
            isActive = true;
            Debug.Log("🛡 Shield activated!");
        }
        else
        {
            Debug.Log("🛡 Shield is already active.");
        }
    }

    public float AbsorbDamageAndReturnLeftover(float damage)
    {
        if (!isActive) return damage;

        currentShieldHealth -= damage;
        Debug.Log($"🛡️ Shield took {damage} damage, remaining: {currentShieldHealth}");

        if (currentShieldHealth <= 0)
        {
            float leftover = -currentShieldHealth; // phần damage vượt quá
            Explode();
            isActive = false;
            gameObject.SetActive(false);
            return leftover;
        }

        return 0f; // absorb hết
    }


    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Health enemyHealth = hit.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(explosionDamage);
                }
            }
        }

        isActive = false;
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    public bool IsActive()
    {
        return isActive;
    }
}
