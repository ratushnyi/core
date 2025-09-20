namespace TendedTarsier.Core.Services.Profile
{
    public interface IProfile
    {
        string Name { get; }
        void RegisterFormatters();
        void OnSectionLoaded();
        void OnSectionCreated();
        void Clear();
    }
}