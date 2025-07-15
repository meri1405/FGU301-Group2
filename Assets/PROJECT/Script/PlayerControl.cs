using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Transform weaponTransform; // Tham chiếu tới vũ khí

    // Boost effects
    private float currentSpeedMultiplier = 1f;
    private float currentDamageMultiplier = 1f;
    private Coroutine speedBoostCoroutine;
    private Coroutine powerBoostCoroutine;
    [SerializeField] private ShieldController shieldController;
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f;

    private Health health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementVector;
    private bool isFacingRight = true; // Biến để theo dõi hướng nhân vật
    private bool wasMoving = false; // Để theo dõi trạng thái di chuyển

    // Start is called before the first frame update
    void Start()
    {
        shieldController = GetComponentInChildren<ShieldController>(true);
        if (shieldController == null)
        {
            Debug.LogError("❌ ShieldController vẫn NULL! Kiểm tra cấu trúc hierarchy hoặc script.");
        }
        else
        {
            Debug.Log("✅ ShieldController FOUND: " + shieldController.name);
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

        if (animator == null)
            UnityEngine.Debug.LogError("Animator component not found on player");
            
        if (health == null)
        {
            UnityEngine.Debug.LogError("Health component not found on player");
            health = gameObject.AddComponent<Health>(); // Tự động thêm Health nếu chưa có
        }
            
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
        if (Keyboard.current.qKey.wasPressedThisFrame && shieldController != null)
        {
            shieldController.ActivateShield();
        }
        // Nếu đã chết, không cho di chuyển
        if (health != null && health.IsDead())
        {
            // Dừng walk sound nếu player chết
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopWalkSound();
            }
            return;
        }

        float horizontal = 0.0f;
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            horizontal = -0.5f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
                
            if (isFacingRight)
                FlipWeapon();
                
            isFacingRight = false;
        }
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            horizontal = 0.5f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
                
            if (!isFacingRight)
                FlipWeapon();
                
            isFacingRight = true;
        }

        float vertical = 0.0f;
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            vertical = 0.5f;
        }
        else if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            vertical = -0.5f;
        }

        // Tính toán vector di chuyển
        movementVector = new Vector2(horizontal, vertical);
        
        // Tính toán tốc độ thực tế (magnitude của vector di chuyển)
        float currentSpeed = movementVector.magnitude;
        bool isMoving = currentSpeed > 0;

        // Cập nhật tham số speed trong Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
        
        // Xử lý âm thanh bước chân
        HandleWalkingSound(isMoving);

        // Di chuyển nhân vật với speed boost
        float effectiveMoveSpeed = moveSpeed * currentSpeedMultiplier;
        Vector2 position = transform.position;
        position.x = position.x + effectiveMoveSpeed * Time.deltaTime * horizontal;
        position.y = position.y + effectiveMoveSpeed * Time.deltaTime * vertical;
        transform.position = position;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log(">> Q Key pressed");
            if (shieldController != null)
            {
                Debug.Log(">> ShieldController found");
                shieldController.ActivateShield();
            }
            else
            {
                Debug.LogWarning(">> ShieldController is null!");
            }
        }
    }
    
    private void HandleWalkingSound(bool isMoving)
    {
        if (AudioManager.Instance != null)
        {
            if (isMoving && !wasMoving)
            {
                // Bắt đầu di chuyển - phát walk sound liên tục
                AudioManager.Instance.StartWalkSound();
            }
            else if (!isMoving && wasMoving)
            {
                // Dừng di chuyển - dừng walk sound ngay lập tức
                AudioManager.Instance.StopWalkSound();
            }
        }
        
        wasMoving = isMoving;
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
    
    // Method để destroy weapon khi player chết
    public void DestroyWeapon()
    {
        if (weaponTransform != null)
        {
            UnityEngine.Debug.Log("Destroying player weapon: " + weaponTransform.name);
            Destroy(weaponTransform.gameObject);
            weaponTransform = null;
        }
        else
        {
            UnityEngine.Debug.Log("No weapon to destroy - weaponTransform is null");
        }
    }
    
    // Method để phát âm thanh khi bị thương (có thể gọi từ Health script)
    public void PlayHurtSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHurtSound();
        }
    }

    // Phương thức để xử lý khi player bị va chạm
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        if (Time.time - lastDamageTime < damageCooldown) return;

        lastDamageTime = Time.time;

        ZombieChase enemy = collision.gameObject.GetComponent<ZombieChase>();
        if (enemy != null)
        {
            float damage = enemy.GetDamage();

            if (shieldController != null && shieldController.IsActive())
            {
                Debug.Log("🛡️ Shield absorbs damage: " + damage);
                shieldController.AbsorbDamage(damage);
                return; // Không trừ máu player
            }

            // Nếu không có khiên hoặc khiên đã nổ
            PlayHurtSound();

            // Đẩy lùi player
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            transform.position += (Vector3)knockbackDir * 1f;

            Debug.Log("💥 Player bị trúng sát thương: " + damage);
        }
    }

    // Đảm bảo dừng walk sound khi player bị disable/destroy
    void OnDisable()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopWalkSound();
        }
    }

    // Methods for boost effects
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        // Dừng coroutine hiện tại nếu có
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        // Bắt đầu speed boost mới
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    public void ApplyPowerBoost(float multiplier, float duration)
    {
        // Dừng coroutine hiện tại nếu có
        if (powerBoostCoroutine != null)
        {
            StopCoroutine(powerBoostCoroutine);
        }

        // Bắt đầu power boost mới
        powerBoostCoroutine = StartCoroutine(PowerBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        currentSpeedMultiplier = multiplier;
        UnityEngine.Debug.Log($"Speed boost activated: {multiplier}x for {duration} seconds");
        
        // Có thể thêm hiệu ứng visual ở đây (đổi màu player, particle effect, etc.)
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.cyan; // Màu xanh để chỉ speed boost
            
            yield return new WaitForSeconds(duration);
            
            spriteRenderer.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        currentSpeedMultiplier = 1f;
        speedBoostCoroutine = null;
        UnityEngine.Debug.Log("Speed boost ended");
    }

    private IEnumerator PowerBoostCoroutine(float multiplier, float duration)
    {
        currentDamageMultiplier = multiplier;
        UnityEngine.Debug.Log($"Power boost activated: {multiplier}x for {duration} seconds");
        
        // Có thể thêm hiệu ứng visual ở đây
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red; // Màu đỏ để chỉ power boost
            
            yield return new WaitForSeconds(duration);
            
            spriteRenderer.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        currentDamageMultiplier = 1f;
        powerBoostCoroutine = null;
        UnityEngine.Debug.Log("Power boost ended");
    }

    // Getter cho damage multiplier để weapon có thể sử dụng
    public float GetCurrentDamageMultiplier()
    {
        return currentDamageMultiplier;
    }

    // Getter cho speed multiplier
    public float GetCurrentSpeedMultiplier()
    {
        return currentSpeedMultiplier;
    }
}