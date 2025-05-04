#if UNITY_EDITOR
using System.IO;
using TendedTarsier.Core.Services.Profile;
using UnityEditor;

namespace TendedTarsier.Core.Utilities
{
    public static class MenuUtilities
    {
        private const string RootMenu = "TendedTarsier/";
        private const string ProfileMenu = RootMenu + "User Profile/";

        [MenuItem(ProfileMenu + "Clean Profiles", false, 1)]
        private static void CleanProfiles()
        {
            if (Directory.Exists(ProfileService.ProfilesDirectory))
            {
                Directory.Delete(ProfileService.ProfilesDirectory, true);
            }
        }
    }
}
#endif