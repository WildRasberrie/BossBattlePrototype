using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 mousePos; //mouse position 
    public static PlayerMovement player;
    public float vel = 8.0f;//create velocity 
    public float rotVel = 120.0f; //set rotate speed
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

    }

    void CameraControl()
    {
        camRot = Input.GetAxis("Mouse Y") * rotVel * Time.fixedDeltaTime; //get vertical mouse movement
        //rotate camera
        if (camRot != 0f)
        {

            camRot = Mathf.Clamp(camRot, -0.3f, 0.3f); //clamp rotation to prevent flipping
            print(camRot);
        }

        cam.transform.Rotate(camRot, 0f, 0f); //rotate camera around X axis             
    }

    void CursorClamp()
    {
        //lock cursor to screen 
        Cursor.lockState = CursorLockMode.Confined;S
        //hide cursor 
        Cursor.visible = false; 
    }
}
