using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Panels;
using UnityEngine;

namespace TendedTarsier.Core.Modules.General
{
    public class LoadingPanel : PanelBase
    {
        [SerializeField]
        public RectTransform _rectTransform;
        [SerializeField]
        public Ease _ease = Ease.InOutBack;
        [SerializeField]
        public float _duration = 0.5f;

        public override async UniTask ShowAnimation()
        {
            _rectTransform.anchoredPosition = new Vector2(0, Screen.height);
            await _rectTransform.DOAnchorPos(Vector2.zero, _duration).SetEase(_ease).ToUniTask();
        }

        public override async UniTask HideAnimation()
        {
            await _rectTransform.DOAnchorPos(new Vector2(0, -Screen.height), _duration).SetEase(_ease).ToUniTask();
        }
    }
}