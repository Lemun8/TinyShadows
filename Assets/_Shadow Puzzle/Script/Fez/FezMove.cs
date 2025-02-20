using UnityEngine;
using System.Collections;

public class FezMove : MonoBehaviour
{
    private int Horizontal = 0;

    public Animator anim;
    public float MovementSpeed = 5f;
    public float Gravity = 1f;
    public CharacterController charController;
    private FacingDirection _myFacingDirection;
    public float JumpHeight = 0f;
    public bool _jumping = false;
    private float degree = 0;

    public FacingDirection CmdFacingDirection
    {
        set
        {
            _myFacingDirection = value;
        }
    }

    // Enum to represent the different facing directions
    public enum FacingDirection
    {
        Front,
        Right,
        Back,
        Left
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
            Horizontal = -1;
        else if (Input.GetAxis("Horizontal") > 0)
            Horizontal = 1;
        else
            Horizontal = 0;

        if (Input.GetKeyDown(KeyCode.Space) && !_jumping)
        {
            _jumping = true;
            StartCoroutine(JumpingWait());
        }

        float moveFactor = MovementSpeed * Time.deltaTime * 10f;
        MoveCharacter(moveFactor);

        //if (anim)
        //{
        //    anim.SetInteger("Horizontal", Horizontal);

        //    float moveFactor = MovementSpeed * Time.deltaTime * 10f;
        //    MoveCharacter(moveFactor);
        //}

        // Smoothly rotate the character towards the target angle
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);
    }

    private void MoveCharacter(float moveFactor)
    {
        Vector3 trans = Vector3.zero;

        // Move based on the current facing direction
        if (_myFacingDirection == FacingDirection.Front)
        {
            trans = new Vector3(Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if (_myFacingDirection == FacingDirection.Right)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, Horizontal * moveFactor);
        }
        else if (_myFacingDirection == FacingDirection.Back)
        {
            trans = new Vector3(-Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if (_myFacingDirection == FacingDirection.Left)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, -Horizontal * moveFactor);
        }

        // Apply jump movement if jumping
        if (_jumping)
        {
            trans.y += JumpHeight * Time.deltaTime;
        }

        // Apply movement to the character controller
        charController.SimpleMove(trans);
    }

    public void UpdateToFacingDirection(FacingDirection newDirection, float angle)
    {
        // Update the character's facing direction and rotation angle
        _myFacingDirection = newDirection;
        degree = angle;
    }

    public IEnumerator JumpingWait()
    {
        yield return new WaitForSeconds(0.35f);
        _jumping = false;
    }
}
