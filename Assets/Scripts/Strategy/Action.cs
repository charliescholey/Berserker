using UnityEngine;

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
    //name of move
    public string actionName;
    //we can add a description of the move
    public string description;
    //how many times can the move be used? (like the pokemon PP system)
    [Range(0, 25)]
    public int totalUses;
    //see earlier enums
    public MoveType type;
    public EffectType effect;
    //how likely is the effect to occur?
    [Range(0.1f, 1f)]
    public float effectChance;
    //can track damage, healing ability, range boost, etc.
    [Range(0, 100)]
    public int power;
    public TargetType target;
    [Range(0, 6)]
    public int range;
}