using TendedTarsier.Core.Services.Modules;
using UnityEngine;

namespace TendedTarsier.Core.Modules.Loading
{
    [CreateAssetMenu(menuName = "Config/LoadingModuleConfig", fileName = "LoadingModuleConfig")]
    public class LoadingModuleConfig : ModuleConfigBase
    {
        [field: SerializeField] public float StartupLoadingDuration { get; set; }
        [field: SerializeField] public LoadingPanel LoadingPanel { get; set; }
    }
}