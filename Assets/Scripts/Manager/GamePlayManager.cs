using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Transform lastPost;
    private float timer;
    private void Awake()
    {
        Time.timeScale = 1.0f;
        timer = 0f;
    }
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!UIManager.Instance.isPause)
                UIManager.Instance.GamePause();
            else
                UIManager.Instance.GameResume();
        }
    }
}
