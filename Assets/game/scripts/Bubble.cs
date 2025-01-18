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

        if(isdontstick)
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
                if (joint == null)
                    joint = target.AddComponent<DistanceJoint2D>();
                else
                {
                    joint.enabled = true;
                }

                joint.connectedBody = Player.instance.rb;
                joint.autoConfigureDistance = false;
            }
        }
    }
    public void Pull()
    {
        joint.enabled = true;
        joint.distance -= 0.01f;
        joint.enabled = false;
    }
    public void Destroypull()
    {
        joint.enabled = false;
    }
}
