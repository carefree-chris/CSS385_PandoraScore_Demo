using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{
    public enum itemCode
    {
        Empty,
        Key,
        Cookie,
        Potion
    }
    public Transform childSprite;
    public itemCode contents;
    public Color color;
    public Color original;

    private void Start()
    {
        color = childSprite.GetComponent<SpriteRenderer>().color;
        original = childSprite.GetComponent<SpriteRenderer>().color;
        childSprite = transform.GetChild(0);
    }


    private void Update()
    {
        updateSprite();
        Debug.Log(contents);
    }

    private void updateSprite()
    {
        if (contents == itemCode.Cookie)
        {
            color = Color.cyan;
            childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else if (contents == itemCode.Potion)
        {
            color = Color.green;
            childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else if (contents == itemCode.Key)
        {
            color = Color.yellow;
            childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            childSprite.GetComponent<SpriteRenderer>().color = original;
        }
    }


}
