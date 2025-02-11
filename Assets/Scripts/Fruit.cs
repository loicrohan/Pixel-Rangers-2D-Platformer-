using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enumeration for different fruit types
public enum FruitType { Apple, Banana, Cherries, Kiwi, Melon, Orange, Pineapple, Strawberry }

// Class that represents a fruit object in the game
public class Fruit : MonoBehaviour
{
    // Serialized field to select the type of fruit in the Inspector
    [SerializeField] private FruitType fruitType;

    // Serialized field to assign the visual effect prefab shown upon fruit pickup
    [SerializeField] private GameObject pickupVFX;

    // Reference to the GameManager instance
    private GameManager gameManager;

    // Reference to the Animator component
    private Animator anim;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Animator component from the child GameObject
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get the singleton instance of GameManager
        gameManager = GameManager.instance;

        // Check if the fruit should have a random look and update visuals accordingly
        SetRandomLookIfNeeded();
    }

    // Method to determine and set the fruit's appearance
    private void SetRandomLookIfNeeded()
    {
        // Check if fruits should not have a random look
        if (gameManager.FruitsHaveRandomLook() == false)
        {
            // Update the visuals to match the specific fruit type
            UpdateFruitVisuals();
            return;
        }

        // If random look is enabled, select a random fruit appearance
        int randomIndex = Random.Range(0, 8); // Generate a random index between 0 and 7
        anim.SetFloat("FruitIndex", randomIndex); // Set the animator parameter to control the fruit's appearance
    }

    // Method to update the fruit's visuals based on its type
    private void UpdateFruitVisuals() => anim.SetFloat("FruitIndex", (int)fruitType);

    // Method triggered when another collider enters the fruit's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to a Player
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            // Play the sound effect for picking up a fruit
            AudioManager.instance.PlaySFX(8);

            // Notify the GameManager that a fruit has been collected
            gameManager.AddFruit();

            // Destroy the fruit GameObject
            Destroy(gameObject);

            // Instantiate the pickup visual effect at the fruit's position
            GameObject newFX = Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }
    }
}