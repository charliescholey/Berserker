using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Actions/ActionDatabase")]
public class ActionDatabase : ScriptableObject
{
    private const string SAVE_FILENAME = "actionData.json";
    private const string TEST_DATA_PATH = "TestData/actions";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILENAME);

    private static ActionDatabaseData s_ActionDatabaseData;
    private static bool s_IsDatabaseLoaded = false;

    public List<Action> actions = new List<Action>();

    void Awake()
    {
        // TODO: remove this once we have a database
        // LoadActionData();
        LoadTestData();
    }

    public Action GetActionByIdentifier(string identifier)
    {
        return actions.Find(a => a.identifier == identifier);
    }

    public void SaveActionData()
    {
        ActionDatabaseData data = new ActionDatabaseData
        {
            actions = actions.ConvertAll(a => a.ToData())
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Action database saved to {SaveFilePath}");
    }

    public void LoadActionData()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning($"Action database file not found at {SaveFilePath}");
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        ActionDatabaseData data = JsonUtility.FromJson<ActionDatabaseData>(json);

        // Clear existing actions
        actions.Clear();

        // Create new actions from data
        foreach (ActionData actionData in data.actions)
        {
            Action action = Action.CreateFromData(actionData);
            actions.Add(action);
        }

        Debug.Log($"Action database loaded with {actions.Count} actions");
    }

    private void LoadTestData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(TEST_DATA_PATH);
        if (jsonFile == null)
        {
            Debug.LogError($"Failed to load test action data from {TEST_DATA_PATH}");
            return;
        }

        ActionDatabaseData data = JsonUtility.FromJson<ActionDatabaseData>(jsonFile.text);

        // Clear existing actions
        actions.Clear();

        // Create new actions from data
        foreach (ActionData actionData in data.actions)
        {
            Action action = Action.CreateFromData(actionData);
            actions.Add(action);
        }

        Debug.Log($"Test action database loaded with {actions.Count} actions");
    }

#if UNITY_EDITOR
    public void SaveActionToScriptableObject(Action action)
    {
        string path = $"Assets/Resources/Actions/{action.identifier}.asset";
        UnityEditor.AssetDatabase.CreateAsset(action, path);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
