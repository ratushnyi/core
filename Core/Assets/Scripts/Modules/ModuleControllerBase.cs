using TendedTarsier.Core.Services.Modules;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules
{
    public abstract class ModuleControllerBase : MonoBehaviour
    {
        [Inject] protected ModuleService ModuleService { get; set; }
        [Inject] protected DiContainer Container { get; set; }

        protected new T Instantiate<T>(T prefab, Transform parent = null) where T : Component
        {
            return Container.InstantiatePrefabForComponent<T>(prefab, parent);
        }
    }
}