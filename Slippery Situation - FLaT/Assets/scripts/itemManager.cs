using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class itemManager : MonoBehaviour
{
    [Header("General")]
    playerMove playerRef;
    public List<GameObject> stoves;

    [Header("Respawn")]
    public Transform firstFreezingPoint;
    public Transform currentRespawnTransform;

    [Header("Player qualities")]
    public float ogDrag;
    public float currentDrag = 10f;
    public float ogMelt;
    public float currentMelt = 0.05f;
    public float deathMelt = 0.5f;

    [Header("Stove shit")]
    public Material stoveMaterial;
    //public GameObject thirdStove;

    [Header("Soda Bubbles")]
    public int mentosCounter = 0;
    public ParticleSystem bubbles;

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
        manageBubbles();
    }

    public void respawn(GameObject player)
    {
        player.transform.position = currentRespawnTransform.position;
    }

    public void ultraRespawn(GameObject player)
    {
        player.transform.position = firstFreezingPoint.position;
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
            Renderer stoveRenderer=other.gameObject.GetComponent<Renderer>();
            stoves.Add(other.gameObject);

            if (stoveRenderer.sharedMaterial == stoveMaterial && stoves.Count>0)
            {
                playerRef.meltSpeed = deathMelt;
            }
        }
        
        if (other.gameObject.CompareTag("towel")) playerRef.groundDrag=currentDrag + 5; //towel
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sunSpot") || other.gameObject.CompareTag("towel"))
        {
            playerRef.groundDrag = ogDrag;
            playerRef.meltSpeed = ogMelt;
        }
        if (other.gameObject.CompareTag("stove"))
        {
            stoves.Remove(other.gameObject);
            if (stoves.Count==0)
            {
                playerRef.groundDrag = ogDrag;
                playerRef.meltSpeed = ogMelt;
            }
        }
    }

    public void manageBubbles()
    {
        var main = bubbles.main;
        var emission = bubbles.emission;
        if (mentosCounter == 0)
        {
            bubbles.gameObject.SetActive(false);
        }
        if (mentosCounter == 1)
        {
            print("IT IS 1");
            bubbles.gameObject.SetActive(true);
            main.simulationSpeed = 0.2f;
            emission.rateOverTime = 5;
        }
        if (mentosCounter == 2)
        {
            print("IT IS 2");
            main.simulationSpeed = 0.7f;
            emission.rateOverTime = 20;
        }
        if (mentosCounter == 3)
        {
            print("IT IS 3");
            main.simulationSpeed = 1f;
            emission.rateOverTime = 50;
        }
    }
}
