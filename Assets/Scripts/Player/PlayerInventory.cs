using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<CollectableScriptableObject> collectableObjects = new List<CollectableScriptableObject>();

    public List<CollectableScriptableObject> CollectableObjects {get {return collectableObjects;}}

    public void OnCollectObject(CollectableScriptableObject _object)
    {
        collectableObjects.Add(_object);
    }
}
