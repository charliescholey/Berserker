using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader
{
    public static void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level: " + levelName); // Check if this appears in the Console
        SceneManager.LoadScene(levelName);
    }
}