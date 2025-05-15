using System;
using JetBrains.Annotations;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine.InputSystem;

namespace TendedTarsier.Core.Services.Input
{
    [UsedImplicitly]
    public class InputService : ServiceBase
    {
        private readonly GameplayInput _gameplayInput;
        public InputAction Speed;
        public IObservable<InputAction.CallbackContext> OnSpeedStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnSpeedPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnSpeedCanceled { get; private set; }
        
        public InputAction Direction;
        public IObservable<InputAction.CallbackContext> OnDirectionStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnDirectionPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnDirectionCanceled { get; private set; }
        
        public IObservable<InputAction.CallbackContext> OnMenuButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnMapButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMapButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMapButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnEnterButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnEnterButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnEnterButtonCanceled { get; private set; }

        public InputService(GameplayInput gameplayInput)
        {
            _gameplayInput = gameplayInput;
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitInput();
        }

        private void InitInput()
        {
            Speed = _gameplayInput.Gameplay.Speed;
            (OnSpeedStarted, OnSpeedPerformed, OnSpeedCanceled) = _gameplayInput.Gameplay.Speed.ToObservable();
            Direction = _gameplayInput.Gameplay.Direction;
            (OnDirectionStarted, OnDirectionPerformed, OnDirectionCanceled) = _gameplayInput.Gameplay.Direction.ToObservable();
            (OnMapButtonStarted, OnMapButtonPerformed, OnMapButtonCanceled) = _gameplayInput.Gameplay.Map.ToObservable();
            (OnMenuButtonStarted, OnMenuButtonPerformed, OnMenuButtonCanceled) = _gameplayInput.Gameplay.Menu.ToObservable();
            (OnEnterButtonStarted, OnEnterButtonPerformed, OnEnterButtonCanceled) = _gameplayInput.Gameplay.Enter.ToObservable();

            _gameplayInput.Gameplay.Enable();
        }
    }
}