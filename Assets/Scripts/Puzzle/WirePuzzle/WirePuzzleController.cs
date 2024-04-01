using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePuzzleController : MonoBehaviour
{
    [SerializeField] List<GameObject> nodes;
    private void Start()
    {
        CheckConnectionNodes();
    }

    public void CheckConnectionNodes()
    {
        foreach (GameObject n in nodes)
        {
            n.GetComponent<WireNode>().DisplayEnergy();
        }
    }
}
