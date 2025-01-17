using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
        DontDestroyOnLoad(gameObject);
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
        playerAnimation.SetFloat("AirSpeedY", playerRb.velocity.y);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position + new Vector3(-0.05f, 0, 0) + 
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
}
