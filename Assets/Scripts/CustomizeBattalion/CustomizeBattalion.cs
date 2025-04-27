using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

using System.Collections.Generic;
using Unity.Collections;

using System.IO;
using System.Linq;

public class CustomizeBattalion : MonoBehaviour
{
    public static CustomizeBattalion Instance;
    
    [SerializeField] TMP_Text selectedCharacterName;
    [SerializeField] TMP_Text selectedCharacterInformation;
    [SerializeField] Image selectedCharacterImage;
    [SerializeField] TMP_Text[] battalionCharacterNames;
    [SerializeField] Button[] battalionCharacterButtons;
    [SerializeField] TMP_Dropdown[] abilityDropdowns;
    [SerializeField] ActionDatabase actionDatabase;
    CharacterData selected;
    private CharacterDatabase characterDatabase;
    private Sprite[] sprites;

    private string characterFilePath = "Assets/Resources/Data/characters.json";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        sprites = Resources.LoadAll<Sprite>("Sprites/CharacterSprites/tilemap_packed");
        int i = 0;
        foreach(Button b in battalionCharacterButtons) {
            int x = i;
            b.onClick.AddListener(delegate{ SelectCharacter(x); });
            i += 1;
        }
        Reset_UI();
    }

    public void Reset_UI() {
        LoadCharacterDatabase();
        SelectCharacter(characterDatabase.characters[0]);
        UpdateDropdowns();
    }

    void UpdateSelectedCharacter() {
        Sprite found = sprites.FirstOrDefault(s => s.name == selected.spritePath);
        selectedCharacterImage.sprite = found;
        selectedCharacterInformation.text = "Character Abilities:";
        UpdateDropdowns();
        
        selectedCharacterName.text = selected.characterName;
        
        UpdateBattalionCharacters();
    }

    void UpdateBattalionCharacters() {
        // Loop through battalion characters
        int i = 0;
        foreach(CharacterData character in characterDatabase.characters) {
            // Display them in each slot - skipping the selected character
            if(character != selected) {
                battalionCharacterNames[i].text = character.characterName;
                Sprite found = sprites.FirstOrDefault(s => s.name == character.spritePath);
                battalionCharacterButtons[i].image.sprite = found; 
                i += 1;
            }
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        selected = character;
        UpdateSelectedCharacter();
    }

    public void SelectCharacter(int battalionIndex)
    {
        int selectedIndex = 0;
        for(int i = 0; i < 4; i++) {
            if(selected == characterDatabase.characters[i]) {
                selectedIndex = i;
            }
        }
        if(battalionIndex < selectedIndex) {
            SelectCharacter( characterDatabase.characters[battalionIndex] );
        } else {
            SelectCharacter( characterDatabase.characters[battalionIndex+1] );
        }
    }

    public void UpdateCharacterAbility()
    {
        List<int> tempList = new List<int>();

        foreach (TMP_Dropdown dropdown in abilityDropdowns)
        {
            string name = dropdown.options[dropdown.value].text;
            
            if (name != "-None-")
            {
                int i = GetActionIndexByName(name);
                if(! tempList.Contains(i)) tempList.Add(i);
            }
        }

        int[] indices = tempList.ToArray();

        characterDatabase.characters.FirstOrDefault(c => c == selected).actionIndices = indices;
    }

    public void UpdateDropdowns()
    {
        foreach(TMP_Dropdown d in abilityDropdowns) {
            d.ClearOptions();
            d.options.Add (new TMP_Dropdown.OptionData() {text="-None-"});
            foreach(Action action in actionDatabase.GetAllActions()) {
                if(SaveFileManager.CurrentPlayerData.UnlockedSkillIndices.Contains( actionDatabase.GetIndexOfAction(action)))
                    d.options.Add (new TMP_Dropdown.OptionData() {text=action.actionName});
            }
            // Get current player's actions
            //CharacterData character = characterDatabase.characters.FirstOrDefault(c => c == selected);
        }
        
        int[] _actions = selected.actionIndices;
        abilityDropdowns[0].value = -1; // -None-
        abilityDropdowns[1].value = -1;
        
        if(_actions.Length >= 1) {
            string actName = actionDatabase.GetActionByIndex(_actions[0]).actionName;
            abilityDropdowns[0].value = abilityDropdowns[0].options.FindIndex(option => option.text == actName);
        }
        if(_actions.Length >= 2) {
            string actName2 = actionDatabase.GetActionByIndex(_actions[1]).actionName;
            abilityDropdowns[1].value = abilityDropdowns[1].options.FindIndex(option => option.text == actName2);
        }
    }

    private void LoadCharacterDatabase()
    {
        string json = File.ReadAllText(characterFilePath);
        characterDatabase = JsonUtility.FromJson<CharacterDatabase>(json);
    }
    private void SaveCharacterDatabase() {
        string json = JsonUtility.ToJson(characterDatabase, true);
        File.WriteAllText(characterFilePath, json);
    }

    public void Cancel() {
    }
    public void Save() {
        SaveCharacterDatabase();
    }

    private int GetActionIndexByName(string name){
        Action act = actionDatabase.GetAllActions().FirstOrDefault(a => a.actionName == name);
        int i = actionDatabase.GetIndexOfAction(act);
        return i;
    } 
}