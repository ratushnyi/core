using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services
{
    public abstract class ServiceBase : IDisposable
    {
        protected readonly CompositeDisposable CompositeDisposable = new();
        [Inject] private DiContainer Container;

        [Inject]
        protected virtual void Initialize()
        {
            Observable.OnceApplicationQuit().Subscribe(_ => Terminate());
        }

        protected T Instantiate<T>(T prefab, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var go = Container.InstantiatePrefabForComponent<T>(prefab, parent);

            if (!worldPositionStays)
            {
                go.transform.localPosition = Vector3.zero;
            }

            Container.BindInstance(go);

            return go;
        }

        private void Terminate()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }
    }
}