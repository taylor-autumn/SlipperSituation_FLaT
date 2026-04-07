using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;

public class itemManager : MonoBehaviour
{
    [Header("General")]
    playerMove playerRef;
    public List<GameObject> stoves;

    [Header("Respawn")]
    public Transform firstFreezingPointTransform;
    public Transform currentRespawnTransform;

    [Header("Player qualities")]
    public float ogDrag;
    public float currentDrag = 10f;
    public float ogMelt;
    public float currentMelt = 0.05f;
    public float deathMelt = 0.5f;
    public float intensity2 = 0.25f;
    public float intensity3 = 0.32f;

    [Header("Stove shit")]
    public Material stoveMaterial;

    [Header("Soda Bubbles")]
    public int mentosCounter = 0;
    public ParticleSystem bubbles;

    [Header("Muddy")]
    public Material myMaterial;
    Color normColor = new Color(0.4816661f, 0.7283878f, 0.8301887f, 0.6509804f);
    Color mudColor = new Color(0.3490566f, 0.1512773f, 0f, 0.6509804f);
    public float changering = 0;
    public GameObject soapParticles;
    private bool dirty = false;
    private bool cleanering = false;

    [Header("Fan")]
    public Transform fan;
    public float fanSpeed;
    public Vector3 fanRotationAxis = Vector3.up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef=GetComponent<playerMove>();
        currentRespawnTransform=firstFreezingPointTransform;
        ogDrag=playerRef.groundDrag;
        ogMelt=playerRef.meltSpeed;

        //muddy
        soapParticles.SetActive(false);
        myMaterial.color = normColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (bubbles != null)
        {
            manageBubbles();
        }

        if (fan != null)
        {
            fan.transform.RotateAround(fan.position, fanRotationAxis, fanSpeed * Time.deltaTime);
        }

        //muddyying
        if (dirty)
        {
            if (changering < 1)
                {
                    changering += 0.0008f;
                    
                }
            float slowingdown = playerRef.groundDrag += 0.0008f;
            if (slowingdown == ogDrag + 10f)
                {
                    playerRef.groundDrag = ogDrag + 10f;
                }
            Color lerpingit = Color.Lerp(normColor, mudColor, changering);
            myMaterial.color = lerpingit;
        }
        if (cleanering)
        {
            if (changering > 0)
                {
                    changering -= 0.0008f;
                    
                }
            float speedingup = playerRef.groundDrag -= 0.005f;
                    if (speedingup < ogDrag + 0.05)
                {
                    playerRef.groundDrag = ogDrag;
                }
            Color lerpingit = Color.Lerp(normColor, mudColor, changering);
            myMaterial.color = lerpingit;
        }
    }

    public void respawn(GameObject player)
    {
        player.transform.position = currentRespawnTransform.position;
    }

    public void ultraRespawn()
    {
        stoves.Clear();
        playerRef.groundDrag = ogDrag;
        playerRef.meltSpeed = ogMelt;

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
        if (other.gameObject.CompareTag("sunSpot1")) //sun spot
        {
            playerRef.groundDrag = currentDrag;
            playerRef.meltSpeed = currentMelt;
        }
        if (other.gameObject.CompareTag("sunSpot2")) //sun spot
        {
            playerRef.groundDrag = currentDrag;
            playerRef.meltSpeed = intensity2;
        }
        if (other.gameObject.CompareTag("sunSpot3")) //sun spot
        {
            playerRef.groundDrag = currentDrag;
            playerRef.meltSpeed = intensity3;
        }

        if (other.gameObject.CompareTag("stove"))
        {
            Renderer stoveRenderer=other.gameObject.GetComponent<Renderer>();
            stoves.Add(other.gameObject);

            if (stoveRenderer.sharedMaterial == stoveMaterial && stoves.Count>0)
            {
                print("THIS IS WORKING");
                playerRef.meltSpeed = deathMelt;
            }
        }
        
        if (other.gameObject.CompareTag("towel")) playerRef.groundDrag=currentDrag + 5; //towel

        //muddy
        if (other.gameObject.CompareTag("mud"))
        {
            dirty = true;
        }

          if (other.gameObject.CompareTag("soap"))
        {
            cleanering = true;
            soapParticles.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sunSpot1") || other.gameObject.CompareTag("sunSpot2") || other.gameObject.CompareTag("sunSpot3") || other.gameObject.CompareTag("towel"))
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
        if (other.gameObject.CompareTag("mud"))
        {
            dirty = false;
        }

          if (other.gameObject.CompareTag("soap"))
        {
            cleanering = false;
            soapParticles.SetActive(false);
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
            bubbles.gameObject.SetActive(true);
            main.simulationSpeed = 0.2f;
            emission.rateOverTime = 5;
        }
        if (mentosCounter == 2)
        {
            main.simulationSpeed = 0.7f;
            emission.rateOverTime = 20;
        }
        if (mentosCounter == 3)
        {
            main.simulationSpeed = 1f;
            emission.rateOverTime = 50;
            //Invoke("loadLevel2", 3f);
        }
    }
    public void loadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

}
