using DG.Tweening;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    Vector2 deltaMouse;
    float xRotation;
    float yRotation;
    bool Isdead;

    void Start()
    {
        Isdead = false;
        AddInputListener();
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onMouseLook += OnMouseLook;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onMouseLook -= OnMouseLook;
    }

    void LateUpdate()
    {
        if(Isdead)return;
        CameraRotate();
    }

    void CameraRotate()
    {
        float mouseX = deltaMouse.x * mouseSensitivity * Time.deltaTime;
        float mouseY = deltaMouse.y * mouseSensitivity  * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void OnDead(Transform lookpoint)
    {
        Isdead = true;
        Debug.Log(Vector3.Dot(transform.forward,lookpoint.forward));
        float _duration = 1;
        if (Vector3.Dot(transform.forward, lookpoint.forward) < 0)
        {
            _duration = 1 * Mathf.Abs(Vector3.Dot(transform.forward, lookpoint.forward) *1.15f);
        }
        else
        {
            _duration = 1 * Mathf.Abs((Vector3.Dot(transform.forward, lookpoint.forward) /2f));
        }
        Debug.Log(_duration);
        transform.DOLookAt(lookpoint.position, _duration ).SetEase(Ease.OutQuad).OnComplete((() => {CustomPostprocessingManager.Instance.DeadSequnce();}));
            //.OnComplete((() => GameManager.Instance.OnChangeGameStage(GameStage.GameOver)));
    }

    void OnMouseLook(Vector2 value)
    {
        deltaMouse = value;
    }

    void OnDestroy()
    {
        RemoveInputListener();
    }
}
