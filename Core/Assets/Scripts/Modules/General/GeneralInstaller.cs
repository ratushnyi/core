using TendedTarsier.Core.Services.Input;
using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Services.Profile;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace TendedTarsier.Core.Modules.General
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private GeneralLoadingPanel _generalLoadingPanel;
        [SerializeField] private GeneralConfig _generalConfig;
        [Header("Engine")]
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Canvas _canvas;

        public override void InstallBindings()
        {
            BindEngine();
            BindPanels();
            BindConfigs();
            BindProfiles();
            BindServices();
        }

        private void BindEngine()
        {
            Container.Bind<GameplayInput>().FromInstance(new GameplayInput()).AsSingle().NonLazy();
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.BindService<ModuleService>();
            Container.BindService<ProfileService>();
            Container.BindService<InputService>();
        }

        private void BindConfigs()
        {
            Container.Bind<GeneralConfig>().FromInstance(_generalConfig).AsSingle().NonLazy();
        }

        private void BindProfiles()
        {
            Container.BindProfile<GeneralProfile>();
        }

        private void BindPanels()
        {
            Container.BindPanel<GeneralLoadingPanel>(_generalLoadingPanel, _canvas);
        }
    }
}