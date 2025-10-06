using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using TendedTarsier.Core.Modules.Project;
using TendedTarsier.Core.Services.Modules;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules.Loading
{
    public class LoadingModuleController : ModuleControllerBase
    {
        private LoadingModuleConfig _moduleConfig;
        private ProjectConfig _projectConfig;
        private ModuleService _moduleService;

        [Inject]
        private void Construct(LoadingModuleConfig moduleConfig, ProjectConfig projectConfig, ModuleService moduleService)
        {
            _moduleConfig = moduleConfig;
            _projectConfig = projectConfig;
            _moduleService = moduleService;
        }

        private void Start()
        {
            Initialize().Forget();
        }

        public override async UniTask Initialize()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_moduleConfig.StartupLoadingDuration));
            _moduleService.LoadModule(_projectConfig.MenuScene).Forget();
        }
    }
}