using System;
using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Services.Modules;
using Zenject;

namespace TendedTarsier.Core.Modules.Loading
{
    public class LoadingModuleController : ModuleControllerBase
    {
        private LoadingModuleConfig _moduleConfig;

        [Inject]
        private void Construct(LoadingModuleConfig moduleConfig)
        {
            _moduleConfig = moduleConfig;
        }

        private void Start()
        {
            Initialize().Forget();
        }

        public override async UniTask Initialize()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_moduleConfig.StartupLoadingDuration));
            ModuleService.LoadModule(_moduleConfig.MenuScene).Forget();
        }
    }
}