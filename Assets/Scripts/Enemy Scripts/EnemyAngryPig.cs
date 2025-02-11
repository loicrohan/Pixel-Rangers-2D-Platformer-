using UnityEngine;

public class EnemyAngryPig : Enemy
{
    [Header("AngryPig details")]
    [SerializeField] private float maxSpeed = 10; // Maximum movement speed of the Angry Pig
    [SerializeField] private float speedIncreaseOnHit = 5; // Speed increase when the pig gets hit for the first time
    private bool isFirstHit = false; // Tracks whether the pig has been hit for the first time

    protected override void Update()
    {
        base.Update(); // Call the base class Update method for common behavior

        if (!isDead) // Only handle movement and turning if the pig is not dead
        {
            HandleMovement(); // Control the pig's movement
            if (isGrounded) HandleTurnAround(); // Check and handle turning around when grounded
        }
    }

    public override void Die()
    {
        if (!isFirstHit) // Check if this is the first hit
        {
            HandleFirstHit(); // Handle behavior for the first hit
        }
        else
        {
            base.Die(); // If it's not the first hit, call the base Die method for standard death behavior
        }
    }

    private void HandleFirstHit()
    {
        isFirstHit = true; // Mark that the pig has been hit for the first time
        canMove = true; // Allow the pig to move
        moveSpeed += speedIncreaseOnHit; // Increase the pig's movement speed
        anim.SetTrigger("hit"); // Trigger the "hit" animation
        anim.SetBool("isAngry", true); // Set the "isAngry" animation state to true
        rb.velocity = Vector2.zero; // Reset the pig's velocity to stop any current movement
        idleDuration = 0; // Reset idle duration to avoid unnecessary idling
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected) // Check if there is no ground in front or a wall is detected
        {
            Flip(); // Flip the pig's direction
            idleTimer = idleDuration; // Reset the idle timer to ensure immediate reaction
            rb.velocity = Vector2.zero; // Stop the pig's current movement
        }
    }

    private void HandleMovement()
    {
        if (idleTimer <= 0 && canMove) // Check if the pig can move and is not idling
        {
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y); // Set the pig's velocity to move horizontally
        }
    }
}