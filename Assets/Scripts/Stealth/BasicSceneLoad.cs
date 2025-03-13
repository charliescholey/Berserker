using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string levelName = "DemoLevel";

    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}