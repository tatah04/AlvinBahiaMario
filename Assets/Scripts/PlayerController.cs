using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerState
{
    small,
    big,
    star,
    red,
}
public class PlayerController : MonoBehaviour

{
    [Header("General")]
    [SerializeField] Rigidbody2D rb;


    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    Vector2 direction;
    //Jump
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] CircleCollider2D jumpCollider;
    [SerializeField] bool isGrounded;
    [SerializeField] float jumpForce;
    bool jumpInput;
    [SerializeField] bool longJumpInput;
    [SerializeField] float fallLow;
    [SerializeField] float fallHigh;

    [Header("Animation")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] bool isFacingRight;

    [Header("Breaking Tiles")]
    [SerializeField] CircleCollider2D topCircleCollider;

    [Header("Score")]
    [SerializeField] int coins;
    [SerializeField] TextMeshProUGUI coinsText;
    public TimeAndScoreTracker timeAndScoreTracker;
    // [SerializeField] TextMeshProUGUI speed;
    [Header("States")]
    public PlayerState playerState;
    [SerializeField] float jumpForceSmall;
    [SerializeField] float jumpForceBig;
    //red
    [SerializeField] Color redState;
    [SerializeField] GameObject spikePrefabR;
    [SerializeField] GameObject spikePrefabL;
    [SerializeField] float shotDelay;
    [SerializeField] float shotDelayMax;
    [SerializeField] private bool canFire;
    //star
    [SerializeField] float starDuration;
    [SerializeField] float starDurationMax;
    [SerializeField] PlayerState lastState;
    [SerializeField] float hue;
    [SerializeField] Color newColor;
    [SerializeField] float starSpeed;
    //Iframe
    [SerializeField] float IframeTime;
    [SerializeField] float IframeTimeMax;
    public bool isInvincible;
    private Color lastColor;
    // Start is called before the first frame update
    void Start()
    {
        if (Stats.lastState != PlayerState.small)
        {
            ChangeState(Stats.lastState);
        }
        else
        {
            ChangeState(PlayerState.small);
        }

        longJumpInput = false;
        jumpInput = false;
        isFacingRight = true;
        isInvincible = false;
        shotDelay = shotDelayMax;
        SoundManager.PlayBgm(0);
        SoundManager.soundManagerInstance.bgmSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        direction = new Vector2(SimpleInput.GetAxisRaw("Horizontal"), 0);
        DetectGround();
        if (Time.timeScale == 1)
        {
            Animate();
        }
        #region Detect Input
        if (SimpleInput.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
        if (SimpleInput.GetKey(KeyCode.Space) && isGrounded == false)
        {
            longJumpInput = true;
        }
        else
        {
            longJumpInput = false;
        }
        if (SimpleInput.GetKeyDown(KeyCode.F))
        {
            fireSpike();
        }
        #endregion

        #region Death
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 0, 0);
            timeAndScoreTracker.GameOver();
        }
        #endregion
    }
    private void FixedUpdate()
    {

        MovePlayer();
        Jump();
        DetectTiles();
    }

    #region Control
    void MovePlayer()
    {
        if (playerState == PlayerState.star)
        {
            rb.AddForce(direction * moveSpeed * 1.5f * Time.fixedDeltaTime * 10f);
            if (Mathf.Abs(rb.velocity.x) > (maxSpeed * 1.5f))
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime * 10f);
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }
    void DetectGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(jumpCollider.bounds.center, jumpCollider.radius, Vector2.down, 0.1f, groundLayerMask);
        if (hit.collider)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void Jump()
    {
        if (jumpInput == true && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //rb.velocity = Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            jumpInput = false;

            //  TileLogic.DestroyTile(transform);
            // Debug.Log(transform.position= transform.TransformPoint(Vector3.up));
            SoundManager.PlaySfx(3);
        }

        if (longJumpInput == true && rb.velocity.y > 0 && isGrounded == false)
        {
            rb.velocity += (Vector2.up * rb.velocity.y * Physics2D.gravity.y * (fallHigh - 1) * Time.fixedDeltaTime);
        }
        else if (longJumpInput == false && rb.velocity.y > 0 && isGrounded == false)
        {
            rb.velocity += (Vector2.up * rb.velocity.y * Physics2D.gravity.y * (fallLow - 1) * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region Render
    void Animate()
    {
        //Flip Detection
        if (direction.x > 0)
        {
            isFacingRight = true;
        }
        else if (direction.x < 0)
        {
            isFacingRight = false;
        }
        spriteRenderer.flipX = !isFacingRight;
        //Running/idle toggle
        if (direction.x != 0 && isGrounded == true)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        int Roundedy = Mathf.RoundToInt(rb.velocity.y);
        if (Roundedy == 0 && isGrounded == true)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        //jump
        if (Roundedy > 0 && isGrounded == false)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (Roundedy < 0 && isGrounded == false)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        //animator.
    }
    #endregion


    #region Other Behaviours


    public void ChangeState(PlayerState newPlayerstate)
    {
        lastState = playerState;
        playerState = newPlayerstate;
        switch (playerState)
        {
            case PlayerState.small:
                spriteRenderer.color = Color.white;
                jumpForce = jumpForceSmall;
                transform.localScale = new Vector3(0.8f, .8f, .8f);
                break;
            case PlayerState.big:
                spriteRenderer.color = Color.white;
                transform.localScale = new Vector3(1f, 1f, 1f);
                jumpForce = jumpForceBig;
                break;
            case PlayerState.star:
                if (lastState != PlayerState.star)
                {
                    SoundManager.PlayBgm(1);
                    StartCoroutine(StarCo());
                }
                
                break;
            case PlayerState.red:
                spriteRenderer.color = redState;
                break;
            default:
                break;
        }
    }
    public void StartStar()
    {
        starDuration = starDurationMax;
        ChangeState(PlayerState.star);
    }
    private IEnumerator StarCo()
    {
        if (lastState == PlayerState.red)
        {
            spriteRenderer.color = Color.white;
        }

        while (starDuration > 0)
        {
            starDuration = starDuration - .25f;
            yield return new WaitForSeconds(.25f);
            hue = hue + (1 / (starDurationMax * 4));
            newColor = Color.HSVToRGB(Mathf.Clamp01(hue), 0.5f, 1f);
            newColor.a = 1;
            spriteRenderer.color = newColor;
            if (starDuration == 0)
            {
                ChangeState(lastState);
                SoundManager.PlayBgm(0);
            }
        }
    }
    private void DetectTiles()
    {
        RaycastHit2D hit = Physics2D.CircleCast(topCircleCollider.bounds.center, jumpCollider.radius, Vector2.up, 0.1f, groundLayerMask);
        if (hit.collider && rb.velocity.y > 0 && playerState != PlayerState.small)
        {
            SoundManager.PlaySfx(14);
            TileLogic.TriggerTileLogic(transform);
        }
        else if (hit.collider && rb.velocity.y > 0 && playerState == PlayerState.small)
        {
            SoundManager.PlaySfx(13);
        }
    }

    public void AddCoin(int coinsToAdd)
    {
        coins = coins + coinsToAdd;
        timeAndScoreTracker.Coin++;
        coinsText.text = coins.ToString();
    }
    public void startIframeCo()
    {
        IframeTime = IframeTimeMax;
        StartCoroutine(IframesCo());
    }
    private IEnumerator IframesCo()
    {

        while (IframeTime > 0 && isInvincible == true)
        {
            IframeTime = IframeTime - 0.25f;
            yield return new WaitForSeconds(.25f);
            if (IframeTime == 0)
            {
                isInvincible = false;
                spriteRenderer.color = lastColor;
                IframeTime = IframeTimeMax;
            }
        }
    }

    public void TakeDamage()
    {

        if (isInvincible == true)
        {
            return;
        }
        isInvincible = true;
        rb.AddForce(jumpForce * new Vector2(-.5f, 1f), ForceMode2D.Impulse);
        startIframeCo();
        switch (playerState)
        {
            case PlayerState.small:
                timeAndScoreTracker.GameOver();
                break;
            case PlayerState.big:
                ChangeState(PlayerState.small);
                break;
            case PlayerState.star:
                //Nothing happens
                break;
            case PlayerState.red:
                ChangeState(PlayerState.big);
                break;
            default:
                break;
        }
        // Debug.Log("gotHurt");
        lastColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        SoundManager.PlaySfx(6);
        animator.Play("hurt");

    }
    public void fireSpike()
    {
        if (playerState == PlayerState.red && canFire == true)
        {
            canFire = false;
            if (isFacingRight)
            {
                GameObject obj = Instantiate(spikePrefabR, transform);
                obj.GetComponent<Spike>().Fire(isFacingRight);
                obj.GetComponent<Spike>().trigger = true;
            }
            else
            {
                GameObject obj = Instantiate(spikePrefabL, transform);
                obj.GetComponent<Spike>().Fire(isFacingRight);
                obj.GetComponent<Spike>().trigger = true;
            }
            StartCoroutine(shotDelayCo());
            SoundManager.PlaySfx(7);
        }
    }
    private IEnumerator shotDelayCo()
    {
        while (shotDelay > 0 && canFire == false)
        {
            shotDelay = shotDelay - .25f;
            yield return new WaitForSeconds(.25f);
            if (shotDelay == 0)
            {
                canFire = true;
                shotDelay = shotDelayMax;
            }
        }
    }


    public void forScaleChange()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + .5f);
    }


    #endregion
}
