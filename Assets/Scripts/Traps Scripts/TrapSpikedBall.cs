using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spikeRb;
    [SerializeField] private float angularVelocity = 200f; // Speed of the swing
    [SerializeField] private float swingFrequency = 2f;    // How often it changes direction

    private void Start()
    {
        // Disable friction and drag to ensure constant swinging
        spikeRb.angularDrag = 0f;
        spikeRb.drag = 0f;

        // Set the initial angular velocity for an immediate swing
        spikeRb.angularVelocity = angularVelocity;
    }

    private void FixedUpdate()
    {
        // Ensure a constant swinging motion without slowing down
        spikeRb.angularVelocity = angularVelocity * Mathf.Sign(Mathf.Sin(Time.time * swingFrequency));
    }
}
