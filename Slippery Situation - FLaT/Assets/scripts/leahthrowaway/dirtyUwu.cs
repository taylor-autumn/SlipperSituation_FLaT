using System;
using UnityEngine;

public class dirtyUwu : MonoBehaviour
{
    public Material myMaterial;
    Color normColor = new Color(0.4816661f, 0.7283878f, 0.8301887f, 0.6509804f);
    Color mudColor = new Color(0.3490566f, 0.1512773f, 0f, 0.6509804f);
    public float changering = 0;
    public GameObject soapParticles;
    private bool dirty = false;
    private bool cleanering = false;

    void Start()
    {
        soapParticles.SetActive(false);
        myMaterial.color = normColor;
    }
    void Update()
    {
        if (dirty)
        {
            if (changering < 1)
                {
                    changering += 0.0002f;
                }
            Color lerpingit = Color.Lerp(normColor, mudColor, changering);
            myMaterial.color = lerpingit;
        }
        if (cleanering)
        {
            if (changering > 0)
                {
                    changering -= 0.0002f;
                }
            Color lerpingit = Color.Lerp(normColor, mudColor, changering);
            myMaterial.color = lerpingit;
        }
    }
    void OnTriggerEnter(Collider other)
    {
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
    void OnTriggerExit(Collider other)
    {
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
}
