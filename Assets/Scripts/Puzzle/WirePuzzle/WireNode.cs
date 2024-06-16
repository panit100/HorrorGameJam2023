using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ConnectNode
{
    public WireNode wireNode;
    public bool spacialCondition;
    public Vector3 correctRotation;
}
public class WireNode : MonoBehaviour
{
    public bool haveEnergy;
    [SerializeField] List<ConnectNode> connectNode;
    [SerializeField] List<WireNode> nextNode;

    [Header("Node")]
    public WirePuzzleController wirePuzzleController;
    [SerializeField] bool isStartNode;
    [SerializeField] bool isEndNode;
    [SerializeField] bool isButton;
    [SerializeField] List<Vector3> correctRotations;
    [SerializeField] List<Image> parentWires;
    void Awake()
    {
        if (isButton)
        {
            GetComponent<Button>().onClick.AddListener(RotateNode);
        }
    }

    void RotateNode()
    {
        this.transform.Rotate(0,0,90f);
        wirePuzzleController.StartCheckNode();
    }

    public void CheckNode()
    {
        if (isStartNode == true)
        {
            haveEnergy = true;
        }
        else
        {
            haveEnergy = false;
            CheckConnectNode();
            if (haveEnergy == true)
            {
                CheckRotation();
            }
            DisplayEnergy();
        }

        if (isEndNode)
        {
            if(haveEnergy == true)
            {
                //Win
                print("Win");
                wirePuzzleController.WinGame();
            }
        }
        else
        {
            foreach (WireNode n in nextNode)
            {
                n.CheckNode();
            }
        }
    }
    void CheckRotation()
    {
        Vector3 currentRotation = GetComponent<RectTransform>().rotation.eulerAngles;
        haveEnergy = correctRotations.Contains(currentRotation);
    }

    void CheckConnectNode()
    {
        foreach (ConnectNode n in connectNode)
        {
            if (n.spacialCondition == true)
            {
                if (n.wireNode.haveEnergy == true)
                {
                    haveEnergy = true;
                    break;
                }
                else
                {
                    haveEnergy = false;
                }
            }
            else if (n.spacialCondition == true && n.spacialCondition == false)
            {
                if (n.wireNode.haveEnergy == true && n.correctRotation == n.wireNode.GetComponent<RectTransform>().rotation.eulerAngles)
                {
                    haveEnergy = true;
                    break;
                }
                else
                {
                    haveEnergy = false;
                }
            }
            else
            {
                if (n.wireNode.haveEnergy == true)
                {
                    haveEnergy = true;
                    break;
                }
                else
                {
                    haveEnergy = false;
                }
            }
        }
    }

    public void DisplayEnergy()
    {
        if (haveEnergy)
        {
            foreach (Image image in parentWires)
            {
                image.color = Color.yellow;
            }
        }
        else
        {
            foreach (Image image in parentWires)
            {
                image.color = Color.black;
            }
        }
    }
}
