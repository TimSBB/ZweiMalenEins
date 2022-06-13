using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShaderAnimation : MonoBehaviour
{

    public GameObject explosionTrigger;
    public Material Dissolve2;
    float temp = -1f;

    private void Start()
    {
        Dissolve2.SetFloat("Vector1_DA911BF4", temp);

    }

    private void Update()
    {
        if (explosionTrigger.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            temp += Time.deltaTime /2 ;
            Dissolve2.SetFloat("Vector1_DA911BF4", temp);
            
        }


    }
}