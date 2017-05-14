using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHero : MonoBehaviour
{
    public GameObject Hero;
    public bool hiding_hero = false;
    

    private void FixedUpdate()
    {
        if (hiding_hero == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Hero.SetActive(true);
                hiding_hero = false;
            }
        }
    }
}
