using UnityEngine;

public class Chest : MonoBehaviour
{
    public int goldValue = 10;

    public Sprite openedChestSprite;

    private GoldManager goldManager;

    private bool isOpened = false;

    void Start()
    {
        goldManager = FindAnyObjectByType<GoldManager>();
    }

    // When the player enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened)
        {
            goldManager.AddGold(goldValue);
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (openedChestSprite != null && sr != null)
            {
                sr.sprite = openedChestSprite;
            }
            GetComponent<Collider2D>().enabled = false;
            isOpened = true;
        }
    }
}
