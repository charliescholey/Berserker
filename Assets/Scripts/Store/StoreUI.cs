using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using Unity.Multiplayer.Center.Common;

/*
Displays characters that the player is able to buy and allows the player to purchase them

Currently this script uses TempCharacter - in the future it will not.

ToDo:
- Once the permanent character data is created, link up this class with that information
so that it displays actual character information
*/
public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance;
    
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text characterInformation;
    [SerializeField] Image characterImage;
    [SerializeField] TMP_Text characterCost;
    TempCharacter selected;
    
    bool init = true;

    //TempCharacter[] characters = new TempCharacter[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        // Once the Store is active in the hierarchy, the information about the first character is displayed
        if(init && this.gameObject.activeInHierarchy) {
            SelectCharacter(this.transform.Find("Character Image (1)").GetComponent<CharacterImageScript>().character);
            UpdateSelectedCharacter();
            init = false;
        }
    }

    void UpdateSelectedCharacter() {
        characterImage.color = selected.GetColor();
        Debug.Log("cost:"+selected.GetCost());
        characterCost.text = "Buy          " + selected.GetCost();
        characterInformation.text = "";
        characterInformation.text = "Bio: " + selected.GetBio();
        characterInformation.text = characterInformation.text + "\nAbility: " + selected.GetAbility();
        characterInformation.text = characterInformation.text + "\nBuffs: " + selected.GetBuffs();
        characterInformation.text = characterInformation.text + "\nNerfs: " + selected.GetNerfs();
        
        characterName.text = selected.GetName();
    }

    // Update is called once per frame
    public void SelectCharacter(TempCharacter character)
    {
        selected = character;
        UpdateSelectedCharacter();
    }
}
