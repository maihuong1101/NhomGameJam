using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Player stats:")]
    public bool isAlive;
    public bool isJumping;
    public bool isGrounded;
    public bool lookingRight;

    //Input
    private float horizontalInput;
    [SerializeField] private float playerSpeed;

    [Header("Jump")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundCheckLayer;
    [SerializeField] private LayerMask waterCheckLayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float jumpForce;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJump;

    private Rigidbody2D playerRb;
    private Animator playerAnimation;
    private BoxCollider2D playerBC;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        isAlive = true;
        lookingRight = true;
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerBC = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        // Set speed when player in the air
        //playerAnimation.SetFloat("AirSpeedY", playerRb.velocity.y);

        if (isAlive)
        {
            Grounded();
            Flip();
            Move();
            Jump();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position + new Vector3(0f,0f,0f) + 
            Vector3.down * groundCheckDistance / 2, new Vector3(boxSize.x, boxSize.y, 1));
    }
    private void Flip()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            lookingRight = true;
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            lookingRight = false;
        }
    }
    private void Move()
    {
        playerRb.velocity = new Vector2(horizontalInput * playerSpeed, playerRb.velocity.y);

        //if (horizontalInput != 0)
        //{
        //    playerAnimation.SetFloat("Speed", 1);
        //}
        //else
        //{
        //    playerAnimation.SetFloat("Speed", 0);
        //}
    }
    public bool IsOnGround()
    {
        if (Physics2D.BoxCast(groundCheckPoint.position, boxSize, 0, Vector2.down, groundCheckDistance, groundCheckLayer))
        {
            return true;
        }
        else return false;
    }
    private void Grounded()
    {
        if (IsOnGround())
        {
            //playerAnimation.SetBool("Grounded", true);
            airJumpCounter = 0;
            isJumping = false;
        }
        else
        {
            //playerAnimation.SetBool("Grounded", false);
        }
    }
    private void Jump()
    {
        //if (Input.GetKeyUp(KeyCode.Space) && playerRb.velocity.y > 0)
        //{
        //    playerAnimation.SetTrigger("Jump");
        //    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        //}

        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround())
        {
            isJumping = true;
            isGrounded = false;
            //playerAnimation.SetTrigger("Jump");
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
        //else if (!IsOnGround() && airJumpCounter < maxAirJump && Input.GetKeyDown(KeyCode.Space))
        //{
        //    airJumpCounter++;
        //    playerAnimation.SetTrigger("Jump");
        //    playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        //}
    }
    private void Water()
    {
        if (Physics2D.BoxCast(groundCheckPoint.position, boxSize, 0, 
            Vector2.down, groundCheckDistance, waterCheckLayer))
        {
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        isAlive = false;
        //playerAnimation.SetTrigger("Death");
        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(UIManager.Instance.ActiveDeathScreen());
    }
}
