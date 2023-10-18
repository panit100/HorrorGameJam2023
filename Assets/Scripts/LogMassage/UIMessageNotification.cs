using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LogMassage
{
    public class UIMessageNotification : MonoBehaviour
    {
        [SerializeField] Image bgImage;
        [SerializeField] float duration;
        [SerializeField] Ease enterEase;
        [SerializeField] Ease exitEase;

        [Button]
        public void PlayEnter()
        {
            DOTween.Kill(this);
            bgImage.DOFillAmount(1f, duration).SetEase(enterEase).SetTarget(this);
        }

        [Button]
        public void PlayExit()
        {
            DOTween.Kill(this);
            bgImage.DOFillAmount(0f, duration).SetEase(exitEase).SetTarget(this);
        }
    }
}