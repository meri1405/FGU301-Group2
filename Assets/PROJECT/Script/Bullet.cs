using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 10f; // Tốc độ của viên đạn
    public float lifetime = 2f; // Thời gian sống của viên đạn
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            // Xử lý va chạm với Zombie
            Destroy(other.gameObject); // Giết Zombie
            Destroy(gameObject); // Hủy viên đạn
        }
        else if (other.CompareTag("Wall"))
        {
            // Hủy viên đạn khi va chạm với tường
            Destroy(gameObject);
        }
    }

}
