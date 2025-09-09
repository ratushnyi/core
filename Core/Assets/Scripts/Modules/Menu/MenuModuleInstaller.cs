using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuModuleInstaller : ModuleInstallerBase<MenuModuleConfig>
    {
        [SerializeField] private Canvas _canvas;

        protected override void InstallModuleBindings()
        {
            BindPanels();
        }

        private void BindPanels()
        {
            Container.BindPanel<MenuPanel>(Config.MenuPanel, _canvas);
        }
    }
}