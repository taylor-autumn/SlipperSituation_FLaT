using TMPro;
using UnityEngine;

public class menuManager : MonoBehaviour
{

    [Header("main ui page")]
    public GameObject uiPage;
    public TMP_Text titleText;
    public TMP_Text wordText;

    [Header("warning page")]
    public GameObject warningPage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void openAbout()
    {
        titleText.text = "About Page";
        wordText.text = "insert about description here";
    }

    public void openCredits()
    {
        titleText.text = "Credits Page";
        wordText.text = "insert credits description here";
    }
}
