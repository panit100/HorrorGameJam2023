using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    protected bool isPress = false;
    public EquipmentType equipmentType;
    public virtual void OnUse()
    {
        isPress = !isPress;
    }

    
}
