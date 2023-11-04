using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] Vector3 movePos;
    [SerializeField] float duration = 1f;
    [SerializeField] Ease ease;
    [SerializeField] bool isDoorOpen = false;
    // [SerializeField] bool scanBeforeInteract = false;

    Vector3 originPos;
    // SpriteRenderer buttonSprite;
    // Scanable scanable;

    
    string doorID => gameObject.name + "_ID";
    void Start()
    {
        // buttonSprite = GetComponent<SpriteRenderer>();

        // if(TryGetComponent<Scanable>(out Scanable _scanable))
        //     scanable = _scanable;

        originPos = transform.position;

        // OnBeingScanned();
    }

    // void Update()
    // {
    //     OnBeingScanned();
    // }

    [Button]
    public void OpenDoorAndBakeNavMesh()
    {
        OpenDoor();
        // AIManager.Instance.BakeNavMeshAfterDelay(duration);
    }

    public void OpenDoor()
    {
        // if(scanable != null)
        //     if(!scanable.AlreadyScan)
        //         return;

        if(isDoorOpen == true)
            return;
            
        isDoorOpen = true;

        DOTween.Kill(doorID);
        transform
            .DOMove(transform.position + movePos, duration)
            .SetEase(ease)
            .SetId(doorID);
    }

    [Button]
    public void CloseDoorAndBakeNavMesh()
    {
        CloseDoor();
        // AIManager.Instance.BakeNavMeshAfterDelay(duration);
    }

    public void CloseDoor()
    {
        if(isDoorOpen == false)
            return;

        isDoorOpen = false;

        DOTween.Kill(doorID);
        transform
            .DOMove(originPos, duration)
            .SetEase(ease)
            .SetId(doorID);
    }

    // public void OnInteract()
    // {
    //     OpenDoor();
    // }

    // public void OnBeingScanned()
    // {
    //     if(scanable != null)
    //         MakeVisibleEnemy(scanable.scanProgress);
    // }

    // void MakeVisibleEnemy(float visibleValue)
    // {
    //     float alpha = Mathf.Clamp(visibleValue / 100f,0f,1f); 

    //     buttonSprite.color = new Color(buttonSprite.color.r,buttonSprite.color.g,buttonSprite.color.b,alpha);
    // }
}
