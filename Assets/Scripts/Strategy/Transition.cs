using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Transition : MonoBehaviour
{
    // Static instance of the Transition class
    public static Transition Instance { get; private set; }
    
    // Called when the script instance is being loaded
    private void Awake()
    {
        // Ensure only one instance of Transition exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep this instance across scenes
    }

    public void onKill(){
        Debug.Log("enemy killed");
        StealthManager.Instance.UnloadStrategyAndEnablePlayer();
    }
    public void onDeath()
    {
        Debug.Log("died");
        Destroy(StealthManager.Instance);
        SceneManager.LoadScene(SaveFileManager.currentLevelName, LoadSceneMode.Single);
    }
}
