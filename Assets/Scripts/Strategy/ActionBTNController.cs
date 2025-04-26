using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/*
Represents a single action button
*/

public class ActionBTNController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshPro;
    public Action action;
    public CharacterController player;
    public TileHighlighter th;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnAttackButtonClicked);
    }

    public void setAction(Action action, CharacterController player, TileHighlighter th)
    {
        this.th = th;
        this.action = action;
        this.player = player;
        this.textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = action.name;
    }

     // When the action button is clicked, executes the action through the game manager
    public void OnAttackButtonClicked()
    {
        GameManager gm = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        if(gm != null && player != null && action != null)
        {
            player.processAction(action);
        }
        // Optionally destroy or hide the button after use.
        destroy();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        th.ClearHighlights();
        th.HighlightTiles(player.processActionRange(action), true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        th.ClearHighlights();
        th.HighlightTiles(player.getMovementRange());
    }


    public void destroy()
    {
        // Hide the button (by hiding its image and text)
        GetComponent<Button>().image.color = Color.clear;
        gameObject.transform.Find("ActionText").gameObject.SetActive(false);

        // Destroy button after 0.5 sec
        Destroy(gameObject, 0.5f);
    }
}
