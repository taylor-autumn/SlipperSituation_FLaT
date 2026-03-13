using UnityEngine;
using TMPro;
using System.Collections;

public class activatingScript : MonoBehaviour
{
    public GameObject textIcon;
    public TextMeshPro textComponent;
    public string[] lines;
    public int secondPart;
    public float textSpeed;
    private int index;
    private playerMove playerMoveScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMoveScript = GameObject.Find("Player").GetComponent<playerMove>();
            playerMoveScript.enabled = false;
            Invoke("dialingit", 0.5f);
        }
    }
    void StartDialougue()
    {
        //index = 0;
        print("start");
        StartCoroutine(TypeLine());
    }

    void dialingit(){
        print("dialed");
        textIcon.SetActive(true);
        textComponent.text = string.Empty;
        StartDialougue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
                if (index > secondPart - 1)
                {
                textIcon.SetActive(false);
                playerMoveScript.enabled = true;
                }
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if (index < secondPart - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index++;
            textIcon.SetActive(false);
            playerMoveScript.enabled = true;
        }
        if (index > secondPart - 1)
        {
            textComponent.text = string.Empty;
            index = Random.Range(secondPart, lines.Length);
            StartCoroutine(TypeLine());
        }
    }
}
