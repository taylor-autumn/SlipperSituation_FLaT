using UnityEngine;

public class itemManager : MonoBehaviour
{
    [Header("General")]
    playerMove playerRef;

    [Header("Respawn")]
    public Transform firstFreezingPoint;
    public Transform currentRespawnTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef=GetComponent<playerMove>();
        currentRespawnTransform=firstFreezingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawn(GameObject player)
    {
        player.transform.position = currentRespawnTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("freezingPoint"))
        {
            if (currentRespawnTransform != other.transform)
            {
                print("new respawn");
                currentRespawnTransform = other.transform;
                playerRef.resetScale();
            }
        }
    }
}
