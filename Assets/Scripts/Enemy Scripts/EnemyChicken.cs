using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] private float aggroDuration;      // Duration for which the chicken remains aggressive after detecting the player
    [SerializeField] private float detectionRange;     // Distance within which the chicken can detect the player
    [SerializeField] private LayerMask playerLayer;    // Specifies the layer used for player detection

    private float aggroTimer;                          // Timer to track how long the chicken stays aggressive
    private bool playerDetected;                       // Flag to check if the player has been detected
    private bool canFlip = true;                       // Controls if the chicken can flip direction

    private Player _player;                            // Reference to the player object

    protected override void Start()
    {
        base.Start();                                  // Call the base Start method for common setup

        // Assign the player object if not manually assigned
        if (_player == null)
        {
            _player = FindFirstObjectByType<Player>(); // Find the player object in the scene
        }
    }

    protected override void Update()
    {
        base.Update();                                 // Call the base Update method for common behavior

        aggroTimer -= Time.deltaTime;                  // Decrease the aggro timer each frame

        if (isDead)                                    // If the chicken is dead, stop further execution
            return;

        DetectPlayer();                                // Check for player detection

        if (playerDetected)                            // If the player is detected
        {
            canMove = true;                            // Allow the chicken to move
            aggroTimer = aggroDuration;                // Reset the aggro timer
        }

        if (aggroTimer < 0)                            // If the aggro timer runs out
            canMove = false;                           // Stop the chicken's movement

        HandleMovement();                              // Handle the chicken's movement

        if (isGrounded)                                // If the chicken is on the ground
            HandleTurnAround();                        // Handle turning around logic
    }

    private void DetectPlayer()
    {
        // Detect the player using a circular overlap check within the detection range
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        // Set the playerDetected flag based on whether the player is within range
        playerDetected = playerCollider != null;
    }

    private void HandleTurnAround()
    {
        // If there is no ground ahead or a wall is detected
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();                                    // Flip the direction
            canMove = false;                           // Stop moving after turning around
            rb.velocity = Vector2.zero;                // Reset the chicken's velocity
        }
    }

    private void HandleMovement()
    {
        if (!canMove || _player == null)               // Ensure the chicken can move and the player reference exists
            return;

        // Move towards the player
        HandleFlip(_player.transform.position.x);      // Flip the chicken towards the player's position
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y); // Set the chicken's velocity
    }

    protected override void HandleFlip(float xValue)
    {
        // Check if the chicken needs to flip direction based on the player's position
        if ((xValue < transform.position.x && facingRight) || (xValue > transform.position.x && !facingRight))
        {
            if (canFlip)                               // If flipping is allowed
            {
                canFlip = false;                       // Prevent immediate re-flipping
                Invoke(nameof(Flip), .3f);             // Delay the flip to avoid instant flipping
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();                                   // Call the base Flip method to handle the actual flipping
        canFlip = true;                                // Allow flipping again after the current flip
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;                      // Set the color of the Gizmo to red
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw a wireframe sphere to visualize the detection range
    }
}