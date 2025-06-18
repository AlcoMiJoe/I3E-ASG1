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

    bool canTriggerInteract = false; // NEW: Flag to check if player can interact via trigger
    bool canRaycastInteract = false; // NEW: Flag to check if player can interact via raycast

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
    LadderBehaviour currentLadder = null; // Reference to the ladder escape behaviour

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
        if (other.CompareTag("SewageHazard"))
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
            canTriggerInteract = true; // NEW: Trigger-based collectible interaction
            currentOrb = other.GetComponent<OrbBehaviour>();
            currentOrb.HighlightOrb();
        }
    }

    void OnInteract()
    {
        Debug.Log("Player interaction initiated"); // Log interaction initiation
        if (canTriggerInteract || canRaycastInteract) // NEW: Use combined flag
        {
            if (currentDoor != null)
            {
                Debug.Log("Interacting with door");
                currentDoor.OpenDoor();
            }

            else if (currentLever != null)
            {
                Debug.Log("Attempting to interact with lever");
                bool flipped = currentLever.TryFlipLever(this);
                if (!flipped)
                {
                    interactionPrompt.text = "Lever inactive – need at least 1000 points.";
                    interactionPrompt.gameObject.SetActive(true);
                }
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
            else if (currentLadder != null)
                {
                    Debug.Log("Attempting to escape via ladder");
                    bool escaped = currentLadder.TryEscape(this);
                    if (!escaped)
                    {
                        interactionPrompt.text = "Ladder locked – need 2000+ points to escape.";
                        interactionPrompt.gameObject.SetActive(true);
                    }
                }

        }
        else
        {
            Debug.Log("Cannot interact with the current object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SewageHazard"))
        {
            Debug.Log("Player exited sewage hazard");
            isInSewage = false;
            sewageTimer = 0f;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentScore = 0;
        Debug.Log("Player started with health: " + currentHealth + " and score: " + currentScore);

        scoreText.text = "Score: " + currentScore.ToString();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameOverPanel.SetActive(false);
        respawnButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (gameOverPanel.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

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

        // Reset raycast interaction detection
        canRaycastInteract = false; // NEW: Only reset raycast, not trigger interaction

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
                    interactionPrompt.text = "";

                    newOrb.HighlightOrb();
                    canRaycastInteract = true; // NEW
                    interactionPrompt.text = "E to collect";
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
                        interactionPrompt.text = "";
                    }

                    newBreakable.Highlight();
                    canRaycastInteract = HasAxe; // NEW

                    if (HasAxe)
                    {
                        interactionPrompt.text = "E to break";
                    }
                    else
                    {
                        interactionPrompt.text = "Player: Looks like I need something to break this";
                    }

                    interactionPrompt.gameObject.SetActive(true);
                }
            }
            else if (hitObject.CompareTag("Door"))
            {
                currentDoor = hitObject.GetComponent<DoorBehaviour>();
                if (currentDoor != null)
                {
                    canRaycastInteract = true; // NEW
                    interactionPrompt.text = "E to interact with door";
                    interactionPrompt.gameObject.SetActive(true);
                }
            }
            else if (hitObject.CompareTag("Lever"))
            {
                currentLever = hitObject.GetComponent<LeverBehaviour>();
                if (currentLever != null)
                {
                    if (currentScore >= currentLever.targetScore)
                    {
                        canRaycastInteract = true;
                        interactionPrompt.text = "E to flip lever";
                    }
                    else
                    {
                        canRaycastInteract = false; // Player sees the prompt but can't interact
                        interactionPrompt.text = "Lever locked – need 1000+ points";
                    }
                }
            }
            else if (hitObject.CompareTag("Ladder"))
            {
                currentLadder = hitObject.GetComponent<LadderBehaviour>();
                if (currentLadder == null)
                {
                    Debug.LogWarning("LadderBehaviour component not found on the ladder object.");
                }
                if (currentLadder != null)
                {
                    canRaycastInteract = true;
                    if (currentScore >= currentLadder.requiredScore)
                    {
                        interactionPrompt.text = "E to escape via ladder";
                    }
                    else
                    {
                        interactionPrompt.text = "Ladder locked – need 2000+ points to escape.";
                    }
                    interactionPrompt.gameObject.SetActive(true);
                }
            }


        }

        if (previousOrb != null && previousOrb != newOrb)
        {
            previousOrb.UnhighlightOrb();
            interactionPrompt.text = "";
        }
        if (previousBreakable != null && previousBreakable != newBreakable)
        {
            previousBreakable.Unhighlight();
            interactionPrompt.text = "";
        }
        if (currentDoor != null && (hitInfo.collider == null || hitInfo.collider.gameObject != currentDoor.gameObject))
        {
            currentDoor = null;
            interactionPrompt.text = "";
            interactionPrompt.gameObject.SetActive(false);
        }
        if (currentLever != null && (hitInfo.collider == null || hitInfo.collider.gameObject != currentLever.gameObject))
        {
            currentLever = null;
            interactionPrompt.text = "";
            interactionPrompt.gameObject.SetActive(false);
        }
        if (currentLadder != null && (hitInfo.collider == null || hitInfo.collider.gameObject != currentLadder.gameObject))
        {
            currentLadder = null;
            interactionPrompt.text = "";
            interactionPrompt.gameObject.SetActive(false);
        }


        currentOrb = newOrb;
        previousOrb = newOrb;

        lookingAtBreakable = newBreakable;
        previousBreakable = newBreakable;
    }
}
