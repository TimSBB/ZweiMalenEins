using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStyler : MonoBehaviour
{

    private Material textMat;

    public void TextHover(GameObject sender)
    {
        textMat = sender.GetComponent<Renderer>().material;
        textMat.SetColor("Color_", new Color(255f,0f, 10f));
    }

    public void TextHoverExit(GameObject sender)
    {
        textMat = sender.GetComponent<Renderer>().material;
        textMat.SetColor("Color_", new Color(255f, 255f, 255f));
    }

}
