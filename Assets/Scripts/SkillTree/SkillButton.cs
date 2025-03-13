using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

/*
Skill Buttons in skill tree
Currently, the root skill starts as being available.
Skills are made available when all of their prequisite skills are purchased.
At the moment, an available skill can be clicked to be "purchased".

Eventually, the Skill Tree will be integrated with the player's actions and with the currency system.
*/
public class SkillButton : MonoBehaviour
{
    /*
    Makes the skill icons inaccessible when the skill is locked
    */
    [SerializeField] public bool locked;
    [SerializeField] public bool purchased;
    [SerializeField] Image buttonColour;
    [SerializeField] Image skillIcon;
    [SerializeField] TMP_Text skillName;
    [SerializeField] public SkillButton[] prereqSkills;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateButton();
        this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { ClickSkill(); });
    }

    void ClickSkill() {
        if(! locked && ! purchased) {
            purchased = true;
        }
        UpdateButton();
        SkillTreeUI.Instance.updateSkills();
    }

    public void UpdateButton() {
        if(locked) {
            buttonColour.color = Color.gray;
            skillIcon.color = Color.clear;
            this.gameObject.GetComponent<EventTrigger>().enabled = false;
        } else if(! purchased) {
            buttonColour.color = Color.white;
            skillIcon.color = Color.gray;
            this.gameObject.GetComponent<EventTrigger>().enabled = true;
        } else {
            buttonColour.color = Color.white;
            skillIcon.color = new Color32(242, 6, 37, 255);
            this.gameObject.GetComponent<EventTrigger>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
