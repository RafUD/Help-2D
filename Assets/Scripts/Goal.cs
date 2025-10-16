using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // set in Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Level complete!");
            StartCoroutine(LevelCompleteSequence(collision.gameObject));
        }
    }

    private IEnumerator LevelCompleteSequence(GameObject player)
    {
        // Optional: disable movement/shooting
        Movement move = player.GetComponent<Movement>();
        Shooting shoot = player.GetComponent<Shooting>();

        if (move != null) move.enabled = false;
        if (shoot != null) shoot.enabled = false;

        // Optional animation or sound
        yield return new WaitForSeconds(1.5f); // small pause

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
