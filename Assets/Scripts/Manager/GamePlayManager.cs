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
        //SaveLastPos();
        //RespawnPlayer(Player);
    }
    //private void SaveLastPos()
    //{
    //    if (PlayerController.Instance.isGrounded)
    //    {
    //        timer += Time.deltaTime;
    //        if (timer > 2)
    //        {
    //            lastPost = PlayerController.Instance.transform;

    //            Debug.Log(lastPost.position);

    //            timer = 0;
    //        }
    //    }
    //}
    //private void RespawnPlayer(GameObject player)
    //{
    //    if (!PlayerController.Instance.isAlive)
    //    {
    //        player.transform.position = lastPost.position;
    //        player.transform.rotation = lastPost.rotation;
    //    }
    //}

}
