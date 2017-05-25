using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour {

    protected int keysRequired;
    protected int currentKeys;
    private bool win = false;
    private float winTimer = 2;

    private void Start()
    {
        keysRequired = 4;
        currentKeys = 0;
    }

    private void Update()
    {
        if(win == true)
        {
            if(winTimer < 0)
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI>().WinGame();
            winTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("The Player is at the door");
            currentKeys = collision.gameObject.GetComponent<Player>().keysHeld;
            if (Input.GetButtonDown("Jump"))
            {
                if (currentKeys >= keysRequired)
                {
                    //DO THINGS!!!
                    Debug.Log("The Door Opens..");
                    GetComponentInChildren<Animator>().SetBool("Open", true);
                    win = true;
                }
            }
        }
    }
}
