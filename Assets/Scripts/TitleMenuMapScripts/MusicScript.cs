using Unity.Burst.Intrinsics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour
{
    
    [SerializeField] AudioSource buttonAudio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        updateButtonClicks();
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        updateButtonClicks();
    }
    public void buttonPress() {
        buttonAudio.Play();
    }

    void updateButtonClicks() {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None); // parameter makes it include inactive UI elements with buttons
        foreach (Button b in buttons) {
            b.onClick.AddListener(buttonPress);
        }
    }

    public void updateMasterVolume(Slider slider){
        AudioListener.volume = slider.value;
    }

}
