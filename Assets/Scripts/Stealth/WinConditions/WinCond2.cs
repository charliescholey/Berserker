using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinCond2 : MonoBehaviour
{
    [SerializeField]
    public GoldManager tracked;

    // Update is called once per frame
    void Update()
    {
        if(tracked.currentGold >= 500){
            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine()
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);

        if (SceneManager.GetActiveScene().name != "Map2")
            {
                //Debug.LogWarning(SceneManager.GetActiveScene().name + " is not the Map1 scene");
            }else{
                Debug.Log("Win condition met");
                StealthManager.Instance.ReturnToWorldMap();
            }
            
    }

}
