using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 spawnPosition;

    public int maxLives = 3;
    private int currentLives;

    public HeartsUI heartsUI;

    public PlayerProjectile projectile;

    public GameObject GameOverCanvas;

    [Header("Damage Feedback")]
    public float knockbackX = 5f;        // Horizontal push amount
    public float miniJumpY = 4f;         // How high the "hit jump" goes
    public float invincibilityTime = 1f; // Duration of invulnerability
    public float blinkSpeed = 0.1f;      // Sprite blinking speed

    private bool isInvincible = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;



    //Coin
    private int coinCounter = 0;
    public TMP_Text counterText;


    //Potion
    public bool isPoweredUp = false;
    public float powerUpDuration = 5f;

    private void Start()
    {
        spawnPosition = transform.position;
        currentLives = maxLives;
        heartsUI.SetMaxHearts(maxLives);
       

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

        //Coin
        if (collision.collider.CompareTag("Coin") && collision.gameObject.activeSelf)
        {
            collision.gameObject.SetActive(false);
            coinCounter += 1;
            counterText.text = coinCounter.ToString();
        }


        else if (collision.collider.CompareTag("FallCollider"))
        {
            Debug.Log("Player fell!");
            TakeDamage(1);
            Die();
        }

        //Hearts
        else if (collision.collider.CompareTag("Heart") && collision.gameObject.activeSelf)
        {
            collision.gameObject.SetActive(false);

            // Restore hearts to full
            currentLives = maxLives;
            heartsUI.UpdateHearts(currentLives);

            Debug.Log("Full health restored!");
        }


        //Potion
        else if (collision.collider.CompareTag("Potion") && collision.gameObject.activeSelf)
        {
            collision.gameObject.SetActive(false);

            // Power up projectile
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(PotionPowerUp());

            Debug.Log("GIANT FIREBALLS");
        }
    }


    private IEnumerator PotionPowerUp()
    {
        isPoweredUp = true;
        Debug.Log("Power-up activated!");
        yield return new WaitForSeconds(powerUpDuration);
        isPoweredUp = false;
        Debug.Log("Power-up ended!");
    }


    private void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("Lives left: " + currentLives);
        //UI
        heartsUI.UpdateHearts(currentLives);


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