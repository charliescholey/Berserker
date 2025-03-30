using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialAbilityBTNController : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public SpecialAbility specialAbility;
    public CharacterController player;  

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnSpecialAbilityClicked);
    }

    public void SetAbility(SpecialAbility ability, CharacterController player)
    {
        this.specialAbility = ability;
        this.player = player;

        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        // Display the name of the special ability on the button
        textMeshPro.text = ability != null ? ability.abilityName : "Special Ability";
    }

    private void OnSpecialAbilityClicked()
    {
        if (player != null && specialAbility != null)
        {
            // This calls the special ability’s activation method
            specialAbility.ActivateAbility(player.gameObject);
        }
        Destroy(gameObject, 0.2f);
    }

    public void destroy()
    {
        Destroy(gameObject, 0.2f);
    }
}
