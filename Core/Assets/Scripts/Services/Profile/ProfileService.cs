using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using MemoryPack;
using TendedTarsier.Core.Modules.Project;
using TendedTarsier.Core.Utilities.Extensions;
using TendedTarsier.Core.Utilities.MemoryPack.FormatterProviders;
using UniRx;
using UnityEngine;

namespace TendedTarsier.Core.Services.Profile
{
    [UsedImplicitly]
    public class ProfileService : ServiceBase
    {
        public static readonly string ProfilesDirectory = Path.Combine(Application.persistentDataPath, ProjectConstants.ProfilesDirectory);

        public IObservable<Unit> ClearAllObservable => _clearAllSubject;
        private readonly ISubject<Unit> _clearAllSubject = new Subject<Unit>();
        private readonly List<IProfile> _profiles;

        public ProfileService(List<IProfile> profiles)
        {
            _profiles = profiles;
        }

        protected override void Initialize()
        {
            base.Initialize();

            RegisterFormatters();
            LoadSections();
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

        private void LoadSections()
        {
            foreach (var profile in _profiles)
            {
                profile.RegisterFormatters();
                LoadSection(profile);
            }
        }

        private void LoadSection(IProfile profile)
        {
            var path = GetSectionPath(profile.Name);
            if (File.Exists(path))
            {
                try
                {
                    var bytesData = File.ReadAllBytes(path);
                    var referenceObject = MemoryPackSerializer.Deserialize(profile.GetType(), bytesData);

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
            profile.Init(this);
        }

        public void SaveAll()
        {
            foreach (var profile in _profiles)
            {
                Save(profile);
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
                var byteData = MemoryPackSerializer.Serialize(profile.GetType(), profile);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                File.WriteAllBytes(file, byteData);
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
                profile.Clear();
            }
            _clearAllSubject.OnNext(Unit.Default);
        }

        public override void Dispose()
        {
            base.Dispose();
            SaveAll();
        }
    }
}