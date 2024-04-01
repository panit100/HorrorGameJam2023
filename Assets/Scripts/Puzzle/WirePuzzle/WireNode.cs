using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireNode : MonoBehaviour
{
    public bool haveEnergy;

    [Header("Node")]
    public WirePuzzleController wirePuzzleController;
    [SerializeField] bool isNotRotate;
    public bool isStart;
    [SerializeField] List<Image> parentWires;
    void Awake()
    {
        if (!isNotRotate)
        {
            GetComponent<Button>().onClick.AddListener(RotateNode);
            int randomRotation = Random.Range(0, 4);
            transform.Rotate(0, 0, 90* randomRotation);
        }
    }

    void RotateNode()
    {
        this.transform.Rotate(0,0,90f);
        //CheckNode();
    }

    public void CheckNode()
    {
        if (isStart)
        {
            haveEnergy = true;
        }
        else
        {
            haveEnergy = false;
        }
        DisplayEnergy();
        wirePuzzleController.CheckConnectionNodes();
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
