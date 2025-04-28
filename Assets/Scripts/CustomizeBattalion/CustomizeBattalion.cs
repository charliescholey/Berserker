using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

/**
* CustomizeBattalion manages the character selection and ability assignment for the battalion.
*/
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

    private List<CharacterData> characterList;
    private CharacterData selected;
    private Sprite[] sprites;

    void Start()
    {
        Instance = this;
        sprites = Resources.LoadAll<Sprite>("Sprites/CharacterSprites/tilemap_packed");
        InitializeButtons();
        Reset_UI();
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < battalionCharacterButtons.Length; i++)
        {
            int index = i;
            battalionCharacterButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }
    }

    public void Reset_UI()
    {
        LoadCharacterList();
        if (characterList != null && characterList.Count > 0)
        {
            SelectCharacter(characterList[0]);
        }
        UpdateDropdowns();
    }

    private void LoadCharacterList()
    {
        Debug.Log("Loading characters from CharacterController database...");

        var characterDataList = CharacterController.GetAllCharacters();
        if (characterDataList == null || characterDataList.Count == 0)
        {
            Debug.LogError("No characters found!");
            characterList = new List<CharacterData>();
            return;
        }

        characterList = characterDataList;

        Debug.Log("Loaded " + characterList.Count + " characters into CustomizeBattalion.");
    }

    private void UpdateSelectedCharacter()
    {
        if (selected == null)
        {
            Debug.LogError("No character selected!");
            return;
        }

        Sprite found = sprites.FirstOrDefault(s => s.name == selected.spritePath);
        selectedCharacterImage.sprite = found;
        selectedCharacterInformation.text = "Character Abilities:";
        selectedCharacterName.text = selected.characterName;

        UpdateDropdowns();
        UpdateBattalionCharacters();
    }

    private void UpdateBattalionCharacters()
    {
        if (characterList == null) return;

        int i = 0;
        foreach (CharacterData character in characterList)
        {
            if (character != selected && i < battalionCharacterNames.Length)
            {
                battalionCharacterNames[i].text = character.characterName;
                Sprite found = sprites.FirstOrDefault(s => s.name == character.spritePath);
                battalionCharacterButtons[i].image.sprite = found;
                i++;
            }
        }

        // Clear any extra UI elements if fewer characters
        for (; i < battalionCharacterNames.Length; i++)
        {
            battalionCharacterNames[i].text = "-";
            battalionCharacterButtons[i].image.sprite = null;
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        selected = character;
        UpdateSelectedCharacter();
    }

    public void SelectCharacter(int battalionIndex)
    {
        if (characterList == null || selected == null) return;

        int selectedIndex = characterList.IndexOf(selected);
        if (selectedIndex == -1)
        {
            Debug.LogError("Selected character not found in character list.");
            return;
        }

        if (battalionIndex < selectedIndex)
        {
            SelectCharacter(characterList[battalionIndex]);
        }
        else
        {
            if (battalionIndex + 1 < characterList.Count)
                SelectCharacter(characterList[battalionIndex + 1]);
        }
    }

    public void UpdateCharacterAbility()
    {
        if (selected == null) return;

        List<int> tempList = new List<int>();

        foreach (TMP_Dropdown dropdown in abilityDropdowns)
        {
            string name = dropdown.options[dropdown.value].text;

            if (name != "-None-")
            {
                int index = GetActionIndexByName(name);
                if (!tempList.Contains(index))
                    tempList.Add(index);
            }
        }

        selected.actionIndices = tempList.ToArray();

        SaveSelectedCharacter();
    }

    public void UpdateDropdowns()
    {
        foreach (TMP_Dropdown dropdown in abilityDropdowns)
        {
            dropdown.ClearOptions();
            dropdown.options.Add(new TMP_Dropdown.OptionData { text = "-None-" });

            foreach (Action action in actionDatabase.GetAllActions())
            {
                if (SaveFileManager.CurrentPlayerData.UnlockedSkillIndices.Contains(actionDatabase.GetIndexOfAction(action)))
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData { text = action.actionName });
                }
            }
        }

        if (selected == null) return;

        int[] selectedActions = selected.actionIndices;

        for (int i = 0; i < abilityDropdowns.Length; i++)
        {
            abilityDropdowns[i].value = 0; // Default to "-None-"
            if (i < selectedActions.Length)
            {
                string actionName = actionDatabase.GetActionByIndex(selectedActions[i]).actionName;
                int index = abilityDropdowns[i].options.FindIndex(option => option.text == actionName);
                if (index != -1)
                {
                    abilityDropdowns[i].value = index;
                }
            }
        }
    }

    private int GetActionIndexByName(string name)
    {
        Action act = actionDatabase.GetAllActions().FirstOrDefault(a => a.actionName == name);
        return actionDatabase.GetIndexOfAction(act);
    }

    public void SaveSelectedCharacter()
    {
        if (selected == null)
        {
            Debug.LogError("No selected character to save!");
            return;
        }

        CharacterController.SaveCharacterData(selected);
        Debug.Log($"Saved character: {selected.characterName}");
    }

    public void Cancel()
    {
        // Implement cancel logic if needed
    }

    public void Save()
    {
        SaveSelectedCharacter();
    }
}
