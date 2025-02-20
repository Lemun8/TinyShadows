using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement and Rotation")]
    private Vector2 _input;
    private CharacterController characterController;
    private Vector3 _direction;
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;
    [SerializeField] private float speed;

    [Space]
    [Header("Gravity")]
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3f;
    private float _velocity;

    [Header("Pushing")]
    public bool isPushing = false;
    private Vector3 pushDirection;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {   
        PlayerGravity();
        PlayerMove();

        if (!isPushing) // Only allow rotation if not pushing
        {
            PlayerRotation();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);

        if (isPushing)
        {
            // Allow movement left, right, forward, and backward along the push direction
            Vector3 rightMovement = Vector3.Cross(Vector3.up, pushDirection).normalized * _input.x;
            Vector3 forwardMovement = pushDirection * _input.y;

            // Combine directions for full movement while pushing
            _direction = forwardMovement + rightMovement;
        }
        else
        {
            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;

            // Project forward and right vectors onto the ground plane (y = 0)
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            _direction = camForward * _input.y + camRight * _input.x;
        }
    }

    private void PlayerMove()
    {
        characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void PlayerRotation()
    {
        if (_input.sqrMagnitude == 0)
            return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void PlayerGravity()
    {
        if (characterController.isGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    public void StartPushing(Vector3 direction)
    {
        isPushing = true;
        pushDirection = direction.normalized;

        // Align player rotation with push direction
        float targetAngle = Mathf.Atan2(pushDirection.x, pushDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
    }

    public void StopPushing()
    {
        isPushing = false;
    }
}
