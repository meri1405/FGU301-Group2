using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Transform weaponTransform; // Tham chiếu tới vũ khí

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementVector;
    private bool isFacingRight = true; // Biến để theo dõi hướng nhân vật

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            Debug.LogError("Animator component not found on player");
            
        // Nếu chưa gán vũ khí trong Inspector, tìm kiếm nó theo tên
        if (weaponTransform == null)
        {
            Transform weaponChild = transform.Find("Weapon 3");
            if (weaponChild != null)
                weaponTransform = weaponChild;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0.0f;
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -0.5f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
                
            if (isFacingRight)
                FlipWeapon();
                
            isFacingRight = false;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 0.5f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
                
            if (!isFacingRight)
                FlipWeapon();
                
            isFacingRight = true;
        }

        float vertical = 0.0f;
        if (Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 0.5f;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -0.5f;
        }

        // Tính toán vector di chuyển
        movementVector = new Vector2(horizontal, vertical);
        
        // Tính toán tốc độ thực tế (magnitude của vector di chuyển)
        float currentSpeed = movementVector.magnitude;

        // Cập nhật tham số speed trong Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }

        // Di chuyển nhân vật
        Vector2 position = transform.position;
        position.x = position.x + moveSpeed * Time.deltaTime * horizontal;
        position.y = position.y + moveSpeed * Time.deltaTime * vertical;
        transform.position = position;
    }
    
    // Hàm để lật vũ khí
    private void FlipWeapon()
    {
        if (weaponTransform != null)
        {
            // Đảo ngược hướng vũ khí bằng cách đảo ngược scale.x
            Vector3 localScale = weaponTransform.localScale;
            localScale.x *= -1;
            weaponTransform.localScale = localScale;
            
            // Điều chỉnh vị trí của vũ khí để giữ nó ở đúng vị trí tương đối 
            Vector3 localPos = weaponTransform.localPosition;
            localPos.x *= -1;
            weaponTransform.localPosition = localPos;
        }
    }
}