using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int maxHealth = 100; // Maximum health of the player
    int currentHealth = 100; // Current health of the player
    bool canInteract = false; // Flag to check if the player can interact with objects
    bool isInSewage = false;
    int currentScore = 0;
    float sewageTimer = 0f;
    BreakableBehaviour lookingAtBreakable = null;

    public bool HasAxe = false; // Property to check if the player has an axe

    [SerializeField] // Serialize field to allow adjustment in the Unity Inspector
    public int sewageDamagePerSecond = 5;

    [SerializeField]
    float interactionDistance = 5f;
    [SerializeField]
    Transform interactionPoint; // Point from which the player interacts with objects

    DoorBehaviour currentDoor = null;
    OrbBehaviour currentOrb = null; // Reference to the currently detected coin
    AxeBehaviour axe = null;

    public void ModifyHealth(int amount)
    {
        currentHealth += amount; // Modify the player's health by the specified amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health does not exceed max or go below zero
        Debug.Log("Player health modified. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!"); // Log player death
                                           // Here you can add logic for player death, like respawning or ending the game
        }
    }

    public void ModifyScore(int amount)
    {
        // This method can be used to modify the player's score
        // Implement score logic here if needed
        Debug.Log("Score modified by: " + amount);
        currentScore += amount; // Modify the player's score by the specified amount
        Debug.Log("Current score: " + currentScore); // Log the current score
    }



    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger with: " + other.gameObject.name); // Log the object the player collided with
        if (other.CompareTag("Door"))
        {
            canInteract = true; // Allow interaction with the door
            currentDoor = other.GetComponent<DoorBehaviour>(); // Get the DoorBehaviour component from the door
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
        if (canInteract)
        {
            if (currentDoor != null)
            {
                Debug.Log("Interacting with door"); // Log interaction
                currentDoor.OpenDoor(); // Call the OpenDoor method on the door
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
        else return;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))   // Check if the player exited a door trigger
        {
            Debug.Log("Player exited door trigger");
            canInteract = false; // Disable interaction with the door
                                 // Reset currentDoor to null when exiting the door trigger
            currentDoor = null;
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

    // Update is called once per frame
    void Update()
    {
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

        RaycastHit hit;
        // Cast a ray from the interaction point to check for breakable objects
        if (Physics.Raycast(interactionPoint.position, interactionPoint.forward, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Breakable"))
            {
                lookingAtBreakable = hit.collider.gameObject.GetComponent<BreakableBehaviour>();
                if (lookingAtBreakable != null)
                {
                    if (HasAxe)
                        Debug.Log("E to break");
                    else
                        Debug.Log("Looks like I need something to break this.");
                }
            }
            else if (hit.collider.CompareTag("Collectible"))
            {
                canInteract = true; // Allow interaction with the collectible
                currentOrb = hit.collider.gameObject.GetComponent<OrbBehaviour>();
                currentOrb.HighlightOrb(); // Highlight the collectible to indicate it can be collected
                Debug.Log("E to collect orb");
            }
        }
        else if (currentOrb != null)
        {
            currentOrb.UnhighlightOrb(); // Unhighlight the orb if no longer detected
            currentOrb = null;
        }
    }
}
