using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour {

    private GameObject[] sprites;
    private bool isRunning = false;
    public float timer = 2;
    private Vector3 endPos;
    private SpriteRenderer spRend;
    private string[] names;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(isRunning == true)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, .2f);

            if(timer > 1)
            {
                spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, (2f - timer) * 1f);
            }
            else
            {
                spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, 1);
            }

            if(timer < 1)
            {
                if (timer > .75)
                {
                    spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, 0);
                }
                else if (timer > .5)
                {
                    spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, 1);
                }
                else if (timer > .25)
                {
                    spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, 0);
                }
                else
                {
                    spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, 1);
                }
            }

            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            GameObject.Destroy(gameObject);
        }
	}

    public void run(string name)
    {
        sprites = new GameObject[transform.childCount];
        names = new string[transform.childCount];

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = transform.GetChild(i).gameObject;
            sprites[i].GetComponent<SpriteRenderer>().color = new Color(sprites[i].GetComponent<SpriteRenderer>().color.r, sprites[i].GetComponent<SpriteRenderer>().color.g, sprites[i].GetComponent<SpriteRenderer>().color.b, 0);
            names[i] = sprites[i].name;
            sprites[i].SetActive(false);
        }

        endPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);


        for (int i = 0; i < names.Length; i++)
        {
            if (name == names[i])
            {
                sprites[i].SetActive(true);
                isRunning = true;
                spRend = sprites[i].GetComponent<SpriteRenderer>();
            }
        }
    }
}
