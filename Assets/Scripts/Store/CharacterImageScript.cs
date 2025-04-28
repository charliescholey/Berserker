using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



/*
Manages the character image icons in the store and what character they are associated with
*/
public class CharacterImageScript : MonoBehaviour
{
    private List<CharacterData> summaryList;
    [Range(0, 3)]
    public int characterIndex;
    [System.NonSerialized]
    public CharacterData character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCharacters();
        if (summaryList.Count > 0)
        {
            character = summaryList[0]; // Access the first character in the array
            this.gameObject.GetComponent<Image>().color = Color.white; // Set the color to white
            this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
        }
        else
        {
            Debug.LogError("No characters available in summaryList.");
        }
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

        summaryList = characterDataList;

        Debug.Log("Loaded " + summaryList.Count + " characters into the Store.");
    }

    public void onClick()
    {
        if (characterIndex >= 0 && characterIndex < summaryList.Count)
        {
            Debug.Log("Clicked on character: " + characterIndex);
            StoreUI.Instance.SelectCharacter(summaryList[characterIndex]); // Access the character by index
        }
        else
        {
            Debug.LogError("Invalid characterIndex: " + characterIndex);
        }
    }
}