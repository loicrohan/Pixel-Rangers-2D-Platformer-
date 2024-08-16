using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[Header("Player Controller References")]
    [SerializeField] float jumpForce = 18f, runSpeed = 150f;
    float dirX;
    Rigidbody2D rb;
    BoxCollider2D BxCldr2D;

    [SerializeField] LayerMask groundMask;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool gamePaused = false;
    //[SerializeField] private bool doubleJump;
    private enum MovementState {idle,run,jump,fall }
    [SerializeField] AudioSource jumpAudio/*, runAudio*/;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BxCldr2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        //if(isGrounded() && !Input.GetKeyDown(KeyCode.Space))
        //{
        //    doubleJump = false;
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (isGrounded() || doubleJump)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        //        doubleJump = !doubleJump;
        //        jumpAudio.Play();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            jumpAudio.Play();
        }
        HandleAnimation();
        PauseGame();
        //RunSoundFX();
    }

    //void RunSoundFX()
    //{
    //    if (rb.velocity.x != 0 )
    //    {
    //        if (!runAudio.isPlaying)
    //        {
    //            runAudio.Play();
    //        }
    //    }
    //    else
    //    {
    //        runAudio.Stop();
    //    }
    //}

    void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
        }

        if (gamePaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(dirX * runSpeed * Time.deltaTime, rb.velocity.y, 0f);
    }

    bool isGrounded()
    {
        return Physics2D.BoxCast(BxCldr2D.bounds.center, BxCldr2D.bounds.size, 0, Vector2.down, 0.1f, groundMask);
    }

    void HandleAnimation()
    {
        MovementState state;
        if (dirX > 0)
        {
            spriteRenderer.flipX = false;
            state = MovementState.run;
            
        }
        else if (dirX < 0)
        {
            spriteRenderer.flipX = true;
            state = MovementState.run;           
        }
        else
        {
            state = MovementState.idle;
        }
        if(rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }
        animator.SetInteger("state", (int)state);
    }
}