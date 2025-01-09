using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBirdDraft : Enemy // Inheriting from Enemy class
{
    [Header("FatBird Details")]
    [SerializeField] private float detectionDistance;
    [SerializeField] private float fallForce = 10f;
    [SerializeField] private float returnSpeed = 2f;
    [SerializeField] private float returnDelay = 2f;
    [SerializeField] private float fallAnimationDuration = 1.0f; // Duration for the fall animation

    private bool isFalling = false;
    private bool isReturning = false;
    private bool groundHit = false;
    private Vector2 initialPosition;

    protected override void Start() // Override Start method
    {
        base.Start(); // Call the base class Start method
        initialPosition = transform.position;
    }

    protected override void Update()
    {
        if (!isFalling && !isReturning && PlayerInDetectionRange())
        {
            FallToGround();
        }

        // Check if bird is on the ground and player is no longer in range to start returning
        if (groundHit && !PlayerInDetectionRange() && !isReturning)
        {
            StartCoroutine(PrepareToReturn());
        }

        if (isReturning)
        {
            ReturnToInitialPosition();
        }

        HandleAnimator();
    }

    private bool PlayerInDetectionRange()
    {
        // Raycast downward to detect the player within detection distance
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionDistance, whatIsPlayer);
        return hit.collider != null; // Return true if player detected, else false
    }


    private void FallToGround()
    {
        isFalling = true;
        rb.isKinematic = false;
        rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);
        groundHit = false;
        anim.SetTrigger("fall"); // Trigger fall animation if available
        StartCoroutine(CompleteFall());
    }

    private IEnumerator CompleteFall()
    {
        yield return new WaitForSeconds(fallAnimationDuration);
        isFalling = false; // Falling sequence is complete
        groundHit = true;  // Now we are on the ground
        anim.SetBool("hitGround", true); // Explicitly set the hitGround animation trigger
        yield return new WaitForSeconds(0f); // Allow some time for the animation to play
        anim.SetBool("hitGround", false); // Reset hitGround to false to end the animation
    }


    protected override void HandleAnimator()
    {
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isReturning", isReturning);
        anim.SetBool("hitGround", groundHit);
    }

    private IEnumerator PrepareToReturn()
    {
        groundHit = false;  // Reset ground hit
        yield return new WaitForSeconds(returnDelay);  // Delay before returning
        isReturning = true; // Start returning after delay
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void ReturnToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            isReturning = false;
            rb.isKinematic = true;  // Ensure kinematic is enabled for idle state
            anim.SetBool("isReturning", false);
        }
    }

    protected override void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, detectionDistance); // Show detection radius
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(initialPosition, 0.1f); // Visualize initial position

        // Visualize the detection radius for debugging in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * detectionDistance); // Draw the ray for debugging
    }
}