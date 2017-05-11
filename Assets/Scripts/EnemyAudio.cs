using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {

    private AudioSource a;
    private float T = 2;

	// Use this for initialization
	void Start () {
        a = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if (T < 0)
            {
                a.Play();
                T = 2;
            }
            Debug.Log(T);
            T -= Time.deltaTime;
        }
	}
}
