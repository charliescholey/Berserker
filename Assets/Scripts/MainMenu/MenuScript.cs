using UnityEngine;
using UnityEngine.SceneManagement;

/*
Manages the Main Menu.

Eventually, this class will have the functionality to load games and make new games.
Currently, it just loads the World Map scene when you click Resume Game.
*/
public class MenuScript : MonoBehaviour
{
    // Loads the WorldMap scene when the player clicks Resume Game
    public void resumeGame() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("WorldMap");
    }
}
