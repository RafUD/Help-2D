using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float jumping = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool isDead = false;




    private void Update()
    {
        if (isDead) return;

        // Flip direction if left key is pressed
        if (!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Flip();
        }
        else if (isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Flip();
        }


        // Jumping logic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumping);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // Always move in the direction the player is facing
        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    [SerializeField] private float deathBounceForce = 24f;
    [SerializeField] private float deathSpinTorque = 300f;

    public void PlayerDeath()
    {
        Debug.Log("PlayerDeath called");
        isDead = true;

        // Disable player input and collision
        GetComponent<Collider2D>().enabled = false;

        // Remove all constraints to allow spinning
        rb.constraints = RigidbodyConstraints2D.None;

        // Reset velocity and bounce up
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * deathBounceForce, ForceMode2D.Impulse);

        // Add spin torque
        rb.AddTorque(deathSpinTorque, ForceMode2D.Impulse);
    }

    public void Respawn()
    {
        // Reset facing direction to right
        if (!isFacingRight)
        {
            Flip(); 
        }

        Debug.Log("Player Respawned");
        isDead = false;

        // Re-enable player collider
        GetComponent<Collider2D>().enabled = true;

        // Freeze rotation again so the player doesn't spin during normal play
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Stop all motion
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Reset rotation to upright
        transform.rotation = Quaternion.identity;
    }
}
