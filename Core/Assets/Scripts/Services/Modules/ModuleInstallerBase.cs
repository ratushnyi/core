using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Modules
{
    public abstract class ModuleInstallerBase : MonoInstaller
    {
        [SerializeField] private ModuleControllerBase _moduleController;

        protected abstract void InstallModuleBindings();
        public override void InstallBindings()
        {
            BindModule();
            InstallModuleBindings();
        }

        private void BindModule()
        {
            Container.Bind<ModuleControllerBase>().FromInstance(_moduleController).AsSingle().NonLazy();
        }
    }
    
    public abstract class ModuleInstallerBase<T> : ModuleInstallerBase where T : ModuleConfigBase
    {
        [field: SerializeField] protected T ModuleConfig { get; set; }
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            BindConfigs();
        }


        protected virtual void BindConfigs()
        {
            Container.Bind<T>().FromInstance(ModuleConfig).AsSingle().NonLazy();
        }
    }
}