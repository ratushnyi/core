using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Panels;
using TendedTarsier.Core.Services.Modules;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuModuleController : ModuleControllerBase
    {
        private PanelLoader<MenuPanel> _menuPanelLoader;

        [Inject]
        private void Construct(PanelLoader<MenuPanel> menuPanelLoader)
        {
            _menuPanelLoader = menuPanelLoader;
        }

        public override UniTask Initialize()
        {
            return _menuPanelLoader.Show();
        }
    }
}