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
        private LoadingConfig _config;
        private ProjectConfig _projectConfig;
        private ModuleService _moduleService;

        [Inject]
        private void Construct(LoadingConfig config, ProjectConfig projectConfig, ModuleService moduleService)
        {
            _config = config;
            _projectConfig = projectConfig;
            _moduleService = moduleService;
        }

        private void Start()
        {
            Initialize().Forget();
        }

        public override async UniTask Initialize()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.StartupLoadingDuration));
            _moduleService.LoadModule(_projectConfig.MenuScene).Forget();
        }
    }
}