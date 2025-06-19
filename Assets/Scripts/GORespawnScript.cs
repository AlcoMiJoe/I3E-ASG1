/*
* Author: Alecxander Dela Paz
* Date: 2025-06-18
* Description: Handles the respawn behavior of game objects.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GORespawnScript : MonoBehaviour
{
    public void RestartLevel() // Method to restart the current level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene by its build index
        Debug.Log("Level restarted."); // Log message for debugging purposes
    }   
}
