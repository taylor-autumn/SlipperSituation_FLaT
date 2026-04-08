using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMove : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraFollow;
    private Vector3 camOffset;

    [Header("Player Info")]
    public bool dead = false;
    public Transform iceCube;
    Rigidbody rb;
    itemManager itemRef;
    public float deathHeight = -8;

    [Header("UI SHIT")]
    public Image meltBar;
    public TMP_Text deadText;
    public GameObject deathPopUp;

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

    //enum shit
    public enum gameMode
    {
        game,
        menu
    }
    public gameMode currentMode;

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
        //camerassutf
        camOffset = cameraFollow.position - transform.position;
        //other
        deadText.gameObject.SetActive(false);
        deathPopUp.SetActive(false);
    }

    private void Update()
    {
        //movement
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

            if (grounded)
                {
                    rb.linearDamping = groundDrag;
                }
            else
                {
                    rb.linearDamping = 0;
                }
        //Camera
        cameraFollow.position = transform.position + camOffset;

        //respawn shit
        if (dead)
        {
            deadText.gameObject.SetActive(true);
            deathPopUp.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                ultraRespawn();
                resetMoveableObjects();
                deadText.gameObject.SetActive(false);
                deathPopUp.SetActive(false);
            }
        }
        if (!dead &&  transform.position.y < deathHeight)
        {
            respawn();
        }
    }

    private void FixedUpdate()
    {
        if (!dead && currentMode==gameMode.game) MovePlayer();
        //melting
        if (melting && currentMode == gameMode.game) melt();
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
        //melthing continuous
        Vector3 scale = iceCube.transform.localScale;

        //health bar
        meltBar.fillAmount = scale.y;

        //melt function

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

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        dead = false;
    }

    public void ultraRespawn()
    {
        print("ultra respawning");
        itemRef.ultraRespawn();
        resetScale();

        Transform firstFreezingPoint = itemRef.firstFreezingPointTransform;
        Transform currentRespawn = itemRef.currentRespawnTransform;

        currentRespawn = firstFreezingPoint;
        gameObject.transform.position=currentRespawn.transform.position;

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

    //==============IN GAME MENU SHIT================//
    public void mainMenuGo()
    {
        SceneManager.LoadScene("menu");
    }
    public void restartLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void enterMenu()
    {
        currentMode = gameMode.menu;
    }

    public void exitMenu()
    {
        currentMode = gameMode.game;
    }

}