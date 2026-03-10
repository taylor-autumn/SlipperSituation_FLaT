using UnityEngine;

public class playerMove : MonoBehaviour
{
    [Header("Player Info")]
    public bool dead = false;
    public Transform iceCube;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    [Header("Melting")]
    public float meltSpeed = 0.018f;
    private bool melting = true;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float groundDrag;
    public float driftFactor = 0.9f;
    public float airMultiplier;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        originalScale = iceCube.localScale;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down,
            playerHeight * 0.5f + 0.3f, whatIsGround);

        if (!dead)
        {
            MyInput();
            SpeedControl();

            if (melting) Melt();
        }

        if (dead && Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (transform.position.y < -8)
        {
            Respawn();
        }

        rb.linearDamping = grounded ? groundDrag : 0;
    }

    void FixedUpdate()
    {
        if (!dead)
            MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        float multiplier = grounded ? 1f : airMultiplier;

        if (moveDirection != Vector3.zero)
        {
            Vector3 targetForce = moveDirection.normalized * moveSpeed * 10f * multiplier;
            rb.AddForce(targetForce, ForceMode.Force);
        }

        Vector3 velocity = rb.linearVelocity;

        Vector3 forwardVel = transform.forward * Vector3.Dot(velocity, transform.forward);
        Vector3 sidewaysVel = transform.right * Vector3.Dot(velocity, transform.right);

        sidewaysVel *= driftFactor;

        rb.linearVelocity = forwardVel + sidewaysVel + Vector3.up * velocity.y;
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    void Melt()
    {
        Vector3 scale = iceCube.localScale;

        if (scale.y > 0.01f)
        {
            float meltAmount = meltSpeed * Time.deltaTime;

            scale.y -= meltAmount;
            iceCube.localScale = scale;

            transform.position -= new Vector3(0, meltAmount / 2, 0);
        }
        else
        {
            dead = true;
        }
    }

    void Respawn()
    {
        transform.localPosition = originalPosition;
        iceCube.localScale = originalScale;

        transform.rotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        dead = false;
    }
}
