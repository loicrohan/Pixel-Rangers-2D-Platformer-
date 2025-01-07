using System.Collections;
using UnityEngine;

public class EnemyTurtleDraft : Enemy
{
    [SerializeField] private float spikeIdleDuration = 4f; // Duration spikes stay on/off
    [SerializeField] private float transitionDuration = 1f; // Duration of transition animation

    private EdgeCollider2D edgeCollider;
    private bool spikesOut;

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
        spikesOut = true; // Start with spikes out
        SetSpikes(spikesOut);
        StartCoroutine(SpikeLoop());
    }

    private IEnumerator SpikeLoop()
    {
        while (true)
        {
            // Transition to retracted spikes
            spikesOut = false;
            TriggerSpikeTransition();
            yield return new WaitForSeconds(transitionDuration); // Wait for transition animation

            // Set spikes retracted and wait for idle duration
            SetSpikes(spikesOut);
            yield return new WaitForSeconds(spikeIdleDuration); // Wait for idle duration with spikes in

            // Transition to extended spikes
            spikesOut = true;
            TriggerSpikeTransition();
            yield return new WaitForSeconds(transitionDuration); // Wait for transition animation

            // Set spikes extended and wait for idle duration
            SetSpikes(spikesOut);
            yield return new WaitForSeconds(spikeIdleDuration); // Wait for idle duration with spikes out
        }
    }

    private void SetSpikes(bool active)
    {
        if (anim != null)
        {
            anim.SetBool("AreSpikesOut", active);
        }

        if (edgeCollider != null)
            edgeCollider.enabled = active; // Enable collider only when spikes are out
    }

    private void TriggerSpikeTransition()
    {
        if (anim != null)
        {
            anim.SetTrigger("SpikeTransition");
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        // Add custom collision handling for the turtle if needed.
    }

    protected override void HandleAnimator()
    {
        // Add custom animations handling if necessary.
    }

    public override void Die()
    {
        base.Die();
        // Add additional death behavior for the turtle if needed.
    }
}