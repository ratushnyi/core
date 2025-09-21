using System;
using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Services.Modules;
using Zenject;

namespace TendedTarsier.Core.Modules.Loading
{
    public class LoadingModuleController : ModuleControllerBase
    {
        private LoadingModuleConfig _moduleConfig;
        private ModuleService _moduleService;

        [Inject]
        private void Construct(LoadingModuleConfig moduleConfig, ModuleService moduleService)
        {
            _moduleConfig = moduleConfig;
            _moduleService = moduleService;
        }

        private void Start()
        {
            Initialize().Forget();
        }

        public override async UniTask Initialize()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_moduleConfig.StartupLoadingDuration));
            _moduleService.LoadModule(_moduleConfig.MenuScene).Forget();
        }
    }
}