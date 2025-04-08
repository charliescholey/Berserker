using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SkillDatabaseData
{
    public List<SkillData> skills = new List<SkillData>();
}

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Skills/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    private const string SAVE_FILENAME = "skillData.json";
    private const string TEST_DATA_PATH = "TestData/skills";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILENAME);

    private static SkillDatabaseData s_SkillDatabaseData;
    private static bool s_IsDatabaseLoaded = false;

    public List<Skill> skills = new List<Skill>();

    void Awake()
    {
        Debug.Log("SkillDatabase Awake called");
        if (skills == null)
        {
            skills = new List<Skill>();
        }
        LoadTestData();
    }

    public Skill GetSkillByIdentifier(string identifier)
    {
        return skills.Find(s => s.identifier == identifier);
    }

    public void SaveSkillData()
    {
        SkillDatabaseData data = new SkillDatabaseData
        {
            skills = skills.ConvertAll(s => s.ToData())
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Skill database saved to {SaveFilePath}");
    }

    public void LoadSkillData()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning($"Skill database file not found at {SaveFilePath}");
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        SkillDatabaseData data = JsonUtility.FromJson<SkillDatabaseData>(json);

        // Clear existing skills
        skills.Clear();

        // Create new skills from data
        foreach (SkillData skillData in data.skills)
        {
            Skill skill = Skill.CreateFromData(skillData);
            skills.Add(skill);
        }

        Debug.Log($"Skill database loaded with {skills.Count} skills");
    }

    private void LoadTestData()
    {
        Debug.Log("Loading test skills data...");
        TextAsset jsonFile = Resources.Load<TextAsset>(TEST_DATA_PATH);
        if (jsonFile == null)
        {
            Debug.LogError($"Failed to load skills JSON file at path: {TEST_DATA_PATH}");
            return;
        }

        Debug.Log($"Successfully loaded JSON file. Content length: {jsonFile.text.Length}");
        SkillDatabaseData data = JsonUtility.FromJson<SkillDatabaseData>(jsonFile.text);
        if (data == null)
        {
            Debug.LogError("Failed to parse skills JSON data");
            return;
        }

        Debug.Log($"Parsed {data.skills.Count} skills from JSON");
        skills.Clear();
        foreach (SkillData skillData in data.skills)
        {
            Skill skill = Skill.CreateFromData(skillData);
            if (skill != null)
            {
                Debug.Log($"Created skill: {skill.skillName} (ID: {skill.identifier})");
                skills.Add(skill);
            }
            else
            {
                Debug.LogError($"Failed to create skill from data: {skillData.skillName}");
            }
        }
    }

#if UNITY_EDITOR
    public void SaveSkillToScriptableObject(Skill skill)
    {
        string path = $"Assets/Resources/Skills/{skill.identifier}.asset";
        UnityEditor.AssetDatabase.CreateAsset(skill, path);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}