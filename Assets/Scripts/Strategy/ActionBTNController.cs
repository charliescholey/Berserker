using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ActionBTNController : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public Action action;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(clicked);
    }

    public void setAction(Action action, PlayerController player)
    {
        this.action = action;
        this.player = player;
        this.textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = action.name;
    }

    void clicked()
    {
        player.takeAction(action);
    }

    public void destroy()
    {
        Destroy(gameObject, 0.2f);
    }
}
