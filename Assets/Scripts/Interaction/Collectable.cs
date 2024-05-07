using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, InteractObject
{
    [SerializeField] CollectableScriptableObject collectableData;

    public void OnInteract()
    {
        PlayerManager.Instance.PlayerInventory.OnCollectObject(collectableData);

        Destroy(gameObject);
    }
}
