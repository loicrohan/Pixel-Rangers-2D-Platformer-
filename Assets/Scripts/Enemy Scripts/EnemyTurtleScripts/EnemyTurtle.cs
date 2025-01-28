using System.Collections;
using UnityEngine;

public class EnemyTurtle : Enemy
{
    [SerializeField] private float offDuration = 4f; // Duration fire stays off/on
    [SerializeField] private EdgeCollider2D edgeCollider;
    private bool isActive;

    protected override void Awake()
    {
        base.Awake();
        edgeCollider = GetComponentInChildren<EdgeCollider2D>();

        if (edgeCollider == null)
            Debug.LogError("EdgeCollider2D component is missing on " + gameObject.name);
    }

    protected override void Start()
    {
        base.Start();
        SetSpikes(true); // Start with Spike on
        StartCoroutine(SpikeLoop());
    }

    private IEnumerator SpikeLoop()
    {
        while (true) // Infinite loop
        {
            SetSpikes(false); // Turn off spike
            yield return new WaitForSeconds(offDuration); // Wait for offDuration
            SetSpikes(true); // Turn on spike
            yield return new WaitForSeconds(offDuration); // Wait for offDuration
        }
    }

    private void SetSpikes(bool active)
    {
        if (anim != null)
            anim.SetBool("AreSpikesOut", active);

        if (edgeCollider != null)
            edgeCollider.enabled = active;

        isActive = active;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        // Add custom collision handling for the turtle here if needed.
    }

    protected override void HandleAnimator()
    {
        //base.HandleAnimator();
        // Add custom animations handling if necessary.
    }

    public override void Die()
    {
        base.Die();
        // Add additional death behavior for the turtle here if needed.
    }
}