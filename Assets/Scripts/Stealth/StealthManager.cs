using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StealthManager : MonoBehaviour
{
    public static StealthManager Instance;
    
    [SerializeField]
    private GameObject player;

    private ExpManager expManager;

    private string returnLevelName;
    private string stratLevelName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            expManager = FindAnyObjectByType<ExpManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAndActivateScene(string sceneName, string returnLevelName)
    {
        this.returnLevelName = returnLevelName;
        stratLevelName = sceneName;
        StartCoroutine(LoadAndActivateSceneCoroutine(sceneName));
    }

    // hides stealth objects during strategy, then brings them back when we go back into stealth
    private void SetSceneRootsActive(string sceneName, bool active)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid()) return;

        foreach (var root in scene.GetRootGameObjects())
        {
            // keep the StealthManager itself alive
            if (root.GetComponent<StealthManager>() != null) 
                continue;

            root.SetActive(active);
        }
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

        SetSceneRootsActive(returnLevelName, false);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);
        Debug.Log("Scene activated: " + newScene.name);
    }

    public void UnloadStrategyAndEnablePlayer()
    {
        StartCoroutine(UnloadStrategyAndEnablePlayerCoroutine());
    }

    public void ReturnToWorldMap()
    {
        if(SceneManager.GetActiveScene().name != stratLevelName){
            StartCoroutine(ReturnToWorldMapCoroutine());
        }
        
    }

    private IEnumerator UnloadStrategyAndEnablePlayerCoroutine()
    {
        SetSceneRootsActive(returnLevelName, true);

        Debug.Log("Starting unload process for 'Strategy'");

        if (SceneManager.GetActiveScene().name == stratLevelName)
        {
            Scene initialScene = SceneManager.GetSceneByName(returnLevelName);
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
            SceneManager.UnloadSceneAsync(stratLevelName);
        }

        Debug.Log("Active scene is now: " + SceneManager.GetActiveScene().name);

        Scene strategyScene = SceneManager.GetSceneByName(stratLevelName);
        if (!strategyScene.isLoaded)
        {
            Debug.LogError("Strategy scene is not loaded!");
            yield break;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(stratLevelName);
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
        expManager.AddExp(100);

        
    }

    private IEnumerator ReturnToWorldMapCoroutine()
    {
        expManager.AddExp(150);
        SceneManager.LoadScene("Levels/Scenes/WorldMap", LoadSceneMode.Single);
        yield return null; // Ensure the coroutine yields at least once
    }

    public void OnDeath()
    {
        Debug.Log("died");
        SceneManager.LoadScene("DemoLevel", LoadSceneMode.Single);
    }
}
