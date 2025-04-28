using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldText.text = SaveFileManager.CurrentPlayerData.GetGold().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = SaveFileManager.CurrentPlayerData.GetGold().ToString();
    }
}
