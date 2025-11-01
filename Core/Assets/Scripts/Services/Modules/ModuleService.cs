using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TendedTarsier.Core.Modules.Loading;
using TendedTarsier.Core.Panels;
using TendedTarsier.Core.Utilities.Extensions;
using UniRx;
using UnityEngine.SceneManagement;
#if NETCODE
using Unity.Netcode;
#endif

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
            
#if NETCODE
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                await NetworkManager.Singleton.SceneManager.OnLoadCompleteAsObservable().First();
            }
            else
#endif
            {
                await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            }
            
            CurrentModule = UnityEngine.Object.FindFirstObjectByType<ModuleControllerBase>();
            await CurrentModule.Initialize();
            await _loaderPanel.Hide();
        }
    }
}