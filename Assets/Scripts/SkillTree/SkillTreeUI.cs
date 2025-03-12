using UnityEngine;
using UnityEngine.UI;

/*
Manages the Skill Tree
Currently, updates the skills when a skill is purchased to unlock dependent skills

Eventually, the Skill Tree will be integrated with the player's actions.
*/
public class SkillTreeUI : MonoBehaviour
{
    public static SkillTreeUI Instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSkills() {
        foreach(Transform child in transform) {
            SkillButton childSkillButton = child.GetComponent<SkillButton>();
            bool lockSkill = false;
            foreach(SkillButton prereq in childSkillButton.prereqSkills) {
                if(! prereq.purchased) lockSkill = true;
            }
            childSkillButton.locked = lockSkill;
            childSkillButton.UpdateButton();
        }
    }
}
