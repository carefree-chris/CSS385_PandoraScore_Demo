using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    public GameObject EnemyMM;
    public GameObject Enemy;
    public Camera mm;

	// Use this for initialization
	void Start () {
        mm.rect = new Rect(new Vector2(.75f, .75f), new Vector2(.25f, .25f));
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            Vector3 v3 = Input.mousePosition;
            v3 = mm.ScreenToWorldPoint(v3);
            v3.z = -1;

            EnemyMM.transform.position = v3;
        }

        float x = EnemyMM.transform.position.x * 1000;
        float y = ((EnemyMM.transform.position.y - 1000) * 1000/1.5f) - 2000;

        Enemy.transform.position = new Vector3(x, y, 0);
    }
}
