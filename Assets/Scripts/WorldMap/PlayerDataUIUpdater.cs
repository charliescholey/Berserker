using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDataUIUpdater : MonoBehaviour
{
    public TMP_Text xpText;
    public TMP_Text moneyText;
    
    public static int CalculateLevel(int totalExp)
    {
        int level = 1;
        int expToNextLevel = 30;

        while (totalExp >= expToNextLevel)
        {
            totalExp -= expToNextLevel;
            level++;
            expToNextLevel += 15;
        }

        return level;
    }
    void Start()
    {
        if (SaveFileManager.CurrentPlayerData != null)
        {
            xpText.text = " Level: " + CalculateLevel(SaveFileManager.CurrentPlayerData.exp).ToString();
            moneyText.text = SaveFileManager.CurrentPlayerData.gold.ToString();
        }
        else
        {
            Debug.LogWarning("CurrentPlayerData is null. Ensure a game is resumed or started.");
        }
    }
}