using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDetection : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) {
            Debug.Log("Player has been spotted by enemy character!");
            SceneManager.LoadScene("Strategy", LoadSceneMode.Single);
        }
    }
}