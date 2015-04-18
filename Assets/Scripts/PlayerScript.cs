using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    public enum Element
    {
        Earth,
        Air,
        Fire,
        Water
    }

    private Element currentElement;
    private ParticleSystem partSystem;

    private ParticleSystem air;
    private ParticleSystem fire;
    private ParticleSystem earth;
    private ParticleSystem water;

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
    private float airCooldown = 1f;
    private float currentAirCooldown = 0f;

    private CharacterMotor motor;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            switch(child.name)
            {
                case "Air":
                    air = child.GetComponent<ParticleSystem>();
                    break;
                case "Earth":
                    earth = child.GetComponent<ParticleSystem>();
                    break;
                case "Fire":
                    fire = child.GetComponent<ParticleSystem>();
                    break;
                case "Water":
                    water = child.GetComponent<ParticleSystem>();
                    break;
            }
        }
        partSystem = fire;
        currentElement = Element.Fire;
    }

	void Start() 
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

    void Update()
    {
        HandleMouseLock();

        if(!escapeToggled)
        {
            LookAround();
        }

        Move();

        Fire();
        currentAirCooldown = Mathf.Clamp(currentAirCooldown - Time.deltaTime, 0f, airCooldown);
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

    private void Fire()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!partSystem.isPlaying)
                partSystem.Play();
        }
        else
        {
            if (partSystem.isPlaying)
                partSystem.Stop();
        }


        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            partSystem = fire;
            currentElement = Element.Fire;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            partSystem = air;
            currentElement = Element.Air;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Manipulatable"))
        {
            switch(currentElement)
            {
                case Element.Fire:
                    other.SendMessage("OnHeat");
                    break;
                case Element.Air:
                    if (currentAirCooldown > 0)
                        break;

                    currentAirCooldown = airCooldown;
                    ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];
                    int safeLength = partSystem.GetSafeCollisionEventSize();
                    if (collisionEvents.Length < safeLength)
                        collisionEvents = new ParticleCollisionEvent[safeLength];
                    int numCollisionEvents = partSystem.GetCollisionEvents(other, collisionEvents);

                    for (int i = 0; i < numCollisionEvents; i++)
                    {
                        other.SendMessage("OnAir", collisionEvents[i]);
                    }
                    break;
            }
        }
        
    }
}
