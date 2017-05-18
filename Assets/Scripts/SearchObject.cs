using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{
    private SoundManager soundManager;
    private Animator animator;
    public GameObject ItemCreate;
    public bool isOpen = false;
    public int gold = 1;


    public enum itemCode
    {
        Empty,
        Key,
        Cookie,
        Potion,
        Gold
    }
    //public Transform childSprite;
    public itemCode contents;
   // public Color color;
    //public Color original;


    void Awake()
    {
        soundManager = soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //color = childSprite.GetComponent<SpriteRenderer>().color;
        //original = childSprite.GetComponent<SpriteRenderer>().color;
        //childSprite = transform.GetChild(0);
    }


    private void Update()
    {
        //updateSprite();
        //Debug.Log(contents);
    }

    /*
    private void updateSprite()
    {
        if (contents == itemCode.Cookie)
        {
           // color = Color.cyan;
            //childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else if (contents == itemCode.Potion)
        {
            //color = Color.green;
            //childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else if (contents == itemCode.Key)
        {
            //color = Color.yellow;
            //childSprite.GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            //childSprite.GetComponent<SpriteRenderer>().color = original;
        }
    }
    */
    public void open()
    {
        if (isOpen == false)
        {
            animator.SetBool("Open", true);
            soundManager.RandomizeSfx(soundManager.openObject);

            if(GameObject.Find("Player").GetComponent<Player>().getMoveState() == Player.moveState.sneak)
            {
                animator.speed = .5f;
            }
            else
            {
                animator.speed = 1f;
            }

            if (contents == itemCode.Cookie)
            {
                GameObject a = GameObject.Instantiate(ItemCreate);
                a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                a.GetComponent<ItemFloat>().run("Cookie");
                soundManager.PlaySingle(soundManager.receiveCookie);
            }
            else if (contents == itemCode.Potion)
            {
                GameObject a = GameObject.Instantiate(ItemCreate);
                a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                a.GetComponent<ItemFloat>().run("Potion");
                soundManager.PlaySingle(soundManager.receivePotion);
            }
            else if (contents == itemCode.Key)
            {
                GameObject a = GameObject.Instantiate(ItemCreate);
                a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                a.GetComponent<ItemFloat>().run("Key");
                soundManager.PlaySingle(soundManager.receiveKey);
            }
            else
            {
                GameObject a = GameObject.Instantiate(ItemCreate);
                a.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                a.GetComponent<ItemFloat>().run("GoldBar");
                soundManager.PlaySingle(soundManager.receiveGold);
            }

            isOpen = true;
        }
    }
}
