using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour {

    private Camera c;
    private CameraScript camerascript;

    private string TriggerName;
    //private Rigidbody2D coll;

	// Use this for initialization
	void Start () {

        c = FindObjectOfType<Camera>();

        camerascript = c.GetComponent<CameraScript>();

        TriggerName = transform.name;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.name == "Player")
        {
            if (TriggerName == "RightTrigger")
            {
                camerascript.TransitionRoom(3);
            }
            else if (TriggerName == "LeftTrigger")
            {
                camerascript.TransitionRoom(1);
            }
            else if (TriggerName == "UpTrigger")
            {
                camerascript.TransitionRoom(2);
            }
            else if (TriggerName == "DownTrigger")
            {
                camerascript.TransitionRoom(0);
            }
            else
            {
                Debug.LogError("Player Trigger Error");
            }
        }
    }
}
