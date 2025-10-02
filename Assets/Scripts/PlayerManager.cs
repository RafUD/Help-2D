using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private const string tag = "FallCollider";
    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(tag))
        {
            Die();
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
        yield return new WaitForSeconds(1.5f);

        // Reset position
        transform.position = spawnPosition;

        // Re-enable the movement script
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.Respawn();
        }
    }


}
