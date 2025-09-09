using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Modules
{
    public abstract class ModuleControllerBase : MonoBehaviour, IDisposable
    {
        [Inject] protected ModuleService ModuleService { get; set; }
        [Inject] protected DiContainer Container { get; set; }
        protected readonly CompositeDisposable CompositeDisposable =  new();

        protected new T Instantiate<T>(T prefab, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var go = Container.InstantiatePrefabForComponent<T>(prefab, parent);

            if (!worldPositionStays)
            {
                go.transform.localPosition = Vector3.zero;
            }
            
            Container.BindInstance(go);

            return go;
        }

        public virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        public abstract UniTask Initialize();
    }
}