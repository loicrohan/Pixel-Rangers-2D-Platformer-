using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost : Enemy
{
    [Header("Ghost Details")]
    [SerializeField] private float activeDuration;
    private float activeTimer;
    [Space]
    [SerializeField] private float xMinDistance;
    [SerializeField] private float yMinDistance;
    [SerializeField] private float yMaxDistance;

    private bool isChasing;
    //private Transform target;
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

        if (isDead)
            return;

        activeTimer -= Time.deltaTime;

        if (isChasing == false && idleTimer < 0)
        {
            StartChase();
        }
        else if (isChasing && activeTimer < 0)
        {
            EndChase();
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (canMove == false)
            return;

        HandleFlip(_player.transform.position.x);
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void StartChase()
    {
        // Assuming you have a reference to the player directly
        if (_player == null)
        {
            _player = FindFirstObjectByType<Player>(); // Find the player directly if not already assigned
        }

        if (_player == null)
        {
            EndChase();
            return; // No player found, end chase
        }

        // Calculate a position to start the chase
        float xOffset = Random.Range(0, 100) < 50 ? -1 : 1; // Randomly offset x position
        float yPosition = Random.Range(yMinDistance, yMaxDistance);

        // Position the enemy ghost to start chasing the player
        transform.position = _player.transform.position + new Vector3(xMinDistance * xOffset, yPosition);

        activeTimer = activeDuration; // Reset active timer for chase duration
        isChasing = true; // Set chasing state
        anim.SetTrigger("appear"); // Trigger appearance animation
    }


    private void EndChase()
    {
        idleTimer = idleDuration;
        isChasing = false;
        anim.SetTrigger("disappear");
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