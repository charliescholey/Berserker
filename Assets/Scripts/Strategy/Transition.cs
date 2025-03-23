using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Transition : MonoBehaviour
{
    public void onKill(){
        Debug.Log("enemy killed");
        StealthManager.Instance.UnloadStrategyAndEnablePlayer();
    }
    public void OnDeath()
    {
        Debug.Log("died");
        Destroy(StealthManager.Instance);
        SceneManager.LoadScene("DemoLevel", LoadSceneMode.Single);
    }
}
