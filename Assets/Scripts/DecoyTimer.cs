using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTimer : MonoBehaviour
{
    public float lifespan;

    private void Awake()
    {
        GameObject.Destroy(this.gameObject, lifespan);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButton("Jump"))
            {
                collision.gameObject.GetComponent<Player>().cookiesHeld++;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
