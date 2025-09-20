using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules.Loading
{
    public class LoadingModuleInstaller : ModuleInstallerBase<LoadingModuleConfig>
    {
        private Canvas _canvas;

        [Inject]
        public void Construct(Canvas canvas)
        {
            _canvas = canvas;
        }

        protected override void InstallModuleBindings()
        {
            BindPanels();
            BindServices();
        }
        
        private void BindPanels()
        {
            ProjectContext.Instance.Container.BindPanel<LoadingPanel>(ModuleConfig.LoadingPanel, _canvas);
        }

        private void BindServices()
        {
            ProjectContext.Instance.Container.BindService<ModuleService>();
        }
    }
}