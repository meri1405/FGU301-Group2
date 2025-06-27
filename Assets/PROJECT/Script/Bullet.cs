using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        public float damage;
        public int per;

        Rigidbody2D rigid;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        public void Init(float damage, int per, Vector3 dir)
        {
            this.damage = damage;
            this.per = per;

            if (per >= 0) {
                // Thiết lập vận tốc cho đạn
                rigid.linearVelocity = dir * 15f; // linearVelocity đã được đổi thành velocity
                
                // Xoay đạn theo hướng di chuyển
                // Nếu sprite đang hướng lên trên, thêm offset 90 độ:
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                
                // Debug để kiểm tra
                Debug.Log($"Bullet angle: {angle}, Direction: {dir}");
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Enemy") || per == -100)
                return;

            per--;

            if (per < 0) {
                rigid.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }