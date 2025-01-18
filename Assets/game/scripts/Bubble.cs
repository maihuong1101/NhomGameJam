using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sr;
    public bool allowPull = false;
    public static Bubble instance;
    public GameObject target; 
    private DistanceJoint2D joint;
    public GameObject anim;
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            target = collision.gameObject;
            StartCoroutine(BoomBoom(collision.CompareTag("dontStick") || collision.CompareTag("DeathZone") ? true : false, collision.tag));

        }
    }
    private IEnumerator BoomBoom(bool isdontstick, string tag)
    {
        sr.enabled = false;
        anim.SetActive(true);
        if (isdontstick)
        {
            Player.instance.CancelThrow();
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);

        }
        else
        {
            Player.instance.SetStiky();
            if (tag == "allowpull")
            {
                allowPull = true;
                rb.bodyType = RigidbodyType2D.Kinematic;
                transform.parent = target.transform;
            }
        }
    }
    public void Pull()
    {
        target.transform.position = Vector2.MoveTowards(target.transform.position, Player.instance.transform.position, 0.01f);
    }
    public void Destroypull()
    {
        Destroy(gameObject);
    }
}
