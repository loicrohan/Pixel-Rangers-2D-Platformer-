using UnityEngine;

public class EnemyAngryPig : Enemy
{
    [Header("AngryPig details")]
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float speedIncreaseOnHit = 5;
    private bool isFirstHit = false;

    protected override void Update()
    {
        base.Update();

        if (!isDead)
        {
            HandleMovement();
            if (isGrounded) HandleTurnAround();
        }
    }

    public override void Die()
    {
        if (!isFirstHit)
        {
            HandleFirstHit();
        }
        else
        {
            base.Die();
        }
    }

    private void HandleFirstHit()
    {
        isFirstHit = true;
        canMove = true;
        moveSpeed += speedIncreaseOnHit;
        anim.SetTrigger("hit");
        anim.SetBool("isAngry", true);
        rb.velocity = Vector2.zero;
        idleDuration = 0;
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
        if (idleTimer <= 0 && canMove)
        {
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
        }
    }
}