using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Modules
{
    public abstract class ModuleControllerBase : MonoBehaviour, IDisposable
    {
        protected readonly CompositeDisposable CompositeDisposable = new();
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        protected new T Instantiate<T>(T prefab, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var go = _container.InstantiatePrefabForComponent<T>(prefab, parent);

            if (!worldPositionStays)
            {
                go.transform.localPosition = Vector3.zero;
            }

            _container.BindInstance(go);

            return go;
        }

        public virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        public abstract UniTask Initialize();
    }
}