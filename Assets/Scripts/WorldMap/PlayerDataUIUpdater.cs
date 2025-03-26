using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDataUIUpdater : MonoBehaviour
{
    public TMP_Text xpText;
    public TMP_Text moneyText;

    void Start()
    {
        if (SaveFileManager.CurrentPlayerData != null)
        {
            xpText.text = SaveFileManager.CurrentPlayerData.exp.ToString() + " EXP";
            moneyText.text = SaveFileManager.CurrentPlayerData.gold.ToString() + " $";
        }
        else
        {
            Debug.LogWarning("CurrentPlayerData is null. Ensure a game is resumed or started.");
        }
    }
}