using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuModuleInstaller : ModuleInstallerBase<MenuModuleController, MenuConfig>
    {
        [SerializeField] private Canvas _canvas;

        protected override void InstallModuleBindings()
        {
            BindPanels();
        }

        private void BindPanels()
        {
            Container.BindPanel(ModuleConfig.MenuPanel, _canvas);
        }
    }
}