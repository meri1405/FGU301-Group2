using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Combat Settings")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damageCooldown = 1f;

    [Header("References")]
    [SerializeField] private Transform weaponTransform;

    

    private float currentSpeedMultiplier = 1f;
    private float currentDamageMultiplier = 1f;

    private Coroutine speedBoostCoroutine;
    private Coroutine powerBoostCoroutine;

    private float lastDamageTime = 0f;
    public GameObject explodingShieldPrefab;
    private Health health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public ExplosionShield explosionShield;

    private Vector2 movementVector;
    private bool isFacingRight = true;
    private bool wasMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

        if (animator == null)
            Debug.LogError("Animator not found on player");

        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found on player");

        if (health == null)
        {
            Debug.LogWarning("Health component missing. Adding default Health.");
            health = gameObject.AddComponent<Health>();
        }

        if (weaponTransform == null)
        {
            Transform weaponChild = transform.Find("Weapon 3");
            if (weaponChild != null)
                weaponTransform = weaponChild;
        }
    }

    private void Update()
    {
        if (health != null && health.IsDead())
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopWalkSound();
            return;
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ActivateExplodingShield();
        }


        HandleInput();
        HandleMovement();
    }
    private void ActivateExplodingShield()
    {
        if (explodingShieldPrefab != null)
        {
            Vector3 shieldPos = transform.position;
            GameObject shield = Instantiate(explodingShieldPrefab, shieldPos, Quaternion.identity);
            shield.transform.SetParent(transform); 
            Debug.Log("🛡️ Shield activated");
        }
    }

    private void HandleInput()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            vertical = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            vertical = -1f;

        movementVector = new Vector2(horizontal, vertical).normalized;

        if (horizontal < 0 && isFacingRight)
        {
            spriteRenderer.flipX = true;
            FlipWeapon();
            isFacingRight = false;
        }
        else if (horizontal > 0 && !isFacingRight)
        {
            spriteRenderer.flipX = false;
            FlipWeapon();
            isFacingRight = true;
        }
       
    }
    
    private void HandleMovement()
    {
        float currentSpeed = movementVector.magnitude;
        bool isMoving = currentSpeed > 0;

        if (animator != null)
            animator.SetFloat("Speed", currentSpeed);

        HandleWalkingSound(isMoving);

        float effectiveSpeed = moveSpeed * currentSpeedMultiplier;
        transform.position += (Vector3)(movementVector * effectiveSpeed * Time.deltaTime);
    }

    private void HandleWalkingSound(bool isMoving)
    {
        if (AudioManager.Instance != null)
        {
            if (isMoving && !wasMoving)
                AudioManager.Instance.StartWalkSound();
            else if (!isMoving && wasMoving)
                AudioManager.Instance.StopWalkSound();
        }

        wasMoving = isMoving;
    }

    private void FlipWeapon()
    {
        if (weaponTransform != null)
        {
            Vector3 scale = weaponTransform.localScale;
            scale.x *= -1;
            weaponTransform.localScale = scale;

            Vector3 pos = weaponTransform.localPosition;
            pos.x *= -1;
            weaponTransform.localPosition = pos;
        }
    }

    public void DestroyWeapon()
    {
        if (weaponTransform != null)
        {
            Debug.Log("Destroying player weapon: " + weaponTransform.name);
            Destroy(weaponTransform.gameObject);
            weaponTransform = null;
        }
        else
        {
            Debug.Log("No weapon to destroy.");
        }
    }

    public void PlayHurtSound()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayHurtSound();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        lastDamageTime = Time.time;

        ZombieChase enemy = collision.gameObject.GetComponent<ZombieChase>();
        if (enemy != null)
        {
            float finalDamage = baseDamage;

            PlayHurtSound();
            health.TakeDamage(finalDamage);

            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            transform.position += (Vector3)knockbackDir * 1f;

            Debug.Log($"💥 Player took damage: {finalDamage}");
        }
    }

    private void OnDisable()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopWalkSound();
    }

   

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    public void ApplyPowerBoost(float multiplier, float duration)
    {
        if (powerBoostCoroutine != null)
            StopCoroutine(powerBoostCoroutine);

        powerBoostCoroutine = StartCoroutine(PowerBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        currentSpeedMultiplier = multiplier;
        Debug.Log($"⚡ Speed boost: {multiplier}x for {duration}s");

        yield return new WaitForSeconds(duration);

        currentSpeedMultiplier = 1f;
        speedBoostCoroutine = null;
        Debug.Log("⚡ Speed boost ended");
    }

    private IEnumerator PowerBoostCoroutine(float multiplier, float duration)
    {
        currentDamageMultiplier = multiplier;
        Debug.Log($"🔥 Power boost: {multiplier}x for {duration}s");

        yield return new WaitForSeconds(duration);

        currentDamageMultiplier = 1f;
        powerBoostCoroutine = null;
        Debug.Log("🔥 Power boost ended");
    }

    public float GetCurrentDamageMultiplier() => currentDamageMultiplier;
    public float GetCurrentSpeedMultiplier() => currentSpeedMultiplier;
}
