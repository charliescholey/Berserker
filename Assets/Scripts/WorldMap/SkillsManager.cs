using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillsManager : MonoBehaviour
{
    public List<Button> activeSkillButtons;
    public List<int> UnlockedSkills = new List<int>();
    public Dictionary<Button, int> _unlockedMap;
    public ActionDatabase actionDatabase;
    private List<Button> _selectedActionBtns = new List<Button>();
    public Button button;

    void Awake()
    {
        SetupTooltipUI();
    }

    void Start()
    {
        _unlockedMap = new Dictionary<Button, int>();
        
        List<Action> acts = new List<Action>(actionDatabase.GetAllActions());

        acts.RemoveAll(action => 
            action.actionName == "Melee" || 
            action.actionName == "Heal" || 
            action.actionName == "Pass Turn");
        
        for(int i = 1; i < acts.Count; i++) {
            // Clone the button
            Button newButton = Instantiate(button, button.transform.parent);
        
            // Get the RectTransform of the original and new button
            RectTransform originalRect = button.GetComponent<RectTransform>();
            RectTransform newRect = newButton.GetComponent<RectTransform>();
        
            // Set the new position
            Vector3 newPosition = originalRect.localPosition;
            newPosition.y += -70 * i;
            newRect.localPosition = newPosition;
            activeSkillButtons.Add(newButton);
        }
        
        for (int i = 0; i < acts.Count; i++)
        {
            var cfg   = acts[i];
            var btn = activeSkillButtons[i];
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null) {
                label.text = "<b>" + cfg.actionName + "</b>\n300";
            }
            _unlockedMap[btn] = actionDatabase.GetIndexOfAction(cfg);
            btn.onClick.AddListener(() => OnActiveClicked(btn));

            var trigger = btn.gameObject.AddComponent<EventTrigger>();
            var entryEnter = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerEnter
            };
            entryEnter.callback.AddListener((_) => TooltipUI.Instance.Show(cfg.description));
            trigger.triggers.Add(entryEnter);

            var entryExit = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerExit
            };
            entryExit.callback.AddListener((_) => TooltipUI.Instance.Hide());
            trigger.triggers.Add(entryExit);
        }
        MusicScript.Instance.updateButtonClicks();
        RefreshUI();
    }
    void OnActiveClicked(Button btn)
    {
        var data  = SaveFileManager.CurrentPlayerData;
        int skillCost = 300; // change
        if( data.exp < skillCost ) {
            return;
        }

        if(! data.UnlockedSkillIndices.Contains( _unlockedMap[btn] )) {
            data.exp -= skillCost;
            FindFirstObjectByType<PlayerDataUIUpdater>().Refresh();
            data.UnlockedSkillIndices.Add( _unlockedMap[btn] );
        }
        Highlight(btn, true);
        if(! _selectedActionBtns.Contains(btn))
            _selectedActionBtns.Add(btn);
        Persist();
        TooltipUI.Instance.Hide();
    }

    void RefreshUI()
    {
        var data = SaveFileManager.CurrentPlayerData;
        
        foreach (var kv in _unlockedMap)
        {
            bool sel = data.UnlockedSkillIndices.Contains( kv.Value );
            Highlight(kv.Key, sel);
            if (sel && ! _selectedActionBtns.Contains(kv.Key)) _selectedActionBtns.Add(kv.Key);
        }
    }

    void Highlight(Button btn, bool on)
    {
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = on ? Color.gray : new Color32(0x77, 0x04, 0x13, 0xFF);
    }

    void Persist()
    {
        string json = JsonUtility.ToJson(SaveFileManager.CurrentPlayerData, true);
        File.WriteAllText(SaveFileManager.saveFilePath, json);
    }
    
    void SetupTooltipUI()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene for TooltipUI");
            return;
        }

        var panelGO = new GameObject("TooltipPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(TooltipUI));
        panelGO.transform.SetParent(canvas.transform, false);
        var panelRT = panelGO.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0, 0);
        panelRT.anchorMax = new Vector2(0, 0);
        panelRT.pivot     = new Vector2(0, 1);
        panelRT.anchoredPosition = Vector2.zero;
        panelRT.sizeDelta = new Vector2(200, 60);
        var panelImg = panelGO.GetComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0.75f);

        var textGO = new GameObject("TooltipText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textGO.transform.SetParent(panelGO.transform, false);
        var textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.offsetMin = new Vector2(5, 5);
        textRT.offsetMax = new Vector2(-5, -5);

        var tmp = textGO.GetComponent<TextMeshProUGUI>();
        tmp.enableWordWrapping = true;
        tmp.color              = Color.white;
        tmp.fontSize           = 18;

        var tooltipUI = panelGO.GetComponent<TooltipUI>();
        tooltipUI.tooltipText = tmp;
    }
    
}

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }
    public TextMeshProUGUI tooltipText;
    RectTransform rectTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance      = this;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition, 
            null, 
            out localPoint
        );
        
        rectTransform.anchoredPosition = localPoint + new Vector2(450, 390);
    }

    public void Show(string description)
    {
        tooltipText.text = description;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
