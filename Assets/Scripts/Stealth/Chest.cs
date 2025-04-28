using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Chest : MonoBehaviour
{
    public int goldValue = 10;

    public Sprite openedChestSprite;

    private GoldManager goldManager;
    private ExpManager expManager;

    private bool isOpened = false;

    public static int openedChestCount = 0;

    void Start()
    {
        goldManager = FindAnyObjectByType<GoldManager>();
        expManager = FindAnyObjectByType<ExpManager>();
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
            openedChestCount += 1;
        }
    }
}
