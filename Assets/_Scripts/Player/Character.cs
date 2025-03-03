using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float gravityScale = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    // TODO: Adjust the collider size in the Inspector to tightly fit the player sprite
    // TODO: Adjust jumping and gravity for better control
    // TODO: Make idle animation loop correctly


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if(!IsOwner || !IsSpawned) return;
        
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip based on movement direction
        if (moveInput > 0) {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        } else if (moveInput < 0) {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }

        // Update IsMoving parameter
        animator.SetBool("IsMoving", moveInput != 0);

        // Trigger jump animation
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Update grounded and vertical parameters
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the character is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the character leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
