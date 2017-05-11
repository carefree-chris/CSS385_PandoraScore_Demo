using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    public bool containsKey;
    public Color color;

    private void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }


    private void Update()
    {
        updateSprite();
        Debug.Log(containsKey);
    }

    private void updateSprite()
    {
        if (containsKey)
        {
            color = Color.yellow;
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            color = Color.black;
            GetComponent<SpriteRenderer>().color = color;
        }
    }


}
