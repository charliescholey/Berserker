using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ActionBTNController : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public Action action;
    public CharacterController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnAttackButtonClicked);
    }

    public void setAction(Action action, CharacterController player)
    {
        this.action = action;
        this.player = player;
        this.textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = action.name;
    }

     //Changed clicked to this 
    public void OnAttackButtonClicked()
    {
        GameManager gm = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        if(gm != null && player != null && action != null)
        {
            gm.ExecuteAttack(player, action);
        }
        // Optionally destroy or hide the button after use.
        destroy();
    }

    public void destroy()
    {
        Destroy(gameObject, 0.2f);
    }
}
