using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen;
    public Collider2D passable;
    public float doorDelay;
    private float doorSwing;
    private Animator[] animations;

    public void Start()
    {
        isOpen = false;
        doorSwing = Time.time;

        animations = GetComponentsInChildren<Animator>();
    }
    private void Update()
    {
        if (isOpen)
        {
            doorSwing -= Time.deltaTime;
            if (doorSwing < 0)
                passable.enabled = false;
        }
        else
        {
            doorSwing -= Time.deltaTime;
            if (doorSwing < 0)
                passable.enabled = true;
        }
    }

    public void updateDoor()
    {
        if (doorSwing < 0)
        {
            if(isOpen)
            { 
                doorSwing = doorDelay;
                animations[0].SetBool("Open", false);
                animations[1].SetBool("Open", false);
                isOpen = false;
            }
            else
            {
                doorSwing = doorDelay;
                animations[0].SetBool("Open", true);
                animations[1].SetBool("Open", true);
                isOpen = true;
            }
        }
    }


}
