using UnityEngine;
using UnityEngine.UI;
using System;

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Multiplayer.Center.Common;
using UnityEngine.TextCore.Text;
using Unity.Collections;

using System.IO;
using System.Linq;

public class CustomizeBattalionModel
{
    [SerializeField] ActionDatabase actionDatabase;
    private CharacterDatabase characterDatabase;
    private Sprite[] sprites;
    private string characterFilePath = "Assets/Resources/Data/characters.json";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CustomizeBattalionModel()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/CharacterSprites/tilemap_packed");
    }

    private CharacterDatabase LoadCharacterDatabase()
    {
        string json = File.ReadAllText(characterFilePath);
        characterDatabase = JsonUtility.FromJson<CharacterDatabase>(json);
        return characterDatabase;
    }

    private void SaveCharacterDatabase() {
        string json = JsonUtility.ToJson(characterDatabase, true);
        //selectedCharacterInformation.text = json;
        File.WriteAllText("erin-data.json", json);
    }
}