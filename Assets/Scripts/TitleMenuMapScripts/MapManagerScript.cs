using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;

public class MapManagerScript : MonoBehaviour
{
    /*
    Displays an information box about a selected level
    */
    public static MapManagerScript Instance;
    public GameObject infoBox;
    public Button playButton;
    public GameObject star;
    public TextMeshProUGUI levelNameText, objectivesText;
    Boolean clicked = false;
    
    void Start()
    {
        Instance = this;
        infoBox.SetActive(false);
    }

    void Update()
    {
        // If the mouse is clicked and the info box has not been displayed this frame, close the info box
        if (clicked == false && Input.GetMouseButtonUp(0) && infoBox.activeSelf) {
            hideInfo(null);
        }
        clicked = false;
    }

    /*
    Displays the information about the selected level in the info box
    Or, if it is already displayed, it hides the information
    */
    public void levelClicked(LevelDescription ld) {
        clicked = true;

        if(levelNameText.text == ld.levelName && infoBox.activeSelf) {
            hideInfo(ld);
            return;
        }
        
        // Updates the information in the info box
        levelNameText.text = ld.levelName;
        objectivesText.text = "";
        foreach(string o in ld.objectives) {
            objectivesText.text = objectivesText.text + o + System.Environment.NewLine;
        }
        infoBox.SetActive(true);
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => SceneManager.LoadScene(ld.sceneToLoad));
        /*
        switch(ld.place) {
            case LevelDescription.type.Base:
                GameObject s = Instantiate(star, ld.transform.position + new Vector3(0, -40), Quaternion.identity, infoBox.transform);  
                break;
            case LevelDescription.type.Village:
                break;
            case LevelDescription.type.City:
                break;
            case LevelDescription.type.Capital:
                break;
        }
        */

        // Move the info box below the clicked icon
        Vector3 iconPosition = ld.transform.position;
        Vector3 newPosition = iconPosition + new Vector3(0, -Screen.height * 0.1f, 0);
        infoBox.transform.position = newPosition;

        infoBox.SetActive(true);
    }

    public void hideInfo(LevelDescription ld) {
        infoBox.SetActive(false);
    }
}