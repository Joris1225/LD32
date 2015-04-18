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
    private ParticleSystem water;

    private RectTransform selectedBox;

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

    private float earthCooldown = 2f;
    private float currentEarthCooldown = 0f;

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

        selectedBox = GameObject.Find("Selected").GetComponent<RectTransform>();
    }

	void Start() 
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandleMouseLock();

        if(!escapeToggled)
        {
            LookAround();
        }

        Move();

        currentAirCooldown = Mathf.Clamp(currentAirCooldown - Time.deltaTime, 0f, airCooldown);
        currentEarthCooldown = Mathf.Clamp(currentEarthCooldown - Time.deltaTime, 0f, earthCooldown);

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            partSystem = fire;
            currentElement = Element.Fire;
            selectedBox.localPosition = new Vector3(-375f, 0f, 0f);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            partSystem = water;
            currentElement = Element.Water;
            selectedBox.localPosition = new Vector3(-125f, 0f, 0f);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            partSystem = air;
            currentElement = Element.Air;
            selectedBox.localPosition = new Vector3(125f, 0f, 0f);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            partSystem = null;
            currentElement = Element.Earth;
            selectedBox.localPosition = new Vector3(375f, 0f, 0f);
        }

        Fire();
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
            switch (currentElement)
            {
                case Element.Water:
                    if(!partSystem.isPlaying)
                        partSystem.Play();
                    break;
                case Element.Fire:
                    if (!partSystem.isPlaying)
                        partSystem.Play();
                    break;
                case Element.Air:
                    if(currentAirCooldown == 0f)
                    {
                        currentAirCooldown = airCooldown;
                        if (!partSystem.isPlaying)
                            partSystem.Play();
                    }
                    break;
                case Element.Earth:
                    if(currentEarthCooldown == 0f)
                    {
                        currentEarthCooldown = earthCooldown;
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
                        {
                            GameObject other = hit.collider.gameObject;
                            if (other.CompareTag("Manipulatable"))
                                other.SendMessage("OnEarth");
                        }
                    }
                    break;
            }
        }
        else
        {
            if (partSystem != null && partSystem.isPlaying)
            {
                partSystem.Stop();
            }
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
                case Element.Water:
                    other.SendMessage("OnWater");
                    break;
            }
        }
        
    }
}
