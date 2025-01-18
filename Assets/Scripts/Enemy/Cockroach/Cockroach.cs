using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockroach : Enemy
{
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundCheckLayer;
    [SerializeField] private LayerMask waterCheckLayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private Transform leftBoundary; // Diem gioi han trai
    [SerializeField] private Transform rightBoundary; // Diem gioi han phai

    private bool movingRight = true; // Huong di chuyen ban dau
    private Vector3 localScale;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Grounded();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position + new Vector3(0f, 0f, 0f) +
            Vector3.down * groundCheckDistance / 2, new Vector3(boxSize.x, boxSize.y, 1));
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
            Move();
        }
    }
    private void Move()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary.position.x)
            {
                movingRight = true;
            }
        }
        Flip();
    }
    private void Flip()
    {
        if (movingRight)
        {
            localScale.x = Mathf.Abs(localScale.x);
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        transform.localScale = localScale;
    }
}
