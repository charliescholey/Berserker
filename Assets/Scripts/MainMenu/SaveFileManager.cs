using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PassiveSkill {
    public string skillName;
    public int attAdd;
    public float attMult;
    public int hpAdd;
    public float hpMult;
    public string description;
}

[System.Serializable]
public class PlayerData {
    public int gold;
    public int exp;
    public int stageReached;

    public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    public List<int> UnlockedSkillIndices = new List<int>();
}

public class SaveFileManager : MonoBehaviour {
    public static PlayerData CurrentPlayerData;
    public static string saveFilePath;
    public static string currentLevelName;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        Debug.Log(saveFilePath.ToString());

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
            stageReached = 1,
            passiveSkills = new List<PassiveSkill>(), 
            UnlockedSkillIndices = new List<int>()
        };

        string json = JsonUtility.ToJson(CurrentPlayerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("New game started. Player data saved:\n" + json);
        SceneManager.LoadScene("WorldMap");
    }
}
