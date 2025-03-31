using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseMovementRange;
    public int currentHealth;
    public string characterClass;
    public string uniqueAbility;
    public string[] actionIdentifiers; // References to actions in the database
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
    private const string TEST_DATA_PATH = "TestData/characters";
    private const string SAVE_FILENAME = "character_data.json";
    #endregion

    #region Static Fields
    private static string SaveFilePath;
    private static CharacterDatabase s_CharacterDatabase;
    private static bool s_IsDatabaseLoaded = false;
    #endregion

    #region Static Constructor
    static CharacterController()
    {
        SaveFilePath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, SAVE_FILENAME);
    }
    #endregion

    #region Private Fields
    private SpriteRenderer spriteRenderer;
    private CharacterData characterData;
    private bool hasActed = false;
    private bool hasMoved = false;
    private bool isSelected = false;
    private Vector2Int gridPosition;
    private List<Action> actions = new List<Action>();
    private BoardManager boardManager;
    private ActionDatabase actionDatabase;
    private int moveRange;
    #endregion

    #region Public Properties
    public string characterName { get; private set; }
    public int baseHealth { get; private set; }
    public int baseAttack { get; private set; }
    public int baseDefense { get; private set; }
    public int baseMovementRange { get; private set; }
    public int currentHealth { get; private set; }
    public string characterClass { get; private set; }
    public string uniqueAbility { get; private set; }
    #endregion

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadTestData();
    }

    public override void spawn(BoardManager bm, Vector2Int cell)
    {
        boardManager = bm;
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
    }

    public void spawn(BoardManager bm, Vector2Int cell, Action[] acts)
    {
        boardManager = bm;
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
        actions = acts.ToList();
    }

    public void spawn(BoardManager bm, Vector2Int cell, string characterName, ActionDatabase actionDatabase)
    {
        LoadCharacterData(characterName, actionDatabase);
        spawn(bm, cell);
    }

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

    public Vector2Int[] getMovementRange()
    {
        //handle as arraylist for dynamic sizing
        List<Vector2Int> cells = new List<Vector2Int>();
        for (int i = -base.moveRange; i <= base.moveRange; i++)
        {
            for (int j = -base.moveRange; j <= base.moveRange; j++)
            {
                //uses existing manhattan distance function from OrderedCharacter
                if (getDist(new Vector2Int(gridPosition.x + i, gridPosition.y + j)) <= base.moveRange)
                {
                    cells.Add(new Vector2Int(gridPosition.x + i, gridPosition.y + j));
                }
            }
        }

        //handle return as array
        return cells.ToArray();
    }

    public override void moveToCell(Vector2Int cell)
    {
        if (getDist(cell) > base.moveRange)
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

    public Action[] GetActions()
    {
        return actions.ToArray();
    }

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
            currentHealth = this.currentHealth,
            characterClass = this.characterClass,
            uniqueAbility = this.uniqueAbility,
            actionIdentifiers = GetActionIdentifiers()
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

        TextAsset jsonFile = Resources.Load<TextAsset>(TEST_DATA_PATH);
        if (jsonFile == null)
        {
            Debug.LogError($"Failed to load test character data from {TEST_DATA_PATH}");
            return;
        }

        s_CharacterDatabase = JsonUtility.FromJson<CharacterDatabase>(jsonFile.text);
        s_IsDatabaseLoaded = true;
        Debug.Log($"Test character database loaded with {s_CharacterDatabase.characters.Count} characters");
    }

    public void LoadCharacterData(string characterName, ActionDatabase actionDatabase)
    {
        if (!s_IsDatabaseLoaded)
        {
            LoadTestData();
        }

        CharacterData data = s_CharacterDatabase.characters.Find(c => c.characterName == characterName);
        if (data == null)
        {
            Debug.LogError($"Character data not found for: {characterName}");
            return;
        }

        // Apply loaded data
        this.characterName = data.characterName;
        this.baseHealth = data.baseHealth;
        this.baseAttack = data.baseAttack;
        this.baseDefense = data.baseDefense;
        this.baseMovementRange = data.baseMovementRange;
        this.currentHealth = data.currentHealth;
        this.characterClass = data.characterClass;
        this.uniqueAbility = data.uniqueAbility;
        base.moveRange = data.baseMovementRange; // Set the base class's moveRange

        // Resolve actions from identifiers
        if (actionDatabase != null && data.actionIdentifiers != null)
        {
            actions = new List<Action>();
            for (int i = 0; i < data.actionIdentifiers.Length; i++)
            {
                Action action = actionDatabase.GetActionByIdentifier(data.actionIdentifiers[i]);
                if (action != null)
                {
                    actions.Add(action);
                }
                else
                {
                    Debug.LogWarning($"Failed to resolve action identifier: {data.actionIdentifiers[i]}");
                }
            }
        }

        Debug.Log($"Character data loaded for: {characterName}");
    }

    private string[] GetActionIdentifiers()
    {
        if (actions == null) return new string[0];

        string[] identifiers = new string[actions.Count];
        for (int i = 0; i < actions.Count; i++)
        {
            identifiers[i] = actions[i]?.identifier ?? string.Empty;
        }
        return identifiers;
    }
}
