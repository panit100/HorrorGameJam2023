using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HorrorJam.AI
{
    public class FakeEnemy : MonoBehaviour
    {
        [Header("Scan Info")]
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] float fadeOutTime = 1.5f;
        Scanable scanable;

        void Awake() 
        {
            scanable = GetComponentInChildren<Scanable>();
        }

        void Start()
        {
            scanable.onScanComplete += FadeOut;
            scanable.onDeactiveScanComplete += HideAI;

            MakeVisibleEnemy(scanable.scanProgress);
        }

        void Update()
        {
            OnBeingScanned();
        }

        public void OnBeingScanned()
        {
            MakeVisibleEnemy(scanable.scanProgress);
        }

        void MakeVisibleEnemy(float visibleValue)
        {
            float alpha = Mathf.Clamp(visibleValue / 100f,0f,1f); 

            sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,alpha);
        }

        void FadeOut()
        {
            scanable.OnDeactiveScanWithDuration(fadeOutTime);
        }

        public void HideAI()
        {
            gameObject.SetActive(false);
        }

        [Button]
        public void ShowAI()
        {
            gameObject.SetActive(true);
        }
    }
}
