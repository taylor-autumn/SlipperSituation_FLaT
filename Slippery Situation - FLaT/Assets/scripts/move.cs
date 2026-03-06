using UnityEngine;

public class move : MonoBehaviour
{
    [Header("Player Info")]
    public bool dead = false;
    public Transform iceCube;
    Rigidbody rb;

    [Header("Melting")]
    public float meltSpeed = 0.018f;
    private Vector3 originalScale;
    private bool melting = true;

    [Header("Movement")]
    public float moveSpeed = 4;
    private Vector3 originalTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = iceCube.transform.localScale;
        originalTransform = transform.localPosition;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
            }

        }
        
        //user cheat
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    print("calling");
        //    if (!melting)
        //    {
        //        melting = true;
        //    }
        //    else
        //    {
        //        melting = false;
        //    }
                
        //}

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
