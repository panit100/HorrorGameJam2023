using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnChangeGameStage(GameStage.Playing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
