using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Actions/ActionDatabase")]
public class ActionDatabase : ScriptableObject
{
    public List<Action> actions = new List<Action>();
}
