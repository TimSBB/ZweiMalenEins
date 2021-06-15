using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomWord : MonoBehaviour
{
    private string[] words = new string[] { "Montag","Krieg","Natur","Himmel","Kälte","Freundschaft","Geruch","Heimat","Angst","Baum","Auto","Zelt","Frühstück","Liebe","Stuhl","Trocken","Geborgenheit","Lippenstift"};
    private int index;
    private string currentWord;
    private string newWordString;
    public GameObject wordText;

    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, words.Length);
        currentWord = words[index];
        TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(currentWord);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newWord()
    {
        newWordString = currentWord;
        while (newWordString == currentWord) { 
            index = Random.Range(0, words.Length);
            newWordString = words[index];
        }
        TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(newWordString);
    }
}
