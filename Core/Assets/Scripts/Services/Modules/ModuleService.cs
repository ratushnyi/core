using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TendedTarsier.Core.Modules.Loading;
using TendedTarsier.Core.Modules.Project;
using TendedTarsier.Core.Panels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TendedTarsier.Core.Services.Modules
{
    [UsedImplicitly]
    public class ModuleService : ServiceBase
    {
        private readonly PanelLoader<LoadingPanel> _loaderPanel;
        public ModuleControllerBase CurrentModule { get; private set; }

        public ModuleService(PanelLoader<LoadingPanel> loaderPanel)
        {
            _loaderPanel = loaderPanel;
        }

        public async UniTask LoadModule(string sceneName)
        {
            CurrentModule?.Dispose();
            await _loaderPanel.Show();
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            CurrentModule = Object.FindFirstObjectByType<ModuleControllerBase>();
            await CurrentModule.Initialize();
            await _loaderPanel.Hide();
        }
    }
}