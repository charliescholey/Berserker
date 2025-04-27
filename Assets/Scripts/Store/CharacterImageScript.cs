using UnityEngine;
using UnityEngine.UI;


/*
Manages the character image icons in the store and what character they are associated with
*/
public class CharacterImageScript : MonoBehaviour
{
    CharacterSummaryList summaryList;
    [Range(0, 3)]
    public int characterIndex;
    [System.NonSerialized]
    public CharacterSummary character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCharacters();
        character = summaryList.characters[0];
        this.gameObject.GetComponent<Image>().color = character.getColor();
        this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
    }

    private void LoadCharacters()
    {
        Debug.Log("Loading characters.json from Resources/Data/");
        TextAsset jsonText = Resources.Load<TextAsset>("Data/characters");
        if (jsonText != null)
        {
            CharacterSummaryList summaryList = JsonUtility.FromJson<CharacterSummaryList>(jsonText.text);
            if (summaryList == null)
            {
                Debug.LogError("summaryList is null after parsing!");
                return;
            }
            if (summaryList.characters == null)
            {
                Debug.LogError("summaryList.characters is null!");
                return;
            }
            Debug.Log("Loaded " + summaryList.characters.Length + " characters.");
            foreach (var character in summaryList.characters)
            {
                Debug.Log($"Name: {character.characterName}, Health: {character.baseHealth}, Attack: {character.baseAttack}, Defense: {character.baseDefense}, Move: {character.baseMovementRange}");
            }
            this.summaryList = summaryList;
        }
        else
        {
            Debug.LogError("Could not load characters.json from Resources/Data/");
        }
    }

    public void onClick()
    {
        Debug.Log("Clicked on character: " + characterIndex);
        StoreUI.Instance.SelectCharacter(summaryList.characters[characterIndex]);
    }
}
