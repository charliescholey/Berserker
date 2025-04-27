using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using Unity.Multiplayer.Center.Common;
using System.Collections.Generic;


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
    private List<CharacterData> summaryList;

    CharacterData selected;

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
                    summaryList = new List<CharacterData>(); // Initialize as an empty list to avoid null issues

            return;
        }


        summaryList = characterDataList;

        Debug.Log("Loaded " + summaryList.Count + " characters into the Store.");
    }


    void Update()
    {
        // Once the Store is active in the hierarchy, the information about the first character is displayed
        if (init && this.gameObject.activeInHierarchy)
        {
            if (summaryList == null || summaryList.Count == 0)
            {
                Debug.LogError("No characters available in summaryList.");
                return;
            }
            // SelectCharacter(this.transform.Find("Character Image (1)").GetComponent<CharacterImageScript>().character);
            SelectCharacter(summaryList[0]);
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
        // characterImage.color = selected.getColor();
        // Debug.Log("cost:" + selected.getCost());
        // characterCost.text = "Buy          " + selected.getCost();

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
            uniqueAbility = selected.uniqueAbility,
            maxHealth = selected.maxHealth,
            maxAttack = selected.maxAttack,
            maxDefense = selected.maxDefense,
            maxMovementRange = selected.maxMovementRange,
            spritePath = selected.spritePath,
            actionIndices = selected.actionIndices,
        };

        CharacterController.SaveCharacterData(data);
        Debug.Log($"Saved character: {data.characterName}");
    }


    // Update is called once per frame
    public void SelectCharacter(CharacterData character)
    {
        selected = character;
        UpdateSelectedCharacter();
    }

    public void PurchaseHealth()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        int gold = SaveFileManager.CurrentPlayerData.gold;
        if (gold < 10)
        {
            Debug.LogError("Not enough gold to purchase character!");
            return;
        }
        if (selected.baseHealth >= selected.maxHealth) { Debug.LogError("Cannot purchase health, already at max!"); return; }
        selected.baseHealth++;
        UpdateSelectedCharacter();
        gold -= 10;
        SaveSelectedCharacter();
    }

    public void PurchaseAttack()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseAttack >= selected.maxAttack) { Debug.LogError("Cannot purchase attack, already at max!"); return; }
        int gold = SaveFileManager.CurrentPlayerData.gold;
        if (gold < 15)
        {
            Debug.LogError("Not enough gold to purchase character!");
            return;
        }

        selected.baseAttack++;
        UpdateSelectedCharacter();
        gold -= 15;
        SaveSelectedCharacter();
    }

    public void PurchaseDefense()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseDefense >= selected.maxDefense) { Debug.LogError("Cannot purchase defense, already at max!"); return; }
        int gold = SaveFileManager.CurrentPlayerData.gold;
        if (gold < 15)
        {
            Debug.LogError("Not enough gold to purchase character!");
            return;
        }
        selected.baseDefense++;
        UpdateSelectedCharacter();
        gold -= 15;
        SaveSelectedCharacter();
    }
    public void PurchaseMovement()
    {
        if (selected == null) { Debug.LogError("selected is null!"); return; }
        if (selected.baseMovementRange >= selected.maxMovementRange) { Debug.LogError("Cannot purchase movement, already at max!"); return; }
        int gold = SaveFileManager.CurrentPlayerData.gold;
        if (gold < 20)
        {
            Debug.LogError("Not enough gold to purchase character!");
            return;
        }
        selected.baseMovementRange++;
        UpdateSelectedCharacter();
        gold -= 20;
        SaveSelectedCharacter();
    }
}
