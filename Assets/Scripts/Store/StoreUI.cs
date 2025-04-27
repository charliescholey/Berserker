using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using Unity.Multiplayer.Center.Common;


// [Serializable]
// public class CharacterSummary
// {
//     public string characterName;
//     public int baseHealth;
//     public int baseAttack;
//     public int baseDefense;
//     public int baseMovementRange;

//     public int maxHealth;
//     public int maxAttack;
//     public int maxDefense;
//     public int maxMovementRange;
//     public string spritePath;

//     public string getName() { return characterName; }
//     public int getCost() { return baseHealth + baseAttack + baseDefense + baseMovementRange; }
//     public string getImage() { return null; } // Placeholder image
//     public int getHealth() { return baseHealth; }
//     public int getAttack() { return baseAttack; }
//     public int getDefense() { return baseDefense; }
//     public int getMovement() { return baseMovementRange; }
//     public int getMaxHealth() { return maxHealth; }
//     public int getMaxAttack() { return maxAttack; }
//     public int getMaxDefense() { return maxDefense; }
//     public int getMaxMovement() { return maxMovementRange; }

//     public Color getColor()
//     {
//         // Placeholder color
//         return new Color(10, 10, 10, 1);
//     }
// }

// [Serializable]
// public class CharacterSummaryList
// {
//     public CharacterSummary[] characters;
// }

/**
* StoreUI is a singleton class that manages the user interface of the store.
* It displays information about characters available for purchase.
*/
public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance;

    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text characterInformation;
    [SerializeField] Image characterImage;
    [SerializeField] TMP_Text characterCost;

    [SerializeField] TMP_Text Health;
    [SerializeField] TMP_Text Attack;
    [SerializeField] TMP_Text Defense;
    [SerializeField] TMP_Text Movement;

    // private CharacterSummaryList summaryList;
    private CharacterData[] summaryList;

    CharacterSummary selected;

    bool init = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        // Load the JSON file from Resources
        LoadCharacters();
    }

    private void LoadCharacters()
    {
        Debug.Log("Loading characters from CharacterController database...");

        var characterDataList = CharacterController.GetAllCharacters();
        if (characterDataList == null || characterDataList.Count == 0)
        {
            Debug.LogError("No characters found!");
            return;
        }


        summaryList = characterDataList

        Debug.Log("Loaded " + summaryList.characters.Length + " characters into the Store.");
    }


    void Update()
    {
        // Once the Store is active in the hierarchy, the information about the first character is displayed
        if (init && this.gameObject.activeInHierarchy)
        {
            // SelectCharacter(this.transform.Find("Character Image (1)").GetComponent<CharacterImageScript>().character);
            SelectCharacter(summaryList.characters[0]);
            UpdateSelectedCharacter();
            init = false;
        }
    }

    void UpdateSelectedCharacter()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (characterImage == null) { Debug.LogError("characterImage is null!"); return; }
        if (Health == null) { Debug.LogError("Health is null!"); return; }
        if (Attack == null) { Debug.LogError("Attack is null!"); return; }
        if (Defense == null) { Debug.LogError("Defense is null!"); return; }
        if (Movement == null) { Debug.LogError("Movement is null!"); return; }
        if (characterName == null) { Debug.LogError("characterName is null!"); return; }
        if (characterCost == null) { Debug.LogError("characterCost is null!"); return; }

        Debug.Log("Updating selected character: " + selected.characterName);
        characterImage.color = selected.getColor();
        Debug.Log("cost:" + selected.getCost());
        characterCost.text = "Buy          " + selected.getCost();

        // Show stats as "base / max"
        Health.text = $"{selected.baseHealth} / {selected.maxHealth}";
        Attack.text = $"{selected.baseAttack} / {selected.maxAttack}";
        Defense.text = $"{selected.baseDefense} / {selected.maxDefense}";
        Movement.text = $"{selected.baseMovementRange} / {selected.maxMovementRange}";

        characterName.text = selected.characterName;

        // Load the correct sprite from the packed sprite sheet
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/CharacterSprites/tilemap_packed");

        Sprite found = null;
        foreach (var sprite in sprites)
        {
            if (sprite.name == selected.spritePath)
            {
                found = sprite;
                break;
            }
        }
        if (found != null)
        {
            characterImage.sprite = found;
        }
        else
        {
            Debug.LogWarning($"Sprite '{selected.spritePath}' not found in tilemap_packed.png");
            characterImage.sprite = null; // Or set a default/fallback sprite
        }
    }

    public void SaveSelectedCharacter()
    {
        if (selected == null)
        {
            Debug.LogError("No selected character to save!");
            return;
        }

        // Create a new CharacterData from selected CharacterSummary
        CharacterData data = new CharacterData
        {
            characterName = selected.characterName,
            baseHealth = selected.baseHealth,
            baseAttack = selected.baseAttack,
            baseDefense = selected.baseDefense,
            baseMovementRange = selected.baseMovementRange,
            uniqueAbility = "", // If you have it, otherwise leave empty
            spritePath = selected.spritePath,
            actionIndices = new int[0] // You might not have actions in Store
        };

        CharacterController.SaveCharacterData(data);
        Debug.Log($"Saved character: {data.characterName}");
    }


    // Update is called once per frame
    public void SelectCharacter(CharacterSummary character)
    {
        selected = character;
        UpdateSelectedCharacter();
    }

    public void PurchaseHealth()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseHealth >= selected.maxHealth) { Debug.LogError("Cannot purchase health, already at max!"); return; }
        selected.baseHealth++;
        UpdateSelectedCharacter();
    }

    public void PurchaseAttack()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseAttack >= selected.maxAttack) { Debug.LogError("Cannot purchase attack, already at max!"); return; }
        selected.baseAttack++;
        UpdateSelectedCharacter();
    }

    public void PurchaseDefense()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseDefense >= selected.maxDefense) { Debug.LogError("Cannot purchase defense, already at max!"); return; }
        selected.baseDefense++;
        UpdateSelectedCharacter();
    }
    public void PurchaseMovement()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseMovementRange >= selected.maxMovementRange) { Debug.LogError("Cannot purchase movement, already at max!"); return; }
        selected.baseMovementRange++;
        UpdateSelectedCharacter();
    }
    // void DisplayCharacterStats(CharacterSummary character)
    // {
    //     // Clear previous bars
    //     foreach (Transform child in statsContainer)
    //         Destroy(child.gameObject);

    //     // List of stat names and their values
    //     var stats = new List<(string label, int baseValue, int maxValue)>
    // {
    //     ("Health", character.baseHealth, character.maxHealth),
    //     ("Attack", character.baseAttack, character.maxAttack),
    //     ("Defense", character.baseDefense, character.maxDefense),
    //     ("Movement", character.baseMovementRange, character.maxMovementRange)
    // };

    //     foreach (var stat in stats)
    //     {
    //         GameObject barObj = Instantiate(statBarPrefab, statsContainer);
    //         TMP_Text label = barObj.transform.Find("Label").GetComponent<TMP_Text>();
    //         Slider slider = barObj.transform.Find("Slider").GetComponent<Slider>();
    //         TMP_Text value = barObj.transform.Find("Value").GetComponent<TMP_Text>();

    //         label.text = stat.label;
    //         slider.maxValue = stat.maxValue;
    //         slider.value = stat.baseValue;
    //         value.text = $"{stat.baseValue} / {stat.maxValue}";
    //     }
    // }
}
