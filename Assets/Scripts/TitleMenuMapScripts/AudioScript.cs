using Unity.Burst.Intrinsics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System;
using UnityEngine.Audio;

public class MusicScript : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    [SerializeField] AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Allow this object to persist across scenes
        foreach(Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            switch(s.type) {
                case Sound.AudioType.music:
                    s.audioSource.loop = true;
                    s.audioSource.playOnAwake = true;
                    s.audioSource.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case Sound.AudioType.soundEffect:
                    s.audioSource.loop = false;
                    s.audioSource.playOnAwake = false;
                    s.audioSource.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
            }
            if(s.audioSource.playOnAwake) {
                s.audioSource.Play();
            }

        }
        updateButtonClicks();
        SceneManager.sceneLoaded += OnSceneLoaded; // Makes OnSceneLoaded method called whenever a new scene is loaded
    }
    
    public void playSoundByName(string soundName) {
        Debug.Log("playSoundByName");

        Sound sound = Array.Find(sounds, s => s.soundName == soundName);
        sound.audioSource.Play();
    }

    // This method is triggered when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        updateButtonClicks(); // Updates button click event listeners
    }

    // Finds all buttons in the scene and adds the buttonPress method as a listener to their onClick event
    // Clicking any button will make the button sound play
    void updateButtonClicks() {
        // Finds all the buttons in the scene
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button b in buttons) {
            // Attach the buttonPress method to the button's onClick event
            b.onClick.AddListener(delegate { playSoundByName("Button"); } );
        }
        
    }

    // These functions are called by the volume sliders on value changed to update the relevant volume
    public void onMusicVolumeChange(float value) {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(value)*20);
    }

    public void onSoundEffectsVolumeChange(float value) {
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(value)*20);
    }

    public void onMasterVolumeChange(float value) {
        musicMixerGroup.audioMixer.SetFloat("Master Volume", Mathf.Log10(value)*20);
    }

}
