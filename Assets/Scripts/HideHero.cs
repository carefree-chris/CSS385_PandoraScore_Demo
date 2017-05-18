using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHero : MonoBehaviour
{
    public GameObject Hero;
    private bool isHiding = false;

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && isHiding)
        {
            LeaveObject();
        }
    }

    public bool GetIsHiding()
    {
        return isHiding;
    }

    public void HideInObject()
    {
        isHiding = true;
        Hero.SetActive(false);
    }

    public void LeaveObject()
    {
        isHiding = false;
        Hero.SetActive(true);
    }


}
