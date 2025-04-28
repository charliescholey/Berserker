using UnityEngine;

/*
Opens the Main Menu when the player clicks enter
*/
public class TitleScreenManager : MonoBehaviour
{
    public GameObject mainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1100, 620, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return)) {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
