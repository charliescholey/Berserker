using UnityEngine;
using UnityEngine.UI;

/*
Manages the character image icons in the store and what character they are associated with
*/
public class CharacterImageScript : MonoBehaviour
{
    [Range(0, 3)]
    public int tempCharacterIndex;
    [System.NonSerialized]
    public TempCharacter character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = TempCharacter.exampleCharacters[tempCharacterIndex];
        this.gameObject.GetComponent<Image>().color = character.GetColor();
        this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
    }

    
    public void onClick()
    {
        StoreUI.Instance.SelectCharacter(character);
    }
}
