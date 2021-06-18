using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RandomWord : MonoBehaviour
{
    public string[] words = new string[] { "Montag","Krieg","Natur","Himmel","Kälte","Freundschaft","Geruch","Heimat","Angst","Baum","Auto","Zelt","Frühstück","Liebe","Stuhl","Trocken","Geborgenheit","Lippenstift"};
    private int index;
    public string currentWord;
    private string newWordString;
    public GameObject wordText;
    private int playerNr;
    private PhotonView PV;
    private bool newwordTrigger;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //index = Random.Range(0, words.Length);
        //currentWord = words[index];
        //TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        //textmeshPro.SetText(currentWord);
    }

    // Update is called once per frame
    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNr == 1 && !newwordTrigger)
        {
            index = Random.Range(0, words.Length);
            currentWord = words[index];

            PV.RPC("RPC_RandomWord", RpcTarget.AllBufferedViaServer, currentWord);
            newwordTrigger = true;

            //TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
            //textmeshPro.SetText(currentWord);
        }
    }

    public void newWord()
    {

        //newWordString = currentWord;
        //while (newWordString == currentWord) { 
        //    index = Random.Range(0, words.Length);
        //    newWordString = words[index];
        //}
        //TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        //textmeshPro.SetText(newWordString);
    }


    [PunRPC]
    void RPC_RandomWord(string word)
    {
        TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(word);
    }
}
