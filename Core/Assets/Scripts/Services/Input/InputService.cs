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
        
        public InputAction Aim;
        public IObservable<InputAction.CallbackContext> OnAimStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnAimPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnAimCanceled { get; private set; }
        
        public InputAction Movement;
        public IObservable<InputAction.CallbackContext> OnMovementStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMovementPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMovementCanceled { get; private set; }
        
        public IObservable<InputAction.CallbackContext> OnMenuButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnMapButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMapButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMapButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnEnterButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnEnterButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnEnterButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnOverviewButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnOverviewButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnOverviewButtonCanceled { get; private set; }

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
            Aim = _gameplayInput.Gameplay.Aim;
            (OnAimStarted, OnAimPerformed, OnAimCanceled) = _gameplayInput.Gameplay.Aim.ToObservable();
            Movement = _gameplayInput.Gameplay.Movement;
            (OnMovementStarted, OnMovementPerformed, OnMovementCanceled) = _gameplayInput.Gameplay.Movement.ToObservable();
            (OnMapButtonStarted, OnMapButtonPerformed, OnMapButtonCanceled) = _gameplayInput.Gameplay.Map.ToObservable();
            (OnMenuButtonStarted, OnMenuButtonPerformed, OnMenuButtonCanceled) = _gameplayInput.Gameplay.Menu.ToObservable();
            (OnEnterButtonStarted, OnEnterButtonPerformed, OnEnterButtonCanceled) = _gameplayInput.Gameplay.Enter.ToObservable();
            (OnOverviewButtonStarted, OnOverviewButtonPerformed, OnOverviewButtonCanceled) = _gameplayInput.Gameplay.Overview.ToObservable();

            _gameplayInput.Gameplay.Enable();
        }
    }
}