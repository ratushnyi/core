using System;
using UniRx;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
#if NETCODE
using Unity.Netcode;
#endif

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class UniRxExtensions
    {
#if NETCODE
        public static IObservable<Unit> OnLoadCompleteAsObservable(this NetworkSceneManager networkSceneManager)
        {
            return Observable.FromEvent<NetworkSceneManager.OnLoadCompleteDelegateHandler>(convert => (_, _, _) => convert(), t => networkSceneManager.OnLoadComplete += t,
                t => networkSceneManager.OnLoadComplete -= t);
        }

        public static IObservable<ConnectionEventData> OnConnectionEventAsObservable(this NetworkManager networkManager)
        {
            return Observable.FromEvent<Action<NetworkManager, ConnectionEventData>, ConnectionEventData>(
                convert => (_, d) => convert(d),
                h => networkManager.OnConnectionEvent += h,
                h => networkManager.OnConnectionEvent -= h
            );
        }
#endif
        public static IObservable<Unit> AsObservable(this Action action)
        {
            return Observable.FromEvent(t => action += t, t => action -= t);
        }

        public static IObservable<T> AsObservable<T>(this Action<T> action)
        {
            return Observable.FromEvent<T>(t => action += t, t => action -= t);
        }

        public static IObservable<(T1, T2)> AsObservable<T1, T2>(this Action<T1, T2> action)
        {
            return Observable.FromEvent<Action<T1, T2>, (T1, T2)>(convert => (t1, t2) => convert((t1, t2)), t => action += t, t => action -= t);
        }

        public static IObservable<(T1, T2, T3)> AsObservable<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            return Observable.FromEvent<Action<T1, T2, T3>, (T1, T2, T3)>(convert => (t1, t2, t3) => convert((t1, t2, t3)), t => action += t, t => action -= t);
        }

        public static (IObservable<InputAction.CallbackContext> started, IObservable<InputAction.CallbackContext> performed, IObservable<InputAction.CallbackContext> canceled) AsObservable(
            this InputAction inputAction)
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