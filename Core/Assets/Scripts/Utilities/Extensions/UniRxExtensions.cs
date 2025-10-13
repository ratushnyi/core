using System;
using UniRx;
using UnityEngine;
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

    public class ReactivePrefs<T> : ReactiveProperty<T>
    {
        private readonly string _key;

        public ReactivePrefs(string key, T defaultValue)
        {
            _key = key;
            if (typeof(T) == typeof(int) || typeof(T) == typeof(bool))
            {
                var value = PlayerPrefs.GetInt(_key, Convert.ToInt32(defaultValue));
                Value = (T)Convert.ChangeType(value, typeof(T));
            }
            else if (typeof(T) == typeof(float))
            {
                var value = PlayerPrefs.GetFloat(_key, Convert.ToSingle(defaultValue));
                Value = (T)Convert.ChangeType(value, typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                var value = PlayerPrefs.GetString(_key, Convert.ToString(defaultValue));
                Value = (T)Convert.ChangeType(value, typeof(T));
            }
            
        }
        
        protected override void SetValue(T value)
        {
            base.SetValue(value);
            if (typeof(T) == typeof(int) || typeof(T) == typeof(bool))
            {
                PlayerPrefs.SetInt(_key, Convert.ToInt32(value));
            }
            else if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(_key, Convert.ToSingle(value));
            }
            else if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(_key, Convert.ToString(value));
            }
        }
    }
}