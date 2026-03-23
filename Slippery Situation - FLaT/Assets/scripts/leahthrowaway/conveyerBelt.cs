using System.Collections.Generic;
using UnityEngine;

public class conveyerBelt : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private List<GameObject> onBelt;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i <= onBelt.Count - 1; i++)
        {
            onBelt[i].GetComponent<Rigidbody>().AddForce(speed * direction);
        }
    }

    void FixedUpdate()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           onBelt.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
           onBelt.Remove(other.gameObject);
        }
    }
}
