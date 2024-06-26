using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed;
    private int maxJumps = 2;
    private int jumpsLeft;
    public bool gameOver = false;
    public int jumpForce;
    public int MoveDirection;
    private bool CDTimerOff = true;
    private bool CanSlam;
    private float dragSpeed;
    private Animator anim;
    bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        jumpsLeft = maxJumps;
        gameOver = false;
        MoveDirection = 1;
        CanSlam = false;
        anim = GetComponent<Animator>();
}

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        //resets double jumps
        if (collision.gameObject.tag == "Ground" && gameOver == false)
        {
            
            jumpsLeft = maxJumps;
            CanSlam = false;
            dragSpeed = -3.5f;
            bool isJumping = false;



        }
      

        else
        {
           
        }


        // Update is called once per frame
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            gameOver = true;
        }
        //ends the game upon making contact with an object
    }


    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        Move(15);

        //Jump Button
        KeyCode.W = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.W) && jumpsLeft > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            jumpsLeft -= 1;
            CanSlam = true;
            dragSpeed = 0;
            isJumping = true;



        }
        if (Input.GetKeyDown(KeyCode.S) && CanSlam == true)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.AddForce(transform.up * -2*jumpForce, ForceMode2D.Impulse);
           



        }
        if(bool isJumping == true)
        {
            anim.SetBool("IsJumping", true);

        }
        else
        {
            anim.SetBool("IsJumping", false);
        }







        //Dash Input
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Dash(.75f));
            StartCoroutine(CDTimer());

        }

        //ends the game if the player goes out of bounds
        if (transform.position.y < -10 || transform.position.x < -15)
        {
            gameOver = true;
        }
        //locks the character in place once the game ends
        if (gameOver == true)
        {
            movementSpeed = 0;
            jumpForce = 0;
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
        }
        transform.Translate(dragSpeed * Time.deltaTime, 0, 0);





    }

    //Handles horizontal movement and deaccerlation
    private void Move(int deacceleration)
    {
        if (Input.GetKey(KeyCode.D))
        {
            movementSpeed += 4 * Time.deltaTime;
            if (movementSpeed > 7)
            {
                movementSpeed = 7;
            }
            MoveDirection = 1;


        }
        if (Input.GetKey(KeyCode.A))
        {
            movementSpeed -= 4 * Time.deltaTime;

            if (movementSpeed < -7)
            {
                movementSpeed = -7;
            }

            MoveDirection = -1;

        }
        //slows the character if no buttons are being pressed
        if (!(Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.S)))
        {
            if (movementSpeed >= 0)
            {
                movementSpeed -= deacceleration * Time.deltaTime;
            }
            if (movementSpeed <= 0)
            {
                movementSpeed += deacceleration * Time.deltaTime;
            }

        }
    }
    //Dash Procedures, tempoariily increases movement speed for a brief period of time.
    private IEnumerator Dash(float timer)
    {
    if (CDTimerOff == true)
        {
            CDTimerOff = false;
            movementSpeed = 15 * MoveDirection;
            yield return new WaitForSeconds(timer);
            
        }
        

    }
    //Cooldown timer for the dash
    private IEnumerator CDTimer()
    {
        yield return new WaitForSeconds(2);
        CDTimerOff = true;

    }
   


}
