using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int keysHeld;
    public int cookiesHeld;
    public int potionsHeld;
    public int goldHeld;

    //For Determining Object bases
    private int objectScalar = 1;
    
    private Animator animator;
    //private BoxCollider2D coll;

    public float walkSpeed;
    public float sneakSpeed;
    public float runSpeed;
    private float speed;

    private bool invisibility = false;
    public float fadeTimer;
    public GameObject Distraction;
     
    public enum moveState
    {
        sneak,
        walk,
        run,
        idle,
        hiding,
        searching
    }
    private moveState motion;

    //private GameObject mainCamera;

    //Collision with Object Interaction
    private enum nextTo
    {
        hide,
        search,
        open
    }
    private nextTo proximity;

    public SpriteRenderer childSprite;

    // Use this for initialization
    void Start () {
        animator = GetComponentInChildren<Animator>();
        //coll = GetComponent<BoxCollider2D>();
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        proximity = nextTo.open;
        motion = moveState.idle;
        childSprite = GetComponentInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Fire1") && !Input.GetButton("Fire2")) //Sneak
        {
            motion = moveState.sneak;
            speed = sneakSpeed;
            //rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * sneakSpeed);
        }
        else if (Input.GetButton("Fire2") && !Input.GetButton("Fire1")) //Run
        {
            motion = moveState.run;
            speed = runSpeed;
            //rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * runSpeed);
        }
        else if (vertical != 0 || horizontal != 0)
        {
            motion = moveState.walk;
            speed = walkSpeed;
            //rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed);
        }
        else if (proximity == nextTo.hide && Input.GetButtonDown("Jump"))
        {
            motion = moveState.hiding;
        }
        else if (motion == moveState.hiding)
        {
            if (Input.GetButtonDown("Jump"))
            {
                motion = moveState.idle;
            }
        }
        else
        {
            motion = moveState.idle;
        }

        if (vertical > 0)
        {
            //if (!coll.IsTouchingLayers(LayerMask.GetMask("Environment")))
            //{
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * vertical * Time.deltaTime, transform.position.z);
            //}
            animator.SetInteger("Direction", 2);
            animator.SetBool("IsMoving", true);
        }
        if (vertical < 0)
        {
            //if (!coll.IsTouchingLayers(LayerMask.GetMask("Environment")))
            //{
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * vertical * Time.deltaTime, transform.position.z);
            //}
            animator.SetInteger("Direction", 0);
            animator.SetBool("IsMoving", true);
        }
        if (horizontal < 0)
        {
            //if (!coll.IsTouchingLayers(LayerMask.GetMask("Environment")))
            //{
                transform.position = new Vector3(transform.position.x + speed * horizontal * Time.deltaTime, transform.position.y, transform.position.z);
            //}
            animator.SetInteger("Direction", 1);
            animator.SetBool("IsMoving", true);
        }
        if (horizontal > 0)
        {
            //if (!coll.IsTouchingLayers(LayerMask.GetMask("Environment")))
            //{
                transform.position = new Vector3(transform.position.x + speed * horizontal * Time.deltaTime, transform.position.y, transform.position.z);
            //}
            animator.SetInteger("Direction", 3);
            animator.SetBool("IsMoving", true);
        }
        if(horizontal + vertical == 0)
        {
            animator.SetBool("IsMoving", false);
        }

        if (motion == moveState.sneak)
        {
            animator.speed = .4f;
        }
        else if (motion == moveState.walk)
        {
            animator.speed = .9f;
        }
        else if (motion == moveState.run)
        {
            animator.speed = 1.4f;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            keysHeld++;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y % 50 * .02f);

        useCookie();
        goInvisible();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        proximity = nextTo.open;
    }

    //Changes State Based on if touching Special Objects
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.transform.position);
        //Debug.Log(animator.GetInteger("Direction"));

        //Scalar for Object Size
        if ((collision.transform.position.x * 10) % 10 != 0 && (collision.transform.position.y * 10) % 10 == 0)
        {
            objectScalar = 1;
        }
        else
        {
            objectScalar = 2;
        }

        //Directional Facing Cases
        if ((animator.GetInteger("Direction") == 1 //Facing Left
            && collision.gameObject.transform.position.x < this.gameObject.transform.position.x
            && Mathf.Abs(collision.gameObject.transform.position.y - this.gameObject.transform.position.y) < 2.25f * objectScalar)
            || (animator.GetInteger("Direction") == 3 //Facing Right
            && collision.gameObject.transform.position.x > this.gameObject.transform.position.x
            && Mathf.Abs(collision.gameObject.transform.position.y - this.gameObject.transform.position.y) < 2.25f * objectScalar)
            || (animator.GetInteger("Direction") == 0 //Facing Down
            && collision.gameObject.transform.position.y < this.gameObject.transform.position.y
            && Mathf.Abs(collision.gameObject.transform.position.x - this.gameObject.transform.position.x) < 2.25f * objectScalar)
            || (animator.GetInteger("Direction") == 2 //Facing Up
            && collision.gameObject.transform.position.y > this.gameObject.transform.position.y
            && Mathf.Abs(collision.gameObject.transform.position.x - this.gameObject.transform.position.x) < 2.25f * objectScalar))
        {
            if (collision.gameObject.tag == "HideObject")
            {
                if (Input.GetButton("Jump"))
                {
                    motion = moveState.hiding;

                    collision.gameObject.GetComponent<HideHero>().Hero = this.gameObject;
                    collision.gameObject.GetComponent<HideHero>().HideInObject();
                    //gameObject.SetActive(false);
                }
                proximity = nextTo.hide;
            }
            if (collision.gameObject.tag == "SearchObject")
            {
                if (Input.GetButton("Jump"))
                {
                    searchItem(collision);
                    proximity = nextTo.search;
                }
            }

        }
        //if (collision.gameObject.tag == "KeyObject")
        //{
        //    bool hasKeyInside = collision.gameObject.GetComponent<KeyObject>().containsKey;
        //    if (hasKeyInside && Input.GetButton("Jump"))
        //    {
        //        collision.gameObject.GetComponent<KeyObject>().containsKey = false;
        //        collision.gameObject.GetComponent<SearchObject>().open();
        //        keysHeld++;
        //        mainCamera.GetComponent<UI>().keyNum.text = "" + keysHeld;
        //        Debug.Log("Amount of Keys" + keysHeld);
        //    }
        //}
    }
    

    private void goInvisible()
    {
        float currentTime = Time.time;
        if (currentTime > fadeTimer)
        {
            invisibility = false;
            childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, 1.0f);
        }

        if (Input.GetButtonDown("Fire3") && potionsHeld > 0)
        {
            if (invisibility == false)
            {
                potionsHeld--;
                invisibility = true;
                childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, .5f);
                fadeTimer = Time.time + 10;
            }
        }
    }

    public bool IsInvisible()
    {
        return invisibility;
    }

    private void useCookie()
    {
        if (Input.GetButtonDown("Submit") && cookiesHeld > 0)
        {
            cookiesHeld--;
            GameObject Decoy = Instantiate(Distraction, transform.position, Quaternion.identity);
        }
    }

    //Search Items
    private void searchItem(Collision2D searching)
    {
        if (proximity == nextTo.search)
        {
            if (searching.gameObject.GetComponent<SearchObject>().isOpen == false)
            {
                //motion = moveState.searching;
                searching.gameObject.GetComponent<SearchObject>().open();

                if (searching.gameObject.GetComponent<SearchObject>().contents == SearchObject.itemCode.Cookie)
                {

                    cookiesHeld++;
                    searching.gameObject.GetComponent<SearchObject>().contents = SearchObject.itemCode.Empty;
                }
                else if (searching.gameObject.GetComponent<SearchObject>().contents == SearchObject.itemCode.Potion)
                {
                    potionsHeld++;
                    searching.gameObject.GetComponent<SearchObject>().contents = SearchObject.itemCode.Empty;
                }
                else if (searching.gameObject.GetComponent<SearchObject>().contents == SearchObject.itemCode.Empty)
                {
                    goldHeld += searching.gameObject.GetComponent<SearchObject>().gold;
                    searching.gameObject.GetComponent<SearchObject>().contents = SearchObject.itemCode.Empty;
                }

                else if (searching.gameObject.GetComponent<SearchObject>().contents == SearchObject.itemCode.Key)
                {
                    keysHeld++;
                    searching.gameObject.GetComponent<SearchObject>().contents = SearchObject.itemCode.Empty;
                }
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    foreach(ContactPoint2D c in collision.contacts)
    //    {
    //        Vector3 cv3 = new Vector3(c.point.x, c.point.y, transform.position.z);
    //        float dist = 50 - Vector3.Distance(transform.position, cv3);
    //        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(c.normal.x * dist, c.normal.y * dist, 0), .8f);
    //    }
    //}

    public moveState getMoveState()
    {
        return motion;
    }
}
