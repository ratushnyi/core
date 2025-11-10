using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MemoryPack;
using TendedTarsier.Core.Modules.Project;
using TendedTarsier.Core.Utilities.Extensions;
using TendedTarsier.Core.Utilities.MemoryPack.FormatterProviders;
using UniRx;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Services.Profile
{
    [UsedImplicitly]
    public class ProfileService : ServiceBase, IInitializable
    {
        public static readonly string ProfilesDirectory = Path.Combine(Application.persistentDataPath, ProjectConstants.ProfilesDirectory);

        public IObservable<Unit> ClearAllObservable => _clearAllSubject;
        private readonly ISubject<Unit> _clearAllSubject = new Subject<Unit>();
        private readonly UniTaskCompletionSource _initializedTask = new();
        private readonly Dictionary<string, IProfile> _profiles = new();

        public void Initialize()
        {
            RegisterFormatters();
            _initializedTask.TrySetResult();
        }

        private void RegisterFormatters()
        {
            MemoryPackFormatterProvider.Register(new ReactivePropertyFormatter<bool>());
            MemoryPackFormatterProvider.Register(new ReactivePropertyFormatter<string>());
            MemoryPackFormatterProvider.Register(new ReactivePropertyFormatter<int>());
            MemoryPackFormatterProvider.Register(new ReactivePropertyFormatter<float>());
            MemoryPackFormatterProvider.Register(new ReactiveCollectionFormatter<bool>());
            MemoryPackFormatterProvider.Register(new ReactiveCollectionFormatter<string>());
            MemoryPackFormatterProvider.Register(new ReactiveCollectionFormatter<int>());
            MemoryPackFormatterProvider.Register(new ReactiveCollectionFormatter<float>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, bool>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, string>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, int>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, float>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, ReactiveProperty<bool>>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, ReactiveProperty<string>>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, ReactiveProperty<int>>());
            MemoryPackFormatterProvider.Register(new ReactiveDictionaryFormatter<string, ReactiveProperty<float>>());
        }

        public void RegisterProfile(IProfile profile)
        {
            if (!_profiles.TryAdd(profile.Name, profile))
            {
                Save(_profiles[profile.Name]);
                _profiles[profile.Name] = profile;
            }

            LoadProfile(profile).Forget();
        }

        public void UnregisterProfile(IProfile profile)
        {
            _profiles.Remove(profile.Name);
        }

        private async UniTaskVoid LoadProfile(IProfile profile)
        {
            await _initializedTask.Task;
            profile.RegisterFormatters();
            var path = GetSectionPath(profile.Name);
            if (File.Exists(path))
            {
                try
                {
#if JSON_PROFILES
                    var referenceObject = JsonUtility.FromJson(File.ReadAllText(path), profile.GetType());
#else
                    var referenceObject = MemoryPackSerializer.Deserialize(profile.GetType(), File.ReadAllBytes(path));
#endif

                    TypeExtensions.PopulateObject(profile, referenceObject);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    CreateSection(profile);
                }
            }
            else
            {
                CreateSection(profile);
            }

            profile.OnSectionLoaded();
        }

        public void SaveAll()
        {
            foreach (var profile in _profiles)
            {
                Save(profile.Value);
            }
        }

        public void Save(IProfile profile)
        {
            try
            {
                if (!Directory.Exists(ProfilesDirectory))
                {
                    Directory.CreateDirectory(ProfilesDirectory);
                }

                var file = GetSectionPath(profile.Name);

#if JSON_PROFILES
                var json = JsonUtility.ToJson(profile);
#else
                var byteData = MemoryPackSerializer.Serialize(profile.GetType(), profile);
#endif
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
#if JSON_PROFILES
                File.WriteAllText(file, json);
#else
                File.WriteAllBytes(file, byteData);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed save section {profile.Name}. Details ↓\n{e.Message} ");
            }
        }

        private void CreateSection(IProfile profile)
        {
            profile.OnSectionCreated();
            Save(profile);
        }

        private string GetSectionPath(string name)
        {
            var fileName = name + ".json";
            return Path.Combine(ProfilesDirectory, fileName);
        }

        public void ClearAll()
        {
            foreach (var profile in _profiles)
            {
                profile.Value.Clear();
            }
            _clearAllSubject.OnNext(Unit.Default);
        }
        
#if NETCODE
        public bool IsServerSaveExist(string serverId)
        {
            serverId += "_";
            var files = Directory.GetFiles(ProfilesDirectory);
            return files.Any(file => file.StartsWith(serverId));
        }
        
        public void ClearServerSave(string serverId)
        {
            serverId += "_";
            var files = Directory.GetFiles(ProfilesDirectory);
            var serverSave = files.Where(file => file.StartsWith(serverId));
            foreach (var file in serverSave)
            {
                File.Delete(file);
            }
        }
#endif

        public override void Dispose()
        {
            base.Dispose();
            SaveAll();
        }
    }
}