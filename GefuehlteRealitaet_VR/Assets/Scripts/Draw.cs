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
    public float distanceThreshold = 0.05f;

    private List<Vector3> currentLinePositions = new List<Vector3>();
    private List<Vector3> currentLinePositionsOther = new List<Vector3>();
    private XRController controller;
    private bool isDrawing = false;
    private bool OtherisDrawing = false;
    private LineRenderer currentLine;
    private LineRenderer currentLineOther;

    private PhotonView PV;
    private int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if input down
        InputHelpers.IsPressed(controller.inputDevice, drawInput, out bool isPressed);

        if(!isDrawing && isPressed)
        {
            StartDrawing();
            PV.RPC("RPC_StartDrawing", RpcTarget.AllBufferedViaServer, playerNumber, drawPositionSource.position);
        }
        else if(isDrawing && !isPressed)
        {
            StopDrawing();
            PV.RPC("RPC_StopDrawing", RpcTarget.AllBufferedViaServer, playerNumber);
        }
        else if(isDrawing && isPressed)
        {
            UpdateDrawing();
            PV.RPC("RPC_UpdateDrawing", RpcTarget.AllBufferedViaServer, playerNumber, drawPositionSource.position);
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
    void RPC_StartDrawing(int playerNumber, Vector3 OtherDrawPositionSource)
    {
        if (playerNumber != this.playerNumber) { 
        OtherisDrawing = true;
        //create line
        GameObject lineGameObject = new GameObject("Line");
        currentLineOther = lineGameObject.AddComponent<LineRenderer>();

        PV.RPC("RPC_UpdateLine", RpcTarget.AllBufferedViaServer, playerNumber, OtherDrawPositionSource);
        }
    }

    [PunRPC]
    void RPC_UpdateLine(int playerNumber, Vector3 OtherDrawPositionSource)
    {
        if (playerNumber != this.playerNumber)
        {
            //update line
            //update line position
            currentLinePositionsOther.Add(OtherDrawPositionSource);
            currentLineOther.positionCount = currentLinePositionsOther.Count;
            currentLineOther.SetPositions(currentLinePositionsOther.ToArray());

            //update line visual
            currentLineOther.material = lineMaterial;
            currentLineOther.startWidth = lineWidth;
        }
    }

    [PunRPC]
    void RPC_StopDrawing(int playerNumber)
    {
        if (playerNumber != this.playerNumber)
        {
            OtherisDrawing = false;
            currentLinePositionsOther.Clear();
            currentLineOther = null;
        }
    }


    [PunRPC]
    void RPC_UpdateDrawing(int playerNumber, Vector3 OtherDrawPositionSource)
    {
        if (playerNumber != this.playerNumber)
        {
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
