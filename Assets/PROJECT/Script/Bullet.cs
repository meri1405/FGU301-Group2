using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage;
    public int per; 

    [Header("Movement Settings")]
    [SerializeField] private float speed = 15f; 
    [SerializeField] private float lifetime = 3f; 

    [Header("Effects")]
    [SerializeField] private GameObject hitEffect; 

    private Rigidbody2D rigid;
    private float baseDamage;
    private PlayerControl playerControl; 

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
            rigid.gravityScale = 0;
        }

        
        playerControl = FindObjectOfType<PlayerControl>();
        if (playerControl == null)
        {
            Debug.LogWarning("PlayerControl not found in scene!");
        }
    }

    void Start()
    {
        
        baseDamage = damage;
        
        
        ApplyDamageBoost();

       
        Destroy(gameObject, lifetime);
    }

    public void Init(float damage, int per, Vector3 dir)
    {
       
        this.baseDamage = damage;
        this.damage = damage;
        this.per = per;

        
        ApplyDamageBoost();

        if (per >= 0)
        {
           
            rigid.linearVelocity = dir * speed;

            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

       
        Debug.Log($"Bullet initialized with base damage: {baseDamage}, final damage: {damage}, penetration: {per}");
    }

    private void ApplyDamageBoost()
    {
        if (playerControl != null)
        {
            float damageMultiplier = playerControl.GetCurrentDamageMultiplier();
            damage = baseDamage * damageMultiplier;
            
            if (damageMultiplier > 1f)
            {
                Debug.Log($"Damage boost applied! Base: {baseDamage} -> Boosted: {damage} (x{damageMultiplier})");
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        baseDamage = newDamage;
        damage = newDamage;
        
        
        ApplyDamageBoost();
        
        Debug.Log($"Bullet damage updated to: {damage}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Bullet hit enemy for {damage} damage");
            }

            
            per--;

            if (per < 0)
            {
                rigid.linearVelocity = Vector2.zero;
                Destroy(gameObject); 
            }
        }
        
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

           
            Destroy(gameObject);
        }
    }
}