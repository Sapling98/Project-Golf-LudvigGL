using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float racastDistance = 0.1f;
    public LayerMask groundLayer;

    private bool isGrounded = false;
    private bool canJump;
    private float moveX;
    private Rigidbody2D rb;

    public int amoutOfJumps = 1;
    private int amoutOfJumpsLeft;


    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amoutOfJumpsLeft = amoutOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCanJump();
        moveX = Input.GetAxisRaw("Horizontal");        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            Jump();
        }
    }

    private void CheckIfCanJump()
    {

        if (isGrounded && rb.velocity.y <= 0)
        {
            amoutOfJumpsLeft = amoutOfJumps;
        }

        if(amoutOfJumpsLeft <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
          if (moveX != 0)
        {
            rb.AddForce(new Vector2 (moveX * moveSpeed, 0f));
        }
    }

    private void Jump()
    {
        if (canJump)
        {   
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            amoutOfJumpsLeft--;
        }
        

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, racastDistance, groundLayer);

        if (hit.collider != null)
        {
            Debug.Log("HitGround");
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,racastDistance);
    }
}