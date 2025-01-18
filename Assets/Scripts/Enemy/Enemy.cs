using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected PlayerController player;
    public float speed;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;


    protected virtual void Awake()
    {
        player = PlayerController.Instance;
    }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {

        }
    }
}
