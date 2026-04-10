using UnityEngine;

public class ExtraTransitionScript : MonoBehaviour
{
    public GameObject firstCam;
    public GameObject mainCam;
    public AudioSource audioSource;
    public AudioClip transitionSound;
    public GameObject thisbword;    
    void Start()
    {
        GameObject.Find("Player").GetComponent<playerMove>().enabled = false;
        firstCam.SetActive(true);
        mainCam.SetActive(false);
        Invoke("soundingit", 1f);
        Invoke("toTheMain", 3f);
    }

    void soundingit()
    {
        audioSource.PlayOneShot(transitionSound);
    }
    void toTheMain()
    {
        GameObject.Find("Player").GetComponent<playerMove>().enabled = true;
        firstCam.SetActive(false);
        mainCam.SetActive(true);
        Destroy(thisbword);
    }
}
