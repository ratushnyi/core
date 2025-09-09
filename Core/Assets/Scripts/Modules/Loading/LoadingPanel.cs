using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Panels;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Loading
{
    public class LoadingPanel : PanelBase
    {
        [SerializeField]
        public RectTransform _rectTransform;

        public override async UniTask ShowAnimation()
        {
            _rectTransform.anchoredPosition = new Vector2(0, Screen.height);
            await _rectTransform.DOAnchorPos(Vector2.zero, AnimationDuration / 2).SetEase(AnimationEase).ToUniTask();
        }

        public override async UniTask HideAnimation()
        {
            await _rectTransform.DOAnchorPos(new Vector2(0, -Screen.height), AnimationDuration / 2).SetEase(AnimationEase).ToUniTask();
        }
    }
}