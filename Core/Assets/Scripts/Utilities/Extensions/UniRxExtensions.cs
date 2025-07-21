using System;
using UniRx;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class UniRxExtensions
    {
        public static (IObservable<InputAction.CallbackContext> started,
            IObservable<InputAction.CallbackContext> performed,
            IObservable<InputAction.CallbackContext> canceled) ToObservable(this InputAction inputAction)
        {
            return (Observable.FromEvent<InputAction.CallbackContext>(t => inputAction.started += t, t => inputAction.started -= t),
                Observable.FromEvent<InputAction.CallbackContext>(t => inputAction.performed += t, t => inputAction.performed -= t),
                Observable.FromEvent<InputAction.CallbackContext>(t => inputAction.canceled += t, t => inputAction.canceled -= t));
        }

        public static IObservable<T> OnClickAsObservable<T>(this Button button, T value = default)
        {
            return Observable.FromEvent<T>(h => button.onClick.AddListener(observeValue(h)), h => button.onClick.RemoveListener(observeValue(h)));

            UnityAction observeValue(Action<T> action)
            {
                return () => action(value);
            }
        }
    }
}