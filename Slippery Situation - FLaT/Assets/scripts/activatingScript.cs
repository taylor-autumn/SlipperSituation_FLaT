using UnityEngine;

public class activatingScript : MonoBehaviour
{
    public GameObject dial;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dial.SetActive(true);
        }
    }
    void OgerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dial.SetActive(false);
        }
    }
}
