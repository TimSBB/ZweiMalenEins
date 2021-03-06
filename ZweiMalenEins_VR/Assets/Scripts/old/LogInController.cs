using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System.Linq;

public class LogInController : MonoBehaviour
{
    private int playerNr;
    private string word;
    private string label;
    private GameObject selectedWord;
    private Material originalMaterial;

    public float Duration = 5;
    private float _Percentage;
    private bool _Fired = false;
    private bool loggedIn = false;

    private Mesh mesh;
    public Material material;
    private Vector3 SnapPos;
    // Start is called before the first frame update
    private void Start()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    void Update()
    {
        if (_Fired)
        {
            _Percentage += Time.deltaTime / Duration;
            selectedWord.GetComponent<Renderer>().material.SetColor("Color_", new Color(_Percentage.Remap(0, 1, 0, 255), _Percentage.Remap(0, 1, 0, 255), _Percentage.Remap(0, 1, 0, 255)));
            //selectedWord.GetComponent<Renderer>().material.SetColor("Color_", Color.HSVToRGB(0.6f, _Percentage, 0.5f));
            //print("Percentage: " + _Percentage);
            if (_Percentage > 1)
            {
                print("Word is logged in!!");
                loggedIn = true;
                _Percentage = 0;
                _Fired = false;

                if (loggedIn)
                {
                    word = selectedWord.name;
                    label = this.tag;
                    print("Label is: " + label);
                    selectedWord.gameObject.tag = label;
                    GameController.current.wordLogIn(playerNr, word, label);
                }
            }
        }

        //if (drawMesh) { HoverDrawMesh(); }
        
    }

    public void OnLogInFeedback()
    {
        selectedWord = this.GetComponent<XRBaseInteractor>().selectTarget.gameObject;
        originalMaterial = selectedWord.GetComponent<Renderer>().material;
        _Percentage = 0;
        _Fired = true;

    }

    public void RevertLogInFeedback()
    {
        _Percentage = 0;
        _Fired = false;
        loggedIn = false;
        print("Revert Log in got triggered");
        selectedWord.GetComponent<Renderer>().material.SetColor("Color_", new Color(255, 255, 255));
        //selectedWord.transform.localScale *= 0.5f;
        if (!loggedIn)
        {
            word = selectedWord.name;
            label = this.tag;
            selectedWord.gameObject.tag = "word";
            GameController.current.wordLogOut(playerNr, word, label);
            this.GetComponent<XRBaseInteractor>().allowSelect = false;
        }
    }



}
public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}