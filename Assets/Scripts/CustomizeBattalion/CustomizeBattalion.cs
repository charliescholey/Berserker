using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Multiplayer.Center.Common;
using UnityEngine.TextCore.Text;
using Unity.Collections;

using System.IO;
using System.Linq;
using UnityEditor.Timeline.Actions;

public class CustomizeBattalion : MonoBehaviour
{
    public static CustomizeBattalion Instance;
    
    [SerializeField] TMP_Text selectedCharacterName;
    [SerializeField] TMP_Text selectedCharacterInformation;
    [SerializeField] Image selectedCharacterImage;
    [SerializeField] TMP_Text[] battalionCharacterNames;
    [SerializeField] Image[] battalionCharacterImages;
    [SerializeField] Button[] battalionCharacterButtons;
    //[SerializeField] Button[] selectCharacterButtons;
    [SerializeField] TMP_Dropdown[] abilityDropdowns;
    [SerializeField] ActionDatabase actionDatabase;
    CharacterData selected;
    //TempCharacter[] battalion = new TempCharacter[4];
    //List<TempCharacter> bench;
    private CharacterDatabase characterDatabase;
    private Sprite[] sprites;

    private string characterFilePath = "Assets/Resources/Data/characters.json";
    
    bool init = true;

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
        ResetUI();
    }

    public void ResetUI() {
        /* Delegate buttons */
        LoadCharacterDatabase();
        SelectCharacter(characterDatabase.characters[0]);
        UpdateDropdowns();
    }

    void Update()
    {
        /*
        if(init && this.gameObject.activeInHierarchy) {
            Debug.Log("LoadCharacterData");
            LoadCharacterDatabase();
        }
        */
        init = false;
        // Once the Store is active in the hierarchy, the information about the first character is displayed
        if(false && init && this.gameObject.activeInHierarchy) {
            
            init = false;
            SelectCharacter(characterDatabase.characters[0]);
            UpdateSelectedCharacter();
            int i = 0;
            foreach(Button b in battalionCharacterButtons) {
                int x = i;
                b.onClick.AddListener(delegate{ SelectCharacter(x); });
                i += 1;
            }
        }
    }

    void UpdateSelectedCharacter() {
        //selectedCharacterImage.color = selected.GetColor();
        Sprite found = sprites.FirstOrDefault(s => s.name == selected.spritePath);
        selectedCharacterImage.sprite = found;
        selectedCharacterInformation.text = "";
        selectedCharacterInformation.text = "Character Ability: " + selected.uniqueAbility + "\n\nGeneral Abilities:";
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
                //battalionCharacterImages[i].color = Color.white;
                //battalionCharacterImages[i].sprite = found;
                battalionCharacterButtons[i].image.sprite = found;
                //Debug.Log("Colour changed to white");
                
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
        Debug.Log("SelectCharacter(int " + battalionIndex + ")");
        int selectedIndex = 0;
        for(int i = 0; i < 4; i++) {
            if(selected == characterDatabase.characters[i]) {
                selectedIndex = i;
            }
        }
        Debug.Log("selectedIndex: " + selectedIndex);
        if(battalionIndex < selectedIndex) {
            SelectCharacter( characterDatabase.characters[battalionIndex] );
        } else {
            SelectCharacter( characterDatabase.characters[battalionIndex+1] );
        }
    }

    public void SwapCharacters(TempCharacter characterToAdd) {
        /* TODO: Swap the selected character with characterToAdd */
    }

    public void UpdateCharacterAbility()
    {
        List<int> tempList = new List<int>();

        foreach (TMP_Dropdown dropdown in abilityDropdowns)
        {
            int val = dropdown.value;
            if (val >= 1 && !tempList.Contains(val-1))
            {
                tempList.Add(val-1);
            }
        }

        int[] indices = tempList.ToArray();
        
        //Debug.Log("Selected indices: " + string.Join(", ", indices));

        characterDatabase.characters.FirstOrDefault(c => c == selected).actionIndices = indices;
    }

    public void UpdateDropdowns()
    {
        foreach(TMP_Dropdown d in abilityDropdowns) {
            d.ClearOptions();
            d.options.Add (new TMP_Dropdown.OptionData() {text="-None-"});
            foreach(Action action in actionDatabase.GetAllActions()) {
                d.options.Add (new TMP_Dropdown.OptionData() {text=action.actionName});
            }
            // Get current player's actions
            //CharacterData character = characterDatabase.characters.FirstOrDefault(c => c == selected);
        }
        
        int[] _actions = selected.actionIndices;
        abilityDropdowns[0].value = -1; // -None-
        abilityDropdowns[1].value = -1;
        //abilityDropdowns[0].RefreshShownValue();
        
        if(_actions.Length >= 1) {
            abilityDropdowns[0].value = _actions[0]+1;
        }
        if(_actions.Length >= 2) {
            abilityDropdowns[1].value = _actions[1]+1;
        }
    }

    private void LoadCharacterDatabase()
    {
        string json = File.ReadAllText(characterFilePath);
        characterDatabase = JsonUtility.FromJson<CharacterDatabase>(json);
    }
    private void SaveCharacterDatabase() {
        string json = JsonUtility.ToJson(characterDatabase, true);
        //selectedCharacterInformation.text = json;
        File.WriteAllText(characterFilePath, json);
    }

    public void Cancel() {
        init = true;
    }
    public void Save() {
        SaveCharacterDatabase();
        init = true;
    }
}