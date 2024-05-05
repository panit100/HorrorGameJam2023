using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePuzzleController : MonoBehaviour
{
    [SerializeField] WireNode startNode;
    [SerializeField] List<WireNode> allNodes;
    [Header("UI")]
    [SerializeField] GameObject winPanel;

    private void Start()
    {
        winPanel.SetActive(false);
    }

    public void StartCheckNode()
    {
        foreach (WireNode n in allNodes)
        {
            n.haveEnergy = false;
            n.DisplayEnergy();
        }
        startNode.CheckNode();
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
    }
}
