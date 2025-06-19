/*
* Author: Alecxander Dela Paz
* Date: 2025-06-19
* Description: Manages background music playback and transitions.
*/

using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public static BGMScript Instance; // Singleton instance of BGMScript

    [Header("Level 1 Sources")]
    [SerializeField]
    private GameObject level1MusicSource; // Audio source for Level 1 background music
    [SerializeField]
    private GameObject level1AmbienceSource; // Audio source for Level 1 ambient sounds

    [Header("Level 2 Sources")]
    [SerializeField]
    private GameObject level2AmbienceSource;

    public bool IsInLevel2 = false; // Flag to check if the player is in Level 2

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes (optional)
    }


    /// <summary>
    /// Activates Level 1 audio objects and disables Level 2 audio.
    /// </summary>
    public void SwitchToLevel1()
    {
        if (IsInLevel2)
        {
            level2AmbienceSource.SetActive(false); // Disable Level 2 ambience
            level1AmbienceSource.SetActive(true); // Enable Level 1 ambience
            level1MusicSource.SetActive(true); // Enable Level 1 music
            IsInLevel2 = false; // Update the flag to indicate Level 1
            Debug.Log("Switched to Level 1 audio sources.");
        }
    }

    /// <summary>
    /// Activates Level 2 audio objects and disables Level 1 audio.
    /// </summary>
    public void SwitchToLevel2()
    {
        if (!IsInLevel2)
        {
            level1AmbienceSource.SetActive(false); // Disable Level 1 ambience
            level1MusicSource.SetActive(false); // Disable Level 1 music
            level2AmbienceSource.SetActive(true); // Enable Level 2 ambience
            IsInLevel2 = true; // Update the flag to indicate Level 2
            Debug.Log("Switched to Level 2 audio sources.");
        }
    }
}
