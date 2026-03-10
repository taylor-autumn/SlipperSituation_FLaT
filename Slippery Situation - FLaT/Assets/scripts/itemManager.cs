using UnityEngine;

public class itemManager : MonoBehaviour
{
    [Header("General")]
    GameObject player;
    move playerRef;

    [Header("Respawn")]
    Transform currentRespawnTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player=GetComponent<GameObject>();
        playerRef=GetComponent<move>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        print("changing respawn");
        player.transform.position = currentRespawnTransform.position; //this no work
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("freezingPoint"))
        {
            Respawn();
            currentRespawnTransform=other.transform;
        }
    }
}
