using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBee : Enemy
{
    [Header("Bee Details")]
    [SerializeField] private EnemyBulletBee bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 7;
    [SerializeField] private float bulletLifeTime = 2.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastTimeAttacked;

    [SerializeField] private float offset = .25f;
    private List<Vector3> wayPoints = new List<Vector3>();
    private int wayIndex;

    private Player _player; // Changed from Transform to Player

    protected override void Start()
    {
        base.Start();
        canMove = false;
        CreateWayPoints();

        float randomValue = Random.Range(0, .6f);
        Invoke(nameof(AllowMovement), randomValue);
    }

    private void CreateWayPoints()
    {
        wayPoints.Add(transform.position + new Vector3(offset, offset));
        wayPoints.Add(transform.position + new Vector3(offset, -offset));
        wayPoints.Add(transform.position + new Vector3(-offset, -offset));
        wayPoints.Add(transform.position + new Vector3(-offset, offset));
    }

    protected override void Update()
    {
        base.Update();

        HandleMovement();
        FindPlayerIfEmpty();

        // Only attack if there's a valid player and the cooldown has passed
        bool canAttack = Time.time > lastTimeAttacked + attackCooldown && _player != null;

        if (canAttack)
        {
            Debug.Log("Attacking player: " + _player.name); // Debugging
            Attack();
        }
    }

    private void FindPlayerIfEmpty()
    {
        if (_player == null)
        {
            // Change raycast to detect only Player objects and add a range limit if necessary
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, whatIsPlayer); // Added a limit of 10 units

            if (hit.collider != null)
            {
                Player player = hit.transform.GetComponent<Player>();

                if (player != null)
                {
                    _player = player;
                    Debug.Log("Player detected: " + _player.name); // Debugging
                }
                else
                {
                    Debug.Log("No player detected"); // Debugging
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (canMove == false || isDead)
            return;

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayIndex], moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, wayPoints[wayIndex]) < .1f)
        {
            wayIndex++;

            if (wayIndex >= wayPoints.Count)
                wayIndex = 0;
        }
    }

    private void Attack()
    {
        lastTimeAttacked = Time.time;
        anim.SetTrigger("attack");
        CreateBullet();
    }

    private void CreateBullet()
    {
        if (_player == null)
            return; // Ensure the player is valid before creating a bullet

        EnemyBulletBee newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        newBullet.SetupBullet(_player.transform, bulletSpeed, bulletLifeTime); // Pass _player's Transform

        // Do not reset _player here unless you want the enemy to stop targeting immediately after shooting
        //_player = null;
    }

    private void AllowMovement() => canMove = true;

    protected override void HandleAnimator()
    {
        // Keep it empty, unless you need to update parameters
    }
}
