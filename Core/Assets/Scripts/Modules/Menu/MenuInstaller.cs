using TendedTarsier.Core.Modules.General;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MenuConfig _menuConfig;

        public override void InstallBindings()
        {
            BindConfig();
            BindPanels();
        }

        private void BindConfig()
        {
            Container.Bind<MenuConfig>().FromInstance(_menuConfig);
        }

        private void BindPanels()
        {
            Container.BindPanel<LoadingPanel>(_menuConfig.LoadingPanel, _canvas);
            Container.BindPanel<MenuPanel>(_menuConfig.MenuPanel, _canvas);
        }
    }
}