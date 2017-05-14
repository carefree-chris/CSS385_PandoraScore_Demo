using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    //Maximum Movement Values
    public float walkSpeed;
    public float sneakSpeed;
    public float runSpeed;
    private Vector3 movement;
    private Rigidbody2D rb;

    //Finite States
    private enum moveState
    {
        sneak, 
        walk,
        run,
        idle,
        hiding,
        searching
    }
    private moveState motion;

    //Collision with Object Interaction
    private enum nextTo
    {
        hide,
        search,
        open
    }
    private nextTo proximity;

    //DISPLAY
    public Text NextToHero;
    public Text MotionState;

    //Invisibiliy attempt
    SpriteRenderer visible;

    //Hero Instantiation
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        proximity = nextTo.open;
        motion = moveState.idle;
        visible = GetComponent<SpriteRenderer>();

        NextToHero.text = "Next to ";
        MotionState.text = "Activity ";
    }

    //Update 
    private void FixedUpdate()
    {
        updateMovement();
        goInvisible();
        searchItem();

        //DISPLAY
        NextToHero.text = "Next To " + proximity.ToString();
        MotionState.text = "Activity " + motion.ToString();

        //World Clamp
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp01(position.x);
        position.y = Mathf.Clamp01(position.y);
        transform.position = Camera.main.ViewportToWorldPoint(position);
    }
    
    //Changes Movement Speed
    private void updateMovement()
    {
        //Calculate Direction
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        movement = new Vector3(x, y, 0.0f);
        
        //ROTATION
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(transform.forward, movement), Time.smoothDeltaTime * 10);
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
            rb.angularVelocity = 0;
        } 

        Debug.Log(motion);
        Debug.Log(proximity);


        //Movement
        if (Input.GetButton("Fire1") && !Input.GetButton("Fire2")) //Sneak
        {
            motion = moveState.sneak;
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * sneakSpeed);
        }
        else if (Input.GetButton("Fire2") && !Input.GetButton("Fire1")) //Run
        {
            motion = moveState.run;
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * runSpeed);
        }
        else if (movement.magnitude != 0)
        {
            motion = moveState.walk;
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed);
        }
        else if (proximity == nextTo.hide && Input.GetButton("Jump"))
        {
            motion = moveState.hiding;
        }
        else
        {
            motion = moveState.idle;
        }
    }

    //Checks if Away from Objects
    private void OnCollisionExit2D(Collision2D collision)
    {
        proximity = nextTo.open;
    }

    //Changes State Based on if touching Special Objects
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HideObject")
        {
            proximity = nextTo.hide;
        }

        if (collision.gameObject.tag == "SearchObject")
        {
            proximity = nextTo.search;
        }
        /*
        if (collision.gameObject.tag == "UseObject")
        {

        }
        */
    }

    private void goInvisible()
    {

        //INVISIBILITY
        if (motion == moveState.hiding)
        {
            visible.enabled = false;
        }
        else
        {
            visible.enabled = true;
        }
    }

    private void searchItem()
    {
        if (proximity == nextTo.search && Input.GetButton("Jump"))
        {
            motion = moveState.searching;
        }
    }
} 
