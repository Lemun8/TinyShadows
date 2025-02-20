using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    Vector2 _delta;
    private bool isMoving;
    private bool isRotating;
    private bool isBusy;

    private float _xRotation;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = .5f;
    public void Onlook(InputAction.CallbackContext context)
    {
        _delta = context.ReadValue<Vector2>();
    }
    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    if(isBusy) { return; }
    //    isMoving = context.started || context.performed;

    //    if (context.canceled)
    //    {
    //        isBusy = true;
    //        SnapRotation();
    //    }
    //}
    //public void OnRotate(InputAction.CallbackContext context)
    //{
    //    isRotating = context.started || context.performed;
    //}

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed && !isBusy)
        {
            SnapRotation(90);
        }
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (context.performed && !isBusy)
        {
            SnapRotation(-90);
        }
    }

    private void Awake()
    {
        _xRotation = transform.rotation.eulerAngles.x;
    }

    private void LateUpdate()
    {
        if(isMoving)
        {
            var postion = transform.right * (_delta.x * -movementSpeed);
            postion += transform.up * (_delta.y * -movementSpeed);
            transform.position += postion * Time.deltaTime;
        }
        if (isRotating)
        {
            transform.Rotate(new Vector3(_xRotation, - _delta.x * rotationSpeed, 0f));
            transform.rotation = Quaternion.Euler(_xRotation, transform.rotation.eulerAngles.y, 0f);
        }
    }

    private void SnapRotation(float angle)
    {
        float targetYRotation = transform.rotation.eulerAngles.y+angle;
        transform.DORotate(new Vector3(_xRotation, targetYRotation, 0f), 0.5f)
            .OnComplete(() => isBusy = false);

        isBusy = true;
    }

    //private Vector3 SnappedVector()
    //{
    //    var endValue = 0f;
    //    var curY = Mathf.Ceil(transform.rotation.eulerAngles.y);

    //    endValue = curY switch{
    //        >= 0 and <= 90 => 45f,
    //        >= 91 and <= 180 => 135f,
    //        >= 181 and <= 270 => 225f,
    //        _ => 315f
    //    };
    //    return new Vector3(_xRotation,endValue,0f);
    //}
}
