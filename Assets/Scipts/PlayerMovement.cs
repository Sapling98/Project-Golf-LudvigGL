using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpForce = 8.0f;
    public int amountOfJumps = 1;
    public float groundCheckRadius;
    public LayerMask layerMask;

    private float moveHorizontal;
    private Rigidbody2D m_Rigidbody;
    private int amountOfJumpsLeft;
    private bool canJump;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("Movement: " + moveHorizontal);
        if (moveHorizontal != 0 && isGrounded)
        {
            ApplyInput();
        }
        else
        {
            float tempSpeed = speed;
            if (!isGrounded && (m_Rigidbody.velocity.x <= 0.01f || m_Rigidbody.velocity.x >= -0.01f))
            {
                speed = 15f;
                ApplyInput();
                speed = tempSpeed;
            }
        }

        CheckIfGrounded();
        CheckCanJump();
    }

    private void CheckCanJump()
    {
        if (isGrounded && m_Rigidbody.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckIfGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, groundCheckRadius, layerMask);
        isGrounded = hit != null;
    }

    private void Jump()
    {
        if (canJump)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    private void ApplyInput()
    {
        float xInput = moveHorizontal * speed * Time.fixedDeltaTime;
        Vector2 force = new Vector2(xInput, 0);
        Debug.Log("AddForce" + force);
        m_Rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
