using UnityEngine;

public class move1 : MonoBehaviour
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
    private Vector3 originalTransform;// done

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
