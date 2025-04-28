using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;

    void Start()
    {
        goldText.text = SaveFileManager.CurrentPlayerData.GetGold().ToString();
    }


    void Update()
    {
        goldText.text = SaveFileManager.CurrentPlayerData.GetGold().ToString();
    }
}
