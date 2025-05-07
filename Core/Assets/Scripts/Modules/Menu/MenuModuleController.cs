using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Panels;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuModuleController : ModuleControllerBase
    {
        public static string SceneName => "Menu";

        private PanelLoader<MenuPanel> _menuPanel;

        [Inject]
        private void Construct(PanelLoader<MenuPanel> menuPanel)
        {
            _menuPanel = menuPanel;
        }

        public void Start()
        {
            Show().Forget();
        }

        private async UniTaskVoid Show()
        {
            _menuPanel.Show(true).Forget();
            await _menuPanel.Instance.ShowButtons();
        }
    }
}