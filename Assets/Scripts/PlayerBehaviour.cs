using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int maxHealth = 100; // Maximum health of the player
    int currentHealth = 100; // Current health of the player
    bool canInteract = false; // Flag to check if the player can interact with objects
    bool isInSewage = false;
    float sewageTimer = 0f;

    public bool HasAxe = false; // Property to check if the player has an axe

    [SerializeField] // Serialize field to allow adjustment in the Unity Inspector
    public int sewageDamagePerSecond = 5;


    DoorBehaviour currentDoor = null;
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
    }
}
