using UnityEngine;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public TMP_Text expText;
    private int currentExp = 0;

    // Add exp and update the UI
    public void AddExp(int amount)
    {
        currentExp += amount;
        UpdateExpUI();
    }

    // Updates the text display
    void UpdateExpUI()
    {
        expText.text = "Exp Gain: " + currentExp;
    }
}
