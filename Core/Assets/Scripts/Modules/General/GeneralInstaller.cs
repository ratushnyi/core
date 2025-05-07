using TendedTarsier.Core.Services.Input;
using TendedTarsier.Core.Services.Profile;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace TendedTarsier.Core.Modules.General
{
    public class GeneralInstaller : MonoInstaller
    {
        [Header("Engine")]
        [SerializeField] private EventSystem _eventSystem;

        public override void InstallBindings()
        {
            BindEngine();
            BindServices();
            BindProfiles();
        }

        private void BindEngine()
        {
            Container.Bind<GameplayInput>().FromInstance(new GameplayInput()).AsSingle();
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle();
        }

        private void BindServices()
        {
            Container.BindService<ProfileService>();
            Container.BindService<InputService>();
        }

        private void BindProfiles()
        {
            Container.BindProfile<GeneralProfile>();
        }
    }
}