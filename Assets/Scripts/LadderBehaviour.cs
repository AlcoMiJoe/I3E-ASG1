using UnityEngine;
using System.Collections;
using TMPro;

public class LadderBehaviour : MonoBehaviour
{
    public int requiredScore = 2000; // Points needed to escape
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject escapePanel;  // Panel shown when escaping

    private bool isEscaped = false;  // Prevent multiple activations

    // Called when the player interacts with the ladder
    public bool TryEscape(PlayerBehaviour player)
    {
        if (isEscaped)
            return false; // Already escaped

        if (player.currentScore >= requiredScore)
        {
            Debug.Log("Player escaped using the ladder.");
            escapePanel.SetActive(true);
            scoreText.text = "Score: " + player.currentScore; // Update score display
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isEscaped = true;
            return true;
        }
        else
        {
            return false; // Not enough points
        }
    }
}