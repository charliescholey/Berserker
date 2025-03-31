using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ActionData
{
    public string identifier;
    public string actionName;
    public string description;
    public int totalUses;
    public Action.MoveType type;
    public Action.EffectType effect;
    public float effectChance;
    public int power;
    public Action.TargetType target;
    public int range;
}

[System.Serializable]
public class ActionDatabaseData
{
    public List<ActionData> actions = new List<ActionData>();
}

[System.Serializable]
[CreateAssetMenu(fileName = "Action", menuName = "Actions/Action")]
public class Action : ScriptableObject
{
    //track the type of move - what does the move do?
    public enum MoveType
    {
        MELEE,
        RANGED,
        HEAL,
        MOVERAD,
        EXTRATURN,
        PPUP
    }
    //if the move has an effect, what is it?
    public enum EffectType
    {
        NONE,
        BURN,
        POISON,
        PARALYSIS,
        SLEEP
    }
    //who does the move target?
    public enum TargetType
    {
        SELF,
        RADIUS,
        TARGETDIST,
        LINE
    }

    public string identifier; // Unique identifier for the action
    public string actionName; // Name of the action
    public string description; // Description of the action
    [Range(0, 25)]
    public int totalUses; // Number of times the action can be used
    public MoveType type; // Type of move
    public EffectType effect; // Effect of the move
    [Range(0.1f, 1f)]
    public float effectChance; // Chance of the effect occurring
    [Range(0, 100)]
    public int power; // Power of the move
    public TargetType target; // Target of the move
    [Range(0, 6)]
    public int range;   // Range of the move

    public static Action CreateFromData(ActionData data)
    {
        Action action = ScriptableObject.CreateInstance<Action>();
        action.identifier = data.identifier;
        action.actionName = data.actionName;
        action.description = data.description;
        action.totalUses = data.totalUses;
        action.type = data.type;
        action.effect = data.effect;
        action.effectChance = data.effectChance;
        action.power = data.power;
        action.target = data.target;
        action.range = data.range;
        return action;
    }

    public ActionData ToData()
    {
        return new ActionData
        {
            identifier = this.identifier,
            actionName = this.actionName,
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
