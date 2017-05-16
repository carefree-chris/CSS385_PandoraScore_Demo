using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour {
    public float maxSpeed;
    public float startSpeed;
    public bool loop;
    private Vector3 startPosition; 

    // Use this for initialization
    void Start()
    {
        loop = false;
        maxSpeed = 2f;
        startSpeed = 12f;
        
        startPosition = new Vector3(transform.position.x, transform.position.y+11f, transform.position.z);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loop) {
            MoveVertical();
        }
        else {
            Movedown();
            if (transform.position.y < startPosition.y-11f) {
                startPosition.y = startPosition.y - 11f;
                loop = true;
            }
        }


    }

    void Movedown()
    {
        transform.position = new Vector3(transform.position.x, startPosition.y - Time.time * startSpeed, transform.position.z);
    }

    void MoveVertical()
    {
        transform.position = new Vector3(transform.position.x, startPosition.y + Mathf.Sin(Time.time * maxSpeed)*0.1f, transform.position.z);

        if (transform.position.y > 1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < -1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
