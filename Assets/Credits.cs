using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    public GameObject Cred;
    public GameObject Logo;

	// Use this for initialization
	void Start () {
        Cred.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        Cred.gameObject.SetActive(true);
        Logo.gameObject.SetActive(false);
    }

    public void Close()
    {
        Cred.gameObject.SetActive(false);
        Logo.gameObject.SetActive(true);
    }
}
