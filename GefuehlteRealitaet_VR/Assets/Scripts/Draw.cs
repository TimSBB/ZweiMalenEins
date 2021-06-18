using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;


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
    public bool allowDraw = false;
    private LineRenderer currentLine;
    private LineRenderer currentLineOther;

    private PhotonView PV;
    private int playerNr;
    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;

    private Vector3 lineColor;


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 pos = new Vector3();
        //Vector3 norm = new Vector3();
        //int index = 0;
        //bool validTarget = false;

        //bool isLeftRayInteractorHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
        //bool isRightRayInteractorHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);

        //allowDraw = true;

        //if (isRightRayInteractorHovering)
        //{
        //    allowDraw = false;
        //}


        //Check if input down
        InputHelpers.IsPressed(controller.inputDevice, drawInput, out bool isPressed);
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (!isDrawing && isPressed)
        {
            StartDrawing();
            //var color = lineMaterial.GetColor("Color_DCFC887F");
            //lineColor =  new Vector3(color.r, color.g, color.b);
            var mat = lineMaterial.name.Replace(" (Instance)", "");
            PV.RPC("RPC_StartDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position, mat, lineWidth);
        }
        else if(isDrawing && !isPressed)
        {
            StopDrawing();
            PV.RPC("RPC_StopDrawing", RpcTarget.AllBufferedViaServer, playerNr);
        }
        else if(isDrawing && isPressed)
        {
            //lineColor = lineMaterial.GetColor("Color_DCFC887F");
            UpdateDrawing();
            var mat = lineMaterial.name.Replace(" (Instance)", "");
            PV.RPC("RPC_UpdateDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position, mat, lineWidth);
        }
    }

    public void SetLineMaterial(Material newMat)
    {
        lineMaterial = newMat;
    }

    public void SetLineWidth(float width)
    {
        lineWidth = width;
        Debug.Log("width: " + width);
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
        //Debug.Log("LineMaterialName: " + lineMaterial.name.Replace(" (Instance)",""));
        currentLine.startWidth = lineWidth;
        //if (playerNr == 1)
        //{
        //    currentLine.material = Player1_lineMaterial;
        //}
        //if (playerNr == 2)
        //{
        //    currentLine.material = Player2_lineMaterial;
        //}

    }

    void StopDrawing()
    {
        isDrawing = false;
        currentLinePositions.Clear();
        currentLine = null;
    }

    void UpdateDrawing()
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


    [PunRPC]
    void RPC_StartDrawing(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {

        if (playerNumber != playerNr) { 

            OtherisDrawing = true;
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

    [PunRPC]
    void RPC_UpdateLine(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {
        if (playerNumber != playerNr)
        {
            //update line
            //update line position
            if (OtherisDrawing) { 
            currentLinePositionsOther.Add(OtherDrawPositionSource);
            currentLineOther.positionCount = currentLinePositionsOther.Count;
            currentLineOther.SetPositions(currentLinePositionsOther.ToArray());

            //update line visual
            currentLineOther.material = Resources.Load<Material>("Materials/"+ ColorOfLine);
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
            currentLinePositionsOther.Clear();
            currentLineOther = null;
        }
    }


    [PunRPC]
    void RPC_UpdateDrawing(int playerNumber, Vector3 OtherDrawPositionSource, string ColorOfLine, float WidthOfLine)
    {
       if (playerNumber != playerNr)
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
