using UnityEngine;
using UnityEngine.UI;  // You need this for Button

public class StoreButtonHandler : MonoBehaviour
{
    [Range(0, 3)]   // Capital R
    public int buttonIndex;

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);  // Link the function properly
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    void OnClick()
    {
        if (StoreUI.Instance == null)
        {
            Debug.LogError("StoreUI.Instance is null!");
            return;
        }

        switch (buttonIndex)
        {
            case 0:
                StoreUI.Instance.PurchaseHealth();
                break;
            case 1:
                StoreUI.Instance.PurchaseAttack();
                break;
            case 2:
                StoreUI.Instance.PurchaseDefense();
                break;
            case 3:
                StoreUI.Instance.PurchaseMovement();
                break;
            default:
                Debug.LogWarning("Invalid buttonIndex: " + buttonIndex);
                break;
        }
    }
}
