using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            Debug.Log("Player has been spotted by enemy character!");
            
            // Traverse up the parent hierarchy to destroy the enemy root object
            Transform current = transform;
            while (current != null)
            {
                if (current.CompareTag("EnemyRoot"))
                {
                    Destroy(current.gameObject);
                    break;
                }
                current = current.parent;
            }
            
            StealthManager.Instance.LoadAndActivateScene("Strategy");
        }
    }
}