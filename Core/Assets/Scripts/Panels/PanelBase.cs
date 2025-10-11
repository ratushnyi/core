using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Panels
{
    public abstract class ResultPanelBase<T> : PopupBase
    {
        private readonly UniTaskCompletionSource<T> _resultCompletionSource = new();

        public override void Hide(bool force = false)
        {
            _resultCompletionSource.TrySetResult(default);
            base.Hide(force);
        }

        protected void HideWithResult(T result)
        {
            _resultCompletionSource.TrySetResult(result);
            Hide();
        }

        public async UniTask<T> WaitForResult()
        {
            var result = await _resultCompletionSource.Task;
            await WaitForHide();

            return result;
        }
    }

    public class PopupBase : PanelBase
    {
        [Inject] private BackButtonService _backButtonService;

        public override UniTask InitializeAsync()
        {
            _backButtonService.AddAction(() => Hide()).AddTo(this);
            return base.InitializeAsync();
        }
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class PanelBase : MonoBehaviour
    {
        public virtual bool ShowInstantly => _showInstantly;

        [SerializeField] private bool _showInstantly;
        [field: SerializeField] protected bool PlayAnimation { get; private set; } = true;
        [field: SerializeField] protected float AnimationDuration { get; private set; } = 0.2f;
        [field: SerializeField] protected Ease AnimationEase { get; private set; } = Ease.InOutSine;

        private CanvasGroup _canvasGroup;
        private Sequence _sequence;

        public IObservable<bool> HideObservable => _hideSubject;
        private readonly ISubject<bool> _hideSubject = new Subject<bool>();
        private readonly UniTaskCompletionSource _hideCompletionSource = new();

        public virtual UniTask InitializeAsync()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Initialize();
            return UniTask.CompletedTask;
        }

        protected virtual void Initialize()
        {
        }

        public virtual async UniTask ShowAnimation()
        {
            gameObject.SetActive(true);

            if (PlayAnimation)
            {
                transform.localScale = Vector3.one * 2;
                _canvasGroup.alpha = 0;

                _sequence?.Kill();
                _sequence = DOTween.Sequence()
                    .Append(_canvasGroup.DOFade(1, AnimationDuration))
                    .Join(transform.DOScale(Vector3.one, AnimationDuration))
                    .SetEase(AnimationEase)
                    .SetUpdate(true);

                await _sequence.ToUniTask();
            }
        }

        public virtual async UniTask HideAnimation()
        {
            if (PlayAnimation)
            {
                _sequence?.Kill();
                _sequence = DOTween.Sequence()
                    .Append(_canvasGroup.DOFade(0, AnimationDuration))
                    .Join(transform.DOScale(Vector3.one * 2, AnimationDuration))
                    .SetEase(AnimationEase)
                    .SetUpdate(true);

                await _sequence.ToUniTask();
            }

            gameObject.SetActive(false);

            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            _hideCompletionSource.TrySetResult();
        }

        public virtual void Hide(bool force = false)
        {
            _hideSubject.OnNext(force);
        }

        public async UniTask WaitForHide()
        {
            await _hideCompletionSource.Task;
        }
    }
}