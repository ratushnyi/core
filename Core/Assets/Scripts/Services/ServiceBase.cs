using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services
{
    public abstract class ServiceBase : IDisposable, IInitializable
    {
        protected readonly CompositeDisposable CompositeDisposable = new();
        [Inject] private DiContainer _container;

        void IInitializable.Initialize()
        {
            Observable.OnceApplicationQuit().Subscribe(_ => Terminate());
        }

        protected T Instantiate<T>(T prefab, Transform parent = null, bool worldPositionStays = true, params Type[] components) where T : Component
        {
            var instance = _container.InstantiatePrefabForComponent<T>(prefab, parent);

            foreach (var component in components)
            {
                instance.gameObject.AddComponent(component);
            }

            if (!worldPositionStays)
            {
                instance.transform.localPosition = Vector3.zero;
            }

            _container.BindInstance(instance);

            return instance;
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