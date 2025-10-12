using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumping = 16f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    private bool isFacingRight = true;
    private bool isDead = false;
    private bool isCrouching = false;
    private bool isStunned = false;

    [Header("Death Physics")]
    [SerializeField] private float deathBounceForce = 24f;
    [SerializeField] private float deathSpinTorque = 300f;

    private Shooting shooting;  // reference to disable/enable shooting

    private void Start()
    {
        shooting = GetComponent<Shooting>();
    }

    private void Update()
    {
        if (isDead) return;

        HandleFlip();
        HandleJump();
        HandleStop();
    }

    private void FixedUpdate()
    {
        if (isDead || isCrouching || isStunned) return;

        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumping);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void HandleStop()
    {
        bool crouchInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (crouchInput && !isCrouching)
        {
            isCrouching = true;
            rb.linearVelocity = Vector2.zero;

            // Disable shooting when crouching
            if (shooting != null)
                shooting.enabled = false;

            animator?.SetBool("IsStopping", true);
        }
        else if (!crouchInput && isCrouching)
        {
            isCrouching = false;

            // Re-enable shooting after crouch ends
            if (shooting != null)
                shooting.enabled = true;

            animator?.SetBool("IsStopping", false);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleFlip()
    {
        if ((!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow)) ||
            (isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void PlayerDeath()
    {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;

        // Disable shooting on death
        if (shooting != null)
            shooting.enabled = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * deathBounceForce, ForceMode2D.Impulse);
        rb.AddTorque(deathSpinTorque, ForceMode2D.Impulse);
    }

    public void Respawn()
    {
        if (!isFacingRight) Flip();

        isDead = false;
        GetComponent<Collider2D>().enabled = true;

        // Re-enable shooting after respawn
        if (shooting != null)
            shooting.enabled = true;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
    }
}
