using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuModuleController _menuModuleController;
        [SerializeField] private MenuConfig _menuConfig;
        [SerializeField] private Canvas _canvas;

        public override void InstallBindings()
        {
            BindModule();
            BindConfig();
            BindPanels();
        }

        private void BindModule()
        {
            Container.Bind<MenuModuleController>().FromInstance(_menuModuleController).AsSingle().NonLazy();;
        }

        private void BindConfig()
        {
            Container.Bind<MenuConfig>().FromInstance(_menuConfig).AsSingle().NonLazy();;
        }

        private void BindPanels()
        {
            Container.BindPanel<MenuPanel>(_menuConfig.MenuPanel, _canvas);
        }
    }
}