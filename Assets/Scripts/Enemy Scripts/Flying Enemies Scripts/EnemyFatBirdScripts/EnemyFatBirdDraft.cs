using System.Collections;
using UnityEngine;

public class EnemyFatBirdDraft : Enemy
{
    [Header("FatBird Details")]
    [SerializeField] private float detectionDistance;  // Distance to detect player
    [SerializeField] private float fallForce = 10f;    // Force applied when falling
    [SerializeField] private float returnSpeed = 2f;   // Speed at which the enemy returns to the initial position
    [SerializeField] private float returnDelay = 2f;   // Delay before returning to the initial position

    private bool isFalling = false;
    private bool isReturning = false;
    private bool groundHit = false;
    private Vector2 initialPosition;  // Store the initial position of the enemy

    protected override void Start()
    {
        base.Start();  // Call the base class's Start method
        initialPosition = transform.position;  // Store initial position at the start
    }

    protected override void Update()
    {
        base.Update();  // Call the base class's Update method
        if (!isFalling && !isReturning)
        {
            DetectPlayer();
        }
        if (isReturning)
        {
            ReturnToInitialPosition();
        }
        HandleAnimator(); // Call to update the animator
    }

    private void DetectPlayer()
    {
        // Raycast downward to detect the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionDistance, whatIsPlayer);
        if (hit.collider != null && !isFalling)
        {
            FallToGround();
        }
    }

    private void FallToGround()
    {
        isFalling = true;
        rb.isKinematic = false;  // Disable kinematic so gravity affects the enemy
        rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);  // Apply force to simulate falling
        groundHit = false;  // Ensure groundHit is false until hitting the ground
        anim.SetTrigger("fall");
        StartCoroutine(CompleteFall());  // Start the coroutine for completing the fall
    }

    private IEnumerator CompleteFall()
    {
        // Wait for the fall animation to complete before allowing the ground hit
        yield return new WaitForSeconds(1.0f);  // Adjust this duration based on the length of your fall animation
        groundHit = true;  // Allow the ground hit animation to start after the fall
        StartCoroutine(PrepareToReturn());  // Start the return process
    }

    protected override void HandleAnimator()
    {
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isReturning", isReturning);
        anim.SetBool("hitGround", groundHit);  // Smooth hit ground animation
    }

    private IEnumerator PrepareToReturn()
    {
        yield return new WaitForSeconds(returnDelay);  // Delay before returning
        isFalling = false;
        isReturning = true;
        groundHit = false;
        rb.velocity = Vector2.zero;  // Stop any remaining downward velocity
        rb.isKinematic = true;  // Re-enable kinematic for controlled movement
    }

    private void ReturnToInitialPosition()
    {
        // Move the enemy back to its initial position smoothly
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
        // Once the enemy reaches the initial position, reset states
        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            isReturning = false;  // Stop returning once the enemy is back at the starting point
            anim.SetBool("isReturning", false);  // Transition back to idle
            rb.isKinematic = true;  // Ensure kinematic remains enabled while idle
        }
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * detectionDistance); // Draw the ray for debugging
    }
}