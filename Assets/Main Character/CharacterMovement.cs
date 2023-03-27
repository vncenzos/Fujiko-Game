using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private bool canJump = true;
    public float speed = 4.0f;
    public Vector2 jumpHeight;
    public float grappleLength;
    private Rigidbody2D physicsBody;
    private DistanceJoint2D distanceJoint2D;
    private LineRenderer lineRenderer;
    private Animator animController;
    private float cursorDistance;
    private float distance;
    private Vector2 temp;
    private Vector2 pos;
    private Vector2 direction;
    RaycastHit2D grappleRayHit;
    private string collisionState;

    enum movementState
    {
        Idle,
        Walking,
        Grappling,
        InAir
    }
    movementState characterState;

    // Start is called before the first frame update
    private void Awake()
    {
        animController = GetComponent<Animator>();
        physicsBody = GetComponent<Rigidbody2D>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        distanceJoint2D.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        collisionState = collision.gameObject.tag;
        if (collision.gameObject.tag == "JumpReset")
        {
            canJump = true;
            characterState = movementState.Walking;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collisionState = collision.gameObject.tag;
        if (collision.gameObject.tag == "JumpReset")
        {
            print("Collision stay");
            canJump = true;
            characterState = movementState.Walking;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionState = "null";
        print("Collision finished");
        canJump = false;
        characterState = movementState.InAir;
    }
    // Update is called once per frame
    void Update()
    {
        print(characterState);
        switch (characterState)
        {
            case movementState.Idle:
                animController.SetFloat("Movement", 0);
                animController.SetBool("IsInAir", false);
                speed = 4.0f;
                break;
            case movementState.Walking:
                animController.SetFloat("Movement", Input.GetAxis("Horizontal"));
                animController.SetBool("IsInAir", false);
                speed = 4.0f;
                break;
            case movementState.Grappling:
                animController.SetBool("IsGrappling", true);
                animController.SetBool("IsInAir", true);
                speed = 6.0f;
                break;
            case movementState.InAir:
                animController.SetBool("IsInAir", true);
                animController.SetBool("IsGrappling", false);
                speed = 5.0f;
                break;
        }
        float translation = Input.GetAxis("Horizontal") * speed;
        if (translation == 0 && physicsBody.velocity.y == 0)
        {
            characterState = movementState.Idle;
        }
        if (Input.GetKeyDown("space"))
        {
            if (canJump)
            {
                canJump = false;
                characterState = movementState.InAir;
                physicsBody.AddForce(jumpHeight, ForceMode2D.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (characterState == movementState.InAir)
            {
                pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);       //world position of the player
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);               //scene coordinates from the cursor position
                temp = new Vector2(worldPosition.x, worldPosition.y);                                      //temporary variable to make a 2D vector from worldPostion
                float tempCursorDistance = Vector2.Distance(gameObject.transform.position, temp);          //pre-clamped vector length from player to cursor
                cursorDistance = Mathf.Clamp(tempCursorDistance, grappleLength, grappleLength);            //clamped vector length
                direction = temp - pos;                                                                    //direction of the grapple launch
                direction = direction.normalized;                                                          //normalization of the direction vector
                grappleRayHit = Physics2D.Raycast(pos, direction, cursorDistance);                         //raycast for the grappling hook
                if (grappleRayHit.collider != null)
                {
                    if (grappleRayHit.transform.tag == "Grabbable")
                    {
                        print("Raycast Grabbable succedeed");
                        float tempDistance = Vector2.Distance(gameObject.transform.position, grappleRayHit.point);
                        distance = Mathf.Clamp(tempDistance, 0, grappleLength);
                        distanceJoint2D.connectedAnchor = grappleRayHit.point;
                        distanceJoint2D.distance = distance;
                        distanceJoint2D.enabled = true;
                        lineRenderer.enabled = true;
                    }
                    else
                    {
                        print("Grabbed a non-grabbable object");
                    }
                }
                else
                {
                    print("Raycast hit NOTHING");
                }
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            if (lineRenderer.enabled)
            {
                characterState = movementState.Grappling;
                lineRenderer.SetPosition(0, grappleRayHit.point);
                lineRenderer.SetPosition(1, pos);
            }
        }
        else
        {
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
            if (characterState == movementState.Grappling)
            {
                characterState = movementState.InAir;
            }
        }

        translation *= Time.deltaTime;
        transform.Translate(translation, 0, 0);
    }
}
