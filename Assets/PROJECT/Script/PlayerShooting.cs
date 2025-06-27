using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject bulletPrefab; // Prefab của viên đạn
    public Transform firePoint; // Vị trí bắn viên đạn
        
    public float fireRate = 0.5f; // Tốc độ bắn (số lần bắn mỗi giây)
    private float nextFireTime = 0f; // Thời gian tiếp theo có thể bắn
    void Start()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Cập nhật thời gian tiếp theo có thể bắn
        }
    }

    // Update is called once per frame
    void Shoot()
    {
        Debug.Log("Bắn!");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
