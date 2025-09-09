using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Modules
{
    public abstract class ModuleInstallerBase<T> : MonoInstaller where T : ModuleConfigBase
    {
        [SerializeField] private ModuleControllerBase _moduleController;
        [field: SerializeField] protected T Config { get; set; }

        protected abstract void InstallModuleBindings();
        
        public override void InstallBindings()
        {
            BindModule();
            BindConfigs();
            InstallModuleBindings();
        }

        private void BindModule()
        {
            Container.Bind<ModuleControllerBase>().FromInstance(_moduleController).AsSingle().NonLazy();
        }

        private void BindConfigs()
        {
            Container.Bind<T>().FromInstance(Config).AsSingle().NonLazy();
        }
    }
}