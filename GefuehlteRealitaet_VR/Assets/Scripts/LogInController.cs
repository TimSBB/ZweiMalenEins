using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LogInController : MonoBehaviour
{
    private int LabelNr = 0;
    private int playerNumber;
    private string word;
    private string label;
    private GameObject selectedWord;
    private Material originalMaterial;

    public float Duration = 5;
    private float _Percentage;
    private bool _Fired = false;
    private bool loggedIn = false;


    // Start is called before the first frame update

    void Update()
    {
        if (_Fired)
        {
            _Percentage += Time.deltaTime / (Duration * 2000);
            selectedWord.GetComponent<Renderer>().material.SetColor("Color_", new Color(_Percentage.Remap(0, 1, 0, 255), _Percentage.Remap(0, 1, 0, 255), _Percentage.Remap(0, 1, 0, 255)));
            //selectedWord.GetComponent<Renderer>().material.SetColor("Color_", Color.HSVToRGB(0.6f, _Percentage, 0.5f));
            if (_Percentage > 1)
            {
                print("Word is logged in!!");
                loggedIn = true;
                _Percentage = 0;
                _Fired = false;
            }
        }
    }

    public void OnLogInFeedback()
    {
        selectedWord = this.GetComponent<XRBaseInteractor>().selectTarget.gameObject;
        originalMaterial = selectedWord.GetComponent<Renderer>().material;
        _Percentage = 0;
        _Fired = true;
        if (loggedIn)
        {
            word = selectedWord.name;
            label = this.tag;
            GameController.current.wordLogIn(playerNumber, word, label);
        }
        // selectedWord.GetComponent<Renderer>().material.SetColor("Color_", Color.Lerp(originalMaterial.color, new Color(255f, 0f, 10f), 1.5f));
        //selectedWord.transform.localScale *= 2;
        //GameController.current.wordLogIn(LabelNr, playerNumber);
    }

    public void RevertLogInFeedback()
    {
        _Percentage = 0;
        _Fired = false;
        loggedIn = false;
        selectedWord.GetComponent<Renderer>().material.SetColor("Color_", new Color(255, 255, 255));
        //selectedWord.transform.localScale *= 0.5f;
        if (!loggedIn)
        {
            word = selectedWord.name;
            label = this.tag;
            GameController.current.wordLogIn(playerNumber, word, label);
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