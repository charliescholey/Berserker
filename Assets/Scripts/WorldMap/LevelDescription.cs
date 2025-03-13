using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.UI;
public class LevelDescription : MonoBehaviour
{
    /*
    Keeps track of the level information association with each icon on the map.
    - levelName - name of the level
    - place - whether it is a base, village, city, or the capital
    - objectives - an array that holds the objectives of the level
    - sceneToLoad - the name of the scene for that level
    */
    [SerializeField] public string levelName;
    [SerializeField] public type place;
    [SerializeField] public string[] objectives;
    [SerializeField] public string sceneToLoad;

    public Sprite[] spriteArray;
    [SerializeField] public UnityEngine.UI.Image img;
    public enum type {
        Base = 0, Village = 1, City = 2, Capital = 3
    };
    void Start()
    {
        img.sprite = spriteArray[(int) place];
    }
    // Calls the MapManager to indicate that this icon has been clicked
    public void onClick()
    {
        MapManagerScript.Instance.levelClicked(this);
    }
}
