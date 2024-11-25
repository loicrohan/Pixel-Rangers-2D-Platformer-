using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhostOnTheGround : Enemy
{
    [Header("Ghost details")]
    [SerializeField] private float activeDuration;
    private float activeTimer;
    private bool isActive;

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        activeTimer -= Time.deltaTime;

        if (!isActive && activeTimer <= 0)
        {
            StartActiveState();
        }
        else if (isActive && activeTimer <= 0)
        {
            EndActiveState();
        }

        HandleMovement();

        if (isGrounded)
            HandleTurnAround();
    }

    private void StartActiveState()
    {
        isActive = true;
        activeTimer = activeDuration;
        anim.SetTrigger("appear");
    }

    private void EndActiveState()
    {
        isActive = false;
        activeTimer = activeDuration;
        anim.SetTrigger("disappear");
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    private void MakeInvisible()
    {
        sr.color = Color.clear;
        EnableColliders(false);
    }

    private void MakeVisible()
    {
        sr.color = Color.white;
        EnableColliders(true);
    }

    public override void Die()
    {
        base.Die();
        canMove = false;
    }
}