using System.Collections.Generic;
using UnityEngine;

// Base class for enemy behavior in the game
public class Enemy : MonoBehaviour
{
    #region Variables
    // Cached reference to the SpriteRenderer component
    protected SpriteRenderer sr => GetComponent<SpriteRenderer>();
    // Reference to the player's Transform (not used directly in this version)
    protected Transform player;
    // Cached reference to the Animator component
    protected Animator anim;
    // Cached reference to the Rigidbody2D component
    protected Rigidbody2D rb;
    // Array to hold all colliders of the enemy
    protected Collider2D[] colliders;

    // Header for grouping general information variables in the Inspector
    [Header("General Info")]
    [SerializeField] protected float moveSpeed = 2f; // Movement speed of the enemy
    [SerializeField] protected float idleDuration = 1.5f; // Duration for which the enemy idles
    protected float idleTimer; // Timer to track idle time
    protected bool canMove = true; // Flag to control movement

    // Header for grouping death-related variables in the Inspector
    [Header("Death Details")]
    [SerializeField] protected float deathImpactSpeed = 5; // Speed at which the enemy moves upon death
    [SerializeField] protected float deathRotationSpeed = 150; // Rotation speed during death animation
    protected int deathRotationDirection = 1; // Direction of rotation during death
    protected bool isDead; // Flag to check if the enemy is dead

    // Header for grouping collision-related variables in the Inspector
    [Header("Basic Collision")]
    [SerializeField] protected float groundCheckDistance = 1.1f; // Distance for ground detection
    [SerializeField] protected float wallCheckDistance = 0.7f; // Distance for wall detection
    [SerializeField] protected LayerMask whatIsGround; // Layer mask for ground detection
    [SerializeField] protected float playerDetectionDistance = 15; // Distance for detecting the player
    [SerializeField] protected LayerMask whatIsPlayer; // Layer mask for player detection
    [SerializeField] protected Transform groundCheck; // Transform used for ground check
    protected bool isPlayerDetected; // Flag to indicate if the player is detected
    protected bool isGrounded; // Flag to indicate if the enemy is grounded
    protected bool isWallDetected; // Flag to indicate if a wall is detected
    protected bool isGroundInfrontDetected; // Flag for checking if ground is in front

    protected int facingDir = -1; // Direction the enemy is facing
    protected bool facingRight = false; // Flag to indicate if the enemy is facing right
    #endregion

    // Awake is called when the script instance is being loaded
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>(); // Initialize Animator component
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D component
        colliders = GetComponentsInChildren<Collider2D>(); // Get all Collider2D components in children
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Check if the sprite's X flip needs to be adjusted and flip the enemy if necessary
        if (sr.flipX == true && !facingRight)
        {
            sr.flipX = false;
            Flip();
        }

        // Event subscriptions for player respawn and death (commented out)
        // PlayerManager.OnPlayerRespawn += UpdatePlayersReference;
        // PlayerManager.OnPlayerDeath += UpdatePlayersReference;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleCollision(); // Check for collisions
        HandleAnimator(); // Update animator parameters

        idleTimer -= Time.deltaTime; // Decrement the idle timer

        if (isDead) // Handle death rotation if the enemy is dead
            HandleDeathRotation();
    }

    // Method to handle the enemy's death
    public virtual void Die()
    {
        if (rb.isKinematic)
            rb.isKinematic = false; // Ensure Rigidbody2D is not kinematic

        EnableColliders(false); // Disable all colliders

        anim.SetTrigger("hit"); // Trigger death animation
        rb.velocity = new Vector2(rb.velocity.x, deathImpactSpeed); // Set upward velocity
        isDead = true; // Set the enemy as dead

        if (Random.Range(0, 100) < 50) // Randomize death rotation direction
            deathRotationDirection *= -1;

        // Unsubscribe from player events (commented out)
        // PlayerManager.OnPlayerRespawn -= UpdatePlayersReference;
        // PlayerManager.OnPlayerDeath -= UpdatePlayersReference;
        Destroy(gameObject, 10); // Destroy the enemy after 10 seconds
    }

    // Method to enable or disable all colliders
    protected void EnableColliders(bool enable)
    {
        foreach (var collider in colliders) // Iterate through all colliders
        {
            collider.enabled = enable; // Enable or disable each collider
        }
    }

    // Method to handle the rotation of the enemy during death
    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, (deathRotationSpeed * deathRotationDirection) * Time.deltaTime); // Rotate the enemy
    }

    // Method to handle flipping the enemy based on direction
    protected virtual void HandleFlip(float xValue)
    {
        if ((xValue < transform.position.x && facingRight) || (xValue > transform.position.x && !facingRight))
            Flip(); // Flip the enemy if needed
    }

    // Method to flip the enemy's facing direction
    protected virtual void Flip()
    {
        facingDir *= -1; // Reverse the facing direction
        transform.Rotate(0, 180, 0); // Rotate the enemy's transform
        facingRight = !facingRight; // Toggle the facing right flag
    }

    // Context menu method to change the default facing direction
    [ContextMenu("Change Facing Direction")]
    public void FlipDefaultFacingDireciton()
    {
        sr.flipX = !sr.flipX; // Toggle the sprite's X flip
    }

    // Method to handle updating animator parameters
    protected virtual void HandleAnimator()
    {
        anim.SetFloat("xVelocity", rb.velocity.x); // Set the X velocity parameter in the animator
    }

    // Method to handle collision checks
    protected virtual void HandleCollision()
    {
        // Check if the enemy is grounded using a raycast
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        // Check if there is ground in front using a raycast from the ground check position
        isGroundInfrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        // Check if a wall is detected using a raycast
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        // Check if the player is detected using a raycast
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectionDistance, whatIsPlayer);
    }

    // Method to draw gizmos in the Editor for visualization
    protected virtual void OnDrawGizmos()
    {
        // Draw line for ground check
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        // Draw line for ground check in front
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        // Draw line for wall check
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
        // Draw line for player detection
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectionDistance * facingDir), transform.position.y));
    }
}
