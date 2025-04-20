using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillsManager : MonoBehaviour
{
    [Header("Passive Skills (multi‑select)")]
    public List<PassiveSkill> passiveSkillConfigs = new List<PassiveSkill>
    {
        new PassiveSkill {
            skillName   = "Sharp Edge",
            description = "Increase base attack by 5.",
            attAdd      = 5,
            attMult     = 1f,
            hpAdd       = 0,
            hpMult      = 1f
        },
        new PassiveSkill {
            skillName   = "Battle Hardened",
            description = "Gain an extra 20 HP.",
            attAdd      = 0,
            attMult     = 1f,
            hpAdd       = 20,
            hpMult      = 1f
        },
        new PassiveSkill {
            skillName   = "Stone Skin",
            description = "Increase max HP by 10%.",
            attAdd      = 0,
            attMult     = 1f,
            hpAdd       = 0,
            hpMult      = 1.10f
        },
        new PassiveSkill {
            skillName   = "Berserker's Rage",
            description = "Increase attack by 15%.",
            attAdd      = 0,
            attMult     = 1.15f,
            hpAdd       = 0,
            hpMult      = 1f
        },
        new PassiveSkill {
            skillName   = "Vital Strike",
            description = "Increase attack by 3 and attack multiplier by 5%.",
            attAdd      = 3,
            attMult     = 1.05f,
            hpAdd       = 0,
            hpMult      = 1f
        },
        new PassiveSkill {
            skillName   = "Enduring Strength",
            description = "Gain +1 attack, +10% attack multiplier, +5 HP, and +5% max HP.",
            attAdd      = 1,
            attMult     = 1.10f,
            hpAdd       = 5,
            hpMult      = 1.05f
        }
    };
    public List<Button> passiveSkillButtons;

    [Header("Active Skills (single‑select)")]
    public List<ActiveSkill> activeSkillConfigs = new List<ActiveSkill>
    {
        new ActiveSkill {
            skillName   = "Burn",
            description = "Burns the target for 5 rounds, dealing 30% of attack each round."
        },
        new ActiveSkill {
            skillName   = "Freeze",
            description = "50% accuracy: freezes the target for 2 rounds."
        },
        new ActiveSkill {
            skillName   = "Risky Hit",
            description = "50% accuracy: deals 250% of attack damage."
        },
        new ActiveSkill {
            skillName   = "Safe Hit",
            description = "100% accuracy: deals 75% of attack damage."
        }
    };
    public List<Button> activeSkillButtons;

    private Dictionary<Button, PassiveSkill> _passiveMap;
    private Dictionary<Button, ActiveSkill>  _activeMap;
    private Button                          _selectedActiveBtn;

    void Awake()
    {
        SetupTooltipUI();
    }

    void Start()
    {
        _passiveMap = new Dictionary<Button, PassiveSkill>();
        for (int i = 0; i < passiveSkillConfigs.Count; i++)
        {
            var cfg   = passiveSkillConfigs[i];
            var btn   = passiveSkillButtons[i];
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = cfg.skillName;

            _passiveMap[btn] = cfg;
            btn.onClick.AddListener(() => OnPassiveClicked(btn));

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

        _activeMap = new Dictionary<Button, ActiveSkill>();
        for (int i = 0; i < activeSkillConfigs.Count; i++)
        {
            var cfg   = activeSkillConfigs[i];
            var btn   = activeSkillButtons[i];
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = cfg.skillName;

            _activeMap[btn] = cfg;
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

        RefreshUI();
    }

    void OnPassiveClicked(Button btn)
    {
        var data  = SaveFileManager.CurrentPlayerData;
        var skill = _passiveMap[btn];

        int level      = PlayerDataUIUpdater.CalculateLevel(data.exp);
        int maxAllowed = Mathf.Max(0, level - 1);
        bool hasSkill  = data.passiveSkills.Exists(s => s.skillName == skill.skillName);

        if (!hasSkill && data.passiveSkills.Count >= maxAllowed)
        {
            Debug.LogWarning($"Can only select up to {maxAllowed} passive skills at level {level}.");
            return;
        }

        if (data.passiveSkills.Exists(s => s.skillName == skill.skillName))
        {
            data.passiveSkills.RemoveAll(s => s.skillName == skill.skillName);
            Highlight(btn, false);
        }
        else
        {
            data.passiveSkills.Add(skill);
            Highlight(btn, true);
        }
        Persist();
        TooltipUI.Instance.Hide(); 
    }

    void OnActiveClicked(Button btn)
    {
        var data  = SaveFileManager.CurrentPlayerData;
        var skill = _activeMap[btn];

        if (_selectedActiveBtn != null)
            Highlight(_selectedActiveBtn, false);

        data.activeSkill   = skill;
        Highlight(btn,      true);
        _selectedActiveBtn = btn;
        Persist();
        TooltipUI.Instance.Hide();
    }

    void RefreshUI()
    {
        var data = SaveFileManager.CurrentPlayerData;

        foreach (var kv in _passiveMap)
            Highlight(kv.Key, data.passiveSkills.Exists(s => s.skillName == kv.Value.skillName));

        foreach (var kv in _activeMap)
        {
            bool sel = kv.Value.skillName == data.activeSkill.skillName;
            Highlight(kv.Key, sel);
            if (sel) _selectedActiveBtn = kv.Key;
        }
    }

    void Highlight(Button btn, bool on)
    {
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = on ? Color.gray : Color.white;
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
        rectTransform.anchoredPosition = localPoint + new Vector2(450, 350);
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
