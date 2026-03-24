using System.Runtime.CompilerServices;
using UnityEngine;

public class itemManager : MonoBehaviour
{
    [Header("General")]
    playerMove playerRef;

    [Header("Respawn")]
    public Transform firstFreezingPoint;
    public Transform currentRespawnTransform;

    [Header("Player qualities")]
    public float ogDrag;
    public float currentDrag = 10f;
    public float ogMelt;
    public float currentMelt = 0.05f;
    public float deathMelt = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef=GetComponent<playerMove>();
        currentRespawnTransform=firstFreezingPoint;
        ogDrag=playerRef.groundDrag;
        ogMelt=playerRef.meltSpeed;
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
        if (other.gameObject.CompareTag("freezingPoint")) //respawn
        {
            if (currentRespawnTransform != other.transform)
            {
                print("new respawn");
                currentRespawnTransform = other.transform;
                playerRef.resetScale();
            }
        }
        if (other.gameObject.CompareTag("sunSpot")) //sun spot
        {
            playerRef.groundDrag = currentDrag;
            playerRef.meltSpeed = currentMelt;
        }

        if (other.gameObject.CompareTag("stove"))
        {
            playerRef.meltSpeed = deathMelt;
        }
        
        if (other.gameObject.CompareTag("towel")) playerRef.groundDrag=currentDrag + 5; //towel
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sunSpot") || other.gameObject.CompareTag("towel") || other.gameObject.CompareTag("stove")) //sun spot
        {
            playerRef.groundDrag = ogDrag;
            playerRef.meltSpeed = ogMelt;
        }
    }
}
