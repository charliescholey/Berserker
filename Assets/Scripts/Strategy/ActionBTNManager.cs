using UnityEngine;

public class ActionBTNManager : MonoBehaviour
{
    public ActionBTNController actionBTNPrefab;
    private ActionBTNController[] actionButtons = new ActionBTNController[4];

    public void Create(CharacterController player, Canvas UI) {
        float buttonOffest = 0f;
        int i = 0;
        foreach(Action playerAction in player.GetActions()) {
            actionButtons[i] = Instantiate(actionBTNPrefab);
            actionButtons[i].transform.SetParent(UI.transform);
            actionButtons[i].setAction(playerAction, player);
            actionButtons[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(buttonOffest, 0);
            buttonOffest += actionBTNPrefab.GetComponent<RectTransform>().rect.width * 1.1f;
            i += 1;
        }
    }

    public void destroy()
    {
        foreach(ActionBTNController actionBTNController in actionButtons) {
            if(actionBTNController != null){
                actionBTNController.destroy();
            }
        }
    }
}
