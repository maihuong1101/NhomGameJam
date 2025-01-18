using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sr;
    private void Awake()
    {
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
            StartCoroutine(BoomBoom(collision.CompareTag("dontStick") || collision.CompareTag("DeathZone") ? true : false));

        }
    }
    private IEnumerator BoomBoom(bool isdontstick)
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
        }
    }
}
