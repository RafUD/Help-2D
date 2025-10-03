using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private const string tag = "FallCollider";
    private Vector3 spawnPosition;

    public int lives = 3;
    public GameObject GameOverCanvas;


    private void Start()
    {
        spawnPosition = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(tag))
        {
            lives--;
            Die();
            if(lives == 0)
            {
                RestartUI();
            }
        }
    }

    private void Die()
    {
        // Get the Movement script and call PlayerDeath
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.PlayerDeath();
        }

        

        // Optionally, you can delay the reset
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        // Wait a moment after death
        yield return new WaitForSeconds(2.2f);

        // Reset position
        transform.position = spawnPosition;

        // Re-enable the movement script
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

}
