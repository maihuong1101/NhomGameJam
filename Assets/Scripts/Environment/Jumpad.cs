//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Jumpad : MonoBehaviour
//{
//    [SerializeField] private float force;

//    private SpriteRenderer sr;
//    Color original;

//    void Start()
//    {
//        sr = GetComponent<SpriteRenderer>();
//        original = sr.color;
//    }
//        private void OnTriggerEnter2D(Collider2D collision)
//        {
//            if (collision.CompareTag("Player"))
//            {
//                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force,
//                ForceMode2D.Impulse);
//                Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().velocity);
//                sr.color = Color.red;
//            }
//        }
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        sr.color = original;
//    }
//}
