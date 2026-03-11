using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class moveableObjects : MonoBehaviour
{
    [Header("Object info")]
    private Vector3 ogObjPos;
    Rigidbody rb;
    public float deathHeight = -10f;

    [Header("Platform stuff")]
    public GameObject obj1;
    public GameObject obj2;
    public GameObject targetPlatform;
    public Material transparentMaterial;
    public Material filledMaterial;
    BoxCollider obj1Col;
    BoxCollider obj2Col;
    Renderer obj1Renderer;
    Renderer obj2Renderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        ogObjPos = transform.position;

        //platform stuff
        obj1Col=obj1.GetComponent<BoxCollider>();
        obj2Col=obj2.GetComponent<BoxCollider>();
        obj1Renderer = obj1.GetComponent<Renderer>();
        obj2Renderer = obj2.GetComponent<Renderer>();
        
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
        gameObject.transform.position = ogObjPos;

        transform.rotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void activatePlatform()
    {
        print("enabled");

        obj1Col.enabled = true;
        obj2Col.enabled = false;

        obj1Renderer.material = filledMaterial;
        obj2Renderer.material = transparentMaterial;

    }

    public void resetPlatform()
    {
        print("disabled");
        obj1Col.enabled = false;
        obj2Col.enabled = true;

        obj1Renderer.material = transparentMaterial;
        obj2Renderer.material = filledMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("platform"))
        {
            activatePlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("platform"))
        {
            resetPlatform();
        }
    }
}
