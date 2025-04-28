using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    [SerializeField] private int gold;
    [SerializeField] private int exp;
    [SerializeField] private int stageReached;

    public PlayerData(int gold, int exp, int stageReached, List<int> UnlockedSkillIndices) {
        this.gold = gold;
        this.exp = exp;
        this.stageReached = stageReached;
        this.UnlockedSkillIndices = UnlockedSkillIndices;
    }

    public List<int> UnlockedSkillIndices = new List<int>();

    public void AddGold(int n)
    {
        gold += n;
        SaveFileManager.SaveData(); 
    }

    public void RemoveGold(int n)
    {
        gold -= n;
        SaveFileManager.SaveData(); 
    }

    public void AddXP(int n)
    {
        exp += n;
        SaveFileManager.SaveData(); 
    }

    public void RemoveXP(int n)
    {
        exp -= n;
        SaveFileManager.SaveData(); 
    }

    public int GetGold() {
        return gold;
    }

    public int GetXP() {
        return exp;
    }

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
        CurrentPlayerData = new PlayerData(100, 0, 1, new List<int> {0, 2});
        SaveData();
        SceneManager.LoadScene("WorldMap");
    }

    public static void SaveData() {
        string json = JsonUtility.ToJson(CurrentPlayerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Player data saved:\n" + json);
    }

}
