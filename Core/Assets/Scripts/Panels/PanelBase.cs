using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace TendedTarsier.Core.Panels
{
    public abstract class ResultPanelBase<T> : PanelBase
    {
        private readonly UniTaskCompletionSource<T> _resultCompletionSource = new();

        protected void SetResult(T result)
        {
            _resultCompletionSource.TrySetResult(result);
        }

        public async UniTask<T> WaitForResult()
        {
            var result = await _resultCompletionSource.Task;
            await WaitForHide();

            return result;
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

        protected readonly CompositeDisposable CompositeDisposable = new();

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
                    .SetEase(AnimationEase);
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
                    .SetEase(AnimationEase);
                await _sequence.ToUniTask();
            }

            gameObject.SetActive(false);

            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            _hideCompletionSource.TrySetResult();
        }

        public void Hide(bool force = false)
        {
            _hideSubject.OnNext(force);
        }

        public async UniTask WaitForHide()
        {
            await _hideCompletionSource.Task;
        }

        protected virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}