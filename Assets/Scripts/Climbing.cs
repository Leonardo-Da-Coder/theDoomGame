using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;
    private bool exitWall;
    public float exitWallTime;
    private float exitWallTimer;
    public float wallJumpUpForce;
    public float wallJumpSideForce;

    Vector3 wallNormal;


    public float wallCheckDistance;
    RaycastHit rightWallHit;
    RaycastHit behindWallHit;
    RaycastHit leftWallHit;
    RaycastHit infrontWallHit;

    private bool climbing;
    private bool wallRight;
    private bool wallLeft;
    private bool wallInfront;
    private bool wallBehind;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;

    private bool wallFront;

    private void Start()
    {
        exitWall = false;
    }
    private void Update()
    {
        WallCheck();
        CheckWall();
        StateMachine();

        if (climbing && !exitWall) { ClimbingMovement();
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && !exitWall)
        {
            StartClimbing();

            // timer
            //if (climbTimer > 0) climbTimer -= Time.deltaTime;
            //if (climbTimer < 0) StopClimbing(); 
        }

        //State 3 - None
        else
        {
            StopClimbing();
        }
    }

    private void WallCheck()
    {
        //wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation., out frontWallHit, detectionLength, whatIsWall);
        Collider[] hitWall = Physics.OverlapSphere(transform.position, sphereCastRadius, whatIsWall);
        if (hitWall.Length > 0)
        {
            wallFront = true;
        }
        else
            wallFront = false;

        if (!exitWall && climbing)
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        pm.isClimbing = true;
    }

    private void ClimbingMovement()
    {
        if (Input.GetKey(KeyCode.W))
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        else if(Input.GetKey(KeyCode.S))
            rb.velocity = new Vector3(rb.velocity.x, -climbSpeed, rb.velocity.z);
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -climbSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, climbSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !exitWall)
        {
            WallJump();
        }
        
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.isClimbing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }

    private void WallJump()
    {
        exitWall = true;
        exitWallTimer = exitWallTime;

        Invoke("ResetExitWall", exitWallTimer);


        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        pm.doubleJump = true;
    }

    private void ResetExitWall()
    {
        exitWall = false;
    }

    private void CheckWall()
    {
        
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallInfront = Physics.Raycast(transform.position, orientation.forward, out infrontWallHit, wallCheckDistance, whatIsWall);
        wallBehind = Physics.Raycast(transform.position, -orientation.forward, out behindWallHit, wallCheckDistance, whatIsWall);

        wallNormal = wallRight ? rightWallHit.normal : wallLeft ? leftWallHit.normal : wallInfront ? infrontWallHit.normal : wallBehind ? behindWallHit.normal : new Vector3(0f, 0f, 0f);

    }

}
