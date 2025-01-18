using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    // Start is called before the first frame update
    private float hp;
    public string currentanimname;
    public bool IsDead;
    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }
    protected void ChangeAnim(string animname)
    {
        if (currentanimname != animname)
        {
            anim.ResetTrigger(animname);
            currentanimname = animname;
            anim.SetTrigger(currentanimname);
        }
    }
    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }
        }
    }  
}
