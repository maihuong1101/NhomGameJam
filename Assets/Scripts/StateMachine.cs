using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static StateMachine instance; 
    [SerializeField] private state currentState;
    [SerializeField] private state defaultState;
    [SerializeField] private List<GameObject> listStateObject = new List<GameObject>();
    [SerializeField] private List<Animator> animators = new List<Animator>();

    [SerializeField] bool idle = false;
    [SerializeField] bool idle_xoay = false;
    [SerializeField] bool runing = false;
    public void changeState(state setState)
    {
        listStateObject[(int)currentState].SetActive(false);
        listStateObject[(int)setState].SetActive(true);
        currentState = setState;
        animators[(int)currentState].SetTrigger("Play");
    }
    public void changeIdle()
    {
        changeState(state.Idle);
    }
    public void changeIdle_Xoay()
    {
        changeState(state.Idle_Xoay);
    }
    public void changeRuning()
    {
        changeState(state.Runing);
    }
    public void Start()
    {
        for(int i = 0; i < listStateObject.Count; i++)
        {
            listStateObject[i].SetActive(false);
        }
        changeState(defaultState);
    }

    //// tesst anim 
    //public void Update()
    //{
    //    if(idle)
    //    {
    //        changeIdle();
    //        idle = false;
    //    }
    //    if (idle_xoay)
    //    {
    //        changeIdle_Xoay();
    //        idle_xoay = false;
    //    }
    //    if (runing)
    //    {
    //        changeRuning();
    //        runing = false;
    //    }
    //}
}
public enum state
{
    Idle,
    Idle_Xoay,
    Runing,
    Runing_Xoay,
    Jumping,
    Xoay,
    Nem,
    DuGiay
}