using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRino : Enemy
{
    [Header("Rino Details")]
    [SerializeField] private float maxSpeed;               // Maximum speed the Rino can reach
    [SerializeField] private float speedUpRate = .6f;      // Rate at which the Rino speeds up
    private float defaultSpeed;                            // Default movement speed
    [SerializeField] private Vector2 impactPower;          // Force applied when the Rino hits a wall

    [Header("Effects")]
    [SerializeField] private ParticleSystem dustFx;        // Particle effect for dust when hitting a wall
    [SerializeField] private Vector2 cameraImpulseDir;     // Direction of the camera impulse effect
    private CinemachineImpulseSource impulseSource;        // Cinemachine impulse source for camera shake effects

    protected override void Start()
    {
        base.Start();                                      // Call the base Start method for common setup

        canMove = false;                                   // Initially, the Rino can't move
        defaultSpeed = moveSpeed;                          // Store the default speed
        impulseSource = GetComponent<CinemachineImpulseSource>(); // Get the Cinemachine impulse source component
    }

    protected override void Update()
    {
        base.Update();                                     // Call the base Update method for common behavior
        HandleCharge();                                    // Handle the charging behavior
    }

    private void HitWallImpact()
    {
        dustFx.Play();                                     // Play the dust particle effect
        impulseSource.m_DefaultVelocity = new Vector2(cameraImpulseDir.x * facingDir, cameraImpulseDir.y); // Set camera impulse direction
        impulseSource.GenerateImpulse();                   // Generate a camera shake impulse
    }

    private void HandleCharge()
    {
        if (!canMove)                                      // If the Rino can't move, return early
            return;

        HandleSpeedUp();                                   // Handle speeding up the Rino

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y); // Set the Rino's velocity

        if (isWallDetected)                                // If a wall is detected
            WallHit();                                     // Handle the wall hit

        if (!isGroundInfrontDetected)                      // If there is no ground ahead
            TurnAround();                                  // Handle turning around
    }

    private void HandleSpeedUp()
    {
        moveSpeed += Time.deltaTime * speedUpRate;         // Increase the speed over time

        if (moveSpeed >= maxSpeed)                         // Cap the speed at maxSpeed
            maxSpeed = moveSpeed;
    }

    private void TurnAround()
    {
        SpeedReset();                                      // Reset the speed to default
        canMove = false;                                   // Stop movement
        rb.velocity = Vector2.zero;                        // Reset velocity
        Flip();                                            // Flip the direction
    }

    private void WallHit()
    {
        canMove = false;                                   // Stop movement

        HitWallImpact();                                   // Trigger wall impact effects
        SpeedReset();                                      // Reset speed

        AudioManager.instance.PlaySFX(1);                  // Play wall hit sound effect
        anim.SetBool("hitWall", true);                     // Set wall hit animation
        rb.velocity = new Vector2(impactPower.x * -facingDir, impactPower.y); // Apply impact force
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;                          // Reset movement speed to default
    }

    private void ChargeIsOver()
    {
        anim.SetBool("hitWall", false);                    // Reset wall hit animation
        Invoke(nameof(Flip), 1);                           // Delay flipping for 1 second
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();                            // Call the base collision handler

        if (isPlayerDetected && isGrounded)                // If the player is detected and the Rino is grounded
            canMove = true;                                // Allow the Rino to move
    }
}