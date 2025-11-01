using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Panels
{
    [UsedImplicitly]
    public class PanelLoader<T> where T : PanelBase
    {
        public enum State
        {
            Hide = 0,
            Show = 1,
            Hiding = 2,
            Showing = 3,
        }

        private readonly T _prefab;
        private readonly Canvas _canvas;
        private readonly DiContainer _container;
        private IDisposable _hideDisposable;
        private AutoResetUniTaskCompletionSource _hideCompletionSource = AutoResetUniTaskCompletionSource.Create();

        public T Instance { get; private set; }
        public State PanelState { get; private set; }

        private PanelLoader(T prefab, Canvas canvas, DiContainer container)
        {
            _prefab = prefab;
            _canvas = canvas;
            _container = container;

            if (_prefab.ShowInstantly)
            {
                Show().Forget();
            }
        }

        public async UniTask<T> Show(bool immediate = false, bool waitForCompletion = true, IEnumerable<object> extraArgs = null)
        {
            if (Instance != null)
            {
                Debug.LogError($"You try to Show {nameof(T)} panel, but it already Showed.");
                return Instance;
            }

            PanelState = State.Showing;
            extraArgs ??= Array.Empty<object>();
            await Load(extraArgs);

            if (waitForCompletion)
            {
                await awaitCompletion();
            }
            else
            {
                awaitCompletion().Forget();
            }

            return Instance;

            async UniTask awaitCompletion()
            {
                await Instance.InitializeAsync();
                if (!immediate)
                {
                    await Instance.ShowAnimation();
                }
                PanelState = State.Show;
            }
        }

        public async UniTask Hide(bool immediate = false)
        {
            if (Instance == null)
            {
                Debug.LogError($"You try to Hide {nameof(T)} panel, but it not been Showed.");
                return;
            }

            if (PanelState == State.Hiding)
            {
                return;
            }
            
            Instance.Hide(immediate);
            await _hideCompletionSource.Task;
        }

        private async UniTaskVoid HideInternal(bool immediate = false)
        {
            _hideDisposable.Dispose();
            PanelState = State.Hiding;
            if (!immediate)
            {
                await Instance.HideAnimation();
            }
            PanelState = State.Hide;
            Unload();
        }

        private UniTask Load(IEnumerable<object> extraArgs)
        {
            Instance = _container.InstantiatePrefabForComponent<T>(_prefab, _canvas.transform, extraArgs);
            _hideDisposable = Instance.HideObservable.Subscribe(t => HideInternal(t).Forget());
            return UniTask.CompletedTask;
        }

        private void Unload()
        {
            UnityEngine.Object.DestroyImmediate(Instance.gameObject);
            Instance = null;
            _hideCompletionSource.TrySetResult();
            _hideCompletionSource = AutoResetUniTaskCompletionSource.Create();
        }
    }
}