using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDetection : MonoBehaviour
{

    public bool isPlayerDetected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string levelName = SceneManager.GetActiveScene().name;
        string currLevel = SceneManager.GetActiveScene().name;
        if (collision.CompareTag("player"))
        {
            Debug.Log("Player has been spotted by enemy character!");
            
            // Traverse up the parent hierarchy to destroy the enemy root object
            Transform current = transform;
            while (current != null)
            {
                if (current.CompareTag("EnemyRoot"))
                {
                    levelName = current.GetComponent<LevelJump>().levelName;
                    Destroy(current.gameObject);
                    break;
                }
                current = current.parent;
            }
            
            StealthManager.Instance.LoadAndActivateScene(levelName, currLevel);
        }

        Debug.Log("Hit Cone!");
    }
}