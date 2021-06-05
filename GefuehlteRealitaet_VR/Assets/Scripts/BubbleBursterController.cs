using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BubbleBursterController : MonoBehaviourPun
{
    private List<string> BlockedRays;
    public int raycount;
    // Start is called before the first frame update
    void Start()
    {
        GameController.current.onSameWordsLogged += OnSameWords;
        BlockedRays = new List<string>();
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }
 

    private void OnSameWords(string BlockedRay, bool plus)
    {
        //if (base.photonView.IsMine) {
            if (plus) {
                BlockedRays.Add(BlockedRay);
            } else
            {
                BlockedRays.Remove(BlockedRay);
            }

            var blockedcount = BlockedRays.Count;
            print(" BlockedRays.Count:  " +blockedcount);

            if (BlockedRays.Count == raycount)
            {
                print("let the bubble burst!!!");
                GameObject.Find("Floor").GetComponent<Renderer>().material.SetColor("Color_", new Color(255f, 255f, 255f));
            }
        //}
    }


    private void OnDestroy()
    {
        GameController.current.onSameWordsLogged -= OnSameWords;
    }
}
