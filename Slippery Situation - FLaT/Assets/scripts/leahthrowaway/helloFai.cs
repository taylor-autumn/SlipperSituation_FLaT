using UnityEngine;

public class helloFai : MonoBehaviour
{
    public Transform target; 
    public float speed = 50f;
    public Vector3 rotationAxis = Vector3.up; 
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, rotationAxis, speed * Time.deltaTime);

    }
}
