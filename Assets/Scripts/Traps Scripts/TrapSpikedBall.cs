using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spikeRb;
    [SerializeField] private float pushForce;

    private void Start()
    {
        Vector2 pushVector = new Vector2(pushForce, 0);
        spikeRb.AddForce(pushVector, ForceMode2D.Impulse);
    }

}