using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStyler : MonoBehaviour
{

    private Material textMat;
    private Color previousColor;

    public void TextHover(GameObject sender)
    {
        if (!sender.GetComponent<XRGrabNetworkInteractable>().isSelected) { 
        textMat = sender.GetComponent<Renderer>().material;
        previousColor = textMat.GetColor("Color_");
        textMat.SetColor("Color_", new Color(255f,0f, 10f));
        }
    }

    public void TextHoverExit(GameObject sender)
    {
        if (!sender.GetComponent<XRGrabNetworkInteractable>().isSelected)
        {
            textMat = sender.GetComponent<Renderer>().material;
            textMat.SetColor("Color_", previousColor);
        }

    }

}
