using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScp : MonoBehaviour
{

    [SerializeField] Animator transisionAnim;
    [SerializeField] private Transform nextPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transisionAnim.SetBool("transit", true);
            
            Player.instance.transform.position = nextPos.position;
            Invoke("BackToNormal", 2f);
        }
    }
    void BackToNormal()
    {
        transisionAnim.SetBool("transit", false);
    }
    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(1f);
    }
}
