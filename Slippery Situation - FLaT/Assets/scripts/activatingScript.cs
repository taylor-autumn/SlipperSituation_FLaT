using UnityEngine;
using TMPro;
using System.Collections;

public class activatingScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public int finalLine;
    public float textSpeed;
    private int index;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //reference the player controller and disable it
            
            Invoke("dialingit", .5f);
        }
    }
    void StartDialougue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void Start(){

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
        if (index < finalLine - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
          index = Random.Range(finalLine, lines.Length);
        }
    }
}
