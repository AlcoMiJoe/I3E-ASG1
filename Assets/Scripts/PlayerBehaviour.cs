using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Importing SceneManagement for scene handling

// PlayerBehaviour is responsible for the player's actions, health, score, and interactions with the game world
// It handles player health, score, interactions with doors, collectibles, and breakable objects
// It also manages player death and respawn functionality

public class PlayerBehaviour : MonoBehaviour
{
    int maxHealth = 100; // Maximum health of the player
    int currentHealth = 100; // Current health of the player
    bool canInteract = false; // Flag to check if the player can interact with objects
    bool isInSewage = false;
    public int currentScore = 0;
    float sewageTimer = 0f;
    BreakableBehaviour lookingAtBreakable = null;

    OrbBehaviour previousOrb = null;
    BreakableBehaviour previousBreakable = null;


    public bool HasAxe = false; // Property to check if the player has an axe
    bool isDead = false; // Property to check if the player is dead

    [SerializeField] // Serialize field to allow adjustment in the Unity Inspector
    public int sewageDamagePerSecond = 5;

    [SerializeField]
    float interactionDistance = 5f;
    [SerializeField]
    Transform interactionPoint; // Point from which the player interacts with objects
    
    [SerializeField]
    AudioClip damageSound; // Sound to play when interacting with objects
    [SerializeField]
    AudioClip healingSound; // Sound to play when interacting with objects
    [SerializeField]
    TextMeshProUGUI scoreText; // UI Text to display player's health
    [SerializeField]
    TextMeshProUGUI interactionPrompt; 

    [SerializeField]
    Slider healthSlider; // UI Slider to display player's health
    [SerializeField]
    GameObject gameOverPanel; // Panel to display when the player dies
    [SerializeField]
    Button respawnButton; // Button to respawn the player after death


    DoorBehaviour currentDoor = null;
    LeverBehaviour currentLever = null; // Reference to the currently detected lever
    OrbBehaviour currentOrb = null; // Reference to the currently detected coin
    AxeBehaviour axe = null;

    public void ModifyHealth(int amount)
    {
        if (amount < 0)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        else if (amount > 0)
        {
            AudioSource.PlayClipAtPoint(healingSound, transform.position);
        }

        currentHealth += amount; // Modify the player's health by the specified amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health does not exceed max or go below zero
        healthSlider.value = currentHealth; // Update the health slider to reflect the current health
        Debug.Log("Player health modified. Current health: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true; // Set the player as dead
            Time.timeScale = 0f; // Pause the game
            gameOverPanel.SetActive(true); // Show the game over panel
            interactionPrompt.gameObject.SetActive(false); // Hide the interaction prompt
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Player has died!"); // Log player death
        }
    }

    public void ModifyScore(int amount)
    {
        // This method can be used to modify the player's score
        // Implement score logic here if needed
        Debug.Log("Score modified by: " + amount);
        currentScore += amount; // Modify the player's score by the specified amount
        Debug.Log("Current score: " + currentScore); // Log the current score
        scoreText.text = "Score: " + currentScore.ToString(); // Update the score text in the UI
    }



    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger with: " + other.gameObject.name); // Log the object the player collided with
        if (other.CompareTag("Door"))
        {
            canInteract = true; // Allow interaction with the door
            currentDoor = other.GetComponent<DoorBehaviour>(); // Get the DoorBehaviour component from the door
        }
        else if (other.CompareTag("Lever"))
        {
            canInteract = true; // Allow interaction with the lever
            currentLever = other.GetComponent<LeverBehaviour>(); // Get the LeverBehaviour component from the lever
            interactionPrompt.text = "E to interact with lever"; // Update interaction prompt text
            interactionPrompt.gameObject.SetActive(true);
        }
        else if (other.CompareTag("SewageHazard"))
        {
            isInSewage = true; // Player is in sewage hazard
            Debug.Log("Player entered sewage hazard");
        }
        else if (other.CompareTag("Axe"))
        {
            axe = other.GetComponentInParent<AxeBehaviour>();
            if (!HasAxe)
            {
                Debug.Log("Collecting axe...");
                axe.CollectAxe(this);
            }
            else
            {
                Debug.Log("AxeBehaviour not found or already has axe.");
            }
        }
        else if (other.CompareTag("Collectible"))
        {
            // Set the canInteract flag to true
            // Get the CoinBehaviour component from the detected object
            canInteract = true;
            currentOrb = other.GetComponent<OrbBehaviour>();
            currentOrb.HighlightOrb(); // Highlight the coin to indicate it can be collected
        }
    }

    void OnInteract()
    {
        Debug.Log("Player interaction initiated"); // Log interaction initiation
        if (canInteract)
        {
            if (currentDoor != null)
            {
                Debug.Log("Interacting with door"); // Log interaction
                currentDoor.OpenDoor(); // Call the OpenDoor method on the door
            }
            else if (currentLever != null)
            {
                Debug.Log("Interacting with lever"); // Log interaction
                currentLever.FlipLever(); // Call the FlipLever method on the lever
            }
            else if (lookingAtBreakable != null && HasAxe)
            {
                lookingAtBreakable.Break();
            }
            else if (currentOrb != null)
            {
                Debug.Log("Collecting orb");
                currentOrb.CollectOrb(this);
            }            
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))   // Check if the player exited a door trigger
        {
            Debug.Log("Player exited door trigger");
            canInteract = false; // Disable interaction with the door
            currentDoor = null; // Reset currentDoor to null when exiting the door trigger
            interactionPrompt.text = ""; // Clear interaction prompt text
            interactionPrompt.gameObject.SetActive(false); // Hide the interaction prompt when exiting the door trigger
        }
        else if (other.CompareTag("Lever")) // Check if the player exited a lever trigger
        {
            Debug.Log("Player exited lever trigger");
            canInteract = false; // Disable interaction with the lever
            currentLever = null; // Reset currentLever to null when exiting the lever trigger
            interactionPrompt.text = ""; // Clear interaction prompt text
            interactionPrompt.gameObject.SetActive(false); // Hide the interaction prompt when exiting the lever trigger
        }
        else if (other.CompareTag("SewageHazard"))  // Check if the player exited a sewage hazard trigger
        {
            Debug.Log("Player exited sewage hazard");
            isInSewage = false; // Player is no longer in sewage hazard
                                // Reset sewage timer when exiting sewage hazard
            sewageTimer = 0f;
        }
        else if (currentOrb != null)
        {
            // If the object that exited the trigger is the same as the current coin
            if (other.gameObject == currentOrb.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current coin to null
                // This prevents the player from interacting with the coin
                canInteract = false;
                currentOrb.UnhighlightOrb(); // Unhighlight the coin to indicate it can no longer be collected
                currentOrb = null;
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game..."); // Log the game restart action
        Time.timeScale = 1f; // Resume the game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
    }

    void Start()
    {
        // Initialize player health and score
        currentHealth = maxHealth; // Set current health to maximum health
        currentScore = 0; // Initialize score to zero
        Debug.Log("Player started with health: " + currentHealth + " and score: " + currentScore);

        scoreText.text = "Score: " + currentScore.ToString(); // Update the score text in the UI

        healthSlider.maxValue = maxHealth; // Set the maximum value of the health slider
        healthSlider.value = currentHealth; // Set the initial value of the health slider to current health

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        gameOverPanel.SetActive(false); // Hide the game over panel at the start
        respawnButton.onClick.AddListener(RestartGame); // Add listener to respawn button to restart the game
    }



    // Update is called once per frame
    void Update()
    {
        if (gameOverPanel.activeInHierarchy)
        {
            Cursor.visible = true; // Show the cursor when the game over panel is active
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false; // Hide the cursor when the game is active
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        }

        // Damage player if inside sewage hazard
        if (isInSewage)
        {
            sewageTimer += Time.deltaTime;

            if (sewageTimer >= 1f)
            {
                ModifyHealth(-sewageDamagePerSecond);
                sewageTimer = 0f;
                Debug.Log("Sewage damaged player. Current health: " + currentHealth);
            }
        }

        RaycastHit hitInfo;
        Debug.DrawRay(interactionPoint.position, interactionPoint.forward * interactionDistance, Color.magenta);

        // Reset current detection
        canInteract = false;

        OrbBehaviour newOrb = null;
        BreakableBehaviour newBreakable = null;

        if (Physics.Raycast(interactionPoint.position, interactionPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.CompareTag("Collectible"))
            {
                newOrb = hitObject.GetComponent<OrbBehaviour>();
                if (newOrb != null)
                {
                    if (previousOrb != null && previousOrb != newOrb)
                        previousOrb.UnhighlightOrb();
                        interactionPrompt.text = ""; // Clear interaction prompt text

                    newOrb.HighlightOrb();
                    canInteract = true;
                    interactionPrompt.text = "E to collect"; // Update interaction prompt text

                }
            }
            else if (hitObject.CompareTag("AxeBreak"))
            {
                newBreakable = hitObject.GetComponent<BreakableBehaviour>();
                if (newBreakable != null)
                {
                    if (previousBreakable != null && previousBreakable != newBreakable)
                    {
                        previousBreakable.Unhighlight();
                        interactionPrompt.text = ""; // Clear interaction prompt text
                    }

                    newBreakable.Highlight();
                    canInteract = HasAxe;

                    if (HasAxe)
                    {
                        interactionPrompt.text = "E to break"; // Update interaction prompt text
                    }
                    else
                    {
                        interactionPrompt.text = "Player: Looks like I need something to break this"; // Update interaction prompt text
                    }
                    interactionPrompt.gameObject.SetActive(true); // Ensure the interaction prompt is visible
                }
            }
        }

        // If previously highlighted orb is no longer looked at
        if (previousOrb != null && previousOrb != newOrb)
        {
            previousOrb.UnhighlightOrb();
            interactionPrompt.text = ""; // Clear interaction prompt text
        }
        if (previousBreakable != null && previousBreakable != newBreakable)
        {
            previousBreakable.Unhighlight();
            interactionPrompt.text = ""; // Clear interaction prompt text
        }

        // Update references
        currentOrb = newOrb;
        previousOrb = newOrb;

        lookingAtBreakable = newBreakable;
        previousBreakable = newBreakable;

    }
}
