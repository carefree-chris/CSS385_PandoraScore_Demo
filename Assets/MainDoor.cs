using System.Collections;
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
        Debug.Log("current key count " + currentKeys);
        Debug.Log("required keys " + keysRequired);
        if (currentKeys >= keysRequired)
        {
            //DO THINGS!!!
            Debug.Log("The Door Opens..");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentKeys = collision.gameObject.GetComponent<Player>().keysHeld;
        }

            
    }
}
