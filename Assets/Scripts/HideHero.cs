using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHero : MonoBehaviour
{
    public GameObject Hero;
    

    private void Update()
    {
            if (Input.GetButtonUp("Jump"))
            {
                Hero.SetActive(true);
            }
    }
}
