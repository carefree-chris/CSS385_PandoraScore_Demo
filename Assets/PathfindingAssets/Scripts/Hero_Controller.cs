using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Controller : MonoBehaviour {
    
    
    [SerializeField] private float heroSpeed = 5f;
    [SerializeField] private GameObject cookie;

	void Update () {
        

        if (Input.GetAxis("Horizontal") > 0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        if (Input.GetAxis("Horizontal") < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Input.GetButton("Vertical"))
        {
            transform.position += transform.up * heroSpeed * Input.GetAxis("Vertical") * Time.smoothDeltaTime;
        }

        if (Input.GetButton("Horizontal"))
        {
            transform.position += transform.right * heroSpeed * Input.GetAxis("Horizontal") * Time.smoothDeltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(cookie, transform.position, transform.rotation);
        }

    }
}
