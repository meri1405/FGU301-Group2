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

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
            rigid.gravityScale = 0;
        }
    }

    void Start()
    {
        // Hủy viên đạn sau thời gian lifetime
        Destroy(gameObject, lifetime);
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            // Thiết lập vận tốc cho đạn
            rigid.linearVelocity = dir * speed; // Sử dụng velocity thay vì linearVelocity

            // Xoay đạn theo hướng di chuyển
            // Offset góc phụ thuộc vào hướng mặc định của sprite đạn
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra va chạm với Enemy
        if (collision.CompareTag("Enemy"))
        {

            // Hoặc sử dụng component Health nếu enemy có
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
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