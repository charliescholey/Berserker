using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapLoader
{
    public static void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level: " + levelName); // Check if this appears in the Console
        SceneManager.LoadScene(levelName);
    }
}