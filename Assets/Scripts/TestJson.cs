using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJson : MonoBehaviour
{
    [ContextMenuItem("Save","SaveJson")]
    [ContextMenuItem("Load","LoadJSON")]
    public TestClass[] testClass;


    void LoadJSON()
    {
        testClass = CSVHelper.LoadCSVAsObject<TestClass>("Testing");
    }

    
}
