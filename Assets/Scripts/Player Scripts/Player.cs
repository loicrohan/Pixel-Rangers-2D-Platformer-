using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private DifficultyType gameDifficulty;
    private GameManager gameManager;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;

    private bool canBeControlled = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private float defaultGravityScale;
    private bool canDoubleJump;

    [Header("Buffer & Coyote jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .5f;
    private float coyoteJumpActivated = -1;

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;


    [Header("Collision")]
    [SerializeField] private float groundCheckDistnace;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isAirborne;
    private bool isWallDetected;

    private Joystick joystick;
    private float xInput;
    private float yInput;

    private bool facingRight = true;
    private int facingDir = 1;

    [Header("Player Visuals")]
    [SerializeField] private AnimatorOverrideController[] animsOC;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private int skinId;
    #endregion
    #region Awake,Start & Update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();

        FindFirstObjectByType<JumpButton_UI>().UpdatePlayersRefs(this);
        joystick = FindFirstObjectByType<Joystick>();
    }

    private void Start()
    {
        defaultGravityScale = rb.gravityScale;
        gameManager = GameManager.instance;

        UpdateGameDifficulty();
        RespawnFinished(false);
        UpdateSkin();
    }

    private void Update()
    {
        UpdateAirbornStatus();

        if (canBeControlled == false)
        {
            HandleCollision();
            HandleAnimations();
            return;
        }

        if (isKnocked)
            return;

        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimations();

        
    }
    #endregion
    #region Difficulty
    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Normal)
        {

            if (gameManager.FruitsCollected() <= 0)
            {
                Die();
                gameManager.RestartLevel();
            }
            else
                gameManager.RemoveFruit();

            return;
        }

        if (gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }

    }

    private void UpdateGameDifficulty()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null)
            gameDifficulty = difficultyManager.difficulty;
    }
    #endregion
    public void UpdateSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        if (skinManager == null)
            return;

        anim.runtimeAnimatorController = animsOC[skinManager.choosenSkinId];
    }

    public void RespawnFinished(bool finished)
    {
        float gravityScale = rb.gravityScale;

        if (finished)
        {
            rb.gravityScale = defaultGravityScale;
            canBeControlled = true;
            cd.enabled = true;

            AudioManager.instance.PlaySFX(11);
        }
        else
        {
            rb.gravityScale = 0;
            canBeControlled = false;
            cd.enabled = false;
        }
    }
    #region Knockback & Die
    public void Knockback(float sourceDamageXPosition)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceDamageXPosition)
            knockbackDir = -1;

        if (isKnocked)
            return;

        //AudioManager.instance.PlaySFX(9);
        StartCoroutine(KnockbackRoutine());
        //anim.SetTrigger("knockback");
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
    }
    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        anim.SetBool("isKnocked", true);

        AudioManager.instance.PlaySFX(9);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        anim.SetBool("isKnocked", false);

    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(0, false);
        GameObject newDeathVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion
    public void StopPlayerMove() => Time.timeScale = 0f;

    public void Push(Vector2 direction,float duration = 0)
    {
        StartCoroutine(PushCoroutine(direction, duration));
    }

    private IEnumerator PushCoroutine(Vector2 direction,float duration)
    {
        canBeControlled = false;

        rb.velocity = Vector2.zero;
        rb.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        canBeControlled = true;
    }

    #region Airborn & Landing
    private void UpdateAirbornStatus()
    {
        if (isGrounded && isAirborne)
            HandleLanding();

        if (!isGrounded && !isAirborne)
            BecomeAirborne();
    }
    private void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.velocity.y < 0)
            ActivateCoyoteJump();
    }

    private void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;

        AttemptBufferJump();
    }
    #endregion

    private void HandleInput()
    {
        //xInput = Input.GetAxisRaw("Horizontal");
        //yInput = Input.GetAxisRaw("Vertical");

        xInput = joystick.Horizontal;
        yInput = joystick.Vertical;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    #region Coyote & Buffer Jump

    private void RequestBufferJump()
    {
        if (isAirborne)
            bufferJumpActivated = Time.time;
    }
    private void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }
    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;
    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;

    #endregion
    #region Jump & Wall Jump
    public void JumpButton()
    {
        JumpAttempt();
        RequestBufferJump();
    }
    private void JumpAttempt()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow;

        if (isGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

        CancelCoyoteJump();
    }

    private void Jump()
    {
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void DoubleJump()
    {
        AudioManager.instance.PlaySFX(3);
        StopCoroutine(WallJumpRoutine());
        isWallJumping = false;
        canDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        anim.SetTrigger("2xJump");

    }
    private void WallJump()
    {
        AudioManager.instance.PlaySFX(12);

        canDoubleJump = true;
        rb.velocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);

        Flip();
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }
    #endregion
    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.velocity.y < 0;
        float yModifer = yInput < 0 ? 1 : .05f;

        if (canWallSlide == false)
            return;


        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifer);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistnace, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }
    #region Animations & Movements
    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void HandleMovement()
    {
        if (isWallDetected)
            return;

        if (isWallJumping)
            return;

        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void HandleFlip()
    {
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistnace));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}