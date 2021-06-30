using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System.IO;
using SimpleJSON;
using TMPro;

public class Draw : MonoBehaviour
{
    public InputHelpers.Button drawInput;
    public Transform drawPositionSource;
    public float lineWidth = 0.03f;
    public Material lineMaterial;
    public Material Player1_lineMaterial;
    public Material Player2_lineMaterial;
    public float distanceThreshold = 0.05f;

    private List<Vector3> currentLinePositions = new List<Vector3>();
    private List<Vector3> currentLinePositionsOther = new List<Vector3>();
    private XRController controller;
    public bool isDrawing = false;
    public bool OtherisDrawing = false;

    private LineRenderer currentLine;
    private LineRenderer currentLineOther;

    private PhotonView PV;
    private int playerNr;
    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;

    private Vector3 lineColor;
    public bool DoNetworkDraw = false;
    public int publictintenstand = 300;
    private int tintenstand;
    public bool nextScene = false;
    public bool firstDraw = false;
    public bool allowDraw = true;
    public bool OtherisDrawingAllowedToDraw;




    // Start is called before the first frame update
    void Start()
    {
        tintenstand = publictintenstand;
        PV = GetComponent<PhotonView>();
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI textmeshPro = GameObject.Find("tintenstandzahl").GetComponent<TextMeshProUGUI>();
        var debustring = "tintentsatnd: " + tintenstand.ToString() + "\n" + "allowdraw: " + allowDraw + "\n" + "otherisdrawing: " + OtherisDrawing +"\n" + "OtherisDrawingAllowedToDraw: " + OtherisDrawingAllowedToDraw;
        textmeshPro.SetText(debustring);
        Debug.Log("allow Draw" + allowDraw);
        //Check if input down
        InputHelpers.IsPressed(controller.inputDevice, drawInput, out bool isPressed);
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;

            if (!isDrawing && isPressed)
            {
                if (!OtherisDrawing || !nextScene)
                {
                        var mat = lineMaterial.name.Replace(" (Instance)", "");
                        PV.RPC("RPC_StartDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position, mat, lineWidth);
                }

            }
            else if (isDrawing && !isPressed)
            {
                StopDrawing();
                PV.RPC("RPC_StopDrawing", RpcTarget.AllBufferedViaServer, playerNr);
            }

            else if (isDrawing && isPressed)
            {
                UpdateDrawing();
                var mat = lineMaterial.name.Replace(" (Instance)", "");
                PV.RPC("RPC_UpdateDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position, mat, lineWidth);
            }
    }

    // will be set in ChangeScene
    public void SetNetworkDrawing()
    {
        DoNetworkDraw = true;
        allowDraw = true;
        OtherisDrawingAllowedToDraw = true;
    }

    // is called on interface item when trigger enters interface item
    public void SetLineMaterial(Material newMat)
    {
        lineMaterial = newMat;
    }

    // is called on interface item when trigger enters interface item
    public void SetLineWidth(float width)
    {
        lineWidth = width;
        //Debug.Log("width: " + width);
    }

   

    void StartDrawing()
    {
            isDrawing = true;
            //create line

            GameObject lineGameObject = new GameObject("Line");
            lineGameObject.transform.SetParent(GameObject.Find("Drawing").transform);
            currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;

            UpdateLine();
    }

    void UpdateLine()
    {
        //update line
        //update line position

        currentLinePositions.Add(drawPositionSource.position);
        currentLine.positionCount = currentLinePositions.Count;
        currentLine.SetPositions(currentLinePositions.ToArray());

        //update line visual
        currentLine.material = lineMaterial;
        currentLine.startWidth = lineWidth;
        if (nextScene)
        {
           
            if (tintenstand > 0)
            {
                tintenstand--;
            }
            if (tintenstand <= 0)
            {
                PV.RPC("RPC_SetAllowDraw", RpcTarget.AllBufferedViaServer, playerNr);
                StopDrawing();
                PV.RPC("RPC_StopDrawing", RpcTarget.AllBufferedViaServer, playerNr);
            }

        }
       

    }

    void StopDrawing()
    {
        isDrawing = false;
        currentLinePositions.Clear();
        currentLine = null;
    }

    void UpdateDrawing()
    {
        if (allowDraw)
        {
            //check if we have a line
            if (!currentLine || currentLinePositions.Count == 0)
                return;

            Vector3 lastSetPosition = currentLinePositions[currentLinePositions.Count - 1];
            if (Vector3.Distance(lastSetPosition, drawPositionSource.position) > distanceThreshold)
            {
                UpdateLine();
            }
        }
    }

    [PunRPC]
    void RPC_SetAllowDraw(int playerNumber)
    {
        if (playerNumber != playerNr)
        {
            allowDraw = true;
            OtherisDrawingAllowedToDraw = false;
            tintenstand = publictintenstand;
        }
        else
        {
            allowDraw = false;
            OtherisDrawingAllowedToDraw = true;
        }
    }
    [PunRPC]
    void RPC_SetInitalAllowDraw(int playerNumber)
    {
        if (playerNumber != playerNr)
        {
            allowDraw = false;
            OtherisDrawingAllowedToDraw = true;
        }
        else
        {
            allowDraw = true;
            OtherisDrawingAllowedToDraw = false;
            tintenstand = publictintenstand;
        }
    }


    [PunRPC]
    void RPC_StartDrawing(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {

        if (playerNumber != playerNr) {
            OtherisDrawing = true;
 
            if (DoNetworkDraw)
            {
                if (nextScene && OtherisDrawingAllowedToDraw)
                {
                    allowDraw = false;
                    if (!firstDraw)
                    {
                        firstDraw = true;
                        PV.RPC("RPC_SetAllowDraw", RpcTarget.AllBufferedViaServer, playerNr);
                    }
                }
                //create line
                GameObject lineGameObject = new GameObject("Line");
                lineGameObject.transform.SetParent(GameObject.Find("Drawing").transform);
                currentLineOther = lineGameObject.AddComponent<LineRenderer>();
                currentLineOther.useWorldSpace = false;
                //currentLineOther.material = Resources.Load<Material>("Materials/" + ColorOfLine);
                //print("material: " + currentLineOther.material);

                PV.RPC("RPC_UpdateLine", RpcTarget.AllBufferedViaServer, playerNumber, OtherDrawPositionSource, ColorOfLine, WidthOfLine);
            }
        }
        else
        {
            OtherisDrawing = false;
            if (allowDraw)
            {
                if (nextScene && OtherisDrawingAllowedToDraw)
                {
                    if (!firstDraw)
                    {
                        firstDraw = true;
                        PV.RPC("RPC_SetInitalAllowDraw", RpcTarget.AllBufferedViaServer, playerNr);
                    }
                }
                StartDrawing();
            }
        }
    }

    [PunRPC]
    void RPC_UpdateLine(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {
        if (playerNumber != playerNr)
        {
            //update line
            //update line position
            if (OtherisDrawing && OtherisDrawingAllowedToDraw) {
                currentLinePositionsOther.Add(OtherDrawPositionSource);
                currentLineOther.positionCount = currentLinePositionsOther.Count;
                currentLineOther.SetPositions(currentLinePositionsOther.ToArray());

                //update line visual
                currentLineOther.material = Resources.Load<Material>("Materials/" + ColorOfLine);
                currentLineOther.startWidth = WidthOfLine;
            }

        }
    }

    [PunRPC]
    void RPC_StopDrawing(int playerNumber)
    {
        if (playerNumber != playerNr)
        {
            OtherisDrawing = false;
            if (DoNetworkDraw)
            {
                currentLinePositionsOther.Clear();
                currentLineOther = null;
            }
        }
    }


    [PunRPC]
    void RPC_UpdateDrawing(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {
        if (playerNumber != playerNr)
        {
            if (DoNetworkDraw)
            {
                //check if we have a line
                if (!currentLineOther || currentLinePositionsOther.Count == 0)
                    return;

                Vector3 lastSetPosition = currentLinePositionsOther[currentLinePositionsOther.Count - 1];
                if (Vector3.Distance(lastSetPosition, OtherDrawPositionSource) > distanceThreshold)
                {
                    PV.RPC("RPC_UpdateLine", RpcTarget.AllBufferedViaServer, playerNumber, OtherDrawPositionSource, ColorOfLine, WidthOfLine);
                }
            }
        }
    }




    // is called on interface item when trigger enters interface item
    public void SetTipMaterial(Material newMat)
    {

            PV.RPC("RPC_UpdateTipColor", RpcTarget.AllBufferedViaServer, playerNr, newMat.name.Replace(" (Instance)", ""));

    }

    [PunRPC]
    void RPC_UpdateTipColor(int playerNumber, string ColorOfTip)
    {
        if (playerNumber != playerNr)
        {
            var tipMat = Resources.Load<Material>("Materials/" + ColorOfTip);
            if (playerNr == 1)
            {
                //update tip color
                GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/Gross").GetComponent<Renderer>().material = tipMat;
                GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/Mittel").GetComponent<Renderer>().material = tipMat;
                GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/klein").GetComponent<Renderer>().material = tipMat;
            }
            if (playerNr == 2)
            {
                //update tip color
                GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/Gross").GetComponent<Renderer>().material = tipMat;
                GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/Mittel").GetComponent<Renderer>().material = tipMat;
                GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/klein").GetComponent<Renderer>().material = tipMat;
            }

        }
    }


    //// is called on interface item when trigger enters interface item
    public void SetTipWidth(string tipAuswahl)
    {

            PV.RPC("RPC_UpdateTipWidth", RpcTarget.AllBufferedViaServer, playerNr, tipAuswahl);
 
    }
    [PunRPC]
    void RPC_UpdateTipWidth(int playerNumber, string WidthOfTip)
    {
        if (playerNumber != playerNr)
        {
            if (playerNr == 1)
            {
                // find Ringe and X
                var gross = GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/Gross");
                var mittel = GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/Mittel");
                var klein = GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/klein");
                var radier = GameObject.Find("Network Player 2(Clone)").transform.Find("Right Hand/DrawController_Prefab/Radierer");

                //update tip width
                if (WidthOfTip == "Gross_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = true;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "Mittel_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = true;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "klein_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = true;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "Radierer_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            if (playerNr == 2)
            {
                // find Ringe and X
                var gross = GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/Gross");
                var mittel = GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/Mittel");
                var klein = GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/klein");
                var radier = GameObject.Find("Network Player(Clone)").transform.Find("Right Hand/DrawController_Prefab/Radierer");

                //update tip width
                if (WidthOfTip == "Gross_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = true;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "Mittel_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = true;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "klein_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = true;
                    radier.GetComponent<MeshRenderer>().enabled = false;
                }
                if (WidthOfTip == "Radierer_Auswahl")
                {
                    gross.GetComponent<MeshRenderer>().enabled = false;
                    mittel.GetComponent<MeshRenderer>().enabled = false;
                    klein.GetComponent<MeshRenderer>().enabled = false;
                    radier.GetComponent<MeshRenderer>().enabled = true;
                }

            }

        }
        
    
    }



    // is called when new galleryInterface Item is hovered with controller >> GalleryDome >> bb_..
    public void SetGalleryItem(int loadIndex)
    {
        PV.RPC("RPC_SetGalleryItem", RpcTarget.AllBufferedViaServer, playerNr, loadIndex);
    }
    [PunRPC]
    void RPC_SetGalleryItem(int playerNumber, int loadIndex)
    {
        if (playerNumber != playerNr)
        {
            var gL = GameObject.Find("GalleryDome").GetComponent<GalleryLoader>();
            gL.LoadItem(loadIndex);
        }
    }
}
