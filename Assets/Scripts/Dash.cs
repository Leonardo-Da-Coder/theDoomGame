using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Refrences")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Settings")]
    public bool useCameraFoward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashTime;
    private float dashTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            DashMovement();
        }

        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
    }

    private void DashMovement()
    {
        if (dashTimer > 0) return;
        else dashTimer = dashTime;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        Transform forwardT;

        if (useCameraFoward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirections(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke("DelayedDashForce", 0.025f);

        Invoke("ResetDash", dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);

    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0f;
        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirections(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }

}
