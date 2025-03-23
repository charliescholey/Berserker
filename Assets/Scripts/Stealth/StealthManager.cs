using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StealthManager : MonoBehaviour
{
    public static StealthManager Instance;
    
    [SerializeField]
    private GameObject player;

    private ExpManager expManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            expManager = FindAnyObjectByType<ExpManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAndActivateScene(string sceneName)
    {
        StartCoroutine(LoadAndActivateSceneCoroutine(sceneName));
    }

    private IEnumerator LoadAndActivateSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;  // Wait for the next frame
        }
        yield return null;
        
        // Use the stored reference to disable the player.
        if (player != null)
        {
            player.SetActive(false);
        }
        else
        {
            Debug.LogError("Player reference not set in StealthManager!");
        }

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);
        Debug.Log("Scene activated: " + newScene.name);
    }

    public void UnloadStrategyAndEnablePlayer()
    {
        StartCoroutine(UnloadStrategyAndEnablePlayerCoroutine());
    }

    private IEnumerator UnloadStrategyAndEnablePlayerCoroutine()
    {
        Debug.Log("Starting unload process for 'Strategy'");

        if (SceneManager.GetActiveScene().name == "Strategy")
        {
            Scene initialScene = SceneManager.GetSceneByName("DemoLevel");
            if (initialScene.IsValid() && initialScene.isLoaded)
            {
                SceneManager.SetActiveScene(initialScene);
                Debug.Log("Switched active scene to: " + initialScene.name);
            }
            else
            {
                Debug.LogError("Initial scene not found or not loaded!");
                yield break;
            }
        }

        Debug.Log("Active scene is now: " + SceneManager.GetActiveScene().name);

        Scene strategyScene = SceneManager.GetSceneByName("Strategy");
        if (!strategyScene.isLoaded)
        {
            Debug.LogError("Strategy scene is not loaded!");
            yield break;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("Strategy");
        if (unloadOperation == null)
        {
            Debug.LogError("UnloadSceneAsync returned null.");
            yield break;
        }

        Debug.Log("Unload operation started.");

        // Wait for the unload to complete.
        yield return new WaitUntil(() => unloadOperation.isDone);

        Debug.Log("Unload operation complete.");

        // Re-enable the player using the stored reference.
        if (player != null)
        {
            player.SetActive(true);
            Debug.Log("Stealth reloaded: Player re-enabled.");
        }
        else
        {
            Debug.LogError("Player reference not set in StealthManager!");
        }
        expManager.AddExp(25);
    }

    public void OnDeath()
    {
        Debug.Log("died");
        SceneManager.LoadScene("DemoLevel", LoadSceneMode.Single);
    }
}
