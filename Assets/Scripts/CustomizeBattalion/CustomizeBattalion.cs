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


public class CustomizeBattalion : MonoBehaviour
{
    public static CustomizeBattalion Instance;
    
    [SerializeField] TMP_Text selectedCharacterName;
    [SerializeField] TMP_Text selectedCharacterInformation;
    [SerializeField] Image selectedCharacterImage;
    [SerializeField] TMP_Text[] battalionCharacterNames;
    [SerializeField] Image[] battalionCharacterImages;
    [SerializeField] Button[] selectCharacterButtons;
    [SerializeField] TMP_Dropdown[] abilityDropdowns;
    [SerializeField] ActionDatabase actionDatabase;
    //TempCharacter selected;
    CharacterData selected;
    TempCharacter[] battalion = new TempCharacter[4];
    List<TempCharacter> bench;
    private CharacterDatabase characterDatabase;

    private string characterFilePath = "Assets/Resources/Data/characters.json";
    
    bool init = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        /* Delegate buttons */
        LoadCharacterDatabase();
        //battalion[0] = TempCharacter.exampleCharacters[0];
        //battalion[1] = TempCharacter.exampleCharacters[1];
        //battalion[2] = TempCharacter.exampleCharacters[2];
        //battalion[3] = TempCharacter.exampleCharacters[3];
        SelectCharacter(characterDatabase.characters[0]);
        //UpdateSelectedCharacter();
        int i = 0;
        foreach(Button b in selectCharacterButtons) {
            int x = i;
            b.onClick.AddListener(delegate{ SelectCharacter(x); });
            i += 1;
        }
        UpdateDropdowns();
    }

    void Update()
    {
        // Once the Store is active in the hierarchy, the information about the first character is displayed
        if(false && init && this.gameObject.activeInHierarchy) {
            
            init = false;
        
            //battalion[0] = TempCharacter.exampleCharacters[0];
            //battalion[1] = TempCharacter.exampleCharacters[1];
            //battalion[2] = TempCharacter.exampleCharacters[2];
            //battalion[3] = TempCharacter.exampleCharacters[3];
            SelectCharacter(characterDatabase.characters[0]);
            UpdateSelectedCharacter();
            int i = 0;
            foreach(Button b in selectCharacterButtons) {
                int x = i;
                b.onClick.AddListener(delegate{ SelectCharacter(x); });
                i += 1;
            }
        }
    }

    void UpdateSelectedCharacter() {
        //selectedCharacterImage.color = selected.GetColor();
        
        selectedCharacterInformation.text = "";
        selectedCharacterInformation.text = "Character Ability: " + selected.uniqueAbility + "\n\nGeneral Abilities:";
        
        selectedCharacterName.text = selected.characterName;

        UpdateBattalionCharacters();
        UpdateDropdowns();
    }

    void UpdateBattalionCharacters() {
        // Loop through battalion characters
        int i = 0;
        foreach(CharacterData character in characterDatabase.characters) {
            Debug.Log(character.characterName);
            // Display them in each slot - skipping the selected character
            if(character != selected) {
                battalionCharacterNames[i].text = character.characterName;
                //battalionCharacterImages[i].color = character.GetColor();
                i += 1;
            }
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        selected = character;
        UpdateSelectedCharacter();
        UpdateDropdowns();
    }

    public void SelectCharacter(int battalionIndex)
    {
        int selectedIndex = 0;
        for(int i = 0; i < 4; i++) {
            if(selected == characterDatabase.characters[i]) {
                selectedIndex = i;
            }
        }
        Debug.Log("Battalion Index: " + battalionIndex.ToString());
        Debug.Log("Selected Index: " + selectedIndex.ToString());
        if(battalionIndex < selectedIndex) {
            SelectCharacter( characterDatabase.characters[battalionIndex] );
        } else {
            SelectCharacter( characterDatabase.characters[battalionIndex+1] );
        }
    }

    public void SwapCharacters(TempCharacter characterToAdd) {
        /* TODO: Swap the selected character with characterToAdd */
    }

    public void UpdateCharacterAbility(int i)
    {
        /* TODO: When a dropdown is changed, update the character abilities */
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
            CharacterData character = characterDatabase.characters.FirstOrDefault(c => c == selected);

        }
        /* TODO: Update the dropdowns to reflect the character abilities */

    }

    private void LoadCharacterDatabase()
    {
        string json = File.ReadAllText(characterFilePath);
        characterDatabase = JsonUtility.FromJson<CharacterDatabase>(json);
    }
    private void SaveCharacterDatabase() {
        string json = JsonUtility.ToJson(characterDatabase, true);
        Debug.Log(json);
        //File.WriteAllText(characterFilePath, json);
    }
}