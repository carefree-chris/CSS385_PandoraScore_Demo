using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHero : MonoBehaviour
{
    public GameObject Hero;
    

    private void FixedUpdate()
    {
            if (Input.GetButtonDown("Jump"))
            {
                Hero.SetActive(true);
            }
    }
}
