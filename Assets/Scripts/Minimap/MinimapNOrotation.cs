using UnityEngine;

public class MinimapNOrotation : MonoBehaviour
{
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
