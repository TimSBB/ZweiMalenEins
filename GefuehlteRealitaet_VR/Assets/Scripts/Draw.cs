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

        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;

        bool isLeftRayInteractorHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
        bool isRightRayInteractorHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);

        allowDraw = true;

        if (isRightRayInteractorHovering)
        {
            allowDraw = false;
        }


        //Check if input down
        InputHelpers.IsPressed(controller.inputDevice, drawInput, out bool isPressed);
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (!isDrawing && isPressed && allowDraw)
        {
            StartDrawing();
            var color = lineMaterial.GetColor("Color_DCFC887F");
            lineColor =  new Vector3(color.r, color.g, color.b);
            PV.RPC("RPC_StartDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position, lineColor);
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
            PV.RPC("RPC_UpdateDrawing", RpcTarget.AllBufferedViaServer, playerNr, drawPositionSource.position);
        }
    }

    public void SetLineMaterial(Material newMat)
    {
        lineMaterial = newMat;
    }

    void StartDrawing()
    {
        isDrawing = true;
        //create line
        
        GameObject lineGameObject = new GameObject("Line");
        currentLine = lineGameObject.AddComponent<LineRenderer>();


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
    void RPC_StartDrawing(int playerNumber, Vector3 OtherDrawPositionSource, Vector3 ColorOfLine)
    {
        //print("playerNumber = "+ playerNumber);
        //print("playerNr = " + playerNr);

        if (playerNumber != playerNr) { 
        OtherisDrawing = true;
        //create line
        GameObject lineGameObject = new GameObject("Line");
        currentLineOther = lineGameObject.AddComponent<LineRenderer>();
            var color = new Color(ColorOfLine.x, ColorOfLine.y, ColorOfLine.z, 1.0f);
        currentLineOther.material.SetColor("Color_", color);
            //print("Remote Start Drawing got triggered");

            PV.RPC("RPC_UpdateLine", RpcTarget.AllBufferedViaServer, playerNumber, OtherDrawPositionSource);
        }
    }

    [PunRPC]
    void RPC_UpdateLine(int playerNumber, Vector3 OtherDrawPositionSource)
    {
        if (playerNumber != playerNr)
        {
            //print("playerNumber = " + playerNumber);
            //print("playerNr = " + playerNr);
            //print("Remote Update Line got triggered");
            //update line
            //update line position
            currentLinePositionsOther.Add(OtherDrawPositionSource);
            currentLineOther.positionCount = currentLinePositionsOther.Count;
            currentLineOther.SetPositions(currentLinePositionsOther.ToArray());

            //update line visual
            //if (playerNr == 1)
            //{
            //    currentLineOther.material = Player2_lineMaterial;
            //}
            //if (playerNr == 2)
            //{
            //    currentLineOther.material = Player1_lineMaterial;
            //}
            //currentLineOther.material = lineMaterial;
            //currentLineOther.material.SetColor("Color_", new Color(255f, 0f, 0f));
            currentLineOther.startWidth = lineWidth;
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
    void RPC_UpdateDrawing(int playerNumber, Vector3 OtherDrawPositionSource)
    {
       if (playerNumber != playerNr)
        {
            //print("Remote Update Drawing got triggered");
            //check if we have a line
            if (!currentLineOther || currentLinePositionsOther.Count == 0)
                return;

            Vector3 lastSetPosition = currentLinePositionsOther[currentLinePositionsOther.Count - 1];
            if (Vector3.Distance(lastSetPosition, OtherDrawPositionSource) > distanceThreshold)
            {
                PV.RPC("RPC_UpdateLine", RpcTarget.AllBufferedViaServer, playerNumber, OtherDrawPositionSource);
            }
        }
    }

}
