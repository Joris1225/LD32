using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    public bool escapeToggled = false;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public float minimumX = -360f;
    public float maximumX = 360f;

    public float minimumY = -60f;
    public float maximumY = 60f;

    private float rotationY = 0f;
    private float moveSpeed = 2f;

    private CharacterMotor motor;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        
    }

	void Start() 
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            HandleMouseLock();
        }

        if(!escapeToggled)
        {
            LookAround();
        }

        Move();
	}
	

    private void LookAround()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxisRaw("Mouse X") * sensitivityX;

            rotationY += Input.GetAxisRaw("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxisRaw("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    private void Move()
    {
        // Get the input vector from kayboard or analog stick
        Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1.0f, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength *= directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector *= directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector * moveSpeed;
        motor.inputJump = Input.GetButton("Jump");
    }

    private void HandleMouseLock()
    {
        if (escapeToggled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            escapeToggled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            escapeToggled = true;
        }
    }
}
