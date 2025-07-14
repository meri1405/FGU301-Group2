using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage;
    public int per; // Số lần đạn có thể xuyên qua mục tiêu

    [Header("Movement Settings")]
    [SerializeField] private float speed = 15f; // Tốc độ đạn
    [SerializeField] private float lifetime = 3f; // Thời gian tồn tại của đạn

    [Header("Effects")]
    [SerializeField] private GameObject hitEffect; // Hiệu ứng khi bắn trúng

    private Rigidbody2D rigid;
    private float baseDamage; // Lưu damage gốc
    private PlayerControl playerControl; // Tham chiếu tới PlayerControl

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
            rigid.gravityScale = 0;
        }

        // Tìm PlayerControl trong scene
        playerControl = FindObjectOfType<PlayerControl>();
        if (playerControl == null)
        {
            Debug.LogWarning("PlayerControl not found in scene!");
        }
    }

    void Start()
    {
        // Lưu damage gốc khi bullet được tạo
        baseDamage = damage;
        
        // Áp dụng damage boost ngay khi bullet được tạo
        ApplyDamageBoost();

        // Hủy viên đạn sau thời gian lifetime
        Destroy(gameObject, lifetime);
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        // Lưu damage gốc
        this.baseDamage = damage;
        this.damage = damage;
        this.per = per;

        // Áp dụng damage boost
        ApplyDamageBoost();

        if (per >= 0)
        {
            // Thiết lập vận tốc cho đạn
            rigid.linearVelocity = dir * speed;

            // Xoay đạn theo hướng di chuyển
            // Offset góc phụ thuộc vào hướng mặc định của sprite đạn
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Debug log để kiểm tra damage boost
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
        
        // Áp dụng lại damage boost
        ApplyDamageBoost();
        
        Debug.Log($"Bullet damage updated to: {damage}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra va chạm với Enemy
        if (collision.CompareTag("Enemy"))
        {
            // Sử dụng component Health nếu enemy có
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Bullet hit enemy for {damage} damage");
            }

            // Giảm số lần xuyên qua
            per--;

            if (per < 0)
            {
                rigid.linearVelocity = Vector2.zero;
                Destroy(gameObject); // Hủy đạn thay vì chỉ SetActive(false)
            }
        }
        // Xử lý va chạm với các đối tượng khác (tường, vật cản...)
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            // Tạo hiệu ứng va chạm nếu có
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // Hủy đạn khi va chạm với vật cản
            Destroy(gameObject);
        }
    }
}