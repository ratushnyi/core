using System;
using MemoryPack;
using TendedTarsier.Core.Utilities.Extensions;
using UniRx;

namespace TendedTarsier.Core.Services.Profile
{
    public abstract class ProfileBase : IProfile, IDisposable
    {
        protected readonly CompositeDisposable CompositeDisposable = new();

        private ProfileService _profileService;

        [MemoryPackIgnore]
        public abstract string Name { get; }

        public void Init(ProfileService profileService)
        {
            _profileService = profileService;
        }

        public virtual void RegisterFormatters()
        {
        }

        public virtual void OnSectionCreated()
        {
        }

        public virtual void OnSectionLoaded()
        {
        }

        public void Save()
        {
            _profileService.Save(this);
        }

        public void Clear()
        {
            var newInstance = Activator.CreateInstance(GetType());
            TypeExtensions.PopulateObject(this, newInstance);
            OnSectionCreated();
            _profileService.Save(this);
        }

        public virtual void Dispose()
        {
            Save();
            CompositeDisposable.Dispose();
        }
    }
}