using TMPro;
using UnityEngine;

public class HPTextController : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private OrderedCharacter character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void setCharacter(OrderedCharacter character)
    {
        Debug.Log("Setting character to: " + character);
        this.character = character;
    }

    // Update is called once per frame
    void Update()
    {
        if(character == null)
        {
            return;
        }
        UpdateHPText();

        textMeshPro.transform.position = Camera.main.WorldToScreenPoint(character.transform.position) + new Vector3(0, 0.25f, 0);

        if(character.hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Public method to update the displayed HP
    public void UpdateHPText()
    {
        textMeshPro.text = character.hp.ToString();
    }
}
