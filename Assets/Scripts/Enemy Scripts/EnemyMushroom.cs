using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : Enemy
{
    protected override void Update()
    {
        base.Update(); // Call the base Update method for common enemy behavior

        if (isDead)    // Check if the mushroom is dead
            return;    // Exit the Update method if the mushroom is dead

        HandleMovement(); // Handle the mushroom's movement logic

        if (isGrounded)   // Check if the mushroom is on the ground
            HandleTurnAround(); // Handle logic for turning around
    }

    private void HandleTurnAround()
    {
        // Check if there is no ground in front or a wall is detected
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();                    // Flip the mushroom's direction
            idleTimer = idleDuration;  // Reset the idle timer to delay next movement
            rb.velocity = Vector2.zero; // Stop any current movement
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)             // Check if the idle timer is still running
            return;                    // Exit the method if the mushroom is idling

        // Set the mushroom's velocity to move in the current direction
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
}