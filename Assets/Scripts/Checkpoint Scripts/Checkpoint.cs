using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that represents a checkpoint in the game
public class Checkpoint : MonoBehaviour
{
    // Property to get the Animator component of the GameObject
    private Animator anim => GetComponent<Animator>();

    // Boolean to track if the checkpoint is currently active
    private bool active;

    // Serialized field to determine if the checkpoint can be reactivated
    [SerializeField] private bool canBeReactivated;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize the 'canBeReactivated' variable from the GameManager instance
        canBeReactivated = GameManager.instance.canReactivate;
    }

    // Method triggered when another collider enters the checkpoint's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the checkpoint is already active, exit the method
        if (active)
            return;

        // Check if the collider belongs to a Player
        Player player = collision.GetComponent<Player>();

        if (player != null)
            // Activate the checkpoint if a Player enters
            ActivateCheckpoint();
    }

    // Method to activate the checkpoint
    private void ActivateCheckpoint()
    {
        // Set the checkpoint as active
        active = true;

        // Trigger the activation animation
        anim.SetTrigger("Activate");

        // Update the player's respawn position to this checkpoint
        PlayerManager.instance.UpdateRespawnPosition(transform);
    }
}