using Unity.VisualScripting;
using UnityEngine;

public class billBoard : MonoBehaviour
{
    
    public Camera mainCamera;//Rotation LookAt target. 
    public GameObject camObj;

    void Start()
    {
        mainCamera = Camera.main; 
        camObj = GameObject.Find("Main Camera");
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // transform.LookAt(mainCamera.transform.position);
            // transform.Rotate(90, 0, 0); 
            Vector3 camPosition = new Vector3(0,camObj.transform.position.y+180,0);
            transform.forward = camPosition;
            //transform.Rotate(0,camObj.transform.position.y, 0);
        }
    }
}