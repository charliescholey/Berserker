using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/*
Draws the stars on the top of the world map
Eventually, this will be dependent on the player's progress which is why this is in a script
*/
public class StarScript : MonoBehaviour
{
    public Image star;
    public int num = 9;
    float offset = 40f;

    float x = -500f;
    float y = 275f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drawStars();
    }

    void drawStars() {
        for(int i = 0; i < num; i++) {
            //Instantiate(star, new Vector3(transform.position.x + offset*i, transform.position.y, 0), transform.rotation);
            //Debug.Log(transform.position.y);
            Image s = Instantiate(star, new Vector3(x + offset*i, y, 0), transform.rotation);
            //GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            s.transform.SetParent (this.transform.parent, false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

