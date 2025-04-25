using Unity.Burst.Intrinsics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System;
using UnityEngine.Audio;
using System.Linq;

/*
Manages the music and sound effects for the game
*/

/*
To add a sound effect to the game:
In the inspector, under the Audio game object, add a sound to the array of sounds.
Give it a name, drag in its audio clip from assets, and select sound effect.
Use the playSoundByName function to play the sound in the game.
*/

public class MusicScript : MonoBehaviour
{
    public static MusicScript Instance;

    [SerializeField] Sound[] sounds;
    [SerializeField] AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] AudioMixerGroup musicMixerGroup;

    // This is an array of all the scene names where all the buttons will make a "tick" sound when clicked
    string[] scenesWithButtonClicks = {"MainMenu", "WorldMap"};
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Allow this object to persist across scenes

        // Sounds are added in the Inspector with their name, audio clip, and type (music, sound effect)
        // Loop through all of these sounds and create audio sources for them
        foreach(Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            // If a sound is music, make it play when the game starts and on a loop
            // If a sound is a sound effect, make it not play when the game starts and not play on a loop
            // Set the audio mixing group depending on the type of sound - this will allow us to change the volume 
            // of all sounds of a certain type at the same time.
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

            // Start to play any sounds that are meant to play when the game starts
            if(s.audioSource.playOnAwake) {
                s.audioSource.Play();
            }

        }

        //onMasterVolumeChange(PlayerPrefs.GetFloat("Master Volume", 1));
        //onMusicVolumeChange(PlayerPrefs.GetFloat("Music Volume", 1));
        //onSoundEffectsVolumeChange(PlayerPrefs.GetFloat("Sound Effects Volume", 1));
        
        updateButtonClicks();
        SceneManager.sceneLoaded += OnSceneLoaded; // Makes OnSceneLoaded method called whenever a new scene is loaded
    }
    
    // Plays a sound given its name
    public void playSoundByName(string soundName) {
        Sound sound = Array.Find(sounds, s => s.soundName == soundName);
        sound.audioSource.Play();
    }

    // This method is triggered when a new scene is loaded
    // If the loaded scene is one where all the button should make a "tick" sound, it calls updateButtonClicks
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scenesWithButtonClicks.Contains(scene.name)) {
            updateButtonClicks(); // Updates button click event listeners
        }
    }

    // Finds all buttons in the scene and adds the buttonPress method as a listener to their onClick event
    // Clicking any button will make the button sound play
    public void updateButtonClicks() {
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
        //PlayerPrefs.SetFloat("Music Volume", value);
    }

    public void onSoundEffectsVolumeChange(float value) {
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(value)*20);
        //PlayerPrefs.SetFloat("Sound Effects Volume", value);
    }

    public void onMasterVolumeChange(float value) {
        musicMixerGroup.audioMixer.SetFloat("Master Volume", Mathf.Log10(value)*20);
        //PlayerPrefs.SetFloat("Master Volume", value);
    }

}
