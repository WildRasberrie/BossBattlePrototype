using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 mousePos; //mouse position 
    public static PlayerMovement player;
    bool space;
    public float playerHeight =1.0f;
    public LayerMask IsGround; //layer mask for ground
    public float jumpSpeed = 5.0f; // jump height
    public float vel = 8.0f;//create velocity 
    public float rotVel = 120.0f; //set rotate speed

    bool onGround; //check if player is on the ground

    float camRot; //camera rotation
    GameObject cam;
    private Rigidbody rb; //ref player's rigidbody

    void Awake()
    {
        player = this; //set player to this script
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's rb
        cam = Camera.main.gameObject; //get main camera
                                      // Clamp & Hide cursor 
        CursorClamp();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Jump();
    }

    void FixedUpdate()
    {
        // Add player control 
        PlayerControl();
        // Add camera control
        CameraControl();
    }

    void PlayerControl()
    {
        //move player 
        float moveUp = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveUp * vel * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        //rotate player
        float turnHorizontal = Input.GetAxis("Mouse X") * rotVel * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnHorizontal, 0f); //Horizontal rotation

        rb.MoveRotation(rb.rotation * turnRotation);

        // jump player if space pressed
        if (space)
        {
            space = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); //apply force to player rigidbody
        }

    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            space = true;
        }  
     }

    void CameraControl()
    {
        camRot = Input.GetAxis("Mouse Y") * rotVel * Time.fixedDeltaTime; //get vertical mouse movement
        //rotate camera
        if (camRot != 0f)
        {
            camRot = Mathf.Clamp(camRot, -0.3f, 0.3f); //clamp rotation to prevent flipping
        }

        cam.transform.Rotate(camRot, 0f, 0f); //rotate camera around X axis             
    }

    void CursorClamp()
    {
        //lock cursor to screen 
        Cursor.lockState = CursorLockMode.Confined; 
        //hide cursor 
        Cursor.visible = false;
    }

    void GroundCheck()
    {
        //check if player is on the ground
        onGround = (Physics.Raycast(transform.position, Vector3.down, 1.2f, IsGround));
    }
}
