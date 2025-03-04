using UnityEngine;

/**
* GameManager class -- handles gameplay logic.
*/

public class GameManager : MonoBehaviour
{
    //tracks whether the player has an object selected
    private bool hasSelected = false;
    //link to boardmanager
    public BoardManager boardManager;
    //tracks active player
    private PlayerController player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currently empty
    }

    // Update is called once per frame
    void Update()
    {
        //get where the mouse is in the world
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)){
            if(hasSelected){
                //if a player is selected, move them to the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player.moveToCell(cell);
                hasSelected = false;
                player.setSelected(false);
                player = null;
            }else{
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player = boardManager.detectSelected(cell);
                if(player != null){
                    hasSelected = true;
                    player.setSelected(true);
                }
            }
        }

        if(Input.GetMouseButtonDown(1)){
            //if the right mouse button is clicked, deselect the player
            hasSelected = false;
            player.setSelected(false);
        }

    }
}
