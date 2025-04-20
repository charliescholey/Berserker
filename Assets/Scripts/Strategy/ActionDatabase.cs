using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Actions/ActionDatabase")]

public class ActionDatabase : ScriptableObject
{
    [SerializeField]
    private List<Action> actions = new List<Action>();

    public Action GetActionByIndex(int index)
    {
        if (index >= 0 && index < actions.Count)
            return actions[index];
        return null;
    }

    public int GetIndexOfAction(Action action)
    {
        return actions.IndexOf(action);
    }

    public List<Action> GetAllActions()
    {
        return actions;
    }
}