using UnityEngine;
using UnityEngine.EventSystems;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public static class InputExtensions
    {
        public enum ApplicationRunMode
        {
            Device,
            Editor,
            Simulator
        }

        public static ApplicationRunMode RunMode
        {
            get
            {
#if UNITY_EDITOR
                return UnityEngine.Device.Application.isEditor && !UnityEngine.Device.Application.isMobilePlatform ? ApplicationRunMode.Editor : ApplicationRunMode.Simulator;
#else
                return ApplicationRunMode.Device;
#endif
            }
        }

        public static bool IsOverUI(int pointerId)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }

        public static bool IsDesktopInput => !IsMobileInput && !IsConsoleInput;
        public static bool IsMobileInput => Application.isMobilePlatform || RunMode == ApplicationRunMode.Simulator;
        public static bool IsConsoleInput => Application.isConsolePlatform;
    }
}