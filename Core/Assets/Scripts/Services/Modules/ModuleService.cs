using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TendedTarsier.Core.Modules;
using TendedTarsier.Core.Modules.General;
using TendedTarsier.Core.Panels;
using UnityEngine.SceneManagement;

namespace TendedTarsier.Core.Services.Modules
{
    [UsedImplicitly]
    public class ModuleService : ServiceBase
    {
        private readonly PanelLoader<GeneralLoadingPanel> _loadingPanel;
        public ModuleControllerBase CurrentModule {get; private set;}

        public ModuleService(PanelLoader<GeneralLoadingPanel> loadingPanel)
        {
            _loadingPanel = loadingPanel;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            CurrentModule = SceneManager.GetActiveScene().GetRootGameObjects()[0].GetComponent<ModuleControllerBase>();
        }
        
        public async UniTask LoadModule(string sceneName)
        {
            await _loadingPanel.Show();
            SceneManager.LoadScene(sceneName);
            await _loadingPanel.Hide();
        }
    }
}