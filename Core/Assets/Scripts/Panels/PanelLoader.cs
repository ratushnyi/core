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

        public async UniTask Hide(bool immediate = false, bool waitForCompletion = false)
        {
            _hideDisposable.Dispose();
            if (Instance == null)
            {
                Debug.LogError($"You try to Hide {nameof(T)} panel, but it not been Showed.");
                return;
            }

            PanelState = State.Hiding;
            if (!immediate)
            {
                await Instance.HideAnimation();
            }
            PanelState = State.Hide;

            if (waitForCompletion)
            {
                await Unload();
            }
            else
            {
                Unload().Forget();
            }
        }

        private UniTask Load(IEnumerable<object> extraArgs)
        {
            Instance = _container.InstantiatePrefabForComponent<T>(_prefab, _canvas.transform, extraArgs);
            _hideDisposable = Instance.HideObservable.Subscribe(t => Hide(t).Forget());
            return UniTask.CompletedTask;
        }

        private UniTask Unload()
        {
            UnityEngine.Object.DestroyImmediate(Instance.gameObject);
            Instance = null;
            return UniTask.CompletedTask;
        }
    }
}