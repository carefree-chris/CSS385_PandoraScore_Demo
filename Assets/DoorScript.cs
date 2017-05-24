using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen;
    public Collider2D passable;
    public float doorDelay;
    private float doorSwing;

    public void Start()
    {
        isOpen = false;
        doorSwing = Time.time;
    }
    private void Update()
    {
        if (isOpen)
        {
            passable.enabled = false;
        }
        else
        {
            passable.enabled = true;
        }
    }

    public void updateDoor()
    {
        if (Time.time > doorSwing)
        {
            if(isOpen)
            { 
                doorSwing = Time.time + doorDelay;
                isOpen = false;
            }
            else
            {
                doorSwing = Time.time + doorDelay;
                isOpen = true;
            }
        }
    }


}
