using UnityEngine;

public class ZombieChase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public float speed = 3f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (player == null) return;

            // Di chuyển về phía player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Quay mặt zombie theo vị trí player
            if (player.position.x < transform.position.x)
            {
                // Player bên trái → zombie nhìn trái
                spriteRenderer.flipX = true;
            }
            else
            {
                // Player bên phải → zombie nhìn phải
                spriteRenderer.flipX = false;
            }
        }
    }
}
