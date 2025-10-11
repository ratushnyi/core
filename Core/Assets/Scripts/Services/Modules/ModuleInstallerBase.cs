using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Modules
{
    public abstract class ModuleInstallerBase<TController> : MonoInstaller where TController : ModuleControllerBase
    {
        [SerializeField] private TController _moduleController;

        protected abstract void InstallModuleBindings();
        public override void InstallBindings()
        {
            BindModule();
            InstallModuleBindings();
        }

        private void BindModule()
        {
            Container.BindWithParents<TController>().FromInstance(_moduleController).AsSingle().NonLazy();
        }
    }
    
    public abstract class ModuleInstallerBase<TController, TConfig> : ModuleInstallerBase<TController> where TConfig : ConfigBase where TController : ModuleControllerBase
    {
        [field: SerializeField] protected TConfig ModuleConfig { get; set; }
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            BindConfigs();
        }

        private void BindConfigs()
        {
            Container.Bind<TConfig>().FromInstance(ModuleConfig).AsSingle().NonLazy();
        }
    }
}