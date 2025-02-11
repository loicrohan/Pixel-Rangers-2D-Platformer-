using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyTrunk class inherits from the Enemy base class
public class EnemyTrunk : Enemy
{
    [Header("Trunk Details")]
    [SerializeField] private EnemyBullet bulletPrefab; // Prefab for the bullet to be shot
    [SerializeField] private Transform gunPoint; // Point from which bullets are shot
    [SerializeField] private float bulletSpeed = 7; // Speed at which the bullet travels
    [SerializeField] private float attackCooldown = 1.5f; // Time between consecutive attacks
    private float lastTimeAttacked; // Tracks the last time the enemy attacked

    // Overrides the Update method from the base Enemy class
    protected override void Update()
    {
        base.Update(); // Call the base class Update method

        if (isDead) // If the enemy is dead, stop further updates
            return;

        // Check if enough time has passed to attack again
        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayerDetected && canAttack) // Attack if the player is detected and cooldown allows
            Attack();

        HandleMovement(); // Handle the enemy's movement

        if (isGrounded) // Handle turning around when grounded
            HandleTurnAround();
    }

    // Method to handle attacking
    private void Attack()
    {
        idleTimer = idleDuration + attackCooldown; // Reset idle timer
        lastTimeAttacked = Time.time; // Update the last attacked time
        anim.SetTrigger("attack"); // Trigger the attack animation
    }

    // Method to create and shoot a bullet
    private void CreateBullet()
    {
        EnemyBullet newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity); // Instantiate a new bullet

        Vector2 bulletVelocity = new Vector2(facingDir * bulletSpeed, 0); // Set bullet velocity
        newBullet.SetVelocity(bulletVelocity); // Apply velocity to the bullet

        if (facingDir == 1) // Flip the bullet sprite if facing right
            newBullet.FlipSprite();

        Destroy(newBullet.gameObject, 10); // Destroy the bullet after 10 seconds to prevent lingering
    }

    // Method to handle turning the enemy around when necessary
    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected) // Turn around if there's no ground ahead or a wall is detected
        {
            Flip(); // Flip the enemy's facing direction
            idleTimer = idleDuration; // Reset idle timer
            rb.velocity = Vector2.zero; // Stop the enemy's movement temporarily
        }
    }

    // Method to handle the enemy's movement
    private void HandleMovement()
    {
        if (idleTimer > 0) // If idle, do not move
            return;

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y); // Move the enemy in the facing direction
    }
}