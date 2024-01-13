using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectableDataSlot : MonoBehaviour
{
    public TMP_Text dataHeaderText;
    CollectableScriptableObject collectableObject;
    public CollectableScriptableObject CollectableObject {get {return collectableObject;} set {collectableObject = value;}}
}
