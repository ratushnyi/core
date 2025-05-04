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

        public IObservable<InputAction.CallbackContext> OnGyroscopeStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnGyroscopePerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnGyroscopeCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnLeftStickStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnLeftStickPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnLeftStickCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnRightStickStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnRightStickPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnRightStickCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnXButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnXButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnXButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnYButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnYButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnYButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnAButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnAButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnAButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnBButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnBButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnBButtonCanceled { get; private set; }

        public IObservable<InputAction.CallbackContext> OnMenuButtonStarted { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnMenuButtonCanceled { get; private set; }

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
            (OnGyroscopeStarted, OnGyroscopePerformed, OnGyroscopeCanceled) = _gameplayInput.Gameplay.Gyroscope.ToObservable();
            (OnLeftStickStarted, OnLeftStickPerformed, OnLeftStickCanceled) = _gameplayInput.Gameplay.LeftStick.ToObservable();
            (OnRightStickStarted, OnRightStickPerformed, OnRightStickCanceled) = _gameplayInput.Gameplay.RightStick.ToObservable();
            (OnXButtonStarted, OnXButtonPerformed, OnXButtonCanceled) = _gameplayInput.Gameplay.ButtonX.ToObservable();
            (OnYButtonStarted, OnYButtonPerformed, OnYButtonCanceled) = _gameplayInput.Gameplay.ButtonY.ToObservable();
            (OnAButtonStarted, OnAButtonPerformed, OnAButtonCanceled) = _gameplayInput.Gameplay.ButtonA.ToObservable();
            (OnBButtonStarted, OnBButtonPerformed, OnBButtonCanceled) = _gameplayInput.Gameplay.ButtonB.ToObservable();
            (OnMenuButtonStarted, OnMenuButtonPerformed, OnMenuButtonCanceled) = _gameplayInput.Gameplay.Menu.ToObservable();

            _gameplayInput.Gameplay.Enable();
        }
    }
}