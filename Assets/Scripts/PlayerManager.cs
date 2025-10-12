using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 spawnPosition;

    public int maxLives = 3;
    private int currentLives;

    public GameObject GameOverCanvas;

    [Header("Damage Feedback")]
    public float knockbackX = 5f;        // Horizontal push amount
    public float miniJumpY = 4f;         // How high the "hit jump" goes
    public float invincibilityTime = 1f; // Duration of invulnerability
    public float blinkSpeed = 0.1f;      // Sprite blinking speed

    private bool isInvincible = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        spawnPosition = transform.position;
        currentLives = maxLives;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy && !isInvincible)
        {
            Debug.Log("OUCH!");
            TakeDamage(enemy.damage);

            // --- Knockback mini jump logic ---
            Vector2 knockDir = (transform.position - enemy.transform.position).normalized;
            float direction = Mathf.Sign(knockDir.x);

            // Reset and apply a short bounce in the opposite direction
            rb.linearVelocity = new Vector2(direction * knockbackX, miniJumpY);

            // Disable movement temporarily so bounce isn’t cancelled
            Movement movement = GetComponent<Movement>();
            if (movement != null)
                movement.Stun(0.4f); // about half a second of knockback lock


            StartCoroutine(InvincibilityFlash());
        }

        if (collision.collider.CompareTag("FallCollider"))
        {
            Debug.Log("Player fell!");
            TakeDamage(1);
            Die();
        }
    }

    private void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("Lives left: " + currentLives);

        // Play hit animation
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // Knockback + temporary invincibility handled elsewhere
        if (currentLives <= 0)
        {
            Die();
            RestartUI();
        }
    }


    private void Die()
    {
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.PlayerDeath();
        }

        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2.2f);
        transform.position = spawnPosition;

        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.Respawn();
        }
    }

    private void RestartUI()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- Invincibility flashing effect ---
    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityTime)
        {
            sr.enabled = !sr.enabled; // blink sprite
            yield return new WaitForSeconds(blinkSpeed);
            elapsed += blinkSpeed;
        }

        sr.enabled = true;
        isInvincible = false;
    }
}