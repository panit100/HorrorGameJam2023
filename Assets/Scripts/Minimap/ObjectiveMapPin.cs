using Sirenix.OdinInspector;
using UnityEngine;

namespace Minimap
{
    public class ObjectiveMapPin : MonoBehaviour
    {
        [SerializeField] bool isShowOnStart;
        GameObject pinObject;
        void Start()
        {
            pinObject = Minimap.Instance.CreateObjectivePin(this);
            if (isShowOnStart)
                Show();
        }
        
        [Button]
        public void Show()
        {
            pinObject.gameObject.SetActive(true);
        }

        [Button]
        public void Hide()
        {
           pinObject.gameObject.SetActive(false);
        }
    }
}