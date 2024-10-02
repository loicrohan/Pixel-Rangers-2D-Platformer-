using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] private float aggroDuration;      // How long the chicken stays aggressive
    [SerializeField] private float detectionRange;     // The range within which the chicken can detect the player
    [SerializeField] private LayerMask playerLayer;    // Layer for detecting the player

    private float aggroTimer;
    private bool playerDetected;
    private bool canFlip = true;

    // Ensure player is assigned
    private Player _player;

    protected override void Start()
    {
        base.Start();
        // Find the player object if not manually assigned
        if (_player == null)
        {
            _player = FindFirstObjectByType<Player>();
        }
    }

    protected override void Update()
    {
        base.Update();

        aggroTimer -= Time.deltaTime;

        if (isDead)
            return;

        DetectPlayer();

        if (playerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;  // Reset aggro timer when player is detected
        }

        if (aggroTimer < 0)
            canMove = false;  // Stop moving when aggro timer runs out

        HandleMovement();

        if (isGrounded)
            HandleTurnAround();
    }

    // Detect if the player is within detection range
    private void DetectPlayer()
    {
        // Check if the player is within detection range using a Physics2D circle overlap
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        // If a player is detected, set playerDetected to true
        playerDetected = playerCollider != null;
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();
            canMove = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (!canMove || _player == null)  // Ensure player reference exists
            return;

        // Move towards the player
        HandleFlip(_player.transform.position.x);
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), .3f);  // Delay the flip to avoid instant flipping
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

    // Optional: For visualizing the detection range in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}