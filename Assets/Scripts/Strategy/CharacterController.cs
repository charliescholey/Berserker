using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseMovementRange;
    public int maxHealth;
    public int maxAttack;
    public int maxDefense;
    public int maxMovementRange;
    public string uniqueAbility;
    public int[] actionIndices; // References to actions in the database
    public string spritePath;
}

[System.Serializable]
public class CharacterDatabase
{
    public List<CharacterData> characters = new List<CharacterData>();
}

/**
 * PlayerController class -- manages the player-controlled characters.
 
 * NOTE: this class should not be used for gameplay design. Ideally,
 * this class only contains player information and the rest is
 * handled by the GameManager.

 * Functionality to add:
 * - track when turn has been completed
 * - set player sprite
 * - set player stats (from JSON eventually)
 * - attacking & all other actions
 */
public class CharacterController : OrderedCharacter
{
    #region Constants
    private const string DATA_PATH = "Data/characters";
    private const string SAVE_FILENAME = "character_data.json";
    #endregion

    #region Static Fields
    private static string SaveFilePath;
    private static CharacterDatabase s_CharacterDatabase;
    private static bool s_IsDatabaseLoaded = false;
    #endregion

    #region Private Fields
    private SpriteRenderer spriteRenderer;
    private CharacterData characterData;
    private bool hasMoved = false;
    private bool isSelected = false;
    private List<Action> actions = new List<Action>();
    private ActionDatabase actionDatabase;
    #endregion

    #region Public Properties
    public bool hasActed = false;
    public string characterName { get; private set; }
    public int baseHealth { get; private set; }
    public int baseAttack { get; private set; }
    public int baseDefense { get; private set; }
    public int baseMovementRange { get; private set; }
    public string uniqueAbility { get; private set; }
    public string spritePath { get; private set; }
    public int maxHealth { get; private set; }
    public int maxAttack { get; private set; }
    public int maxDefense { get; private set; }
    public int maxMovementRange { get; private set; }
    #endregion

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (string.IsNullOrEmpty(SaveFilePath))
        {
            SaveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        }
        LoadTestData();
    }

    #region Spawning
    public override void spawn(BoardManager bm, Vector2Int cell)
    {
        gridPosition = cell;
        boardManager = bm;
        transform.position = boardManager.cellToWorld(cell);
    }

    public void spawn(BoardManager bm, Vector2Int cell, Action[] acts)
    {
        gridPosition = cell;
        boardManager = bm;
        transform.position = boardManager.cellToWorld(cell);
        actions = acts.ToList();
    }

    public void spawn(BoardManager bm, Vector2Int cell, string characterName, ActionDatabase actionDatabase)
    {
        gridPosition = cell;
        this.actionDatabase = actionDatabase;
        LoadCharacterData(characterName, actionDatabase);
        spawn(bm, cell);
    }

    #endregion
    #region Turn Management
    public void resetTurn()
    {
        hasMoved = false;
        hasActed = false;
        isSelected = false;
    }

    public override bool isTurnComplete()
    {   //temp solve until combat is implemented
        if (hasMoved && hasActed)
        {
            return true;
        }
        return false;
    }

    public override void takeAction(Action action)
    {
        hasActed = true;
        Debug.Log("Player has taken action: " + action.name);
    }

    public Action[] GetActions()
    {
        return actions.ToArray();
    }


    public override void moveToCell(Vector2Int cell)
    {
        if (!boardManager.checkCell(cell))
        {
            return;
        }
        if (getDist(cell) > moveRange)
        {
            return;
        }
        if (gridPosition.x == cell.x && gridPosition.y == cell.y)
        {
            return;
        }
        if (hasMoved)
        {
            return;
        }
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
        hasMoved = true;
    }
    #endregion

    #region Player Selection
    public void toggleHighlight()
    {
        if (isSelected)
        {
            if (hasMoved)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.cyan;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void setSelected(bool selected)
    {
        isSelected = selected;
        toggleHighlight();
    }
    #endregion
    #region Damage

    public override void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // to deal with dying, again not sure how we are dealing with it
        Debug.Log($"{gameObject.name} died.");
        gameObject.SetActive(false);
    }
    #endregion


    #region Save and Load
    public static void SaveCharacterData(CharacterData data)
    {
        if (!s_IsDatabaseLoaded)
        {
            LoadCharacterDatabase();
        }

        // Update existing character or add new one
        int existingIndex = s_CharacterDatabase.characters.FindIndex(c => c.characterName == data.characterName);
        if (existingIndex >= 0)
        {
            s_CharacterDatabase.characters[existingIndex] = data;
        }
        else
        {
            s_CharacterDatabase.characters.Add(data);
        }

        string json = JsonUtility.ToJson(s_CharacterDatabase, true);
        if (SaveFilePath == null)
        {
            SaveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        }
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Character database saved to {SaveFilePath}");
    }

    public void SaveCharacterData()
    {
        CharacterData data = new CharacterData
        {
            characterName = this.characterName,
            baseHealth = this.baseHealth,
            baseAttack = this.baseAttack,
            baseDefense = this.baseDefense,
            baseMovementRange = this.baseMovementRange,
            uniqueAbility = this.uniqueAbility,
            actionIndices = GetActionIndices(),
            spritePath = this.spritePath,
            maxHealth = this.maxHealth,
            maxAttack = this.maxAttack,
            maxDefense = this.maxDefense,
            maxMovementRange = this.maxMovementRange,
        };

        SaveCharacterData(data);
    }

    private static void LoadCharacterDatabase()
    {
        if (s_IsDatabaseLoaded) return;

        if (!File.Exists(SaveFilePath))
        {
            s_CharacterDatabase = new CharacterDatabase();
            s_IsDatabaseLoaded = true;
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        s_CharacterDatabase = JsonUtility.FromJson<CharacterDatabase>(json);
        s_IsDatabaseLoaded = true;
    }

    private static void LoadTestData()
    {
        if (s_IsDatabaseLoaded) return;

        TextAsset jsonFile = Resources.Load<TextAsset>(DATA_PATH);
        if (jsonFile == null)
        {
            Debug.LogError($"Failed to load test character data from {DATA_PATH}");
            return;
        }

        s_CharacterDatabase = JsonUtility.FromJson<CharacterDatabase>(jsonFile.text);
        s_IsDatabaseLoaded = true;
        Debug.Log($"Test character database loaded with {s_CharacterDatabase.characters.Count} characters");
    }

    public void LoadCharacterData(string characterName, ActionDatabase actionDatabase)
    {
        if (s_CharacterDatabase == null)
        {
            Debug.LogError("Character database is null!");
            return;
        }

        CharacterData data = s_CharacterDatabase.characters.Find(c => c.characterName == characterName);
        if (data == null)
        {
            Debug.LogError($"Character data not found for: {characterName}");
            return;
        }




        // Apply character data
        this.characterName = data.characterName;
        this.baseHealth = data.baseHealth;
        this.baseAttack = data.baseAttack;
        this.baseDefense = data.baseDefense;
        this.baseMovementRange = data.baseMovementRange;
        hp = data.baseHealth;
        this.uniqueAbility = data.uniqueAbility;
        this.spritePath = data.spritePath;
        moveRange = data.baseMovementRange;

        // Load and set the character sprite from prefab
        if (!string.IsNullOrEmpty(data.spritePath))
        {
            // spritePath is the sprite name from the sliced sheet
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/CharacterSprites/tilemap_packed");
            Sprite found = sprites.FirstOrDefault(s => s.name == data.spritePath);
            if (found != null)
            {
                spriteRenderer.sprite = found;
            }
            else
            {
                Debug.LogWarning($"Sprite '{data.spritePath}' not found in tilemap_packed.png");
            }
        }

        if (actionDatabase != null && data.actionIndices != null)
        {
            actions = new List<Action>();
            foreach (int idx in data.actionIndices)
            {
                Action action = actionDatabase.GetActionByIndex(idx);
                if (action != null)
                {
                    actions.Add(action);
                }
                else
                {
                    Debug.LogWarning($"Failed to resolve action at index: {idx}");
                }
            }
        }

        // Log Character Actions
        /*Debug.Log($"Actions for {characterName}:");
        foreach (Action action in actions)
        {
            Debug.Log($"- {action.actionName}");
        }

        Debug.Log($"Character data loaded for: {characterName}");*/
    }

    public static List<CharacterData> GetAllCharacters()
    {
        if (!s_IsDatabaseLoaded)
        {
            LoadTestData();
        }
        return s_CharacterDatabase.characters;
    }


    private int[] GetActionIndices()
    {
        if (actions == null) return new int[0];
        int[] indices = new int[actions.Count];
        for (int i = 0; i < actions.Count; i++)
        {
            indices[i] = actionDatabase.GetIndexOfAction(actions[i]);
        }
        return indices;
    }

    #endregion

    private void logActions()
    {
        Debug.Log($"Actions for {characterName}:");
        foreach (Action action in actions)
        {
            Debug.Log($"- {action.actionName}");
        }
    }
}
