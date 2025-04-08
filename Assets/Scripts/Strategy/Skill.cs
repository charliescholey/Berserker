using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string identifier;
    public string skillName;
    public string description;
    public int totalUses;
    public int type; // 0 = Passive, 1 = Active
    public int effect; // 0 = None, 1 = Buff, 2 = Debuff
    public float effectChance;
    public int power;
    public int target; // 0 = Self, 1 = Ally, 2 = Enemy
    public int range;
}

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    public string identifier;
    public string skillName;
    public string description;
    public int totalUses;
    public int type;
    public int effect;
    public float effectChance;
    public int power;
    public int target;
    public int range;

    public static Skill CreateFromData(SkillData data)
    {
        Skill skill = CreateInstance<Skill>();
        skill.identifier = data.identifier;
        skill.skillName = data.skillName;
        skill.description = data.description;
        skill.totalUses = data.totalUses;
        skill.type = data.type;
        skill.effect = data.effect;
        skill.effectChance = data.effectChance;
        skill.power = data.power;
        skill.target = data.target;
        skill.range = data.range;
        return skill;
    }

    public SkillData ToData()
    {
        return new SkillData
        {
            identifier = this.identifier,
            skillName = this.skillName,
            description = this.description,
            totalUses = this.totalUses,
            type = this.type,
            effect = this.effect,
            effectChance = this.effectChance,
            power = this.power,
            target = this.target,
            range = this.range
        };
    }
}