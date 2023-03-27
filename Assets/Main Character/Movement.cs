using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

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
    private bool isFacingRight;
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isJumping;
    private bool isGroundPounding;
    private float globalGravity = -9.81f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        print(rb.velocity.y);

        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

        print(lastJumpTime);
        print(lastGroundedTime);

        if (isGrounded())
        {
            lastGroundedTime = jumpCoyoteTime;
            isJumping = false;
            isGroundPounding = false;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        //Jump behaviour
        if (Input.GetButtonDown("Jump"))
        {

            lastJumpTime = jumpBufferTime;

        }

        //Ground pound behaviour
        if (!isGroundPounding && !isJumping && Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(Vector2.down * groundPoundForce, ForceMode.Impulse);
            isGroundPounding = true;
            rb.velocity = new Vector2(rb.velocity.x * groundPoundHorizontalMomentum, rb.velocity.y);
        }

        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode.Impulse);
            lastGroundedTime = 0;
            lastJumpTime = 0;
            isJumping = true;
        }

        //Jump cut behaviour
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        //movement with forces allows for easier implementation of other platforming elements
        #region Horizontal Run 

        float targetSpeed = horizontal * moveSpeed;

        float speedDiff = targetSpeed - rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

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

        #region Jump Gravity

     //   if (rb.velocity.y < 0)
        {
     //       rb.gravityScale = gravityScale * fallGravityMultiplier;

     //       rb.AddForce(movement * Vector3.down);
        }

        #endregion

        #region Fall speed clamping
        if (!isGrounded())
        {
            float targetSpeedG = globalGravity * maxFallingSpeed;

            float speedDiffG = targetSpeedG - rb.velocity.y;

            rb.AddForce(speedDiffG * Vector3.down);
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
        Collider[] collisions = Physics.OverlapSphere(groundCheck.position, 0.2f, groundLayer);
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
