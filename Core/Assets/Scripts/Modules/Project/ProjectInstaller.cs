using TendedTarsier.Core.Services.Audio;
using TendedTarsier.Core.Services.Input;
using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Services.Profile;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace TendedTarsier.Core.Modules.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Engine")]
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Canvas _canvas;

        public override void InstallBindings()
        {
            BindEngine();
            BindProfiles();
            BindServices();
        }

        private void BindEngine()
        {
            Container.Bind<GameplayInput>().FromInstance(new GameplayInput()).AsSingle().NonLazy();
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle().NonLazy();
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.BindService<ProfileService>();
            Container.BindService<InputService>();
            Container.BindService<AudioService>();
        }

        private void BindProfiles()
        {
            Container.BindProfile<ProjectProfile>();
        }
    }
}