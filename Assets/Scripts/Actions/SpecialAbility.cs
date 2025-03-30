using UnityEngine;

[CreateAssetMenu(fileName = "NewSpecialAbility", menuName = "Actions/Special Ability")]
public abstract class SpecialAbility : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string description;
    public int power;
    public float cooldown;

    // Abstract method to define what happens when the ability is activated.
    public abstract void ActivateAbility(GameObject user);
}
