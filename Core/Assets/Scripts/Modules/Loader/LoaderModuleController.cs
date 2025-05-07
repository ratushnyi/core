using System;
using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Modules.Menu;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Loader
{
    public class LoaderModuleController : ModuleControllerBase
    {
        [SerializeField] private float _loadingDelay = 2f;
        public void Start()
        {
            LoadModule().Forget();
        }

        private async UniTaskVoid LoadModule()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_loadingDelay));
            ModuleService.LoadModule(MenuModuleController.SceneName).Forget();
        }
    }
}
