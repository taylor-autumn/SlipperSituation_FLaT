using UnityEngine;

public class moveableObjects : MonoBehaviour
{
    [Header("Object info")]
    Vector3 ogObjPos;
    Quaternion ogObjRot;
    Rigidbody rb;
    public float deathHeight = -10f;

    [Header("Platform stuff")]
    public GameObject obj1;
    public GameObject obj2;
    public GameObject targetPlatform;
    public GameObject targetPlatform2;
    public GameObject signalObj1;
    public GameObject signalObj2;
    public Material transparentMaterial;
    public Material filledMaterial;
    public Material onMaterial;
    bool jam2Entered = false;

    //other components
    BoxCollider obj1Col;
    BoxCollider obj2Col;
    Renderer obj1Renderer;
    Renderer obj2Renderer;
    Renderer signalRenderer1;
    Renderer signalRenderer2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb=GetComponent<Rigidbody>();
        ogObjPos = transform.localPosition;
        ogObjRot = transform.localRotation;

        //platform stuff
        if (obj1 != null)
        {
            obj1Col = obj1.GetComponent<BoxCollider>();
            obj1Renderer = obj1.GetComponent<Renderer>();
        }
        if (obj2 != null)
        {
            obj2Col = obj2.GetComponent<BoxCollider>();
            obj2Renderer = obj2.GetComponent<Renderer>();
        }
        if (signalObj1!=null) signalRenderer1= signalObj1.GetComponent<Renderer>();
        if (signalObj2 != null) signalRenderer2 = signalObj2.GetComponent<Renderer>();


    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y <= deathHeight)
        {
            respawnObj();
        }
    }

    public void respawnObj()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.localPosition = ogObjPos;
        transform.localRotation = ogObjRot;
    }

    public void activatePlatform()
    {

        if (obj1 != null)
        {
            obj1Col.enabled = true;
            obj1Renderer.material = filledMaterial;
        }
        if (obj2 != null && gameObject.name != "jam jar blue")
        {
            obj2Col.enabled = false;
            obj2Renderer.material = transparentMaterial;
        }

        if (signalRenderer1!=null) signalRenderer1.material = onMaterial;

    }

    public void activateStove2()
    {
        jam2Entered = true;
        //specifically for the fuck ass second stove blue jam shit
        if (obj2 != null)
        {
            obj2Col.enabled = true;
            obj2Renderer.material = filledMaterial;
        }
        if (signalRenderer2 != null) signalRenderer2.material = onMaterial;
    }

    public void resetStove2()
    {
        if (obj2 != null)
        {
            obj2Col.enabled = false;
            obj2Renderer.material = transparentMaterial;
        }
        if (signalRenderer2 != null) signalRenderer2.material = transparentMaterial;
    }

    public void resetPlatform()
    {
        if (obj1 != null)
        {
            obj1Col.enabled = false;
            obj1Renderer.material = transparentMaterial;
        }
        if (obj2!=null && gameObject.name!="jam jar blue")
        {
            obj2Col.enabled = true;
            obj2Renderer.material = filledMaterial;
        }
        if (signalRenderer1 != null) signalRenderer1.material = transparentMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "moveObj")
        {
            if (other.gameObject == targetPlatform.gameObject)
            {
                activatePlatform();
            }
            if (gameObject.name=="jam jar blue" && targetPlatform2 != null && other.gameObject == targetPlatform2.gameObject)
            {
                activateStove2();
            }
        }
        if ((gameObject.tag==("mentos")) && (other.gameObject.CompareTag("can")))
        {
            print("Thing in can");
            //item ref
            GameObject player = GameObject.Find("Player");
            itemManager itemRef = player.GetComponent<itemManager>();
            itemRef.mentosCounter += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "moveObj")
        {
            if (other.gameObject == targetPlatform.gameObject)
            {
                resetPlatform();
            }
            else if (jam2Entered && 
                gameObject.name == "jam jar blue" && 
                targetPlatform2 !=null && 
                other.gameObject== targetPlatform2.gameObject)
            {
                jam2Entered = false;
                resetStove2();
            }
        }
    }

}
