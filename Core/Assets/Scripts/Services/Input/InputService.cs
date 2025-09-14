using System;
using JetBrains.Annotations;
using TendedTarsier.Core.Utilities.Extensions;
using UnityEngine.InputSystem;
using Zenject;

namespace TendedTarsier.Core.Services.Input
{
    [UsedImplicitly]
    public class InputService : ServiceBase, IInitializable
    {
        private readonly GameplayInput _gameplayInput;

        public GameplayInput.PlayerActions PlayerActions  => _gameplayInput.Player;

        /// Gamepad A/Space
        public IObservable<InputAction.CallbackContext> OnJumpButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnJumpButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnJumpButtonCanceled { get; private set; }

        /// Gamepad B/Control
        public IObservable<InputAction.CallbackContext> OnCrouchButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnCrouchButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnCrouchButtonCanceled { get; private set; }

        /// Gamepad X/Enter
        public IObservable<InputAction.CallbackContext> OnAttackButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnAttackButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnAttackButtonCanceled { get; private set; }

        /// Gamepad Y/E
        public IObservable<InputAction.CallbackContext> OnInteractButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnInteractButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnInteractButtonCanceled { get; private set; }

        /// Left stick click/Left Shift 
        public IObservable<InputAction.CallbackContext> OnSprintButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnSprintButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnSprintButtonCanceled { get; private set; }

        /// D-Pad Left/Left arrow
        public IObservable<InputAction.CallbackContext> OnLeftButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnLeftButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnLeftButtonCanceled { get; private set; }

        /// D-Pad Right/Right arrow
        public IObservable<InputAction.CallbackContext> OnRightButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnRightButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnRightButtonCanceled { get; private set; }

        /// D-Pad Up/Up arrow
        public IObservable<InputAction.CallbackContext> OnUpButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnUpButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnUpButtonCanceled { get; private set; }

        /// D-Pad Down/Down arrow
        public IObservable<InputAction.CallbackContext> OnDownButtonStarted { get; private set; }

        public IObservable<InputAction.CallbackContext> OnDownButtonPerformed { get; private set; }
        public IObservable<InputAction.CallbackContext> OnDownButtonCanceled { get; private set; }

        public InputService(GameplayInput gameplayInput)
        {
            _gameplayInput = gameplayInput;
        }

        public void Initialize()
        {
            InitInput();
        }

        private void InitInput()
        {
            (OnJumpButtonStarted, OnJumpButtonPerformed, OnJumpButtonCanceled) = _gameplayInput.Player.Jump.ToObservable();
            (OnCrouchButtonStarted, OnCrouchButtonPerformed, OnCrouchButtonCanceled) = _gameplayInput.Player.Crouch.ToObservable();
            (OnAttackButtonStarted, OnAttackButtonPerformed, OnAttackButtonCanceled) = _gameplayInput.Player.Attack.ToObservable();
            (OnInteractButtonStarted, OnInteractButtonPerformed, OnInteractButtonCanceled) = _gameplayInput.Player.Interact.ToObservable();

            (OnSprintButtonStarted, OnSprintButtonPerformed, OnSprintButtonCanceled) = _gameplayInput.Player.Sprint.ToObservable();

            (OnLeftButtonStarted, OnLeftButtonPerformed, OnLeftButtonCanceled) = _gameplayInput.Player.Left.ToObservable();
            (OnRightButtonStarted, OnRightButtonPerformed, OnRightButtonCanceled) = _gameplayInput.Player.Right.ToObservable();
            (OnUpButtonStarted, OnUpButtonPerformed, OnUpButtonCanceled) = _gameplayInput.Player.Up.ToObservable();
            (OnDownButtonStarted, OnDownButtonPerformed, OnDownButtonCanceled) = _gameplayInput.Player.Down.ToObservable();

            _gameplayInput.Player.Enable();
        }
    }
}