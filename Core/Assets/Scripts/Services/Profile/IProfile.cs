namespace TendedTarsier.Core.Services.Profile
{
    public interface IProfile
    {
        string Name { get; }
        void Init(ProfileService profileService);
        void OnSectionLoaded();
        void OnSectionCreated();
        void Clear();
    }
}