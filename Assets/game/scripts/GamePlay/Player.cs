using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : Character
{
    public static Player instance;
    [SerializeField] private Rigidbody2D rb;
    //    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private int Speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private float throwForce = 20;
    [SerializeField] private Transform throwpoint;
    [SerializeField] private GameObject rope;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bubbleOrigin;
    [SerializeField] private DistanceJoint2D jointDu;
    private bool isGrounded = true;
    private bool isJumping = false; 
    private Tween rotationTween;
    private bool isSwing = false;

    private GameObject bubble;
    private bool isThrowing = false;
    private bool isSticky = false;
    private float horizontal;
    private int coin = 0;
    private Vector3 savePoint= new Vector3 (- 5 ,0,0);

    public float minAngle = -60f; // Góc nhỏ nhất
    public float maxAngle = -15f; // Góc lớn nhất

    // Chu kỳ thời gian (tính bằng giây)
    public float cycleTime = 1f; // Ví dụ: 2 giây để hoàn thành 1 chu kỳ
    // Trục xoay
    public Vector3 rotationAxis = Vector3.forward; // Xoay quanh trục z
    private float currentAngle;
    private float tweenValue; // Giá trị sẽ tween

    Action updateCheckPoint;
    float timer = 0;
    float timeCDCheckPoint = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        OnInit();
        updateCheckPoint = SavePoint;
    }
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            if (jointDu.enabled)
            {
                CancelThrow();
            }
            jointDu.enabled = false;
            if (isJumping)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                jump();
            }
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                changeanim("run");
                anim.SetFloat("speed", 1);
            }
            else
            {
                anim.SetFloat("speed", 0);
            }

            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                SetSpin();
            }
            if (Input.GetKeyUp(KeyCode.C) && isGrounded)
            {
                OutSpin();
            }
            if (Input.GetKeyDown(KeyCode.V) )
            {
                Pull();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.X))
            {
                UnSwing();
            }
            if (Input.GetKey(KeyCode.X) && isSticky)
            {
                SetSwing();
            }
        }
        if (!isGrounded && rb.velocity.y < 0)
        {
            changeanim("fall");
            isJumping = false;
        }
        if (Mathf.Abs(horizontal) > 0.1f && !isSwing)
        {
            rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            changeanim("idle");
            rb.velocity = Vector2.zero;
        }
        updateCheckPoint.Invoke();
    }
    public void OnInit()
    {
        transform.position = savePoint;
        rb.velocity = Vector2.zero;
        IsDead = false;
        changeanim("idle");
        SavePoint();
    }
    public override void OnDespawn()
    {
        base.OnDespawn(); 
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position-Vector3.left*0.01f, Vector2.down, 1.1f, groundedLayer);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    ///  spin
    /// </summary>

    float currentDestination;
    float i = 0;
    public void SetSpin()
    {
        CancelThrow();
        Debug.Log("setSpin");
        tweenValue = minAngle;
        rotationTween = DOTween.To(() => tweenValue, x => tweenValue = x, maxAngle, 1f) // Tween đến giá trị B trong 1 giây
        .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn và quay về giá trị A
        .OnUpdate(() =>
        {
            Debug.Log($"Current Value: {tweenValue}");
            rope.transform.localRotation = Quaternion.Euler(0, 0, tweenValue);
        })
        .SetEase(Ease.InOutSine);
    }
    public void OutSpin()
    {
        changeanim("spin");
        if (rotationTween != null) rotationTween.Kill();
        Throw();
    }
    /// <summary>
    /// EndSpin 
    /// </summary>
    public void SetSwing()
    {
        if(isSwing)
        {
            return;
        }
        isSwing = true;
        changeanim("Swing");
        jointDu.enabled = true;
        jointDu.connectedBody = bubble.GetComponent<Rigidbody2D>();
    }
    public void UnSwing()
    {
        isSwing = false;
        changeanim("Jumpout");
        jointDu.enabled = false;
        jointDu.connectedBody = null;
        CancelThrow();
    }
    private void ResetAttack()
    {
        changeanim("idle");
    }
    public void Throw()
    {

        if(isThrowing == true)
        {
            return;
        }
        if (bubble == null)
        {
            bubble = Instantiate(bubblePrefab);
        }
        bubble.transform.position = bubbleOrigin.transform.position;
        isThrowing = true;
        bubble.SetActive(true);
        Rigidbody2D rbBubble = bubble.GetComponent<Rigidbody2D>();
        rbBubble.bodyType = RigidbodyType2D.Dynamic;
        rbBubble.AddForce(rope.transform.up*throwForce, ForceMode2D.Impulse);
        Rope.instance.SetBubble(bubble.transform);
        Rope.instance.TurnOnLine();
        changeanim("throw");
    }
    public void CancelThrow()
    {
        isSticky = false;
        Rope.instance.TurnOffLine();
        rope.transform.localRotation = Quaternion.Euler(0, 0, -15);
        isThrowing = false;
        if (bubble == null)
        {
            return;
        }
        Rigidbody2D rbBubble = bubble.GetComponent<Rigidbody2D>();
        rbBubble.bodyType = RigidbodyType2D.Kinematic;
        rbBubble.velocity = Vector2.zero;
        bubble.SetActive(false);
    }
    public void Pull()
    {
        changeanim("throw");
        Invoke(nameof(ResetAttack), 0.5f);
    }
    public void jump()
    {
        isJumping = true;
        changeanim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    private void changeanim(string animname)
    {
        if (currentanimname != animname)
        {
            anim.ResetTrigger(animname);
            currentanimname = animname;
            anim.SetTrigger(currentanimname);
        }
    }
    internal void SavePoint()
    {
        timer += Time.deltaTime;
        if (timer >= timeCDCheckPoint&& isGrounded)
        {
            savePoint = transform.position;
        }
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.tag == "coin")
        //{
        //    coin++;
        //    PlayerPrefs.SetInt("coin", coin);
        //    Destroy(collision.gameObject);
        //    UIManager.instance.SetCoin(coin);
        //}
        if (collision.tag == "DeathZone")
        {
            changeanim("die");
            CancelThrow();
            Invoke(nameof(OnInit), 0.3f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.CompareTag("DeathZone"))
        {
            changeanim("die");
            CancelThrow();
            Invoke(nameof(OnInit), 0.3f);
        }
    }

    public void SetStiky()
    {
        isSticky = true;
    }
}
