using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{
    private Animator animator;
    public GameObject ItemCreate;

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
        animator = GetComponentInChildren<Animator>();

        color = childSprite.GetComponent<SpriteRenderer>().color;
        original = childSprite.GetComponent<SpriteRenderer>().color;
        childSprite = transform.GetChild(0);
    }


    private void Update()
    {
        //updateSprite();
        //Debug.Log(contents);
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

    public void open()
    {
        animator.SetBool("Open", true);

        if (contents == itemCode.Cookie)
        {
            GameObject a = GameObject.Instantiate(ItemCreate);
            a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            a.GetComponent<ItemFloat>().run("Cookie");
        }
        else if (contents == itemCode.Potion)
        {
            GameObject a = GameObject.Instantiate(ItemCreate);
            a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            a.GetComponent<ItemFloat>().run("Potion");
        }
        else if (contents == itemCode.Key)
        {
            GameObject a = GameObject.Instantiate(ItemCreate);
            a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            a.GetComponent<ItemFloat>().run("Key");
        }
        else
        {
            GameObject a = GameObject.Instantiate(ItemCreate);
            a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            a.GetComponent<ItemFloat>().run("GoldBar");
        }
    }
}
