using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpingPower = 12f;
    [SerializeField] float climbSpeed = 6f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private float horizontal;
    private float vertical;
    private bool isAlive = true;
    private bool isLadder;
    private bool isClimbing;

    Rigidbody2D rb;
    Animator animator;
    Collider2D collider2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        Run();
        Jump();
        Flip();
        Climbing();
    }

    private void Run()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        bool horizontalMovement = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Running", horizontalMovement);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Climbing()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;

        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
            bool verticalMovement = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
            animator.SetBool("Climbing", verticalMovement);
        }
        else
        {
            rb.gravityScale = 3f;
            animator.SetBool("Climbing", false);
        }


    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        bool isFacingRight = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if(isFacingRight)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}
