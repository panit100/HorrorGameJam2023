using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    protected bool isPress = false;
    public EquipmentType equipmentType {get; protected set;}
    public virtual void OnUse()
    {
        isPress = !isPress;
    }

    public abstract void HoldAnim();
    public abstract void PutAnim();

    public void ForceSetIsPress(bool toggle)
    {
        isPress = toggle;
    }
}
