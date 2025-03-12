using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioType { soundEffect, music }
    public string soundName;
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] public AudioClip audioClip;
    public AudioType type; 
    [Range(0, 1)] public float volume = 1f;
}
