using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

//create velocity 
    public float vel =5.0f;
    public float rotVel = 100.0f; //set rotate speed
    float camRot; //camera rotation
    GameObject cam; 
    private Rigidbody rb; //ref player's rigidbody

    void Start()
    {

        rb= GetComponent<Rigidbody>(); // Access player's rb
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        //move player 
        float moveUp = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveUp * vel * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        //rotate player
        float turnHorizontal = Input.GetAxis("Mouse X") * rotVel * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnHorizontal, 0f); //Horizontal rotation

        camRot += turnHorizontal; //update camera rotation
         rb.MoveRotation(rb.rotation * turnRotation);
    }
}
