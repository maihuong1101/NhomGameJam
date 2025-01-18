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
            transisionAnim.SetTrigger("End");
            Invoke("BackToNormal", 1f);
        }
    }
    void BackToNormal()
    {
        transisionAnim.SetTrigger("Start");
        Player.instance.transform.position = nextPos.position;
    }
    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(1f);
    }
}
