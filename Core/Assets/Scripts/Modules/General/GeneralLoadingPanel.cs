using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Panels;
using UnityEngine;
using UnityEngine.Serialization;

namespace TendedTarsier.Core.Modules.General
{
    public class GeneralLoadingPanel : PanelBase
    {
        [SerializeField]
        public RectTransform _rectTransform;
        [SerializeField]
        public Ease _ease = Ease.InOutBack;
        [SerializeField]
        public float _totalDuration = 1.5f;

        public override async UniTask ShowAnimation()
        {
            _rectTransform.anchoredPosition = new Vector2(0, Screen.height);
            await _rectTransform.DOAnchorPos(Vector2.zero, _totalDuration / 2).SetEase(_ease).ToUniTask();
        }

        public override async UniTask HideAnimation()
        {
            await _rectTransform.DOAnchorPos(new Vector2(0, -Screen.height), _totalDuration / 2).SetEase(_ease).ToUniTask();
        }
    }
}