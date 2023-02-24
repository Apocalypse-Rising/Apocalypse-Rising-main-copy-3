using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Animator animator;
    private float horizontal;
    private float speed = 8f;
    public float jumpingPower = 16f;
    private bool doublejump;
    private bool hasMeleeAttacked = false;

    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    private ComboCount comboGet;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("Player");
        comboGet = player.GetComponent<ComboCount>();
    }

    private bool isGrounded()
    {
        Debug.Log(groundCheck.position);
        return Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {


        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", horizontal);

        if (horizontal < 0)
        {
            animator.SetBool("is running", true);

            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (horizontal > 0)
        {
            animator.SetBool("is running", true);

            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            animator.SetBool("is running", false);

        }


        WallSlide();


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            doublejump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            comboGet.comboNum += 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded() && doublejump)
        {
            doublejump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            comboGet.comboNum += 1;

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Debug.Log(isGrounded());

        if (isGrounded() == true)
        {
            animator.SetBool("isjumping", false);
        } else if (isGrounded() == false)
        {
           animator.SetBool("isjumping", true);

        }


        if (Input.GetKeyDown(KeyCode.X) && !hasMeleeAttacked)
        {
            // Trigger the melee animation
            animator.SetBool("meleeAttack", true);
            hasMeleeAttacked = true;

            // Flip the character if it's facing left
            
        }

        // Check if the animation has finished playing
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && hasMeleeAttacked)
        {
            animator.SetBool("meleeAttack", false);
            hasMeleeAttacked = false;

            // Flip the character back to its original direction
            if (isFacingRight)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }


        }

    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy" && rb.position.y > collision.contacts[0].point.y)
        {
            doublejump = true;
            Destroy(collision.collider.gameObject);
        }

    }
    private void WallSlide()
    {

        if (IsWalled() && !isGrounded())
        {
            isWallSliding = true;
            transform.localScale = new Vector3(-2, 2, 2);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

            animator.SetBool("wallSlide", true);
            animator.SetBool("isjumping", false);
           
        }
        else
        {
            animator.SetBool("wallSlide", false);
            isWallSliding = false;
        }
     

       

    }

    private void WallJump()
    {
        if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isWallSliding = false;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

   
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
}
