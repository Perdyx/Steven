using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 5f; // Maximum speed when stick is fully tilted
    public float smoothTime = 0.3f;

    private Vector2 moveInput;
    private Vector3 currentVelocity;
    private Rigidbody rb;
    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        controls = new PlayerControls();

        // Bind the move action
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        // Calculate the desired speed based on the magnitude of the stick input
        float desiredSpeed = moveInput.magnitude * maxSpeed;

        Vector3 targetMoveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized * desiredSpeed;
        Vector3 smoothedMoveDirection = Vector3.SmoothDamp(rb.velocity, targetMoveDirection, ref currentVelocity, smoothTime);

        // Calculate new position
        Vector3 newPosition = rb.position + smoothedMoveDirection * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}