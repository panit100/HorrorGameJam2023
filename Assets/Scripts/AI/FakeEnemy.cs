using System;
using DG.Tweening;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

namespace HorrorJam.AI
{
    public class FakeEnemy : MonoBehaviour
    {
        [Header("Scan Info")]
        [SerializeField] SpriteRenderer sprite;
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
            scanable.OnDeactiveScanWithDuration(1.5f);
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