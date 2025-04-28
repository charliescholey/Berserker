using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public TMP_Text goldText;
    public int currentGold = 0;

    // Add gold and update the UI
    public void AddGold(int amount)
    {
        SaveFileManager.CurrentPlayerData.AddGold(amount);
        currentGold += amount;
        UpdateGoldUI();
    }

    // Updates the text display
    void UpdateGoldUI()
    {
        goldText.text = "Gold Gain: " + currentGold;
    }
}
