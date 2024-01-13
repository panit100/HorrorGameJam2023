using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectableObjectData",menuName = "CollectableObject/Create New Object")]
public class CollectableScriptableObject : ScriptableObject
{
    public string dataHeader;
    public string dataContent;
}
