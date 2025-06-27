using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject bullet;
    public Transform firePoint; // Vị trí bắn viên đạn
    public float fireRate = 0.5f; // Tốc độ bắn (số lần bắn mỗi giây)
    public float bulletForce;

    private float timeBtwFire;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        timeBtwFire -= Time.deltaTime;
        if (Input.GetButton("Fire1") && timeBtwFire < 0)
        {
            FireBullet();
            
        }
    }
    void RotateGun()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir= mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0,0,angle);
        transform.rotation = rotation;
        if(transform.eulerAngles.z >90 && transform.eulerAngles.z<270)
        {
            transform.localScale = new Vector3(1,-1,0);
        }
        else
        {
            transform.localScale  = new Vector3(1, 1, 0);
        }
    }
    void FireBullet()
    {
        timeBtwFire = fireRate;
        GameObject bulletInstance = Instantiate(bullet, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletForce,ForceMode2D.Impulse);
    }
}
