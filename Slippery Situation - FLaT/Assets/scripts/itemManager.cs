using UnityEngine;

public class itemManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeRespawnPoint()
    {
        print("changing respawn");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("freezingPoint"))
        {
            changeRespawnPoint();
        }
    }
}
