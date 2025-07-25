using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerDash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Dash Setting")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public int killsRequiredForDash = 20;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 dashDirection;

    private int killCountSinceLastDash = 0;
    public TMP_Text dashReadyText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashReadyText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canDash && !isDashing)
        {
            dashDirection=new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (dashDirection != Vector2.zero)
            {
                dashReadyText.gameObject.SetActive(false);
                StartCoroutine(Dash());
            }
        }
    }
    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        killCountSinceLastDash = 0;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;

        Debug.Log("Dash used. Wait until 20 more kills to use again.");
    }
   
    public void OnEnemyKilled()
    {
        if (!canDash)
        {
            killCountSinceLastDash++;

            if (killCountSinceLastDash >= killsRequiredForDash)
            {
                canDash = true;
                Debug.Log("Dash is ready again after 20 kills!");
                dashReadyText.gameObject.SetActive(true);
            }
        }
    }
}
