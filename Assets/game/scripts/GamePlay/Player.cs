using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : Character
{
    
    public static Player instance;
    public Rigidbody2D rb;
    //    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private int Speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private float throwForce = 20;
    [SerializeField] private Transform throwpoint;
    [SerializeField] private GameObject rope;
    [SerializeField] private GameObject locXoay;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bubbleOrigin;
    [SerializeField] private DistanceJoint2D jointDu;
    [SerializeField] private forcebar forcebar;
    private bool isGrounded = true;
    private bool isJumping = false; 
    private Tween rotationTween;
    private Tween fillTween;
    private bool isSwing = false;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundCheckLayer;
    public float groundCheckRadius = 0.4f;

    private GameObject bubble;
    private bool isSpin = false;
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
    private float tweenValueFill; // Giá trị sẽ tween

    Action updateCheckPoint;
    float timer = 0;
    float timeCDCheckPoint = 0;

    private void Awake()
    {
        savePoint = transform.position;
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
            if (Input.GetKey(KeyCode.V) )
            {
                Pull();
            }
            if (Input.GetKeyUp(KeyCode.V))
            {
                UnPull();
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
            isJumping = false;
        }
        if (Mathf.Abs(horizontal) > 0.1f && !isSwing)
        {
            rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            anim.SetFloat("Blend", 1);
        }
        else if (isGrounded)
        {
            anim.SetFloat("Blend", 0);
            if(!isThrowing&& !isJumping && ! isSpin)
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
        CancelThrow();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);

    }
    private bool CheckGrounded()
    {
        if(Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundCheckLayer))
        {
            return true;
        }
        else return false;

    }
    /// <summary>
    ///  spin
    /// </summary>


    float currentDestination;
    float i = 0;
    public void SetSpin()
    {
        CancelThrow();
        tweenValue = minAngle;
        rotationTween = DOTween.To(() => tweenValue, x => tweenValue = x, maxAngle, 1f) // Tween đến giá trị B trong 1 giây
        .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn và quay về giá trị A
        .OnUpdate(() =>
        {
            ChangeAnim("spin");
            rope.transform.localRotation = Quaternion.Euler(0, 0, tweenValue);
        })
        .SetEase(Ease.InOutSine);
        locXoay.SetActive(true);
        tweenValueFill = 1; 
        fillTween = DOTween.To(() => tweenValueFill, x => tweenValueFill = x, 0, 1f) // Tween đến giá trị B trong 1 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn và quay về giá trị A
            .OnUpdate(() =>
            {
                forcebar.SetForce(tweenValueFill);
            })
            .SetEase(Ease.InOutSine);
        forcebar.gameObject.SetActive(true);
        isSpin = true;
    }
    public void OutSpin()
    {
        forcebar.gameObject.SetActive(false);
        changeanim("throw");
        if (rotationTween != null) rotationTween.Kill();
        if (fillTween != null) fillTween.Kill();
        Throw();
        locXoay.SetActive(false);
        isSpin = false;
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
        Destroy(bubble);
    }

    public void Pull()
    {
        if(Bubble.instance== null || Bubble.instance.allowPull != true)
        {
            return;
        }
        Bubble.instance.Pull();
        changeanim("pull"); 
    }
    public void UnPull()
    {
        if (Bubble.instance == null)
        {
            return;
        }
        CancelThrow();
        Bubble.instance.Destroypull();
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
