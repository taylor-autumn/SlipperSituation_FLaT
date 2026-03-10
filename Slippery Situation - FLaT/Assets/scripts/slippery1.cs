using UnityEngine;

public class slippery1 : MonoBehaviour
{
 [Header ("melting/taylor/respawn/wearecool")]
    [Header("Player Info")]
    public bool dead = false;
    public Transform iceCube;
    Rigidbody rb;

    [Header("Melting")]
    public float meltSpeed = 0.018f;
    private Vector3 originalScale;
    private bool melting = true;

    [Header("Taylor Movement")]
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

    private void Start()
    {
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
        //leah double check this

        MyInput();
        SpeedControl();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        //melting
        if (melting) melt();

        if (dead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                respawn();
            }
        }
        if (transform.position.y < -8) respawn();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
         moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        float multiplier = grounded ? 1f : airMultiplier;

        Vector3 targetForce = moveDirection.normalized * moveSpeed * 10f * multiplier;

        rb.AddForce(targetForce, ForceMode.Force); //THIS

        Vector3 velocity = rb.linearVelocity;
        Vector3 forwardVel = transform.forward * Vector3.Dot(velocity, transform.forward);
        Vector3 sidewaysVel = transform.right * Vector3.Dot(velocity, transform.right);

        sidewaysVel *= driftFactor;
        forwardVel *= driftFactor;

        rb.linearVelocity = forwardVel + sidewaysVel + Vector3.up * velocity.y; //THIS
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

            scale.y-=meltAmount;
            iceCube.transform.localScale = scale;

            transform.position -= new Vector3(0, meltAmount / 2, 0);
        }
        else
        {
            dead = true;
        }
    }

     public void respawn()
    {
        transform.localPosition = originalTransform;
        iceCube.transform.localScale = originalScale;

        transform.rotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        dead = false;
        print("resetting");
    }

}