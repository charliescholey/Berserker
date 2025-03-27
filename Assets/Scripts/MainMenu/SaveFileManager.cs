using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    public int gold;
    public int exp;
    public int stageReached;
}

public class SaveFileManager : MonoBehaviour {
    // Static player data accessible from any scene.
    public static PlayerData CurrentPlayerData;

    // Path to the JSON save file.
    public static string saveFilePath;

    // Current level being played
    public static string currentLevelName;

    void Awake() {
        // Persist this GameObject across scene loads.
        DontDestroyOnLoad(gameObject);
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    public void ResumeGame() {
        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            CurrentPlayerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Game resumed. Player data loaded:\n" + json);
            SceneManager.LoadScene("WorldMap");
        }
        else {
            Debug.Log("No save file found. Cannot resume game.");
        }
    }

    public void StartNewGame() {
        CurrentPlayerData = new PlayerData {
            gold = 0,
            exp = 0,
            stageReached = 1
        };

        string json = JsonUtility.ToJson(CurrentPlayerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("New game started. Player data saved:\n" + json);
        SceneManager.LoadScene("WorldMap");
    }
}
