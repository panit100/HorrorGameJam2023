using UnityEngine;

namespace Minimap
{
    public class Minimap : Singleton<Minimap>
    {
        [SerializeField] GameObject pinPrefabObj;
        [SerializeField] float objectivePinHeight = 25f;
        [SerializeField] Transform cameraTransform;
        void LateUpdate()
        {
            if (cameraTransform == null)
                return;

            var playerTransform = PlayerManager.Instance.transform;
            var targetPos = playerTransform.position;
            targetPos.y = cameraTransform.transform.position.y;
            cameraTransform.position = targetPos;
            
            var targetAngle = cameraTransform.transform.eulerAngles;
            targetAngle.y = playerTransform.transform.eulerAngles.y;
            cameraTransform.eulerAngles = targetAngle;
        }

        protected override void InitAfterAwake()
        {
            
        }

        public GameObject CreateObjectivePin(ObjectiveMapPin pin)
        {
            var newPin = Instantiate(pinPrefabObj, pin.transform, true);
            newPin.transform.localPosition = Vector3.zero;
            
            var pos = newPin.transform.position;
            pos.y = objectivePinHeight;
            
            newPin.transform.position = pos;
            newPin.gameObject.SetActive(false);
            
            return newPin;
        }
    }
}