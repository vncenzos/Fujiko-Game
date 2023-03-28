using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //public vars
    private float horizontal;
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public float frictionAmount;
    public float jumpHeight;
    public float jumpCutMultiplier;
    public float jumpBufferTime;
    public float jumpCoyoteTime;
    public float gravityScale;
    public float maxFallingSpeed;
    public float fallGravityMultiplier;
    public float groundPoundForce;
    public float groundPoundHorizontalMomentum;

    //private shit
    private bool isFacingRight;
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isJumping;
    private bool isGroundPounding;
    private float globalGravity = -9.81f;
    private float sackWeight;

    //serialized fields
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        sackWeight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Timers for buffer and coyote time
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

        //Check to see if the character is grounded
        if (isGrounded())
        {
            lastGroundedTime = jumpCoyoteTime;
            isJumping = false;
            isGroundPounding = false;
        }

        //Horizontal axis input 
        horizontal = Input.GetAxisRaw("Horizontal");

        //Jump buffer input
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpTime = jumpBufferTime;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        #region Gravity related stuff

        float actualGravity = rb.velocity.y < 0 ? (globalGravity * gravityScale * fallGravityMultiplier) : (globalGravity * gravityScale); //Gravity is higher when falling 
        rb.AddForce(actualGravity * Vector3.up, ForceMode.Acceleration);

        #endregion

        #region Ground pound behaviour
        if (!isGroundPounding && !isGrounded() && Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector2.down * groundPoundForce, ForceMode.Impulse);
            isGroundPounding = true;
            rb.velocity = new Vector2(rb.velocity.x * groundPoundHorizontalMomentum, rb.velocity.y);
        }
        #endregion

        #region Jump behaviour
        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {   
            rb.velocity = new Vector3(rb.velocity.x, 0 , 0);
            rb.AddForce(Vector2.up * jumpHeight * sackWeight, ForceMode.Impulse);
            lastGroundedTime = 0;
            lastJumpTime = 0;
            isJumping = true;
        }
        #endregion

        #region Jump cut behaviour
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0.0001f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpCutMultiplier, 0);
        }
        #endregion

        //movement with forces allows for easier implementation of other platforming elements
        #region Horizontal Run 

        float targetSpeed = horizontal * moveSpeed;

        float speedDiff = targetSpeed - rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower * sackWeight) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);

        #endregion

        //friction is enforced on the code level
        #region Friction

        if (lastGroundedTime > 0 && Mathf.Abs(horizontal) < 0.1f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(rb.velocity.x);

            rb.AddForce(Vector3.right * -amount, ForceMode.Impulse);
        }

        #endregion
    }

    //This function flips the x scale to save on the amount of work spent on the left side animations or sprites
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //This function checks if the character is on the ground
    private bool isGrounded()
    {
        bool isTouchingGround = false;
        Collider[] collisions = Physics.OverlapSphere(groundCheck.position, 0.1f, groundLayer);
        foreach (var collision in collisions)
        {
            if (collision.gameObject.layer == 6)
            {
                //6 is ground
                isTouchingGround = true;
            }
            else
            {
                isTouchingGround = false;
            }
        }
        return isTouchingGround;
    }
}
