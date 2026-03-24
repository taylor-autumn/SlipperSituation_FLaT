using Unity.VisualScripting;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    [Header("Player Info")]
    public bool dead = false;
    public Transform iceCube;
    Rigidbody rb;
    itemManager itemRef;

    [Header("Melting")]
    public float meltSpeed = 0.018f;
    private Vector3 originalScale;
    private bool melting = true;

    [Header("Movementing")]
    private Vector3 originalTransform;  
    public float moveSpeed;
    public float groundDrag;
    public float driftFactor = 0.9f;

    public float airMultiplier;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    [Header("roatating it")]
    public float rotationSpeed;
    private Quaternion targetRotation;

    private void Start()
    {
        //itemManagerReference
        itemRef=GetComponent<itemManager>();
        //melting
        originalScale = iceCube.transform.localScale;
        originalTransform = transform.localPosition;
        //both
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //movement
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        //respawn shit
        if (dead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                respawn();
                resetMoveableObjects();
                //change this to reset to the very FIRST level respawn
            }
        }
        if (transform.position.y < -8)
        {
            respawn();
        }
    }

    private void FixedUpdate()
    {
        if (!dead) MovePlayer();
        //melting
        if (melting) melt();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (!dead)
        {
            //movement
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            //directional change
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                targetRotation = Quaternion.Euler(0f, 90f, 0f);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                targetRotation = Quaternion.Euler(0f, -90f, 0f);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                targetRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }

    private void MovePlayer()
    {
         moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        float multiplier = grounded ? 1f : airMultiplier;

        Vector3 targetForce = moveDirection.normalized * moveSpeed * 10f * multiplier;

        rb.AddForce(targetForce, ForceMode.Force);

        Vector3 velocity = rb.linearVelocity;
        Vector3 forwardVel = transform.forward * Vector3.Dot(velocity, transform.forward);
        Vector3 sidewaysVel = transform.right * Vector3.Dot(velocity, transform.right);

        sidewaysVel *= driftFactor;
        forwardVel *= driftFactor;

        rb.linearVelocity = forwardVel + sidewaysVel + Vector3.up * velocity.y;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void melt()
    {
        Vector3 scale = iceCube.transform.localScale;

        if (scale.y > 0.01f)
        {
            float meltAmount = meltSpeed * Time.deltaTime;

            scale.y -= meltAmount;
            iceCube.transform.localScale = scale;

            rb.MovePosition(rb.position - new Vector3(0, meltAmount / 2, 0)); //change tranform position and rigid body were together
        }
        else
        {
            dead = true;
        }
    }

     public void respawn()
    {
        itemRef.respawn(gameObject);
        resetScale();

        transform.rotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        dead = false;
    }

    public void resetScale()
    {
        iceCube.transform.localScale = originalScale;
    }

    public void resetMoveableObjects()
    {
        GameObject movObjParent = GameObject.Find("moveableObjects");
        foreach (Transform obj in movObjParent.transform)
        {
            moveableObjects objScript = obj.GetComponent<moveableObjects>();
            objScript.respawnObj();
        }

    }

}