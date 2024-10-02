using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnailBody : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float zRotation;

    public void SetupBody(float yVelocity, float zRotation, int facingDir)
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.velocity = new Vector2(rb.velocity.x, yVelocity);

        this.zRotation = zRotation;

        if (facingDir == 1)
            sr.flipX = true;
    }

    private void Update()
    {
        transform.Rotate(0, 0, zRotation * Time.deltaTime);
    }
}