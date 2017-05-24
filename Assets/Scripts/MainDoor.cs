﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour {

    protected int keysRequired;
    protected int currentKeys;

    private void Start()
    {
        keysRequired = 4;
        currentKeys = 0;
    }

    private void Update()
    {
        //currentKeys = GameObject.Find("Player").GetComponent<Player>().keysHeld;
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
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI>().WinGame();
                }
            }
        }
    }
}
