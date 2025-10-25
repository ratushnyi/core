using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using Unity.Netcode;

namespace TendedTarsier.Core.Utilities.Extensions
{
#if NETCODE
    public static class NetworkExtensions
    {
        public static Tween DOValue<T>(this NetworkVariable<T> variable, T value, float duration)
        {
            return (value, variable) switch
            {
                (int v, NetworkVariable<int> nv) => DOTween.To(() => nv.Value, t => nv.Value = t, v, duration),
                (float v, NetworkVariable<float> nv) => DOTween.To(() => nv.Value, t => nv.Value = t, v, duration),
                _ => null
            };
        }

        public static bool TryGet<T>(this NetworkList<T> list, Func<T, bool> predicate, out T result) where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < list.Count; index++)
            {
                if (!predicate(list[index]))
                {
                    continue;
                }
                result = list[index];
                return true;
            }

            result = default;
            return false;
        }

        public static int IndexOf<T>(this NetworkList<T> list, Func<T, bool> predicate) where T : unmanaged, IEquatable<T>
        {
            for (var i = 0; i < list.Count; i++)
            {
                var old = list[i];
                if (!predicate(old))
                {
                    continue;
                }

                return i;
            }
            return -1;
        }

        public static T FirstOrDefault<T>(this NetworkList<T> list, Func<T, bool> predicate) where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < list.Count; index++)
            {
                if (predicate(list[index]))
                {
                    return list[index];
                }
            }

            return default;
        }

        public static bool Any<T>(this NetworkList<T> list, Func<T, bool> predicate) where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < list.Count; index++)
            {
                if (predicate(list[index]))
                {
                    return true;
                }
            }

            return false;
        }

        public static void SerializeList<T, TSerializer>(this BufferSerializer<TSerializer> serializer, ref List<T> list) where TSerializer : IReaderWriter where T : INetworkSerializable, new()
        {
            var count = list?.Count ?? 0;
            serializer.SerializeValue(ref count);

            if (serializer.IsReader)
            {
                list = new List<T>(count);
                for (var i = 0; i < count; i++)
                {
                    T element = new();
                    element.NetworkSerialize(serializer);
                    list.Add(element);
                }
            }
            else if (list != null)
            {
                for (var i = 0; i < count; i++)
                {
                    var element = list[i];
                    element.NetworkSerialize(serializer);
                }
            }
        }

        public static ClientRpcParams ToClientRpcParams(this ulong targetClientId)
        {
            return new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { targetClientId } } };
        }

        public static IObservable<T> AsObservable<T>(this NetworkVariable<T> networkVariable)
        {
            return Observable.FromEvent<NetworkVariable<T>.OnValueChangedDelegate, T>(convert => (_, newValue) => convert(newValue), t => networkVariable.OnValueChanged += t,
                t => networkVariable.OnValueChanged -= t);
        }

        public static IObservable<NetworkListEvent<T>> AsObservable<T>(this NetworkList<T> networkVariable) where T : unmanaged, IEquatable<T>
        {
            return Observable.FromEvent<NetworkList<T>.OnListChangedDelegate, NetworkListEvent<T>>(convert => (t) => convert(t), t => networkVariable.OnListChanged += t,
                t => networkVariable.OnListChanged -= t);
        }

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
    }
#endif
}