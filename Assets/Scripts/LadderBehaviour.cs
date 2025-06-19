/*
* Author: Alecxander Dela Paz
* Date: 2025-06-18
* Description: Handles the player's interaction with the ladder for escaping the game.
*/


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// LadderBehaviour handles the logic for escaping the game via a ladder.
/// It checks if the player has enough points to escape and displays a win panel.
/// </summary>
public class LadderBehaviour : MonoBehaviour
{
    /// <summary>
    /// Points needed to escape.
    /// </summary>
    public int requiredScore = 2000; // Set this to the required score for escaping

    [SerializeField]
    private TextMeshProUGUI scoreText; // Reference to the score text UI element

    [SerializeField]
    public GameObject escapePanel;  // Panel shown when escaping

    [SerializeField]
    private Button escapeButton; // Button to confirm escape
    [SerializeField]
    AudioClip winSound; // Sound played on escape

    private bool isEscaped = false;  // Prevent multiple activations

    /// <summary>
    /// Called when the player interacts with the ladder. Triggers escape if conditions are met.
    /// </summary>
    /// <param name="player">The player interacting with the ladder</param>
    /// <returns>True if escaped, false otherwise</returns>
    public bool TryEscape(PlayerBehaviour player)
    {
        if (isEscaped)
            return false; // Already escaped

        if (player.currentScore >= requiredScore)
        {
            Debug.Log("Player escaped using the ladder.");
            isEscaped = true;
            ShowEscapePanel(player.currentScore);
            return true;
        }
        else
        {
            return false; // Not enough points
        }
    }

    /// <summary>
    /// Displays the escape panel and updates the score.
    /// </summary>
    /// <param name="finalScore">The player's final score</param>
    private void ShowEscapePanel(int finalScore)
    {
        escapePanel.SetActive(true);
        scoreText.text = "Score: " + finalScore;

        // Unlock and show the cursor so player can click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play the win sound
        AudioSource.PlayClipAtPoint(winSound, transform.position);

        // Ensure the game is paused
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Restarts the game by reloading the current scene.
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Sets up the ladder and binds the escape button.
    /// </summary>
    void Start()
    {
        escapePanel.SetActive(false); // Hide escape panel at start
        escapeButton.onClick.AddListener(RestartGame); // Add listener to escape button

        // Hide cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Optional: Handles cursor visibility dynamically. 
    /// (You can disable this if cursor visibility is now managed correctly during escape)
    /// </summary>
    void Update()
    {
        if (escapePanel.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
